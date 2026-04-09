# Cards de Desenvolvimento — GerenciamentoUsuarios / Gerenciamento de Usuários

> **Leia antes de executar qualquer card:**
>
> - `.github/copilot-instructions.md` — arquitetura, nomenclatura e regras absolutas do projeto
> - `.github/feature-gerenciamento-usuarios.md` — descrição completa da feature, regras de negócio e critérios de aceite
> - `.github/instructions/<camada>.instructions.md` — indicado em cada card
>
> Execute os cards na **ordem numérica**. Cada card depende dos anteriores indicados.
> Não escreva código fora do que a especificação do card pede — siga os padrões das instructions.

---

## Card 0 — Setup da Solution

**Camada:** Infra / Domínio (Setup geral)
**Instruction:** `.github/copilot-instructions.md`
**Depende de:** Nenhum

### Contexto

Este é o ponto de partida do projeto. Antes de qualquer feature ser implementada, a solution e todos os projetos precisam existir com as classes base compartilhadas que todas as features irão reutilizar.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `GerenciamentoUsuarios.slnx` | Raiz da solution |
| `GerenciamentoUsuarios.Api.csproj` | `GerenciamentoUsuarios.Api/` |
| `GerenciamentoUsuarios.Aplicacao.csproj` | `GerenciamentoUsuarios.Aplicacao/` |
| `GerenciamentoUsuarios.DataTransfer.csproj` | `GerenciamentoUsuarios.DataTransfer/` |
| `GerenciamentoUsuarios.Dominio.csproj` | `GerenciamentoUsuarios.Dominio/` |
| `GerenciamentoUsuarios.Infra.csproj` | `GerenciamentoUsuarios.Infra/` |
| `GerenciamentoUsuarios.Ioc.csproj` | `GerenciamentoUsuarios.Ioc/` |
| `GerenciamentoUsuarios.Jobs.csproj` | `GerenciamentoUsuarios.Jobs/` |
| `RegraDeNegocioExcecao.cs` | `GerenciamentoUsuarios.Dominio/libs/Excecoes/` |
| `ValidarRegistroNaoFoiEncontrado.cs` | `GerenciamentoUsuarios.Dominio/libs/Excecoes/` |
| `PaginacaoFiltro.cs` | `GerenciamentoUsuarios.Dominio/libs/Filtros/` |
| `PaginacaoConsulta.cs` | `GerenciamentoUsuarios.Dominio/libs/Consultas/` |
| `IRepositorioBase.cs` | `GerenciamentoUsuarios.Dominio/libs/Repositorios/` |
| `IUnitOfWork.cs` | `GerenciamentoUsuarios.Dominio/libs/UnitOfWork/` |
| `RepositorioBase.cs` | `GerenciamentoUsuarios.Infra/Comum/Repositorios/` |
| `UnitOfWork.cs` | `GerenciamentoUsuarios.Infra/Comum/UnitOfWork/` |
| `AppDbContext.cs` | `GerenciamentoUsuarios.Infra/Contexto/` |
| `ConfiguracoesInjecoesDependencia.cs` | `GerenciamentoUsuarios.Ioc/` |
| `ConfiguracoesDbContext.cs` | `GerenciamentoUsuarios.Ioc/` |
| `ConfiguracoesAutoMapper.cs` | `GerenciamentoUsuarios.Ioc/` |

### Especificação

**Target framework:** `net10.0` em todos os projetos.

**Referências entre projetos:**

| Projeto | Referencia |
| --- | --- |
| `GerenciamentoUsuarios.Api` | `Aplicacao`, `DataTransfer`, `Ioc` |
| `GerenciamentoUsuarios.Aplicacao` | `Dominio`, `DataTransfer` |
| `GerenciamentoUsuarios.DataTransfer` | `Dominio` |
| `GerenciamentoUsuarios.Infra` | `Dominio` |
| `GerenciamentoUsuarios.Ioc` | `Dominio`, `Infra`, `Aplicacao` |
| `GerenciamentoUsuarios.Jobs` | `Dominio`, `Ioc` |
| `GerenciamentoUsuarios.Dominio` | Nenhum |

**Pacotes NuGet obrigatórios:**

| Projeto | Pacote |
| --- | --- |
| `GerenciamentoUsuarios.Api` | `Microsoft.AspNetCore.Authentication.JwtBearer` |
| `GerenciamentoUsuarios.Api` | `Swashbuckle.AspNetCore` |
| `GerenciamentoUsuarios.Infra` | `Microsoft.EntityFrameworkCore.SqlServer` |
| `GerenciamentoUsuarios.Infra` | `Microsoft.EntityFrameworkCore.Tools` |
| `GerenciamentoUsuarios.Infra` | `BCrypt.Net-Next` |
| `GerenciamentoUsuarios.Aplicacao` | `AutoMapper` |
| `GerenciamentoUsuarios.Ioc` | `AutoMapper` |

**Comportamentos dos artefatos base:**

- `RegraDeNegocioExcecao` — exceção customizada lançada quando uma regra de negócio é violada. Herda de `Exception`. Recebe a mensagem de erro no construtor.
- `ValidarRegistroNaoFoiEncontrado` — classe utilitária estática com método que recebe um objeto (entidade) e lança `RegraDeNegocioExcecao` com mensagem padrão quando o objeto é nulo.
- `PaginacaoFiltro` — classe base para filtros de listagem, fornecendo as propriedades: `Qt` (quantidade por página, padrão 10), `Pg` (número da página, padrão 1), `CpOrd` (campo de ordenação, string opcional), `TpOrd` (tipo de ordenação, string opcional).
- `PaginacaoConsulta<T>` — classe genérica que representa o resultado de uma listagem paginada. Contém: `Lista` (coleção do tipo T), `Total` (total de registros), `Qt` (tamanho da página), `Pg` (página atual).
- `IRepositorioBase<T>` — interface genérica com os métodos síncronos e assíncronos de CRUD: `Inserir`, `Editar`, `Excluir`, `Recuperar` (por id e por expressão), `Query` (retorna IQueryable<T>), e variantes Async com `CancellationToken`.
- `IUnitOfWork` — interface com os métodos `BeginTransactionAsync`, `CommitAsync`, `RollbackAsync`.
- `RepositorioBase<T>` — implementação de `IRepositorioBase<T>` usando `AppDbContext`. Recebe `AppDbContext` no construtor. Implementa todos os métodos da interface.
- `UnitOfWork` — implementação de `IUnitOfWork` usando a transaction do `AppDbContext`.
- `AppDbContext` — contexto EF Core vazio. Herda de `DbContext`. No método `OnModelCreating`, chama `ApplyConfigurationsFromAssembly` para descobrir todos os mapeamentos automaticamente. Não declara nenhum `DbSet` ainda.
- `ConfiguracoesDbContext` — método de extensão `AddDbContext(IConfiguration)` que registra o `AppDbContext` com SQL Server. A connection string é lida de `configuration["ConnectionStrings:Default"]`.
- `ConfiguracoesInjecoesDependencia` — método de extensão `AddInjecoesDependencia(IServiceCollection)` vazio por enquanto. Registra `IUnitOfWork` como Scoped para `UnitOfWork`.
- `ConfiguracoesAutoMapper` — método de extensão `AddAutoMapper(IServiceCollection)` que cria o `MapperConfiguration` com lista de profiles ainda vazia. Registra o mapper como Singleton.

