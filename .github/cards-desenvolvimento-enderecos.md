# Cards de Desenvolvimento — GerenciamentoUsuarios / Enderecos

> **Leia antes de executar qualquer card:**
>
> - `.github/copilot-instructions.md` — arquitetura, nomenclatura e regras absolutas do projeto
> - `.github/feature-enderecos.md` — descrição completa da feature, regras de negócio e critérios de aceite
> - `.github/infraestrutura-azure-enderecos.md` — serviços Azure utilizados
> - `.github/instructions/<camada>.instructions.md` — indicado em cada card
>
> Execute os cards na **ordem numérica**. Cada card depende dos anteriores indicados.
> Não escreva código fora do que a especificação do card pede — siga os padrões das instructions.

---

## Card 1 — Entidade Endereco

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Nenhum

### Contexto

O endereço é a unidade central desta feature. Cada endereço pertence a um único usuário e contém informações de localização (CEP, logradouro, número, etc.). Um endereço pode ser marcado como principal, indicando que é o endereço preferido do usuário.

### Artefatos a criar

| Arquivo        | Caminho completo                                             |
| -------------- | ------------------------------------------------------------ |
| `Endereco.cs`  | `GerenciamentoUsuarios.Dominio/Enderecos/Entidades/`        |

### Especificação

**Campos / Propriedades:**

| Nome         | Tipo     | Obrigatório? | Regras de validação                                |
| ------------ | -------- | :----------: | -------------------------------------------------- |
| Id           | int      |     Sim      | Gerado automaticamente pelo banco                  |
| UsuarioId    | int      |     Sim      | Referência ao usuário dono do endereço              |
| Cep          | string   |     Sim      | Não pode ser vazio ou nulo                          |
| Logradouro   | string   |     Sim      | Não pode ser vazio ou nulo                          |
| Numero       | string   |     Sim      | Não pode ser vazio ou nulo                          |
| Complemento  | string?  |     Não      | Pode ser nulo ou vazio                              |
| Bairro       | string   |     Sim      | Não pode ser vazio ou nulo                          |
| Cidade       | string   |     Sim      | Não pode ser vazio ou nulo                          |
| Estado       | string   |     Sim      | Não pode ser vazio ou nulo                          |
| Principal    | bool     |     Sim      | Indica se é o endereço principal do usuário         |
| Ativo        | bool     |     Sim      | Controle de soft delete                             |

**Comportamentos:**

- Construtor público que recebe todos os campos obrigatórios (exceto Id e Ativo) e chama os métodos Set correspondentes. O Ativo inicia como `true`
- Construtor vazio `protected` para EF Core
- Método `Set<Propriedade>` para cada propriedade mutável, com validações via `RegraDeNegocioExcecao`
- Método `SetComplemento` que aceita null sem validação (campo opcional)
- Método `DefinirComoPrincipal()` que marca `Principal = true`
- Método `RemoverPrincipal()` que marca `Principal = false`
- Métodos `Ativar()` e `Desativar()` para soft delete
- Propriedade de navegação `virtual` para `Usuario` (referência ao dono)

**Relacionamentos:**

- Pertence a um `Usuario` (many-to-one via `UsuarioId`)

### Restrições

- ⛔ Não usar DataAnnotations
- ⛔ Não colocar lógica de regra sobre "apenas 1 principal por usuário" na entidade — isso pertence ao serviço de domínio

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Dominio.Enderecos.Entidades`
- [ ] Todas as propriedades são `virtual` com setters `protected`
- [ ] Existe construtor vazio `protected`
- [ ] O projeto compila sem erros após a adição

---

## Card 2 — Interface do Repositório IEnderecosRepositorio

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Card 1

### Contexto

O repositório de endereços define o contrato de acesso a dados para a feature. Além dos métodos herdados da base, precisa de um método específico para verificar se o usuário já possui um endereço principal — essencial para a regra de "primeiro endereço é automaticamente principal".

### Artefatos a criar

| Arquivo                       | Caminho completo                                         |
| ----------------------------- | -------------------------------------------------------- |
| `IEnderecosRepositorio.cs`    | `GerenciamentoUsuarios.Dominio/Enderecos/Repositorios/`  |

### Especificação

- Herdar de `IRepositorioBase<Endereco>`
- Declarar um método assíncrono que recebe o `UsuarioId` e retorna o endereço principal ativo daquele usuário (ou null se não houver). Este método será usado pelo serviço de domínio para saber se deve marcar automaticamente como principal e para desmarcar o antigo ao definir um novo

### Restrições

- ⛔ Não definir implementação — apenas a interface

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Dominio.Enderecos.Repositorios`
- [ ] Herda de `IRepositorioBase<Endereco>`
- [ ] O projeto compila sem erros após a adição

