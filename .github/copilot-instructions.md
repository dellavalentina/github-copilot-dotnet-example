# Instruções do Copilot

> Padrões detalhados por camada estão em `.github/instructions/` e são aplicados automaticamente
> pelo VS Code conforme o tipo de arquivo aberto.
> Para implementar uma nova feature CRUD, use o prompt `.github/prompts/nova-feature.prompt.md`.

## Arquitetura em Camadas

```
┌───────────────────────────────────────────────────────────────┐
│                        <Projeto>.Api                          │
│   Controllers, Autenticação JWT, Swagger, Quartz,             │
│                       HostedService                           │
└───────────────────────────────────────────────────────────────┘
                                │
              ┌─────────────────┼─────────────────┐
              │                 │                 │
              ▼                 ▼                 ▼
┌──────────────────────┐ ┌──────────────────────┐ ┌────────────────────────────┐
│ <Projeto>.Aplicacao  │ │    <Projeto>.Jobs    │ │  <Projeto>.DataTransfer    │
│     AppServicos      │ │      IJob impls      │ │ DTOs (Request/Response)    │
└──────────────────────┘ └──────────────────────┘ └────────────────────────────┘
              │                 │
              └─────────────────┴┐
                                 ▼
                    ┌────────────────────────────┐
                    │     <Projeto>.Dominio      │
                    │ Entidades, Serviços,       │
                    │        Comandos            │
                    └────────────────────────────┘
                                │
                                ▼
                    ┌────────────────────────────┐
                    │      <Projeto>.Infra       │
                    │ Repositórios, Mapeamentos  │
                    │         EF Core            │
                    └────────────────────────────┘
                                │
                                ▼
                    ┌────────────────────────────┐
                    │       <Projeto>.Ioc        │
                    │ Injeção de Dependências,   │
                    │          Quartz            │
                    └────────────────────────────┘
```

## Arquitetura e Responsabilidades

- `<Projeto>.Api` hospeda os controllers ASP.NET Core; veja `<Projeto>.Api/Program.cs` para CORS, autenticação JWT, Swagger, registro de perfis AutoMapper, cache em memória e composição dos serviços.
- Os controllers chamam serviços de aplicação em `<Projeto>.Aplicacao` (pasta `Servicos`), que orquestram AutoMapper, serviços de domínio e infraestrutura (e-mail, S3 etc.).
- `<Projeto>.DataTransfer` guarda os DTOs por feature (`Request` herda de `PaginacaoFiltro`, `Response` espelha entidades) trocados com os clientes.
- A lógica de domínio vive em `<Projeto>.Dominio` com entidades, repositórios e comandos/filtros; os setters encapsulam invariantes.
- `<Projeto>.Infra` implementa repositórios e mapeamentos Entity Framework Core (mapeia colunas SQL Server via `IEntityTypeConfiguration<T>`) além da base reutilizável `RepositorioBase<T>`, e contém o `AppDbContext`.
- `<Projeto>.Ioc/ConfiguracoesDbContext.cs` configura o `AppDbContext` com SQL Server e `ConfiguracoesInjecoesDependencia` registra manualmente cada interface—estenda ao criar novos serviços ou repositórios.
- `<Projeto>.Jobs` contém os background jobs Quartz.NET (`IJob`), hospedados no processo da API via `AddQuartzHostedService`. Jobs injetam serviços de domínio e repositórios. A configuração Quartz está em `<Projeto>.Ioc/ConfiguracoesQuartz.cs`.

## Arquitetura e Responsabilidades