### Restrições

- ⛔ Não implementar nenhuma lógica de feature neste card — apenas a estrutura base
- ⛔ Não adicionar DbSet de nenhuma entidade no `AppDbContext` ainda — será feito no Card 11
- ⛔ Não registrar dependências de feature no IoC ainda — será feito no Card 23

### Critério de conclusão

O card está concluído quando:

- [ ] A solution `.slnx` foi criada com todos os 7 projetos
- [ ] Todas as referências entre projetos estão corretas
- [ ] Todos os pacotes NuGet listados estão instalados
- [ ] Todos os artefatos base foram criados nos caminhos corretos
- [ ] `dotnet build GerenciamentoUsuarios.slnx` compila sem erros

---

## Card 1 — Enum PerfilUsuario

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Card 0

### Contexto

O sistema distingue dois tipos de usuário — Usuário Comum e Administrador — com permissões diferentes. Esse enum representa o perfil de cada conta e é armazenado junto com os dados do usuário.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `PerfilUsuario.cs` | `GerenciamentoUsuarios.Dominio/Usuarios/Enums/` |

### Especificação

**Valores do enum:**

| Nome | Valor numérico |
| --- | --- |
| `UsuarioComum` | 1 |
| `Administrador` | 2 |

**Namespace:** `GerenciamentoUsuarios.Dominio.Usuarios.Enums`

### Restrições

- ⛔ Não atribuir valor `0` a nenhum membro — `0` representa estado indefinido e não deve ser um perfil válido

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue o padrão `GerenciamentoUsuarios.Dominio.Usuarios.Enums`
- [ ] Os dois membros com os valores corretos existem
- [ ] O projeto compila sem erros após a adição

---

## Card 2 — Entidade Usuario

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Card 0, Card 1

### Contexto

`Usuario` é a entidade central desta feature. Ela representa uma conta no sistema, com seus dados pessoais, credenciais de acesso e o perfil que determina o que o titular pode fazer.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `Usuario.cs` | `GerenciamentoUsuarios.Dominio/Usuarios/Entidades/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Dominio.Usuarios.Entidades`

**Campos / Propriedades:**

| Nome | Tipo | Obrigatório? | Regras de validação |
| --- | --- | :---: | --- |
| `Id` | `int` | Sim | Gerado automaticamente |
| `NomeCompleto` | `string` | Sim | Não pode ser nulo ou vazio |
| `Cpf` | `string` | Sim | Não pode ser nulo ou vazio |
| `DataNascimento` | `DateOnly` | Sim | Não pode ser o valor padrão |
| `Email` | `string` | Sim | Não pode ser nulo ou vazio |
| `SenhaHash` | `string` | Sim | Não pode ser nulo ou vazio |
| `Perfil` | `PerfilUsuario` | Sim | Deve ser um dos valores válidos do enum |
| `Ativo` | `bool` | Sim | Iniciado como `true` na criação |

**Comportamentos:**

- O construtor público recebe todos os campos obrigatórios (exceto `Id` e `Ativo`) e delega a validação para os métodos `Set` correspondentes. `Ativo` é definido como `true` no construtor.
- O construtor vazio `protected` deve existir obrigatoriamente para o EF Core instanciar a entidade ao ler do banco.
- Todas as propriedades devem ser `virtual` e com setter `protected`.
- `SetNomeCompleto(string)` — lança `RegraDeNegocioExcecao` se o valor for nulo ou vazio.
- `SetCpf(string)` — lança `RegraDeNegocioExcecao` se o valor for nulo ou vazio.
- `SetDataNascimento(DateOnly)` — lança `RegraDeNegocioExcecao` se o valor for `default(DateOnly)`.
- `SetEmail(string)` — lança `RegraDeNegocioExcecao` se o valor for nulo ou vazio.
- `SetSenhaHash(string)` — lança `RegraDeNegocioExcecao` se o valor for nulo ou vazio.
- `SetPerfil(PerfilUsuario)` — lança `RegraDeNegocioExcecao` se o valor não for um membro válido do enum.
- `Ativar()` — define `Ativo = true`.
- `Desativar()` — define `Ativo = false`.

### Restrições

- ⛔ Nenhum setter pode ser público — toda alteração de estado é feita via métodos `Set` ou `Ativar`/`Desativar`
- ⛔ Não expor `SenhaHash` fora da entidade — a propriedade deve existir mas nunca deve ser mapeada para `Response`
- ⛔ Não aplicar DataAnnotations

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Dominio.Usuarios.Entidades`
- [ ] Construtor vazio `protected` existe
- [ ] Todas as propriedades são `virtual` com setter `protected`
- [ ] Todos os métodos `Set` lançam `RegraDeNegocioExcecao` para valores inválidos
- [ ] Os métodos `Ativar()` e `Desativar()` existem
- [ ] O projeto compila sem erros após a adição

---

## Card 3 — IUsuariosRepositorio

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Card 2

### Contexto

Esta interface define o contrato de persistência para a entidade `Usuario`. Além das operações básicas herdadas, declara as consultas específicas necessárias para as validações de unicidade de e-mail e CPF.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `IUsuariosRepositorio.cs` | `GerenciamentoUsuarios.Dominio/Usuarios/Repositorios/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Dominio.Usuarios.Repositorios`

**Herança:** Estende `IRepositorioBase<Usuario>`

**Métodos adicionais:**

| Método | Retorno | Parâmetros | Descrição |
| --- | --- | --- | --- |
| `ExisteEmailAsync` | `Task<bool>` | `string email`, `int? ignorarId`, `CancellationToken ct` | Retorna `true` se outro usuário com o mesmo e-mail já existe. O parâmetro `ignorarId` permite ignorar o próprio usuário na verificação de edição. |
| `ExisteCpfAsync` | `Task<bool>` | `string cpf`, `int? ignorarId`, `CancellationToken ct` | Retorna `true` se outro usuário com o mesmo CPF já existe. O parâmetro `ignorarId` permite ignorar o próprio usuário na verificação de edição. |

### Restrições

- ⛔ Não implementar lógica neste arquivo — apenas a declaração da interface

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Dominio.Usuarios.Repositorios`
- [ ] A interface herda de `IRepositorioBase<Usuario>`
- [ ] Os dois métodos adicionais estão declarados com as assinaturas corretas
- [ ] O projeto compila sem erros após a adição