---

## Card 3 — Comando EnderecosInserirComando

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Nenhum

### Contexto

Comando que transporta os dados necessários para criar um novo endereço. Recebe também o Id do usuário autenticado para vincular o endereço ao usuário correto.

### Artefatos a criar

| Arquivo                         | Caminho completo                                                   |
| ------------------------------- | ------------------------------------------------------------------ |
| `EnderecosInserirComando.cs`    | `GerenciamentoUsuarios.Dominio/Enderecos/Servicos/Comandos/`      |

### Especificação

**Campos / Propriedades:**

| Nome         | Tipo    | Obrigatório? |
| ------------ | ------- | :----------: |
| UsuarioId    | int     |     Sim      |
| Cep          | string  |     Sim      |
| Logradouro   | string  |     Sim      |
| Numero       | string  |     Sim      |
| Complemento  | string? |     Não      |
| Bairro       | string  |     Sim      |
| Cidade       | string  |     Sim      |
| Estado       | string  |     Sim      |

- Apenas propriedades com `get; set;` — sem lógica

### Restrições

- ⛔ Sem validações no comando — validações ficam na entidade

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Comandos`
- [ ] O projeto compila sem erros após a adição

---

## Card 4 — Comando EnderecosEditarComando

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Nenhum

### Contexto

Comando que transporta os dados para editar um endereço existente. Todos os campos exceto Id são nullable para permitir edição parcial.

### Artefatos a criar

| Arquivo                        | Caminho completo                                                   |
| ------------------------------ | ------------------------------------------------------------------ |
| `EnderecosEditarComando.cs`    | `GerenciamentoUsuarios.Dominio/Enderecos/Servicos/Comandos/`      |

### Especificação

**Campos / Propriedades:**

| Nome         | Tipo     | Obrigatório? |
| ------------ | -------- | :----------: |
| Id           | int      |     Sim      |
| Cep          | string?  |     Não      |
| Logradouro   | string?  |     Não      |
| Numero       | string?  |     Não      |
| Complemento  | string?  |     Não      |
| Bairro       | string?  |     Não      |
| Cidade       | string?  |     Não      |
| Estado       | string?  |     Não      |

- Apenas propriedades com `get; set;` — sem lógica. O `Id` é obrigatório para identificar qual endereço editar

### Restrições

- ⛔ Sem validações no comando

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Comandos`
- [ ] O projeto compila sem erros após a adição

---

## Card 5 — Filtro EnderecosListarFiltro

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Nenhum

### Contexto

Filtro de listagem que permite paginar e filtrar endereços de um usuário específico. Sempre filtra pelo UsuarioId, podendo opcionalmente filtrar por status ativo.

### Artefatos a criar

| Arquivo                       | Caminho completo                                                 |
| ----------------------------- | ---------------------------------------------------------------- |
| `EnderecosListarFiltro.cs`    | `GerenciamentoUsuarios.Dominio/Enderecos/Servicos/Filtros/`     |

### Especificação

**Campos / Propriedades:**

| Nome       | Tipo  | Obrigatório? | Descrição                                   |
| ---------- | ----- | :----------: | ------------------------------------------- |
| UsuarioId  | int   |     Sim      | Filtra endereços do usuário autenticado     |
| Ativo      | bool? |     Não      | Filtro opcional por status ativo/inativo    |

- Herda de `PaginacaoFiltro` (que fornece Qt, Pg, CpOrd, TpOrd)

### Restrições

