---
applyTo: "**/Entidades/*.cs,**/Servicos/**/*.cs,**/Repositorios/I*.cs"
---

# Padrões da Camada Domínio (<Projeto>.Dominio)

## Responsabilidade

Conter a lógica de negócio, entidades, repositórios (interfaces) e regras de validação.

## Estrutura de Pastas

```
<Projeto>.Dominio/
├── libs/
│   ├── Consultas/
│   ├── Entidades/
│   ├── Excecoes/
│   ├── Extensions/
│   ├── Filtros/
│   ├── Repositorios/
│   └── UnitOfWork/
└── <Feature>/
    ├── Entidades/
    │   └── <Entidade>.cs
    ├── Repositorios/
    │   └── I<Feature>Repositorio.cs
    └── Servicos/
        ├── Comandos/
        │   ├── <Feature>InserirComando.cs
        │   └── <Feature>EditarComando.cs
        ├── Filtros/
        │   └── <Feature>ListarFiltro.cs
        ├── Consultas/
        │   └── <Integracao><Recurso>Consulta.cs
        ├── Interfaces/
        │   └── I<Feature>Servicos.cs
        └── <Feature>Servicos.cs
```

## Nomenclatura

| Elemento               | Padrão                          | Exemplo                          |
| ---------------------- | ------------------------------- | -------------------------------- |
| Entidade               | Singular, PascalCase            | `Depoimento`, `Plano`, `Usuario` |
| Interface Repositório  | `I<Feature>Repositorio`         | `IDepoimentosRepositorio`        |
| Serviço                | `<Feature>Servicos`             | `DepoimentosServicos`            |
| Interface Serviço      | `I<Feature>Servicos`            | `IDepoimentosServicos`           |
| Comando Inserir        | `<Feature>InserirComando`       | `DepoimentosInserirComando`      |
| Comando Editar         | `<Feature>EditarComando`        | `DepoimentosEditarComando`      |
| Filtro Listar          | `<Feature>ListarFiltro`         | `DepoimentosListarFiltro`        |
| Consulta de Integração | `<Integracao><Recurso>Consulta` | `NorteboxPlantaConsulta`         |

## Padrões de Código

### Entidade

```csharp
namespace <Projeto>.Dominio.<Feature>.Entidades;

public class <Entidade>
{
    public virtual int Id { get; protected set; }
    public virtual string Nome { get; protected set; }
    public virtual bool Ativo { get; protected set; }

    protected <Entidade>() { } // Obrigatório para EF Core instanciar

    public <Entidade>(string nome)
    {
        SetNome(nome);
        Ativo = true;
    }

    public virtual void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new RegraDeNegocioExcecao("Nome é obrigatório");
        Nome = nome;
    }

    public virtual void Ativar() => Ativo = true;
    public virtual void Desativar() => Ativo = false;
}
```

### Regras de Entidade (OBRIGATÓRIAS)

| Regra                        | Motivo                                               |
| ---------------------------- | ---------------------------------------------------- |
| Construtor vazio `protected` | EF Core precisa instanciar ao hidratar do banco      |
| Propriedades `virtual`       | Necessário para lazy loading do EF Core              |
| Setters `protected`          | Impede alteração direta; força uso dos métodos `Set` |
| Método `Set<Propriedade>`    | **OBRIGATÓRIO** para toda propriedade mutável        |
| Validações dentro dos `Set`  | Centraliza invariantes de negócio                    |

### Interface do Repositório

```csharp
namespace <Projeto>.Dominio.<Feature>.Repositorios;

public interface I<Feature>Repositorio : IRepositorioBase<<Entidade>>
{
    // Métodos específicos da feature, se necessário
}
```

### Interface do Serviço

```csharp
namespace <Projeto>.Dominio.<Feature>.Servicos.Interfaces;

public interface I<Feature>Servicos
{
    Task<<Entidade>> InserirAsync(<Feature>InserirComando comando, CancellationToken ct = default);
    Task<<Entidade>> EditarAsync(<Feature>EditarComando comando, CancellationToken ct = default);
    Task ExcluirAsync(int id, CancellationToken ct = default);
    <Entidade> Recuperar(int id);
    PaginacaoConsulta<<Entidade>> Listar(<Feature>ListarFiltro filtro);
}
```