---

## Card 4 — UsuariosInserirComando

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Card 1

### Contexto

Este comando transporta os dados necessários para criar um novo usuário. Ele é produzido pelo serviço de aplicação e consumido pelo serviço de domínio no fluxo de cadastro.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosInserirComando.cs` | `GerenciamentoUsuarios.Dominio/Usuarios/Servicos/Comandos/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Comandos`

**Campos / Propriedades:**

| Nome | Tipo | Obrigatório? | Observação |
| --- | --- | :---: | --- |
| `NomeCompleto` | `string` | Sim | — |
| `Cpf` | `string` | Sim | — |
| `DataNascimento` | `DateOnly` | Sim | — |
| `Email` | `string` | Sim | — |
| `Senha` | `string` | Sim | Texto puro — será transformado em hash pelo serviço de domínio |
| `Perfil` | `PerfilUsuario` | Sim | — |

### Restrições

- ⛔ Sem validações neste arquivo — apenas propriedades com `get; set;`
- ⛔ Sem DataAnnotations

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Comandos`
- [ ] Todos os campos listados existem
- [ ] O projeto compila sem erros após a adição

---

## Card 5 — UsuariosEditarComando

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Card 1

### Contexto

Este comando transporta os dados que um usuário deseja alterar na própria conta. Todos os campos exceto `Id` são opcionais — apenas os campos informados serão atualizados.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosEditarComando.cs` | `GerenciamentoUsuarios.Dominio/Usuarios/Servicos/Comandos/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Comandos`

**Campos / Propriedades:**

| Nome | Tipo | Obrigatório? | Observação |
| --- | --- | :---: | --- |
| `Id` | `int` | Sim | Identifica o usuário a ser editado |
| `NomeCompleto` | `string?` | Não | Null = não alterar |
| `Cpf` | `string?` | Não | Null = não alterar |
| `DataNascimento` | `DateOnly?` | Não | Null = não alterar |
| `Email` | `string?` | Não | Null = não alterar |
| `Senha` | `string?` | Não | Null = não alterar; quando informado, será transformado em hash |

### Restrições

- ⛔ Sem validações neste arquivo — apenas propriedades com `get; set;`
- ⛔ Sem DataAnnotations
- ⛔ O perfil do usuário não pode ser alterado via edição — não incluir campo `Perfil` neste comando

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Comandos`
- [ ] `Id` é obrigatório; demais campos são anuláveis
- [ ] `Perfil` não está presente
- [ ] O projeto compila sem erros após a adição

---

## Card 6 — UsuariosListarFiltro

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Card 0

### Contexto

Este filtro carrega os critérios de busca utilizados pelo serviço de domínio para consultar a lista de usuários. Ele é produzido pelo serviço de aplicação a partir do request de listagem recebido pelo controller.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosListarFiltro.cs` | `GerenciamentoUsuarios.Dominio/Usuarios/Servicos/Filtros/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Filtros`

**Herança:** Estende `PaginacaoFiltro`

**Campos adicionais:**

| Nome | Tipo | Obrigatório? | Descrição |
| --- | --- | :---: | --- |
| `Nome` | `string?` | Não | Filtro parcial por nome completo |
| `Email` | `string?` | Não | Filtro parcial por e-mail |
| `Cpf` | `string?` | Não | Filtro exato por CPF |
| `Ativo` | `bool?` | Não | Null = todos; `true` = apenas ativos; `false` = apenas inativos |

### Restrições

- ⛔ Sem lógica — apenas propriedades com `get; set;`

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Filtros`
- [ ] Herda de `PaginacaoFiltro`
- [ ] Os quatro campos adicionais existem como anuláveis
- [ ] O projeto compila sem erros após a adição

---

## Card 7 — IUsuariosServicos

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Card 2, Card 3, Card 4, Card 5, Card 6

### Contexto

Esta interface define o contrato do serviço de domínio de usuários. Ela declara todas as operações de negócio disponíveis e é o ponto de integração entre a camada de aplicação e a lógica de domínio.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `IUsuariosServicos.cs` | `GerenciamentoUsuarios.Dominio/Usuarios/Servicos/Interfaces/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Interfaces`

**Métodos:**

| Método | Retorno | Parâmetros |
| --- | --- | --- |
| `InserirAsync` | `Task<Usuario>` | `UsuariosInserirComando comando`, `CancellationToken ct` |
| `EditarAsync` | `Task<Usuario>` | `UsuariosEditarComando comando`, `CancellationToken ct` |
| `ExcluirAsync` | `Task` | `int id`, `CancellationToken ct` |
| `ReativarAsync` | `Task` | `int id`, `CancellationToken ct` |
| `Recuperar` | `Usuario` | `int id` |
| `Listar` | `PaginacaoConsulta<Usuario>` | `UsuariosListarFiltro filtro` |

### Restrições

- ⛔ Não implementar lógica neste arquivo — apenas declaração de interface

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Interfaces`
- [ ] Todos os métodos listados estão declarados com as assinaturas corretas
- [ ] O projeto compila sem erros após a adição

---

## Card 8 — UsuariosServicos

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Card 2, Card 3, Card 4, Card 5, Card 6, Card 7

### Contexto

`UsuariosServicos` é onde vivem todas as regras de negócio da feature. É aqui que as validações de unicidade, o hash de senha e as restrições de ativação e reativação são aplicadas.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosServicos.cs` | `GerenciamentoUsuarios.Dominio/Usuarios/Servicos/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Dominio.Usuarios.Servicos`

**Implementa:** `IUsuariosServicos`

**Dependências injetadas no construtor:**

- `IUsuariosRepositorio`

**Comportamentos por método:**

`InserirAsync(UsuariosInserirComando, ct)`:
- Verificar se já existe usuário com o mesmo e-mail usando `IUsuariosRepositorio.ExisteEmailAsync` (sem ignorar nenhum id). Se existir, lançar `RegraDeNegocioExcecao` informando que o e-mail já está em uso.
- Verificar se já existe usuário com o mesmo CPF usando `IUsuariosRepositorio.ExisteCpfAsync` (sem ignorar nenhum id). Se existir, lançar `RegraDeNegocioExcecao` informando que o CPF já está em uso.
- Gerar o hash da senha usando BCrypt a partir do campo `Senha` do comando.
- Instanciar um novo `Usuario` com todos os campos do comando, usando o hash gerado para `SenhaHash`.
- Chamar `IUsuariosRepositorio.InserirAsync(usuario, ct)`.
- Retornar o `Usuario` criado.