- ⛔ Não usar tipos do DataTransfer no Domínio

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Filtros`
- [ ] Herda de `PaginacaoFiltro`
- [ ] O projeto compila sem erros após a adição

---

## Card 6 — Interface IEnderecosServicos

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Card 1, Card 2, Card 3, Card 4, Card 5

### Contexto

Interface que define o contrato do serviço de domínio de endereços. Declara os métodos para inserir, editar, desativar, recuperar, listar e definir endereço como principal.

### Artefatos a criar

| Arquivo                     | Caminho completo                                                      |
| --------------------------- | --------------------------------------------------------------------- |
| `IEnderecosServicos.cs`     | `GerenciamentoUsuarios.Dominio/Enderecos/Servicos/Interfaces/`       |

### Especificação

**Métodos a declarar:**

- `InserirAsync` — recebe `EnderecosInserirComando` e `CancellationToken`, retorna `Endereco`
- `EditarAsync` — recebe `EnderecosEditarComando` e `CancellationToken`, retorna `Endereco`
- `ExcluirAsync` — recebe `int id` e `CancellationToken`, retorna `Task` (soft delete)
- `DefinirPrincipalAsync` — recebe `int id` (do endereço) e `int usuarioId` e `CancellationToken`, retorna `Task` (marca como principal, desmarca o anterior)
- `Recuperar` — recebe `int id`, retorna `Endereco`
- `Listar` — recebe `EnderecosListarFiltro`, retorna `PaginacaoConsulta<Endereco>`

### Restrições

- ⛔ Apenas declaração — sem implementação

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Interfaces`
- [ ] O projeto compila sem erros após a adição

---

## Card 7 — Serviço de Domínio EnderecosServicos

**Camada:** Domínio
**Instruction:** `.github/instructions/dominio.instructions.md`
**Depende de:** Card 1, Card 2, Card 3, Card 4, Card 5, Card 6

### Contexto

O serviço de domínio contém toda a lógica de negócio para endereços. É aqui que vivem as regras de "primeiro endereço é principal automaticamente", "apenas 1 principal por usuário" e "usuário desativado não pode ter endereços cadastrados".

### Artefatos a criar

| Arquivo                    | Caminho completo                                            |
| -------------------------- | ----------------------------------------------------------- |
| `EnderecosServicos.cs`     | `GerenciamentoUsuarios.Dominio/Enderecos/Servicos/`        |

### Especificação

- Implementa `IEnderecosServicos`
- Injeta `IEnderecosRepositorio` e `IUsuariosRepositorio` (precisa verificar se o usuário existe e está ativo)

**Comportamentos por método:**

**InserirAsync:**
1. Buscar o usuário pelo `comando.UsuarioId` usando `IUsuariosRepositorio` — se não existir, lançar `RegraDeNegocioExcecao` ("Usuário não encontrado")
2. Se o usuário estiver desativado (Ativo == false), lançar `RegraDeNegocioExcecao` ("Não é permitido cadastrar endereço para usuário desativado")
3. Verificar se o usuário já possui endereço principal ativo (usando o método do `IEnderecosRepositorio`)
4. Se não possuir, o novo endereço deve ser marcado como principal (`true`)
5. Se já possuir, o novo endereço deve ser salvo como não principal (`false`)
6. Criar a entidade `Endereco` e persistir via repositório

**EditarAsync:**
1. Recuperar o endereço pelo Id — validar existência com `ValidarRegistroNaoFoiEncontrado`
2. Para cada campo não nulo no comando, chamar o `Set` correspondente na entidade
3. Persistir via repositório (o campo Principal não é editado aqui — existe endpoint dedicado)

**ExcluirAsync (soft delete):**
1. Recuperar o endereço pelo Id — validar existência
2. Chamar `Desativar()` na entidade
3. Persistir via `EditarAsync` do repositório

**DefinirPrincipalAsync:**
1. Recuperar o endereço pelo Id — validar existência
2. Verificar se o endereço pertence ao `usuarioId` fornecido — se não, lançar `RegraDeNegocioExcecao` ("Endereço não pertence ao usuário")
3. Buscar o endereço principal atual do usuário (pode ser null)
4. Se o endereço atual principal existir e for diferente, chamar `RemoverPrincipal()` nele e persistir
5. Chamar `DefinirComoPrincipal()` no novo endereço e persistir

