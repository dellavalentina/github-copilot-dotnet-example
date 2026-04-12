---
description: "Executar os cards de desenvolvimento de uma feature. Verifica o ambiente, lê o arquivo de cards, cria todo list para acompanhamento, executa cada card na ordem de dependência, roda dotnet build após cada card e corrige erros automaticamente."
agent: agent
tools: [read, edit, search, execute, todo, vscode, agent]
---

# Executar Cards de Desenvolvimento

Atue como um Desenvolvedor Sênior que executa cards de desenvolvimento de forma metódica e disciplinada. Seu objetivo é implementar cada card seguindo rigorosamente os padrões definidos nas instructions do projeto, verificando a compilação após cada etapa.

---

## Passo 0 — Verificar Ambiente

Antes de executar qualquer card, verifique se o ambiente está pronto:

1. **Verificar .NET SDK:** Execute `dotnet --version` no terminal. Confirme que a versão instalada é compatível com o target framework definido nos cards (ex: `net8.0` requer SDK 8.x, `net9.0` requer SDK 9.x, `net10.0` requer SDK 10.x).

2. **Verificar Solution:** Procure o arquivo `.slnx` na raiz do workspace. Se não existir e o Card 0 (Setup da Solution) estiver nos cards, ele será criado. Se não existir e não houver Card 0, interrompa e informe ao usuário.

3. **Verificar Conexão com Banco:** Se houver um card de Migration nos cards, avise o usuário que será necessária uma connection string configurada quando esse card for alcançado. Não bloqueie a execução agora — apenas informe com antecedência.

Se alguma verificação crítica falhar (SDK ausente), interrompa e oriente o usuário sobre o que configurar.

---

## Passo 1 — Carregar Contexto

1. Pergunte ao usuário o nome da feature usando `vscode_askQuestions` (ex: `gerenciamento-usuarios`, `pedidos`).

2. Leia os seguintes arquivos na ordem:
   - `.github/copilot-instructions.md` — arquitetura e regras absolutas
   - `.github/cards-desenvolvimento-<feature>.md` — os cards a executar
   - `.github/feature-<feature>.md` — regras de negócio (para referência)

3. Identifique o nome da solution a partir do arquivo `.slnx` ou do título do documento de cards.

---

## Passo 2 — Analisar Dependências e Montar Plano

Analise o campo **"Depende de"** de cada card e monte o grafo de dependências.

Organize os cards em **níveis de execução**:

- **Nível 0:** Cards sem dependências (ex: Card 0 — Setup)
- **Nível 1:** Cards que dependem apenas do Nível 0
- **Nível 2:** Cards que dependem de Nível 0 e/ou Nível 1
- E assim por diante.

Cards no mesmo nível são independentes entre si e podem ser executados em qualquer ordem dentro do nível.

> **Nota para otimização futura:** Cards no mesmo nível poderiam ser executados em paralelo via sub-agentes. Na execução atual, eles são executados sequencialmente dentro de cada nível, mas a análise de níveis garante a ordem correta de dependências.

---

## Passo 3 — Criar Todo List

Use `manage_todo_list` para criar a lista de tarefas com **um item por card**, na ordem de execução determinada no Passo 2.

Formato do título de cada item: `Card N — <Título do Card>`

Todos os items iniciam com status `not-started`.

---

## Passo 4 — Executar Cards

Para **cada card**, na ordem determinada no Passo 2:

### 4.1 — Preparar

1. Marque o card como `in-progress` na todo list.
2. Leia a instruction file indicada no campo **"Instruction"** do card (ex: `.github/instructions/dominio.instructions.md`).
3. Releia a especificação completa do card.

### 4.2 — Implementar

1. Crie ou edite os arquivos listados em **"Artefatos a criar/modificar"**.
2. Siga **rigorosamente** os padrões da instruction file da camada.
3. Respeite todas as **Restrições** listadas no card.
4. Use os namespaces, nomenclatura e estruturas definidos em `copilot-instructions.md`.

### 4.3 — Verificar

1. Execute `dotnet build <NomeSolution>.slnx` no terminal.
2. Se o build **passar** ✅:
   - Marque o card como `completed` na todo list.
   - Avance para o próximo card.
3. Se o build **falhar** ❌:
   - Analise os erros de compilação.
   - Tente corrigir os erros (máximo **3 tentativas**).
   - Rode `dotnet build` novamente após cada tentativa.
   - Se após 3 tentativas o build ainda falhar:
     - Salve o estado atual na session memory (card atual, erros encontrados, tentativas feitas).
     - **Pare a execução** e reporte ao usuário:
       - Qual card falhou
       - Quais erros persistem
       - O que já foi tentado
       - Sugestão de correção manual

### 4.4 — Cards Especiais

**Card de Migration (se existir):**

- Antes de executar, verifique se a connection string está configurada executando `dotnet ef` no terminal.
- Se a connection string não estiver configurada, pergunte ao usuário se deseja:
  - Configurar agora via `dotnet user-secrets`
  - Pular o card de migration e continuar com os demais
  - Parar a execução

**Card de Program.cs:**

- Após criar/editar o `Program.cs`, execute `dotnet run --project <Solution>.Api/<Solution>.Api.csproj` brevemente para verificar que a aplicação inicia sem erros.
- Use modo async e verifique a saída inicial. Não espere indefinidamente.

---

## Passo 5 — Verificação Final

Após todos os cards serem executados com sucesso:

1. Execute `dotnet build <NomeSolution>.slnx` uma última vez para confirmar que tudo compila.
2. Se houver `Program.cs`, execute `dotnet run --project <Solution>.Api/<Solution>.Api.csproj` para verificar que a aplicação inicia.
3. Apresente um resumo final:
   - Total de cards executados
   - Cards que precisaram de correção (e quais erros)
   - Build final: ✅ ou ❌
   - Próximos passos sugeridos (ex: rodar auditoria com `auditar-feature`)

---

## Passo 6 — Salvar Aprendizados

Salve em `/memories/repo/` um arquivo `execucao-<feature>.md` com:

- Data da execução
- Nome da feature e solution
- Total de cards executados com sucesso
- Cards que falharam na primeira tentativa (e qual foi o erro)
- Cards que precisaram de correção manual
- Observações úteis para futuras features

Esses dados serão usados para melhorar a geração de cards em futuras execuções.

---

## Regras de Execução

- ⛔ **Nunca pule um card** — execute na ordem de dependências, mesmo que pareça trivial
- ⛔ **Nunca ignore um erro de build** — corrija antes de avançar
- ⛔ **Nunca modifique arquivos de cards** — apenas leia e implemente
- ⛔ **Nunca invente funcionalidade** — implemente apenas o que o card especifica
- ✅ Siga as instructions da camada para cada arquivo criado
- ✅ Use `get_errors` após criar/editar arquivos para detecção rápida de erros antes do build completo
- ✅ Mantenha a todo list atualizada — o usuário acompanha o progresso por ela
- ✅ Se um card depende de outro que falhou, reporte o impedimento em vez de tentar executar