`EditarAsync(UsuariosEditarComando, ct)`:
- Recuperar a entidade pelo `Id` do comando. Usar `ValidarRegistroNaoFoiEncontrado` se a entidade não for encontrada.
- Se `Email` não for nulo, verificar unicidade com `ExisteEmailAsync` passando o `Id` como `ignorarId`. Se existir, lançar `RegraDeNegocioExcecao`.
- Se `Cpf` não for nulo, verificar unicidade com `ExisteCpfAsync` passando o `Id` como `ignorarId`. Se existir, lançar `RegraDeNegocioExcecao`.
- Para cada campo não-nulo do comando, chamar o método `Set` correspondente na entidade.
- Se `Senha` não for nula, gerar hash BCrypt e chamar `SetSenhaHash`.
- Chamar `IUsuariosRepositorio.EditarAsync(usuario, ct)`.
- Retornar o `Usuario` atualizado.

`ExcluirAsync(int id, ct)`:
- Recuperar a entidade pelo `id`. Usar `ValidarRegistroNaoFoiEncontrado` se não encontrada.
- Chamar `usuario.Desativar()`.
- Chamar `IUsuariosRepositorio.EditarAsync(usuario, ct)`.

`ReativarAsync(int id, ct)`:
- Recuperar a entidade pelo `id`. Usar `ValidarRegistroNaoFoiEncontrado` se não encontrada.
- Chamar `usuario.Ativar()`.
- Chamar `IUsuariosRepositorio.EditarAsync(usuario, ct)`.

`Recuperar(int id)`:
- Chamar `IUsuariosRepositorio.Recuperar(id)`.
- Usar `ValidarRegistroNaoFoiEncontrado` se não encontrado.
- Retornar o `Usuario`.

`Listar(UsuariosListarFiltro filtro)`:
- Usar `IUsuariosRepositorio.Query()` para montar a consulta LINQ.
- Aplicar filtro por `Nome` (quando não nulo — correspondência parcial, case-insensitive).
- Aplicar filtro por `Email` (quando não nulo — correspondência parcial, case-insensitive).
- Aplicar filtro por `Cpf` (quando não nulo — correspondência exata).
- Aplicar filtro por `Ativo` (quando não nulo — correspondência exata).
- Aplicar paginação com `Pg` e `Qt` do filtro.
- Retornar `PaginacaoConsulta<Usuario>` com o total e a lista paginada.

### Restrições

- ⛔ Não acessar o repositório fora dos métodos listados — todo acesso a dados é via `IUsuariosRepositorio`
- ⛔ Não colocar lógica de autorização aqui — a verificação de perfil (quem pode chamar `ReativarAsync`) é responsabilidade da camada de API
- ⛔ Não chamar `SaveChanges` manualmente — o UnitOfWork é gerenciado pelo AppServico

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Dominio.Usuarios.Servicos`
- [ ] Implementa `IUsuariosServicos`
- [ ] `IUsuariosRepositorio` é injetado no construtor
- [ ] Unicidade de e-mail é verificada em `InserirAsync` e `EditarAsync`
- [ ] Unicidade de CPF é verificada em `InserirAsync` e `EditarAsync`
- [ ] Hash BCrypt é gerado em `InserirAsync` e quando senha é informada em `EditarAsync`
- [ ] `ExcluirAsync` chama `Desativar()` — nunca remove o registro fisicamente
- [ ] O projeto compila sem erros após a adição

---

## Card 9 — UsuarioConfiguration

**Camada:** Infra
**Instruction:** `.github/instructions/infra.instructions.md`
**Depende de:** Card 1, Card 2

### Contexto

Este mapeamento define como a entidade `Usuario` é persistida no banco de dados SQL Server, incluindo nomes de tabela, colunas e restrições de unicidade.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuarioConfiguration.cs` | `GerenciamentoUsuarios.Infra/Usuarios/Mapeamentos/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Infra.Usuarios.Mapeamentos`

**Implementa:** `IEntityTypeConfiguration<Usuario>`

**Configurações:**

| Propriedade da entidade | Nome da coluna | Tipo / Restrições |
| --- | --- | --- |
| `Id` | `id` | Chave primária, gerado automaticamente |
| `NomeCompleto` | `nome_completo` | `nvarchar(255)`, obrigatório |
| `Cpf` | `cpf` | `nvarchar(14)`, obrigatório |
| `DataNascimento` | `data_nascimento` | `date`, obrigatório |
| `Email` | `email` | `nvarchar(255)`, obrigatório |
| `SenhaHash` | `senha_hash` | `nvarchar(255)`, obrigatório |
| `Perfil` | `perfil` | `int`, obrigatório, armazenado como inteiro |
| `Ativo` | `ativo` | `bit`, obrigatório |

**Nome da tabela:** `usuarios`

**Índices únicos:**
- Índice único na coluna `email`
- Índice único na coluna `cpf`

### Restrições

- ⛔ Não registrar manualmente este mapeamento no `AppDbContext` — ele é descoberto automaticamente por `ApplyConfigurationsFromAssembly`

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Infra.Usuarios.Mapeamentos`
- [ ] Todos os campos estão mapeados nas colunas corretas
- [ ] Índices únicos em `email` e `cpf` estão configurados
- [ ] `Perfil` é armazenado como `int`
- [ ] O projeto compila sem erros após a adição

---

## Card 10 — UsuariosRepositorio

**Camada:** Infra
**Instruction:** `.github/instructions/infra.instructions.md`
**Depende de:** Card 2, Card 3, Card 9

### Contexto

Esta classe implementa o contrato de repositório da feature, herdando as operações base e adicionando as consultas específicas de verificação de unicidade para e-mail e CPF.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosRepositorio.cs` | `GerenciamentoUsuarios.Infra/Usuarios/Repositorios/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Infra.Usuarios.Repositorios`

**Herança:** Estende `RepositorioBase<Usuario>`, implementa `IUsuariosRepositorio`

**Construtor:** Recebe `AppDbContext` e repassa para `RepositorioBase<Usuario>`.

**Implementação dos métodos adicionais:**

`ExisteEmailAsync(string email, int? ignorarId, CancellationToken ct)`:
- Usar `Query()` para verificar se existe algum usuário com o mesmo e-mail.
- Quando `ignorarId` for informado, excluir da verificação o usuário com aquele id.
- Retornar `true` se existir pelo menos um registro correspondente.

`ExisteCpfAsync(string cpf, int? ignorarId, CancellationToken ct)`:
- Usar `Query()` para verificar se existe algum usuário com o mesmo CPF.
- Quando `ignorarId` for informado, excluir o usuário com aquele id da verificação.
- Retornar `true` se existir pelo menos um registro correspondente.

### Restrições