**Recuperar:**
1. Buscar pelo Id — validar existência com `ValidarRegistroNaoFoiEncontrado`
2. Retornar a entidade

**Listar:**
1. Montar query via `IEnderecosRepositorio.Query()`
2. Filtrar por `UsuarioId` (obrigatório)
3. Filtrar por `Ativo` se informado
4. Aplicar paginação (Skip/Take)
5. Retornar `PaginacaoConsulta<Endereco>`

### Restrições

- ⛔ Não acessar DbContext diretamente — apenas via repositório
- ⛔ Não usar DataAnnotations
- ⛔ Toda validação via `RegraDeNegocioExcecao`

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Dominio.Enderecos.Servicos`
- [ ] Todas as regras de negócio da feature estão implementadas
- [ ] O projeto compila sem erros após a adição

---

## Card 8 — Mapeamento EF Core EnderecoConfiguration

**Camada:** Infra
**Instruction:** `.github/instructions/infra.instructions.md`
**Depende de:** Card 1

### Contexto

O mapeamento EF Core define como a entidade Endereco é traduzida para a tabela no banco de dados, configurando colunas, tipos, índices e o relacionamento com a tabela de usuários.

### Artefatos a criar

| Arquivo                       | Caminho completo                                             |
| ----------------------------- | ------------------------------------------------------------ |
| `EnderecoConfiguration.cs`    | `GerenciamentoUsuarios.Infra/Enderecos/Mapeamentos/`        |

### Especificação

- Implementa `IEntityTypeConfiguration<Endereco>`
- Nome da tabela: `enderecos`

**Mapeamento de colunas:**

| Propriedade  | Coluna         | Tipo/Tamanho           | Obrigatório? |
| ------------ | -------------- | ---------------------- | :----------: |
| Id           | id             | int, auto-incremento   |     Sim      |
| UsuarioId    | usuario_id     | int                    |     Sim      |
| Cep          | cep            | varchar(10)            |     Sim      |
| Logradouro   | logradouro     | varchar(255)           |     Sim      |
| Numero       | numero         | varchar(20)            |     Sim      |
| Complemento  | complemento    | varchar(255)           |     Não      |
| Bairro       | bairro         | varchar(255)           |     Sim      |
| Cidade       | cidade         | varchar(255)           |     Sim      |
| Estado       | estado         | varchar(2)             |     Sim      |
| Principal    | principal      | bit                    |     Sim      |
| Ativo        | ativo          | bit                    |     Sim      |

**Relacionamentos:**

- Many-to-one com `Usuario`: a entidade `Endereco` tem `UsuarioId` como foreign key apontando para `Usuario`

**Índices:**

- Índice no campo `usuario_id` para otimizar consultas por usuário

### Restrições

- ⛔ Usar apenas Fluent API — nenhum DataAnnotation
- ⛔ O mapeamento será descoberto automaticamente via `ApplyConfigurationsFromAssembly`

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Infra.Enderecos.Mapeamentos`
- [ ] Implementa `IEntityTypeConfiguration<Endereco>`
- [ ] Todos os campos estão mapeados com nomes de coluna snake_case
- [ ] O projeto compila sem erros após a adição

---

## Card 9 — Repositório EnderecosRepositorio

**Camada:** Infra
**Instruction:** `.github/instructions/infra.instructions.md`
**Depende de:** Card 1, Card 2

### Contexto

Implementação concreta do repositório de endereços. Herda os métodos CRUD do `RepositorioBase` e implementa o método específico declarado na interface para buscar o endereço principal do usuário.

### Artefatos a criar

| Arquivo                      | Caminho completo                                           |
| ---------------------------- | ---------------------------------------------------------- |
| `EnderecosRepositorio.cs`    | `GerenciamentoUsuarios.Infra/Enderecos/Repositorios/`     |

### Especificação

- Herda de `RepositorioBase<Endereco>`
- Implementa `IEnderecosRepositorio`
- Construtor recebe `AppDbContext` e repassa para a base
- Implementar o método declarado na interface que busca o endereço principal ativo de um usuário, usando `Query()` com filtro por `UsuarioId`, `Principal == true` e `Ativo == true`

### Restrições