- `<Projeto>.Api` hospeda os controllers ASP.NET Core; veja `Program.cs` para CORS, autenticação JWT, Swagger, registro de perfis AutoMapper, cache em memória e composição dos serviços.
- Os controllers chamam serviços de aplicação em `<Projeto>.Aplicacao`, que orquestram AutoMapper, serviços de domínio e infraestrutura (e-mail, S3 etc.).
- `<Projeto>.DataTransfer` guarda os DTOs por feature (`Request` herda de `PaginacaoFiltro`, `Response` espelha entidades).
- A lógica de negócio vive em `<Projeto>.Dominio` com entidades, repositórios e comandos; os setters encapsulam invariantes.
- `<Projeto>.Infra` implementa repositórios e mapeamentos EF Core via `IEntityTypeConfiguration<T>` (coletados automaticamente por `ApplyConfigurationsFromAssembly`).
- `<Projeto>.Ioc` registra manualmente cada interface — estenda ao criar novos serviços ou repositórios.
- `<Projeto>.Jobs` contém background jobs Quartz.NET (`IJob`). Configuração em `ConfiguracoesQuartz.cs`.

## Nomenclatura

| Camada       | Padrão                    | Exemplo                     |
| ------------ | ------------------------- | --------------------------- |
| API          | `<Feature>Controller`     | `DepoimentosController`     |
| Aplicação    | `<Feature>AppServico`     | `DepoimentosAppServico`     |
| Aplicação    | `<Feature>Profile`        | `DepoimentosProfile`        |
| DataTransfer | `<Feature><Acao>Request`  | `DepoimentosInserirRequest` |
| DataTransfer | `<Feature>Response`       | `DepoimentosResponse`       |
| Domínio      | `<Entidade>` (singular)   | `Depoimento`                |
| Domínio      | `<Feature>Servicos`       | `DepoimentosServicos`       |
| Domínio      | `<Feature><Acao>Comando`  | `DepoimentosInserirComando` |
| Infra        | `<Feature>Repositorio`    | `DepoimentosRepositorio`    |
| Infra        | `<Entidade>Configuration` | `DepoimentoConfiguration`   |
| Jobs         | `<Acao/Descricao>Job`     | `ColetaMD50Job`             |

**Campos privados:** `_camelCase` | **Parâmetros/locais:** `camelCase`  
**Métodos CRUD:** verbos em português (`Inserir`, `Editar`, `Excluir`, `Recuperar`, `Listar`) com sufixo `Async`  
**Namespaces:** `<Projeto>.<Camada>.<Feature>.<Subcategoria>`

## Fluxo de Dados

```
HTTP Request → Controller → AppServico [AutoMapper] → Comando → Serviço Domínio → Entidade → Repositório [EF Core] → SQL Server
                                                                                    ↓
HTTP Response ← Controller ← AppServico [AutoMapper] ←────────────── Response ←───┘
```

## Regras Absolutas

- ⛔ **DataAnnotations PROIBIDO** em DTOs — validações no Domínio via `RegraDeNegocioExcecao` e `ValidarRegistroNaoFoiEncontrado`
- ⛔ **NÃO** acessar repositório diretamente do controller
- ⛔ **NÃO** acessar repositório diretamente do AppServico em operações de **escrita** — sempre via serviço de domínio
- ⛔ **NÃO** colocar lógica de negócio no AppServico
- ⛔ **NÃO** expor entidades de domínio na API
- ✅ AppServico **pode** acessar `I<Feature>Repositorio` diretamente em operações de **leitura sem regra de negócio** (ex: listagens com filtros simples)
- ✅ **UnitOfWork** obrigatório em toda operação de escrita (Inserir, Editar, Excluir/Desativar)
- ✅ **Soft delete** via `Desativar()` — sem delete físico
- ✅ Todo novo par interface/classe registrado em `ConfiguracoesInjecoesDependencia` e todo `Profile` em `ConfiguracoesAutoMapper`
- ✅ Segredos (SQL Server, JWT, SMTP, AWS) via `dotnet user-secrets` ou variáveis de ambiente — nunca em JSON commitado

## Serviços Transversais

- **JWT:** chaves em secrets/variáveis de ambiente
- **E-mail:** `EmailService` com `EmailSettings` + `AppSettings.FrontendUrl`
- **Arquivos/S3:** `S3Service` (AWS SDK) — fornece key, devolve URL pré-assinada
- **Build:** `dotnet build <Projeto>.slnx` | **Run:** `dotnet run --project <Projeto>.Api/<Projeto>.Api.csproj`