- ⛔ Não sobrescrever métodos do `RepositorioBase<T>` — apenas adicionar os específicos desta feature
- ⛔ Não chamar `SaveChanges` diretamente

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Infra.Usuarios.Repositorios`
- [ ] Estende `RepositorioBase<Usuario>` e implementa `IUsuariosRepositorio`
- [ ] `ExisteEmailAsync` e `ExisteCpfAsync` estão implementados com suporte ao `ignorarId`
- [ ] O projeto compila sem erros após a adição

---

## Card 11 — DbSet Usuario no AppDbContext

**Camada:** Infra
**Instruction:** `.github/instructions/infra.instructions.md`
**Depende de:** Card 2, Card 9

### Contexto

Para que o EF Core reconheça e migre a tabela `usuarios`, a entidade precisa ser declarada como `DbSet` no contexto do banco de dados.

### Artefatos a modificar

| Arquivo | Caminho completo |
| --- | --- |
| `AppDbContext.cs` | `GerenciamentoUsuarios.Infra/Contexto/` |

### Especificação

Adicionar ao `AppDbContext` a propriedade:

- `DbSet<Usuario> Usuarios { get; set; }`

O `OnModelCreating` já chama `ApplyConfigurationsFromAssembly` (configurado no Card 0), portanto não é necessário registrar o mapeamento manualmente.

### Restrições

- ⛔ Não alterar o `OnModelCreating` — apenas adicionar o `DbSet`

### Critério de conclusão

O card está concluído quando:

- [ ] `DbSet<Usuario> Usuarios` foi adicionado ao `AppDbContext`
- [ ] O projeto compila sem erros após a adição

---

## Card 12 — Migration Inicial Usuarios

**Camada:** Infra
**Instruction:** `.github/instructions/infra.instructions.md`
**Depende de:** Card 9, Card 10, Card 11

### Contexto

A migration cria fisicamente a tabela `usuarios` no banco SQL Server com todas as colunas, restrições e índices definidos no mapeamento EF Core.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| Migration gerada pelo EF Core | `GerenciamentoUsuarios.Infra/Migrations/` |

### Especificação

Executar o comando EF Core para adicionar a migration, tendo o projeto `GerenciamentoUsuarios.Infra` como projeto de destino e `GerenciamentoUsuarios.Api` como projeto de inicialização. O nome da migration deve ser `CriarTabelaUsuarios`.

Após gerar a migration, verificar no arquivo gerado que:
- A tabela `usuarios` é criada com todas as colunas listadas no Card 9
- Os índices únicos de `email` e `cpf` estão presentes

### Restrições

- ⛔ Não editar manualmente o arquivo de migration gerado — se algo estiver errado, corrigir o mapeamento (Card 9) e regenerar

### Critério de conclusão

O card está concluído quando:

- [ ] A migration foi gerada em `GerenciamentoUsuarios.Infra/Migrations/`
- [ ] O arquivo de migration contém a criação da tabela `usuarios` com todas as colunas
- [ ] Os índices únicos de `email` e `cpf` estão na migration
- [ ] O projeto compila sem erros após a adição

---

## Card 13 — UsuariosInserirRequest

**Camada:** DataTransfer
**Instruction:** `.github/instructions/datatransfer.instructions.md`
**Depende de:** Card 0, Card 1

### Contexto

Este DTO transporta os dados enviados pelo cliente no momento do cadastro de uma nova conta. É o contrato público de entrada para a operação de criação de usuário.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosInserirRequest.cs` | `GerenciamentoUsuarios.DataTransfer/Usuarios/Request/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.DataTransfer.Usuarios.Request`

**Campos / Propriedades:**

| Nome | Tipo | Obrigatório? | Observação |
| --- | --- | :---: | --- |
| `NomeCompleto` | `string` | Sim | — |
| `Cpf` | `string` | Sim | — |
| `DataNascimento` | `DateOnly` | Sim | — |
| `Email` | `string` | Sim | — |
| `Senha` | `string` | Sim | Texto puro — nunca armazenar diretamente |
| `Perfil` | `PerfilUsuario` | Sim | — |

### Restrições

- ⛔ Sem DataAnnotations
- ⛔ Sem lógica — apenas propriedades com `get; set;`

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.DataTransfer.Usuarios.Request`
- [ ] Todos os campos listados existem com os tipos corretos
- [ ] Nenhum DataAnnotation presente
- [ ] O projeto compila sem erros após a adição

---

## Card 14 — UsuariosEditarRequest

**Camada:** DataTransfer
**Instruction:** `.github/instructions/datatransfer.instructions.md`
**Depende de:** Card 0

### Contexto

Este DTO transporta os dados que o usuário autenticado deseja alterar na própria conta. Todos os campos exceto `Id` são opcionais — o cliente envia apenas o que deseja modificar.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosEditarRequest.cs` | `GerenciamentoUsuarios.DataTransfer/Usuarios/Request/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.DataTransfer.Usuarios.Request`

**Campos / Propriedades:**

| Nome | Tipo | Obrigatório? | Observação |
| --- | --- | :---: | --- |
| `Id` | `int` | Sim | Identifica o usuário a ser editado |
| `NomeCompleto` | `string?` | Não | Null = não alterar |
| `Cpf` | `string?` | Não | Null = não alterar |
| `DataNascimento` | `DateOnly?` | Não | Null = não alterar |
| `Email` | `string?` | Não | Null = não alterar |
| `Senha` | `string?` | Não | Null = não alterar; texto puro quando informado |

### Restrições

- ⛔ Sem DataAnnotations
- ⛔ Sem lógica — apenas propriedades com `get; set;`
- ⛔ Não incluir campo `Perfil` — o perfil não é editável

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.DataTransfer.Usuarios.Request`
- [ ] `Id` é obrigatório; todos os demais são anuláveis
- [ ] `Perfil` não está presente
- [ ] O projeto compila sem erros após a adição

---

## Card 15 — UsuariosListarRequest

**Camada:** DataTransfer
**Instruction:** `.github/instructions/datatransfer.instructions.md`
**Depende de:** Card 0

### Contexto

Este DTO representa os critérios de busca e paginação enviados pelo cliente ao listar usuários. Herda os campos de paginação padrão e adiciona os filtros específicos desta feature.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosListarRequest.cs` | `GerenciamentoUsuarios.DataTransfer/Usuarios/Request/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.DataTransfer.Usuarios.Request`

**Herança:** Estende `PaginacaoFiltro`

**Campos adicionais:**

| Nome | Tipo | Obrigatório? | Observação |
| --- | --- | :---: | --- |
| `Nome` | `string?` | Não | Filtro parcial por nome |
| `Email` | `string?` | Não | Filtro parcial por e-mail |
| `Cpf` | `string?` | Não | Filtro exato por CPF |
| `Ativo` | `bool?` | Não | Null = todos os status |

### Restrições