- ⛔ Não chamar `SaveChanges` manualmente — o `RepositorioBase` já faz isso
- ⛔ Reutilizar `Query()` do `RepositorioBase` para montar consultas

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Infra.Enderecos.Repositorios`
- [ ] Herda de `RepositorioBase<Endereco>` e implementa `IEnderecosRepositorio`
- [ ] O projeto compila sem erros após a adição

---

## Card 10 — Registrar DbSet no AppDbContext

**Camada:** Infra
**Instruction:** `.github/instructions/infra.instructions.md`
**Depende de:** Card 1

### Contexto

O AppDbContext precisa declarar um `DbSet<Endereco>` para que o EF Core reconheça a nova entidade e inclua a tabela nas migrations.

### Artefatos a modificar

| Arquivo            | Caminho completo                                    |
| ------------------ | --------------------------------------------------- |
| `AppDbContext.cs`  | `GerenciamentoUsuarios.Infra/Contexto/`             |

### Especificação

- Adicionar `DbSet<Endereco> Enderecos` no `AppDbContext`
- Adicionar o `using` necessário para o namespace da entidade `Endereco`

### Restrições

- ⛔ Não alterar nenhuma outra parte do arquivo
- ⛔ Não registrar o mapeamento manualmente — `ApplyConfigurationsFromAssembly` já faz isso automaticamente

### Critério de conclusão

- [ ] O `DbSet<Endereco>` foi adicionado ao `AppDbContext`
- [ ] O projeto compila sem erros após a adição

---

## Card 11 — Migration EF Core

**Camada:** Infra
**Instruction:** `.github/instructions/infra.instructions.md`
**Depende de:** Card 8, Card 10

### Contexto

A migration cria a tabela de endereços no banco de dados, refletindo o mapeamento configurado no `EnderecoConfiguration`.

### Artefatos a criar

| Artefato  | Descrição                                                                   |
| --------- | --------------------------------------------------------------------------- |
| Migration | Gerada pelo comando `dotnet ef migrations add AdicionarTabelaEnderecos`     |

### Especificação

- Executar o comando `dotnet ef migrations add AdicionarTabelaEnderecos` apontando para o projeto de Infra como projeto de migrations e o projeto Api como startup project
- Se a connection string não estiver configurada, perguntar ao usuário se deseja configurar via `dotnet user-secrets` ou pular este card

### Restrições

- ⛔ Não editar manualmente os arquivos de migration gerados
- ⛔ Não executar `dotnet ef database update` sem confirmação do usuário

### Critério de conclusão

- [ ] Arquivos de migration foram gerados na pasta `Migrations/` do projeto Infra
- [ ] O projeto compila sem erros após a geração

---

## Card 12 — DTO EnderecosInserirRequest

**Camada:** DataTransfer
**Instruction:** `.github/instructions/datatransfer.instructions.md`
**Depende de:** Nenhum

### Contexto

DTO de entrada para o endpoint de cadastro de endereço. Não inclui `UsuarioId` porque o usuário será identificado pelo token JWT no controller.

### Artefatos a criar

| Arquivo                        | Caminho completo                                                  |
| ------------------------------ | ----------------------------------------------------------------- |
| `EnderecosInserirRequest.cs`   | `GerenciamentoUsuarios.DataTransfer/Enderecos/Request/`           |

### Especificação

**Campos / Propriedades:**

| Nome         | Tipo     |
| ------------ | -------- |
| Cep          | string   |
| Logradouro   | string   |
| Numero       | string   |
| Complemento  | string?  |
| Bairro       | string   |
| Cidade       | string   |
| Estado       | string   |

- Apenas propriedades com `get; set;`

### Restrições

- ⛔ Sem DataAnnotations — validações ficam no Domínio
- ⛔ Sem lógica — apenas propriedades

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.DataTransfer.Enderecos.Request`
- [ ] O projeto compila sem erros após a adição

---

## Card 13 — DTO EnderecosEditarRequest

**Camada:** DataTransfer
**Instruction:** `.github/instructions/datatransfer.instructions.md`
**Depende de:** Nenhum

### Contexto

DTO de entrada para o endpoint de edição de endereço. Todos os campos exceto Id são nullable para permitir edição parcial.

