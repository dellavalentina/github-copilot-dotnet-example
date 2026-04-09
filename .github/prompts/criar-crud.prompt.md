---
agent: agent
description: "Implementar CRUD completo de uma nova feature seguindo a arquitetura em camadas do CEFE"
tools:
  [
    vscode,
    read,
    agent,
    edit,
    search,
    web,
    "microsoftdocs/mcp/*",
    browser,
    vscode.mermaid-chat-features/renderMermaidDiagram,
    todo,
  ]
---

# Nova Feature — CRUD Completo

Implemente o CRUD completo para a feature **`{{feature}}`** seguindo rigorosamente a arquitetura em camadas do projeto.

## Contexto

- Leia primeiro a entidade existente (se houver) para entender campos e validações já implementados
- Siga a ordem de implementação abaixo para evitar erros de compilação

## Ordem de Implementação

### 1. DataTransfer (DTOs)

Criar em `<Projeto>.DataTransfer/{{feature}}/`:

- `Request/{{feature}}InserirRequest.cs` — campos obrigatórios da criação
- `Request/{{feature}}EditarRequest.cs` — `Id` + campos editáveis como nullable
- `Request/{{feature}}ListarRequest.cs` — herda `PaginacaoFiltro` + filtros específicos
- `Response/{{feature}}Response.cs` — espelha propriedades da entidade

**Proibido:** `System.ComponentModel.DataAnnotations` em qualquer DTO.

### 2. Domínio — Comandos

Criar em `<Projeto>.Dominio/{{feature}}/Servicos/Comandos/`:

- `{{feature}}InserirComando.cs`
- `{{feature}}EditarComando.cs`

### 3. Domínio — Interface e Serviço

Criar em `<Projeto>.Dominio/{{feature}}/Servicos/`:

- `Interfaces/I{{feature}}Servicos.cs` — métodos: `InserirAsync`, `EditarAsync`, `DesativarAsync`, `Recuperar`, `Listar`
- `{{feature}}Servicos.cs` — implementar com `I{{feature}}Repositorio`; método `Validar` privado usa `ValidarRegistroNaoFoiEncontrado`

### 4. Aplicação — Profile

Criar em `<Projeto>.Aplicacao/{{feature}}/Profiles/`:

- `{{feature}}Profile.cs` — mapear: `InserirRequest → InserirComando`, `EditarRequest → EditarComando`, `Entidade → Response`

### 5. Aplicação — Interface e AppServico

Criar em `<Projeto>.Aplicacao/{{feature}}/Servicos/`:

- `Interfaces/I{{feature}}AppServico.cs`
- `{{feature}}AppServico.cs` — `IUnitOfWork` obrigatório em operações de escrita

### 6. API — Controller

Criar em `<Projeto>.Api/Controllers/{{feature}}/`:

- `{{feature}}Controller.cs` — rota `api/{{feature-kebab}}`, `[Authorize]`, validar claims em todos os endpoints

### 7. IoC — Registros

Alterar `<Projeto>.Ioc/`:

- `ConfiguracoesInjecoesDependencia.cs` — registrar `I{{feature}}Servicos` e `I{{feature}}AppServico`
- `ConfiguracoesAutoMapper.cs` — adicionar `{{feature}}Profile`

## Checklist Final

- [ ] Sem DataAnnotations nos DTOs
- [ ] Validações de negócio nos setters da entidade (domínio)
- [ ] UnitOfWork em Inserir, Editar, Desativar/Excluir
- [ ] Endpoints com `[Authorize]` + validação de claims
- [ ] Exclusão via `Desativar()` (soft delete — não delete físico)
- [ ] Campos não editáveis ausentes do `EditarRequest`
- [ ] Build sem erros: `dotnet build <Projeto>.slnx`
