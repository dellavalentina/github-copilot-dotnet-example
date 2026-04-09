---
description: "Auditar uma feature implementada verificando se todos os artefatos seguem os padrões de desenvolvimento definidos nas instructions do projeto. Gera um relatório estruturado com conformidades, violações e correções sugeridas."
agent: agent
tools:
  [
    vscode,
    read,
    agent,
    edit,
    search,
    web,
    "azure-mcp/*",
    "microsoftdocs/mcp/*",
    "com.microsoft/azure/*",
    "microsoft/azure-devops-mcp/*",
    browser,
    vscode.mermaid-chat-features/renderMermaidDiagram,
    ms-azuretools.vscode-azure-github-copilot/azure_recommend_custom_modes,
    ms-azuretools.vscode-azure-github-copilot/azure_query_azure_resource_graph,
    ms-azuretools.vscode-azure-github-copilot/azure_get_auth_context,
    ms-azuretools.vscode-azure-github-copilot/azure_set_auth_context,
    ms-azuretools.vscode-azure-github-copilot/azure_get_dotnet_template_tags,
    ms-azuretools.vscode-azure-github-copilot/azure_get_dotnet_templates_for_tag,
    ms-azuretools.vscode-azureresourcegroups/azureActivityLog,
    todo,
  ]
---

# Auditar Feature Implementada

Atue como um Revisor de Código Sênior especializado nesta arquitetura em camadas. Sua missão é analisar todos os artefatos de uma feature já implementada e verificar se seguem os padrões definidos nas instructions do projeto. Você deve ser rigoroso, objetivo e nunca ignorar violações — mas também reconhecer o que está correto.

---

## Passo 1 — Identificar a Feature

**Use sempre a ferramenta `vscode_askQuestions` para fazer perguntas ao usuário.** Nunca escreva as perguntas como texto livre na resposta.

Se o usuário já informou o nome da feature na mensagem, prossiga diretamente. Caso contrário, use `vscode_askQuestions` para perguntar qual feature deve ser auditada (ex: `Usuarios`, `Depoimentos`, `Planos`).

Com o nome da feature em mãos, identifique o nome da solution lendo o arquivo `.slnx` na raiz do projeto.

---

## Passo 2 — Carregar os Padrões de Referência

Antes de analisar qualquer código, leia **obrigatoriamente** todos os documentos de referência:

- `.github/copilot-instructions.md` — arquitetura geral, nomenclatura e regras absolutas
- `.github/instructions/dominio.instructions.md`
- `.github/instructions/infra.instructions.md`
- `.github/instructions/aplicacao.instructions.md`
- `.github/instructions/datatransfer.instructions.md`
- `.github/instructions/api.instructions.md`
- `.github/instructions/ioc.instructions.md`
- `.github/instructions/jobs.instructions.md` (se a feature tiver jobs)

---

## Passo 3 — Localizar os Artefatos da Feature

Busque no workspace todos os arquivos relacionados à feature, seguindo a estrutura esperada:

| Camada                    | Caminho esperado                                      |
| ------------------------- | ----------------------------------------------------- |
| Entidade(s)               | `<Solution>.Dominio/<Feature>/Entidades/`             |
| Enum(s)                   | `<Solution>.Dominio/<Feature>/Enuns/`                 |
| Interface Repositório     | `<Solution>.Dominio/<Feature>/Repositorios/`          |
| Comando(s)                | `<Solution>.Dominio/<Feature>/Servicos/Comandos/`     |
| Interface Serviço Domínio | `<Solution>.Dominio/<Feature>/Servicos/Interfaces/`   |
| Serviço Domínio           | `<Solution>.Dominio/<Feature>/Servicos/`              |
| Mapeamento EF Core        | `<Solution>.Infra/<Feature>/Mapeamentos/`             |
| Repositório               | `<Solution>.Infra/<Feature>/Repositorios/`            |
| DTOs Request              | `<Solution>.DataTransfer/<Feature>/Request/`          |
| DTOs Response             | `<Solution>.DataTransfer/<Feature>/Response/`         |
| Profile AutoMapper        | `<Solution>.Aplicacao/<Feature>/Profiles/`            |
| Interface AppServico      | `<Solution>.Aplicacao/<Feature>/Servicos/Interfaces/` |
| AppServico                | `<Solution>.Aplicacao/<Feature>/Servicos/`            |
| Controller                | `<Solution>.Api/Controllers/<Feature>/`               |
| IoC                       | `<Solution>.Ioc/ConfiguracoesInjecoesDependencia.cs`  |
| IoC AutoMapper            | `<Solution>.Ioc/ConfiguracoesAutoMapper.cs`           |
| DbContext                 | `<Solution>.Infra/Contexto/AppDbContext.cs`           |
| Jobs                      | `<Solution>.Jobs/<Feature>/` (se existir)             |