### Artefatos a criar

| Arquivo                       | Caminho completo                                                  |
| ----------------------------- | ----------------------------------------------------------------- |
| `EnderecosEditarRequest.cs`   | `GerenciamentoUsuarios.DataTransfer/Enderecos/Request/`           |

### Especificação

**Campos / Propriedades:**

| Nome         | Tipo     |
| ------------ | -------- |
| Id           | int      |
| Cep          | string?  |
| Logradouro   | string?  |
| Numero       | string?  |
| Complemento  | string?  |
| Bairro       | string?  |
| Cidade       | string?  |
| Estado       | string?  |

- Apenas propriedades com `get; set;`

### Restrições

- ⛔ Sem DataAnnotations

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.DataTransfer.Enderecos.Request`
- [ ] O projeto compila sem erros após a adição

---

## Card 14 — DTO EnderecosListarRequest

**Camada:** DataTransfer
**Instruction:** `.github/instructions/datatransfer.instructions.md`
**Depende de:** Nenhum

### Contexto

DTO de entrada para o endpoint de listagem de endereços do usuário. Herda paginação e permite filtro opcional por status ativo.

### Artefatos a criar

| Arquivo                       | Caminho completo                                                  |
| ----------------------------- | ----------------------------------------------------------------- |
| `EnderecosListarRequest.cs`   | `GerenciamentoUsuarios.DataTransfer/Enderecos/Request/`           |

### Especificação

**Campos / Propriedades:**

| Nome   | Tipo  | Descrição                           |
| ------ | ----- | ----------------------------------- |
| Ativo  | bool? | Filtro opcional por status ativo    |

- Herda de `PaginacaoFiltro` (fornece Qt, Pg, CpOrd, TpOrd)
- O `UsuarioId` não aparece no request — será injetado pelo controller via token JWT

### Restrições

- ⛔ Sem DataAnnotations

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.DataTransfer.Enderecos.Request`
- [ ] Herda de `PaginacaoFiltro`
- [ ] O projeto compila sem erros após a adição

---

## Card 15 — DTO EnderecosResponse

**Camada:** DataTransfer
**Instruction:** `.github/instructions/datatransfer.instructions.md`
**Depende de:** Nenhum

### Contexto

DTO de saída que representa um endereço na resposta da API. Inclui a indicação de principal para que o frontend saiba qual endereço é o preferido.

### Artefatos a criar

| Arquivo                  | Caminho completo                                                   |
| ------------------------ | ------------------------------------------------------------------ |
| `EnderecosResponse.cs`   | `GerenciamentoUsuarios.DataTransfer/Enderecos/Response/`           |

### Especificação

**Campos / Propriedades:**

| Nome         | Tipo   |
| ------------ | ------ |
| Id           | int    |
| UsuarioId    | int    |
| Cep          | string |
| Logradouro   | string |
| Numero       | string |
| Complemento  | string?|
| Bairro       | string |
| Cidade       | string |
| Estado       | string |
| Principal    | bool   |
| Ativo        | bool   |

- Apenas propriedades com `get; set;`

### Restrições

- ⛔ Sem DataAnnotations
- ⛔ Sem lógica

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.DataTransfer.Enderecos.Response`
- [ ] O projeto compila sem erros após a adição

---

## Card 16 — Profile AutoMapper EnderecosProfile

**Camada:** Aplicação
**Instruction:** `.github/instructions/aplicacao.instructions.md`
**Depende de:** Card 1, Card 3, Card 4, Card 5, Card 12, Card 13, Card 14, Card 15

### Contexto

O Profile define os mapeamentos AutoMapper entre os DTOs de entrada, os comandos de domínio, a entidade e o DTO de saída.

### Artefatos a criar

| Arquivo                  | Caminho completo                                              |
| ------------------------ | ------------------------------------------------------------- |
| `EnderecosProfile.cs`    | `GerenciamentoUsuarios.Aplicacao/Enderecos/Profiles/`         |

### Especificação

**Mapeamentos a declarar:**

- `EnderecosInserirRequest` → `EnderecosInserirComando`
- `EnderecosEditarRequest` → `EnderecosEditarComando`
- `EnderecosListarRequest` → `EnderecosListarFiltro`
- `Endereco` → `EnderecosResponse`

### Restrições

- ⛔ Sem lógica nos mapeamentos — apenas declarações `CreateMap`

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Aplicacao.Enderecos.Profiles`
- [ ] O projeto compila sem erros após a adição