- ⛔ Sem DataAnnotations
- ⛔ Sem lógica — apenas propriedades com `get; set;`

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.DataTransfer.Usuarios.Request`
- [ ] Herda de `PaginacaoFiltro`
- [ ] Os quatro campos adicionais existem como anuláveis
- [ ] O projeto compila sem erros após a adição

---

## Card 16 — UsuariosLoginRequest

**Camada:** DataTransfer
**Instruction:** `.github/instructions/datatransfer.instructions.md`
**Depende de:** Card 0

### Contexto

Este DTO transporta as credenciais enviadas pelo cliente para obter um token de acesso. É o contrato de entrada do endpoint público de autenticação.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosLoginRequest.cs` | `GerenciamentoUsuarios.DataTransfer/Usuarios/Request/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.DataTransfer.Usuarios.Request`

**Campos / Propriedades:**

| Nome | Tipo | Obrigatório? | Observação |
| --- | --- | :---: | --- |
| `Email` | `string` | Sim | — |
| `Senha` | `string` | Sim | Texto puro |

### Restrições

- ⛔ Sem DataAnnotations
- ⛔ Sem lógica — apenas propriedades com `get; set;`

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.DataTransfer.Usuarios.Request`
- [ ] Os dois campos existem
- [ ] O projeto compila sem erros após a adição

---

## Card 17 — UsuariosResponse

**Camada:** DataTransfer
**Instruction:** `.github/instructions/datatransfer.instructions.md`
**Depende de:** Card 1

### Contexto

Este DTO representa os dados de um usuário que a API retorna ao cliente. Expõe apenas as informações seguras — a senha nunca é incluída.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosResponse.cs` | `GerenciamentoUsuarios.DataTransfer/Usuarios/Response/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.DataTransfer.Usuarios.Response`

**Campos / Propriedades:**

| Nome | Tipo | Observação |
| --- | --- | --- |
| `Id` | `int` | — |
| `NomeCompleto` | `string` | — |
| `Cpf` | `string` | — |
| `DataNascimento` | `DateOnly` | — |
| `Email` | `string` | — |
| `Perfil` | `PerfilUsuario` | Enum exposto diretamente |
| `Ativo` | `bool` | — |

### Restrições

- ⛔ Não incluir `SenhaHash` — nunca expor a senha ao cliente
- ⛔ Sem DataAnnotations
- ⛔ Sem lógica — apenas propriedades com `get; set;`

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.DataTransfer.Usuarios.Response`
- [ ] `SenhaHash` não está presente
- [ ] O projeto compila sem erros após a adição

---

## Card 18 — UsuariosProfile

**Camada:** Aplicação
**Instruction:** `.github/instructions/aplicacao.instructions.md`
**Depende de:** Card 2, Card 4, Card 5, Card 6, Card 13, Card 14, Card 15, Card 17

### Contexto

Este profile configura o AutoMapper para converter automaticamente requests em comandos/filtros e entidades em responses, eliminando código manual de mapeamento no serviço de aplicação.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosProfile.cs` | `GerenciamentoUsuarios.Aplicacao/Usuarios/Profiles/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Aplicacao.Usuarios.Profiles`

**Herança:** Estende `Profile` (AutoMapper)

**Mapeamentos a configurar:**

| Origem | Destino | Observação |
| --- | --- | --- |
| `UsuariosInserirRequest` | `UsuariosInserirComando` | Mapeamento direto campo a campo |
| `UsuariosEditarRequest` | `UsuariosEditarComando` | Mapeamento direto campo a campo |
| `UsuariosListarRequest` | `UsuariosListarFiltro` | Mapeamento direto campo a campo |
| `Usuario` | `UsuariosResponse` | Ignorar `SenhaHash` — não mapear para nenhum campo do response |

### Restrições

- ⛔ Não mapear `SenhaHash` para nenhum campo do `UsuariosResponse`
- ⛔ Sem lógica de negócio — apenas configuração de mapeamento

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Aplicacao.Usuarios.Profiles`
- [ ] Os quatro mapeamentos estão configurados
- [ ] `SenhaHash` está explicitamente ignorado no mapeamento de `Usuario` → `UsuariosResponse`
- [ ] O projeto compila sem erros após a adição

---

## Card 19 — IUsuariosAppServico

**Camada:** Aplicação
**Instruction:** `.github/instructions/aplicacao.instructions.md`
**Depende de:** Card 6, Card 13, Card 14, Card 15, Card 17

### Contexto

Esta interface define o contrato do serviço de aplicação que o controller irá consumir. É o ponto de entrada para todas as operações da feature a partir da camada API.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `IUsuariosAppServico.cs` | `GerenciamentoUsuarios.Aplicacao/Usuarios/Servicos/Interfaces/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos.Interfaces`

**Métodos:**

| Método | Retorno | Parâmetros |
| --- | --- | --- |
| `InserirAsync` | `Task<UsuariosResponse>` | `UsuariosInserirRequest request`, `CancellationToken ct` |
| `EditarAsync` | `Task<UsuariosResponse>` | `UsuariosEditarRequest request`, `CancellationToken ct` |
| `ExcluirAsync` | `Task` | `int id`, `int usuarioAutenticadoId`, `CancellationToken ct` |
| `ReativarAsync` | `Task` | `int id`, `CancellationToken ct` |
| `Recuperar` | `UsuariosResponse` | `int id` |
| `Listar` | `PaginacaoConsulta<UsuariosResponse>` | `UsuariosListarRequest request` |
| `LoginAsync` | `Task<string>` | `UsuariosLoginRequest request`, `CancellationToken ct` |

> O parâmetro `usuarioAutenticadoId` em `ExcluirAsync` é necessário para que o serviço de aplicação valide que o usuário só pode desativar a própria conta.
> `LoginAsync` retorna o token JWT gerado.

### Restrições

- ⛔ Não implementar lógica neste arquivo — apenas declaração de interface

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos.Interfaces`
- [ ] Todos os métodos listados estão declarados com as assinaturas corretas
- [ ] O projeto compila sem erros após a adição

---

## Card 20 — UsuariosAppServico

**Camada:** Aplicação
**Instruction:** `.github/instructions/aplicacao.instructions.md`
**Depende de:** Card 7, Card 8, Card 18, Card 19

### Contexto