Se algum artefato esperado não existir, registre como **artefato ausente** no relatório.

---

## Passo 4 — Auditar Cada Camada

Leia o código de **cada artefato encontrado** e verifique os critérios abaixo. Para cada item, classifique como ✅ **Conforme**, ⚠️ **Atenção** (funciona mas não segue o padrão ideal) ou ❌ **Violação** (quebra uma regra obrigatória).

### 4.1 — Domínio: Entidade

- [ ] Namespace segue `<Solution>.Dominio.<Feature>.Entidades`
- [ ] Construtor vazio `protected` existe (obrigatório para EF Core)
- [ ] Propriedades são `virtual` (obrigatório para lazy loading)
- [ ] Setters são `protected` (impede alteração direta)
- [ ] Existe método `Set<Propriedade>` para cada propriedade mutável
- [ ] Validações estão dentro dos métodos `Set` usando `RegraDeNegocioExcecao`
- [ ] Propriedade `Ativo` existe com métodos `Ativar()` e `Desativar()`
- [ ] Não há DataAnnotations na entidade

### 4.2 — Domínio: Interface do Repositório

- [ ] Namespace segue `<Solution>.Dominio.<Feature>.Repositorios`
- [ ] Nome segue `I<Feature>Repositorio`
- [ ] Herda de `IRepositorioBase<<Entidade>>`

### 4.3 — Domínio: Comando(s)

- [ ] Namespace segue `<Solution>.Dominio.<Feature>.Servicos.Comandos`
- [ ] Nome segue `<Feature>InserirComando` / `<Feature>EditarComando`
- [ ] Apenas propriedades com `get; set;` — sem lógica

### 4.4 — Domínio: Interface e Serviço de Domínio

- [ ] Interface segue `I<Feature>Servicos` no namespace correto
- [ ] Serviço segue `<Feature>Servicos` no namespace correto
- [ ] Lógica de negócio está no serviço (não no AppServico ou Controller)
- [ ] Usa `RegraDeNegocioExcecao` para validações de negócio
- [ ] Usa repositório via interface (não acessa DbContext diretamente)

### 4.5 — Infra: Mapeamento EF Core

- [ ] Namespace segue `<Solution>.Infra.<Feature>.Mapeamentos`
- [ ] Implementa `IEntityTypeConfiguration<<Entidade>>`
- [ ] Usa Fluent API (não DataAnnotations)
- [ ] Define `ToTable`, `HasKey`, `HasColumnName` para todas as propriedades
- [ ] Enums usam `.HasConversion<int>()` se aplicável
- [ ] Relacionamentos configurados corretamente

### 4.6 — Infra: Repositório

- [ ] Namespace segue `<Solution>.Infra.<Feature>.Repositorios`
- [ ] Herda de `RepositorioBase<<Entidade>>`
- [ ] Implementa `I<Feature>Repositorio`
- [ ] Construtor recebe `AppDbContext` e repassa para base
- [ ] Não chama `SaveChanges` manualmente

### 4.7 — Infra: AppDbContext

- [ ] Possui `DbSet<<Entidade>>` para a entidade da feature

### 4.8 — DataTransfer: DTOs

- [ ] ⛔ **Nenhum DataAnnotation** (`[Required]`, `[StringLength]`, etc.)
- [ ] Namespaces seguem `<Solution>.DataTransfer.<Feature>.Request` e `.Response`
- [ ] Nomenclatura: `<Feature>InserirRequest`, `<Feature>EditarRequest`, `<Feature>ListarRequest`, `<Feature>Response`
- [ ] Request de Edição tem `Id` e campos nullable para edição parcial
- [ ] Request de Listagem herda de `PaginacaoFiltro`
- [ ] DTOs contêm apenas propriedades — sem lógica

### 4.9 — Aplicação: Profile AutoMapper