---

## Card 17 — Interface IEnderecosAppServico

**Camada:** Aplicação
**Instruction:** `.github/instructions/aplicacao.instructions.md`
**Depende de:** Card 12, Card 13, Card 14, Card 15

### Contexto

Interface do serviço de aplicação de endereços. Define o contrato que o controller utilizará.

### Artefatos a criar

| Arquivo                       | Caminho completo                                                         |
| ----------------------------- | ------------------------------------------------------------------------ |
| `IEnderecosAppServico.cs`     | `GerenciamentoUsuarios.Aplicacao/Enderecos/Servicos/Interfaces/`         |

### Especificação

**Métodos a declarar:**

- `InserirAsync` — recebe `EnderecosInserirRequest`, `int usuarioId`, `CancellationToken`, retorna `EnderecosResponse`
- `EditarAsync` — recebe `EnderecosEditarRequest`, `CancellationToken`, retorna `EnderecosResponse`
- `ExcluirAsync` — recebe `int id`, `CancellationToken`, retorna `Task`
- `DefinirPrincipalAsync` — recebe `int id`, `int usuarioId`, `CancellationToken`, retorna `Task`
- `Recuperar` — recebe `int id`, retorna `EnderecosResponse`
- `Listar` — recebe `EnderecosListarRequest`, `int usuarioId`, retorna `PaginacaoConsulta<EnderecosResponse>`

O `usuarioId` é passado como parâmetro separado (vem do token JWT no controller).

### Restrições

- ⛔ Apenas declaração

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Aplicacao.Enderecos.Servicos.Interfaces`
- [ ] O projeto compila sem erros após a adição

---

## Card 18 — Serviço de Aplicação EnderecosAppServico

**Camada:** Aplicação
**Instruction:** `.github/instructions/aplicacao.instructions.md`
**Depende de:** Card 6, Card 7, Card 16, Card 17

### Contexto

O serviço de aplicação orquestra o fluxo: recebe requests do controller, mapeia para comandos via AutoMapper, delega para o serviço de domínio e retorna responses. Toda operação de escrita usa UnitOfWork.

### Artefatos a criar

| Arquivo                      | Caminho completo                                                  |
| ---------------------------- | ----------------------------------------------------------------- |
| `EnderecosAppServico.cs`     | `GerenciamentoUsuarios.Aplicacao/Enderecos/Servicos/`             |

### Especificação

- Implementa `IEnderecosAppServico`
- Injeta: `IEnderecosServicos`, `IEnderecosRepositorio` (para leituras), `IUnitOfWork`, `IMapper`

**Comportamentos por método:**

**InserirAsync:** Abre transação → mapeia request para comando → injeta o `usuarioId` no comando → delega para `_enderecosServicos.InserirAsync` → commit → mapeia entidade para response

**EditarAsync:** Abre transação → mapeia request para comando → delega para `_enderecosServicos.EditarAsync` → commit → mapeia entidade para response

**ExcluirAsync:** Abre transação → delega para `_enderecosServicos.ExcluirAsync` → commit

**DefinirPrincipalAsync:** Abre transação → delega para `_enderecosServicos.DefinirPrincipalAsync` → commit

**Recuperar:** Delega para `_enderecosServicos.Recuperar` → mapeia para response (sem UnitOfWork)

**Listar:** Mapeia request para filtro → injeta o `usuarioId` no filtro → consulta via repositório com filtros → pagina → mapeia para response (sem UnitOfWork)

### Restrições

- ⛔ Não conter lógica de negócio — apenas orquestração
- ⛔ Não acessar repositório em operações de escrita — sempre via serviço de domínio
- ✅ UnitOfWork em toda operação de escrita (try/catch com Rollback)

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Aplicacao.Enderecos.Servicos`
- [ ] UnitOfWork em todas as operações de escrita
- [ ] O projeto compila sem erros após a adição

---