`UsuariosAppServico` orquestra o fluxo de cada operação: mapeia requests para comandos, controla a transação via UnitOfWork, delega a lógica de negócio ao serviço de domínio e converte o resultado em response. Também aplica as restrições de acesso específicas desta feature (editar e desativar apenas a própria conta).

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosAppServico.cs` | `GerenciamentoUsuarios.Aplicacao/Usuarios/Servicos/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos`

**Implementa:** `IUsuariosAppServico`

**Dependências injetadas no construtor:**

- `IUsuariosServicos`
- `IUsuariosRepositorio` (para operações de leitura direta sem regra de negócio)
- `IUnitOfWork`
- `IMapper`
- `TokenServicos`

**Comportamentos por método:**

`InserirAsync(request, ct)`:
- Abrir transação com `IUnitOfWork.BeginTransactionAsync`.
- Mapear `UsuariosInserirRequest` → `UsuariosInserirComando`.
- Chamar `IUsuariosServicos.InserirAsync`.
- Commitar. Em caso de exceção, fazer rollback e relançar.
- Mapear `Usuario` → `UsuariosResponse` e retornar.

`EditarAsync(request, ct)`:
- O controller deve passar o `id` do usuário autenticado (via JWT claim) e validar que é igual a `request.Id` **antes** de chamar este método, retornando `Unauthorized` se forem diferentes. Caso prefira, essa verificação pode ser feita aqui — ver nota abaixo.
- Abrir transação.
- Mapear `UsuariosEditarRequest` → `UsuariosEditarComando`.
- Chamar `IUsuariosServicos.EditarAsync`.
- Commitar. Em caso de exceção, rollback e relançar.
- Retornar `UsuariosResponse`.

> Nota: a verificação "usuário só edita a própria conta" pode ficar no controller ou no AppServico. Escolher um lugar e ser consistente.

`ExcluirAsync(int id, int usuarioAutenticadoId, ct)`:
- Verificar se `id == usuarioAutenticadoId`. Se não, lançar `RegraDeNegocioExcecao` informando que não é permitido desativar a conta de outro usuário.
- Abrir transação.
- Chamar `IUsuariosServicos.ExcluirAsync(id, ct)`.
- Commitar. Em caso de exceção, rollback e relançar.

`ReativarAsync(int id, ct)`:
- Abrir transação.
- Chamar `IUsuariosServicos.ReativarAsync(id, ct)`.
- Commitar. Em caso de exceção, rollback e relançar.

`Recuperar(int id)`:
- Chamar `IUsuariosServicos.Recuperar(id)`.
- Retornar `UsuariosResponse`.

`Listar(request)`:
- Mapear `UsuariosListarRequest` → `UsuariosListarFiltro`.
- Chamar `IUsuariosServicos.Listar(filtro)`.
- Mapear `PaginacaoConsulta<Usuario>` → `PaginacaoConsulta<UsuariosResponse>`.
- Retornar o resultado.

`LoginAsync(request, ct)`:
- Buscar o usuário pelo e-mail usando `IUsuariosRepositorio.RecuperarAsync(u => u.Email == request.Email)`.
- Se não encontrado, lançar `RegraDeNegocioExcecao` com mensagem genérica (ex: "Credenciais inválidas").
- Se o usuário estiver inativo, lançar `RegraDeNegocioExcecao` com mensagem informando conta desativada.
- Verificar se a senha informada corresponde ao hash armazenado usando BCrypt. Se não, lançar `RegraDeNegocioExcecao` com a mesma mensagem genérica.
- Chamar `TokenServicos.GerarToken(usuario)` e retornar o token.

### Restrições

- ⛔ Não colocar lógica de negócio que já está em `IUsuariosServicos` — não duplicar validações de unicidade, por exemplo
- ⛔ UnitOfWork é obrigatório em todas as operações de escrita
- ⛔ Leituras simples (Recuperar, Listar, Login) não precisam de transação

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos`
- [ ] Implementa `IUsuariosAppServico`
- [ ] UnitOfWork (begin/commit/rollback) é usado em `InserirAsync`, `EditarAsync`, `ExcluirAsync` e `ReativarAsync`
- [ ] `ExcluirAsync` verifica que `id == usuarioAutenticadoId`
- [ ] `LoginAsync` verifica hash BCrypt e rejeita contas inativas
- [ ] O projeto compila sem erros após a adição

---

## Card 21 — TokenServicos

**Camada:** Api
**Instruction:** `.github/instructions/api.instructions.md`
**Depende de:** Card 2

### Contexto

`TokenServicos` é responsável por gerar o token JWT que o cliente usa para se autenticar em todos os endpoints protegidos. As configurações de chave, issuer e audience são lidas de variáveis de ambiente ou user-secrets — nunca de arquivos commitados.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `TokenServicos.cs` | `GerenciamentoUsuarios.Api/Autenticacoes/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Api.Autenticacoes`

**Dependências injetadas no construtor:**

- `IConfiguration`

**Métodos:**

`GerarToken(Usuario usuario)` → `string`:
- Ler do `IConfiguration`: `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`.
- Criar as claims do token:
  - `ClaimTypes.NameIdentifier` = `usuario.Id.ToString()`
  - `ClaimTypes.Email` = `usuario.Email`
  - `ClaimTypes.Role` = nome do membro do enum `usuario.Perfil` (ex: `"Administrador"` ou `"UsuarioComum"`)
- Assinar o token com `SymmetricSecurityKey` usando a chave lida da configuração e o algoritmo `HmacSha256`.
- Definir expiração de 8 horas a partir da geração.
- Retornar o token serializado como string.

### Restrições

