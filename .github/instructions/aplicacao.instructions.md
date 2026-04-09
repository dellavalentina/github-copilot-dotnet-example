---
applyTo: "**/*AppServico.cs,**/*Profile.cs"
---

# Padrões da Camada Aplicação (<Projeto>.Aplicacao)

## Responsabilidade

Orquestrar fluxos de aplicação, mapear DTOs para comandos, delegar para serviços de domínio e retornar responses.

## Estrutura de Pastas

```
<Projeto>.Aplicacao/
└── <Feature>/
    ├── Profiles/
    │   └── <Feature>Profile.cs
    └── Servicos/
        ├── Interfaces/
        │   └── I<Feature>AppServico.cs
        └── <Feature>AppServico.cs
```

## Nomenclatura

| Elemento  | Padrão                 | Exemplo                  |
| --------- | ---------------------- | ------------------------ |
| Serviço   | `<Feature>AppServico`  | `DepoimentosAppServico`  |
| Interface | `I<Feature>AppServico` | `IDepoimentosAppServico` |
| Profile   | `<Feature>Profile`     | `DepoimentosProfile`     |

## Padrões de Código

### Interface

```csharp
namespace <Projeto>.Aplicacao.<Feature>.Servicos.Interfaces;

public interface I<Feature>AppServico
{
    Task<<Feature>Response> InserirAsync(<Feature>InserirRequest request, CancellationToken ct = default);
    Task<<Feature>Response> EditarAsync(<Feature>EditarRequest request, CancellationToken ct = default);
    Task ExcluirAsync(int id, CancellationToken ct = default);
    <Feature>Response Recuperar(int id);
    PaginacaoConsulta<<Feature>Response> Listar(<Feature>ListarRequest request);
}
```

### Serviço de Aplicação com UnitOfWork

```csharp
namespace <Projeto>.Aplicacao.<Feature>.Servicos;

public class <Feature>AppServico : I<Feature>AppServico
{
    private readonly I<Feature>Servicos _<feature>Servicos;
    private readonly I<Feature>Repositorio _<feature>Repositorio; // para leituras diretas sem regra de negócio
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public <Feature>AppServico(
        I<Feature>Servicos <feature>Servicos,
        I<Feature>Repositorio <feature>Repositorio,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _<feature>Servicos = <feature>Servicos;
        _<feature>Repositorio = <feature>Repositorio;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<<Feature>Response> InserirAsync(<Feature>InserirRequest request, CancellationToken ct = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var comando = _mapper.Map<<Feature>InserirComando>(request);
            var entidade = await _<feature>Servicos.InserirAsync(comando, ct);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<<Feature>Response>(entidade);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<<Feature>Response> EditarAsync(<Feature>EditarRequest request, CancellationToken ct = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var comando = _mapper.Map<<Feature>EditarComando>(request);
            var entidade = await _<feature>Servicos.EditarAsync(comando, ct);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<<Feature>Response>(entidade);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task ExcluirAsync(int id, CancellationToken ct = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _<feature>Servicos.ExcluirAsync(id, ct);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    // Leitura com validação de negócio — delega para o serviço de domínio
    public <Feature>Response Recuperar(int id)
    {
        var entidade = _<feature>Servicos.Recuperar(id);
        return _mapper.Map<<Feature>Response>(entidade);
    }

    // Leitura direta via repositório — sem regra de negócio, apenas filtros/paginação
    public PaginacaoConsulta<<Feature>Response> Listar(<Feature>ListarRequest request)
    {
        var filtro = _mapper.Map<<Feature>ListarFiltro>(request);
        var query = _<feature>Repositorio.Query();
        if (filtro.Ativo.HasValue) query = query.Where(x => x.Ativo == filtro.Ativo.Value);
        var total = query.Count();
        var registros = query
            .Skip((filtro.Pg - 1) * filtro.Qt)
            .Take(filtro.Qt)
            .ToList();
        return new PaginacaoConsulta<<Feature>Response>
        {
            Registros = _mapper.Map<List<<Feature>Response>>(registros),
            Total = total
        };
    }
}
```

### Profile AutoMapper

```csharp
namespace <Projeto>.Aplicacao.<Feature>.Profiles;

public class <Feature>Profile : Profile
{
    public <Feature>Profile()
    {
        CreateMap<<Feature>InserirRequest, <Feature>InserirComando>();
        CreateMap<<Feature>EditarRequest, <Feature>EditarComando>();
        CreateMap<<Feature>ListarRequest, <Feature>ListarFiltro>();
        CreateMap<<Entidade>, <Feature>Response>();
    }
}
```

## Regras

- ✅ UnitOfWork em toda operação de escrita (Inserir, Editar, Excluir)
- ✅ Mapear DTOs para comandos via AutoMapper
- ✅ Delegar lógica para serviços de domínio quando houver regra de negócio
- ✅ Acessar `I<Feature>Repositorio` diretamente para operações de **leitura sem regra de negócio** (ex: listagens com filtros simples)
- ❌ NÃO conter lógica de negócio
- ❌ NÃO acessar repositórios diretamente em operações de **escrita** — sempre via serviço de domínio
- ❌ NÃO usar UnitOfWork em consultas (Listar, Recuperar)

## Registro (IoC)

```csharp
services.AddScoped<I<Feature>AppServico, <Feature>AppServico>();
// Profile em ConfiguracoesAutoMapper:
config.AddProfile<<Feature>Profile>();
```