## Card 19 — Controller EnderecosController

**Camada:** Api
**Instruction:** `.github/instructions/api.instructions.md`
**Depende de:** Card 17

### Contexto

O controller expõe os endpoints HTTP para gerenciar endereços. Todos os endpoints requerem autenticação JWT. O usuário autenticado é identificado pelo token — ele só pode gerenciar seus próprios endereços.

### Artefatos a criar

| Arquivo                      | Caminho completo                                             |
| ---------------------------- | ------------------------------------------------------------ |
| `EnderecosController.cs`     | `GerenciamentoUsuarios.Api/Controllers/Enderecos/`           |

### Especificação

- Rota base: `api/enderecos`
- Atributos: `[Route("api/enderecos")]`, `[ApiController]`, `[Authorize]`
- Injeta apenas `IEnderecosAppServico`
- Todos os endpoints extraem o `usuarioId` do claim `ClaimTypes.NameIdentifier` do token JWT

**Endpoints:**

| Método  | Verbo    | Rota                      | Parâmetros                              | Descrição                           |
| ------- | -------- | ------------------------- | --------------------------------------- | ----------------------------------- |
| Inserir | POST     | `api/enderecos`           | `[FromBody] EnderecosInserirRequest`    | Cadastra novo endereço              |
| Editar  | PUT      | `api/enderecos`           | `[FromBody] EnderecosEditarRequest`     | Edita endereço existente            |
| Listar  | GET      | `api/enderecos`           | `[FromQuery] EnderecosListarRequest`    | Lista endereços do usuário logado   |
| Recuperar| GET     | `api/enderecos/{id}`      | `int id`                                | Recupera endereço por Id            |
| Desativar| DELETE  | `api/enderecos/{id}`      | `int id`                                | Desativa (soft delete) um endereço  |
| DefinirPrincipal | PATCH | `api/enderecos/{id}/principal` | `int id`                       | Marca endereço como principal       |

### Restrições

- ⛔ Não colocar lógica de negócio no controller
- ⛔ Não acessar repositórios ou serviços de domínio diretamente
- ⛔ Não expor entidades de domínio

### Critério de conclusão

- [ ] O arquivo foi criado no caminho correto
- [ ] O namespace segue `GerenciamentoUsuarios.Api.Controllers.Enderecos`
- [ ] Todos os endpoints extraem o `usuarioId` do JWT
- [ ] Todos os verbos HTTP estão corretos
- [ ] O projeto compila sem erros após a adição

---

## Card 20 — Registrar Dependências no IoC

**Camada:** IoC
**Instruction:** `.github/instructions/ioc.instructions.md`
**Depende de:** Card 2, Card 6, Card 7, Card 9, Card 16, Card 17, Card 18

### Contexto

Todas as novas interfaces e implementações da feature de endereços precisam ser registradas no contêiner de injeção de dependência para que o sistema as resolva em tempo de execução.

### Artefatos a modificar

| Arquivo                                  | Caminho completo                 |
| ---------------------------------------- | -------------------------------- |
| `ConfiguracoesInjecoesDependencia.cs`    | `GerenciamentoUsuarios.Ioc/`    |
| `ConfiguracoesAutoMapper.cs`            | `GerenciamentoUsuarios.Ioc/`    |

### Especificação

**Em ConfiguracoesInjecoesDependencia.cs — adicionar (Scoped):**

- `IEnderecosRepositorio` → `EnderecosRepositorio`
- `IEnderecosServicos` → `EnderecosServicos`
- `IEnderecosAppServico` → `EnderecosAppServico`

**Em ConfiguracoesAutoMapper.cs — adicionar:**

- `EnderecosProfile`

**Ordem:** Manter a organização existente — repositórios, depois serviços de domínio, depois serviços de aplicação.

### Restrições

- ⛔ Não remover registros existentes
- ⛔ Não alterar o ciclo de vida (todos Scoped)

### Critério de conclusão

- [ ] Todas as interfaces foram registradas em `ConfiguracoesInjecoesDependencia.cs`
- [ ] O `EnderecosProfile` foi registrado em `ConfiguracoesAutoMapper.cs`
- [ ] O projeto compila sem erros após a adição