- ⛔ Nunca hardcodar a chave JWT no código — sempre ler de `IConfiguration`
- ⛔ Não armazenar nem logar o token gerado

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Api.Autenticacoes`
- [ ] `IConfiguration` é injetado no construtor
- [ ] As três claims (`NameIdentifier`, `Email`, `Role`) são incluídas no token
- [ ] A chave é lida de `IConfiguration["Jwt:Key"]`
- [ ] O projeto compila sem erros após a adição

---

## Card 22 — UsuariosController

**Camada:** Api
**Instruction:** `.github/instructions/api.instructions.md`
**Depende de:** Card 19, Card 20, Card 21

### Contexto

O controller expõe os endpoints HTTP da feature, delega ao serviço de aplicação e aplica as restrições de autenticação e autorização em cada rota.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `UsuariosController.cs` | `GerenciamentoUsuarios.Api/Controllers/Usuarios/` |

### Especificação

**Namespace:** `GerenciamentoUsuarios.Api.Controllers.Usuarios`

**Rota base:** `api/usuarios`

**Atributos de classe:** `[Route("api/usuarios")]`, `[ApiController]`, `[Authorize]`

**Dependências injetadas no construtor:**

- `IUsuariosAppServico`

**Endpoints:**

| Método | Verbo HTTP | Rota | Autorização | Descrição |
| --- | --- | --- | --- | --- |
| `Inserir` | POST | `api/usuarios` | `[AllowAnonymous]` | Cadastro público de nova conta |
| `Login` | POST | `api/usuarios/login` | `[AllowAnonymous]` | Autenticação — retorna token JWT |
| `Listar` | GET | `api/usuarios` | `[Authorize]` | Lista paginada com filtros |
| `Recuperar` | GET | `api/usuarios/{id}` | `[Authorize]` | Detalhes de um usuário |
| `Editar` | PUT | `api/usuarios` | `[Authorize]` | Editar dados da própria conta |
| `Desativar` | DELETE | `api/usuarios/{id}` | `[Authorize]` | Desativar a própria conta |
| `Reativar` | PATCH | `api/usuarios/{id}/reativar` | `[Authorize(Roles = "Administrador")]` | Reativar conta desativada |

**Comportamentos especiais:**

`Editar`:
- Extrair o id do usuário autenticado da claim `ClaimTypes.NameIdentifier`.
- Se o id do request (`request.Id`) for diferente do id autenticado, retornar `Unauthorized()`.
- Caso contrário, chamar `IUsuariosAppServico.EditarAsync`.

`Desativar`:
- Extrair o id do usuário autenticado da claim `ClaimTypes.NameIdentifier`.
- Passar ambos os ids para `IUsuariosAppServico.ExcluirAsync(id, usuarioAutenticadoId, ct)`.

`Login`:
- Receber `UsuariosLoginRequest` no corpo.
- Chamar `IUsuariosAppServico.LoginAsync` e retornar o token em `Ok(new { token })`.

**Retornos padrão de todos os endpoints:** `Ok(response)` para sucesso.

### Restrições

- ⛔ Não colocar lógica de negócio no controller — apenas delegar ao AppServico
- ⛔ `Inserir` e `Login` devem ter `[AllowAnonymous]` — sem exigir autenticação
- ⛔ `Reativar` deve ter `[Authorize(Roles = "Administrador")]` — somente Administradores

### Critério de conclusão

O card está concluído quando:

- [ ] O arquivo foi criado no caminho correto
- [ ] Namespace segue o padrão `GerenciamentoUsuarios.Api.Controllers.Usuarios`
- [ ] `Inserir` e `Login` têm `[AllowAnonymous]`
- [ ] `Reativar` tem `[Authorize(Roles = "Administrador")]`
- [ ] `Editar` e `Desativar` verificam identidade via claim antes de chamar o AppServico
- [ ] O projeto compila sem erros após a adição

---

## Card 23 — Registrar Dependências no IoC

**Camada:** IoC
**Instruction:** `.github/instructions/ioc.instructions.md`
**Depende de:** Card 3, Card 7, Card 8, Card 10, Card 18, Card 19, Card 20

### Contexto

Sem o registro no contêiner de injeção de dependências, nenhum dos serviços e repositórios criados nos cards anteriores será encontrado pelo ASP.NET Core, gerando erros em tempo de execução.

### Artefatos a modificar

| Arquivo | Caminho completo |
| --- | --- |
| `ConfiguracoesInjecoesDependencia.cs` | `GerenciamentoUsuarios.Ioc/` |
| `ConfiguracoesAutoMapper.cs` | `GerenciamentoUsuarios.Ioc/` |

### Especificação

**Em `ConfiguracoesInjecoesDependencia`**, adicionar os registros na seguinte ordem:

| Interface | Implementação | Ciclo de Vida |
| --- | --- | --- |
| `IUsuariosRepositorio` | `UsuariosRepositorio` | `Scoped` |
| `IUsuariosServicos` | `UsuariosServicos` | `Scoped` |
| `IUsuariosAppServico` | `UsuariosAppServico` | `Scoped` |
| `TokenServicos` | `TokenServicos` | `Scoped` |

**Em `ConfiguracoesAutoMapper`**, adicionar ao `MapperConfiguration`:

- `config.AddProfile<UsuariosProfile>()`

### Restrições

- ⛔ Não usar `AddSingleton` para `UsuariosRepositorio`, `UsuariosServicos` ou `UsuariosAppServico` — todos dependem do `AppDbContext` que é Scoped
- ⛔ Não esquecer o `UsuariosProfile` no AutoMapper — sua ausência causará erro silencioso de mapeamento em tempo de execução

### Critério de conclusão

O card está concluído quando:

- [ ] Os quatro pares interface/implementação estão registrados como `Scoped`
- [ ] `UsuariosProfile` foi adicionado ao `MapperConfiguration`
- [ ] O projeto compila sem erros após as alterações

---

## Card 24 — Program.cs

**Camada:** Api
**Instruction:** `.github/instructions/program.instructions.md`
**Depende de:** Card 23

### Contexto

O `Program.cs` inicializa toda a aplicação, conectando os serviços registrados no IoC, a autenticação JWT, o Swagger e os middlewares em uma sequência precisa. Esta é a última peça para que a API esteja funcional.

### Artefatos a criar

| Arquivo | Caminho completo |
| --- | --- |
| `Program.cs` | `GerenciamentoUsuarios.Api/` |
| `appsettings.json` | `GerenciamentoUsuarios.Api/` |

### Especificação

**`Program.cs` — ordem de registro dos serviços:**

1. `AddInjecoesDependencia()` — repositórios, serviços e AppServicos
2. `AddAutoMapper()` — profiles AutoMapper
3. `AddDbContext(configuration)` — contexto EF Core com SQL Server
4. `AddControllers()`
5. `AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(...)` — lendo `Jwt:Issuer`, `Jwt:Audience` e `Jwt:Key` do `IConfiguration`
6. `AddEndpointsApiExplorer()` + `AddSwaggerGen(...)` — com suporte a Bearer JWT no Swagger
7. `AddCors(...)` — política padrão permitindo qualquer origem, método e cabeçalho

**`Program.cs` — ordem dos middlewares (pipeline):**

1. `UseSwagger()` + `UseSwaggerUI()` — apenas em ambiente Development
2. `UseCors()` — obrigatoriamente antes de autenticação
3. `UseAuthentication()`
4. `UseAuthorization()`
5. `MapControllers()`

**`appsettings.json` — estrutura obrigatória (sem valores sensíveis):**

- Seção `ConnectionStrings:Default` — vazia, preenchida via user-secrets
- Seção `Jwt:Key` — vazia, preenchida via user-secrets
- Seção `Jwt:Issuer` — pode ter valor padrão (ex: nome da aplicação)
- Seção `Jwt:Audience` — pode ter valor padrão

### Restrições

- ⛔ Nunca commitar valores reais de `ConnectionStrings`, `Jwt:Key` ou qualquer segredo em `appsettings.json` — usar `dotnet user-secrets` em desenvolvimento
- ⛔ `UseCors()` deve vir antes de `UseAuthentication()` — ordem incorreta causa falhas de autenticação em requisições cross-origin

### Critério de conclusão

O card está concluído quando:

- [ ] `Program.cs` foi criado com todos os serviços registrados na ordem correta
- [ ] JWT Bearer está configurado lendo as chaves do `IConfiguration`
- [ ] Swagger está configurado com suporte ao token Bearer
- [ ] A ordem dos middlewares está correta (`UseCors` → `UseAuthentication` → `UseAuthorization` → `MapControllers`)
- [ ] `appsettings.json` existe com a estrutura correta e sem segredos
- [ ] `dotnet run --project GerenciamentoUsuarios.Api/GerenciamentoUsuarios.Api.csproj` inicia sem erros