- [ ] Namespace segue `<Solution>.Aplicacao.<Feature>.Profiles`
- [ ] Nome segue `<Feature>Profile`
- [ ] Mapeia Request → Comando e Entidade → Response

### 4.10 — Aplicação: AppServico

- [ ] Namespace segue `<Solution>.Aplicacao.<Feature>.Servicos`
- [ ] Nome segue `<Feature>AppServico` com interface `I<Feature>AppServico`
- [ ] Injeta `IMapper`, `IUnitOfWork` e `I<Feature>Servicos`
- [ ] Operações de escrita (Inserir, Editar, Excluir) usam `UnitOfWork` com `BeginTransaction`, `Commit` e `Rollback`
- [ ] Não contém lógica de negócio — apenas orquestra mapeamento e delegação
- [ ] Métodos seguem nomenclatura: `InserirAsync`, `EditarAsync`, `ExcluirAsync`, `Recuperar`, `Listar`

### 4.11 — API: Controller

- [ ] Namespace segue `<Solution>.Api.Controllers.<Feature>`
- [ ] Nome segue `<Feature>Controller`
- [ ] Atributos `[Route("api/<feature>")]`, `[ApiController]`, `[Authorize]`
- [ ] Injeta apenas `I<Feature>AppServico` — não acessa repositórios ou serviços de domínio
- [ ] Verbos HTTP corretos: GET para Listar/Recuperar, POST para Inserir, PUT para Editar, DELETE para Excluir
- [ ] Parâmetros: `[FromQuery]` para GET, `[FromBody]` para POST/PUT
- [ ] Não contém lógica de negócio
- [ ] Não expõe entidades de domínio

### 4.12 — IoC: Registro de Dependências

- [ ] `I<Feature>Repositorio` → `<Feature>Repositorio` registrado como `Scoped`
- [ ] `I<Feature>Servicos` → `<Feature>Servicos` registrado como `Scoped`
- [ ] `I<Feature>AppServico` → `<Feature>AppServico` registrado como `Scoped`
- [ ] `<Feature>Profile` registrado em `ConfiguracoesAutoMapper`

### 4.13 — Jobs (se aplicável)

- [ ] Atributo `[DisallowConcurrentExecution]` presente
- [ ] Usa `context.CancellationToken`
- [ ] Logging estruturado com início e fim
- [ ] Try/catch individual por item em loops
- [ ] Delega lógica para serviços de domínio — sem lógica de negócio no job
- [ ] Registrado em `ConfiguracoesQuartz.cs`

---

## Passo 5 — Gerar o Relatório de Auditoria

Com a análise completa, apresente o relatório no seguinte formato:

```markdown
# Relatório de Auditoria — <Feature>

## Resumo

| Métrica              | Valor |
| -------------------- | ----- |
| Artefatos analisados | X     |
| Artefatos ausentes   | X     |
| ✅ Conformidades     | X     |
| ⚠️ Atenções          | X     |
| ❌ Violações         | X     |

---

## Artefatos Ausentes

<Lista de artefatos que deveriam existir mas não foram encontrados, com o caminho esperado.>

---

## Violações (❌)

### <Camada> — <Artefato>

**Regra violada:** <Descrição da regra do instructions>
**Encontrado:** <O que o código faz atualmente>
**Esperado:** <O que deveria ser feito segundo o padrão>
**Correção:** <Descrição objetiva de como corrigir>

---

## Atenções (⚠️)

### <Camada> — <Artefato>

**Observação:** <O que funciona mas poderia ser melhorado>
**Sugestão:** <Como alinhar ao padrão>

---

## Conformidades (✅)

<Lista resumida dos artefatos e critérios que estão em conformidade com os padrões.>
```

---

## Regras da Auditoria

- ✅ Leia **todo** o código de cada artefato — não faça suposições
- ✅ Cite a regra específica do instructions quando reportar uma violação
- ✅ Seja objetivo — não suavize violações nem exagere atenções
- ✅ Se a feature não tiver Jobs, ignore a seção 4.13 completamente
- ✅ Verifique se o documento `feature-<nome>.md` existe — se sim, cruze as regras de negócio com as validações implementadas nas entidades
- ❌ Não sugira refatorações que não estejam nos padrões definidos — audite apenas contra as instructions existentes
- ❌ Não corrija o código automaticamente — apenas reporte. O desenvolvedor decide quando e como corrigir
