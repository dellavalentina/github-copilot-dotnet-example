---
applyTo: "**/*Request.cs,**/*Response.cs"
---

# Padrões da Camada DataTransfer (<Projeto>.DataTransfer)

## Responsabilidade

Definir os contratos de entrada e saída da API (DTOs) isolados das entidades de domínio.

## ⛔ PROIBIÇÃO ABSOLUTA — DataAnnotations

**É EXPRESSAMENTE PROIBIDO** usar `System.ComponentModel.DataAnnotations` nos DTOs.

```csharp
// ❌ PROIBIDO
[Required] [StringLength(100)] public string Campo { get; set; }

// ✅ CORRETO — DTO simples, validação no Domínio
public string Campo { get; set; }
```

Todas as validações devem estar nos **setters e construtores das entidades de domínio**, lançando `RegraDeNegocioExcecao`.

## Estrutura de Pastas

```
<Projeto>.DataTransfer/
└── <Feature>/
    ├── Request/
    │   ├── <Feature>InserirRequest.cs
    │   ├── <Feature>EditarRequest.cs
    │   └── <Feature>ListarRequest.cs
    └── Response/
        └── <Feature>Response.cs
```

## Nomenclatura

| Elemento        | Padrão                    | Exemplo                     |
| --------------- | ------------------------- | --------------------------- |
| Request Inserir | `<Feature>InserirRequest` | `DepoimentosInserirRequest` |
| Request Editar  | `<Feature>EditarRequest`  | `DepoimentosEditarRequest`  |
| Request Listar  | `<Feature>ListarRequest`  | `DepoimentosListarRequest`  |
| Response        | `<Feature>Response`       | `DepoimentosResponse`       |

## Padrões de Código

### Request de Inserção

```csharp
namespace <Projeto>.DataTransfer.<Feature>.Request;

public class <Feature>InserirRequest
{
    public string Campo { get; set; }
    public string? CampoOpcional { get; set; }
}
```

### Request de Edição

```csharp
namespace <Projeto>.DataTransfer.<Feature>.Request;

public class <Feature>EditarRequest
{
    public int Id { get; set; }
    public string? Campo { get; set; } // Nullable para edição parcial
}
```

### Request de Listagem (com Paginação)

```csharp
namespace <Projeto>.DataTransfer.<Feature>.Request;

public class <Feature>ListarRequest : PaginacaoFiltro
{
    // PaginacaoFiltro fornece: Qt (10), Pg (1), CpOrd, TpOrd
    public string? Nome { get; set; }
    public bool? Ativo { get; set; }
}
```

### Response

```csharp
namespace <Projeto>.DataTransfer.<Feature>.Response;

public class <Feature>Response
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set; }
}
```

## Regras

- Herdar de `PaginacaoFiltro` para listagens paginadas
- Campos opcionais devem ser nullable (`?`)
- Sem DataAnnotations — sem exceções
- Sem lógica — apenas propriedades com `get; set;`
- Enums podem ser expostos diretamente nos responses
