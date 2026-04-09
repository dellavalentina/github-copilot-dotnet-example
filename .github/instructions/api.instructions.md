---
applyTo: "**/Controllers/**/*.cs"
---

# Padrões da Camada API (<Projeto>.Api)

## Responsabilidade

Expor endpoints HTTP, receber requisições, delegar para serviços de aplicação e retornar respostas formatadas.

## Estrutura de Pastas

```
<Projeto>.Api/
├── Controllers/
│   └── <Feature>/
│       └── <Feature>Controller.cs
├── Autenticacoes/
│   └── TokenServicos.cs
├── Program.cs
└── appsettings.json
```

## Nomenclatura

| Elemento   | Padrão                              | Exemplo                                               |
| ---------- | ----------------------------------- | ----------------------------------------------------- |
| Controller | `<Feature>Controller`               | `DepoimentosController`, `PlanosController`           |
| Métodos    | Verbos em português                 | `Listar`, `Inserir`, `Editar`, `Excluir`, `Recuperar` |
| Rotas      | `api/<feature>` (plural, minúsculo) | `api/depoimentos`, `api/planos`                       |

## Padrões de Código

### Controller Base

```csharp
[Route("api/<feature>")]
[ApiController]
[Authorize]
public class <Feature>Controller : ControllerBase
{
    private readonly I<Feature>AppServico _<feature>AppServico;

    public <Feature>Controller(I<Feature>AppServico <feature>AppServico)
    {
        _<feature>AppServico = <feature>AppServico;
    }
}
```

### Métodos HTTP

| Ação      | Verbo HTTP | Atributo               | Parâmetro     |
| --------- | ---------- | ---------------------- | ------------- |
| Listar    | GET        | `[HttpGet]`            | `[FromQuery]` |
| Recuperar | GET        | `[HttpGet("{id}")]`    | `int id`      |
| Inserir   | POST       | `[HttpPost]`           | `[FromBody]`  |
| Editar    | PUT        | `[HttpPut]`            | `[FromBody]`  |
| Excluir   | DELETE     | `[HttpDelete("{id}")]` | `int id`      |

### Exemplo Completo

```csharp
[HttpPost]
public async Task<ActionResult<<Feature>Response>> Inserir([FromBody] <Feature>InserirRequest request)
{
    var response = await _<feature>AppServico.InserirAsync(request);
    return Ok(response);
}

[HttpGet]
public ActionResult<PaginacaoConsulta<<Feature>Response>> Listar([FromQuery] <Feature>ListarRequest request)
{
    var response = _<feature>AppServico.Listar(request);
    return Ok(response);
}

[HttpGet("{id}")]
public ActionResult<<Feature>Response> Recuperar(int id)
{
    var response = _<feature>AppServico.Recuperar(id);
    return Ok(response);
}

[HttpPut]
public async Task<ActionResult<<Feature>Response>> Editar([FromBody] <Feature>EditarRequest request)
{
    var response = await _<feature>AppServico.EditarAsync(request);
    return Ok(response);
}

[HttpDelete("{id}")]
public async Task<ActionResult> Excluir(int id)
{
    await _<feature>AppServico.ExcluirAsync(id);
    return Ok();
}
```

### Validação de Usuário Autenticado

```csharp
var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier);
if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out var userId))
    return Unauthorized();
```

## Atributos

- `[Authorize]` — Endpoints que requerem JWT
- `[AllowAnonymous]` — Login, recuperação de senha, endpoints públicos
- `[ApiController]` — Validação automática de ModelState

## Retornos Padrão

- `Ok(response)` — 200 com dados
- `Unauthorized()` — 401 sem autenticação
- `NotFound()` — 404 registro não encontrado
- `BadRequest(errors)` — 400 validação falhou

## Regras

- ❌ NÃO colocar lógica de negócio nos controllers
- ❌ NÃO acessar repositórios diretamente
- ❌ NÃO retornar entidades do domínio
- ✅ Sempre delegar para AppServicos
- ✅ Usar DTOs (Request/Response) para entrada/saída
- ✅ Validar claims do JWT quando necessário