### Comando

```csharp
namespace <Projeto>.Dominio.<Feature>.Servicos.Comandos;

public class <Feature>InserirComando
{
    public string Nome { get; set; }
    public string? CampoOpcional { get; set; }
}
```

### Filtro de Listagem

```csharp
namespace <Projeto>.Dominio.<Feature>.Servicos.Filtros;

public class <Feature>ListarFiltro : PaginacaoFiltro
{
    // PaginacaoFiltro fornece: Qt (10), Pg (1), CpOrd, TpOrd
    public string? Nome { get; set; }
    public bool? Ativo { get; set; }
}
```

### Consulta de Integração (leitura externa)

```csharp
namespace <Projeto>.Dominio.<Feature>.Servicos.Consultas;

// Dados normalizados de integrações externas — sem atributos de serialização
public class <Integracao><Recurso>Consulta
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}
```

### Serviço de Domínio

```csharp
namespace <Projeto>.Dominio.<Feature>.Servicos;

public class <Feature>Servicos : I<Feature>Servicos
{
    private readonly I<Feature>Repositorio _<feature>Repositorio;

    public <Feature>Servicos(I<Feature>Repositorio <feature>Repositorio)
    {
        _<feature>Repositorio = <feature>Repositorio;
    }

    public async Task<<Entidade>> InserirAsync(<Feature>InserirComando comando, CancellationToken ct = default)
    {
        var entidade = new <Entidade>(comando.Nome);
        await _<feature>Repositorio.InserirAsync(entidade, ct);
        return entidade;
    }

    public async Task<<Entidade>> EditarAsync(<Feature>EditarComando comando, CancellationToken ct = default)
    {
        var entidade = Validar(comando.Id);
        if (comando.Nome != null) entidade.SetNome(comando.Nome);
        await _<feature>Repositorio.EditarAsync(entidade, ct);
        return entidade;
    }

    public async Task ExcluirAsync(int id, CancellationToken ct = default)
    {
        var entidade = Validar(id);
        entidade.Desativar(); // soft delete — nunca delete físico
        await _<feature>Repositorio.EditarAsync(entidade, ct);
    }

    public <Entidade> Recuperar(int id) => Validar(id);

    public PaginacaoConsulta<<Entidade>> Listar(<Feature>ListarFiltro filtro)
    {
        var query = _<feature>Repositorio.Query();
        // Aplicar filtros opcionais via LINQ
        if (filtro.Ativo.HasValue) query = query.Where(x => x.Ativo == filtro.Ativo.Value);
        var total = query.Count();
        var registros = query
            .Skip((filtro.Pg - 1) * filtro.Qt)
            .Take(filtro.Qt)
            .ToList();
        return new PaginacaoConsulta<<Entidade>> { Registros = registros, Total = total };
    }

    private <Entidade> Validar(int id)
    {
        var entidade = _<feature>Repositorio.Recuperar(id);
        entidade.ValidarRegistroNaoFoiEncontrado("<Entidade> não encontrada");
        return entidade;
    }
}
```

## Regras Importantes

- Comandos são **exclusivos para escrita** (Inserir, Editar). Leituras de integrações usam Consultas
- Filtros de listagem ficam em `Servicos/Filtros/` e herdam de `PaginacaoFiltro` (nunca usar tipo de DataTransfer no Domínio)
- **Excluir = soft delete:** chamar `Desativar()` + `_repositorio.EditarAsync()` — nunca `_repositorio.ExcluirAsync()`
- **Validação de negócio:** `throw new RegraDeNegocioExcecao("Mensagem")`
- **Registro não encontrado:** `entidade.ValidarRegistroNaoFoiEncontrado("Mensagem")`
- Filtros e paginação aplicados via `IQueryable<T>` do `RepositorioBase<T>.Query()`
