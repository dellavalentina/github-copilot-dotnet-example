# Relatório de Análise — Pipeline de Desenvolvimento por Agentes

> Análise completa do fluxo `descrever-feature` → `servicos-azure` → `cards-desenvolvimento` → `auditar-feature`, com diagnóstico de gargalos e propostas de melhoria usando recursos do GitHub Copilot.

---

## 1. Visão Geral do Fluxo Atual

```
Humano (ideia) 
    → descrever-feature.prompt.md      (gera feature-<nome>.md)
    → servicos-azure.prompt.md         (gera infraestrutura-azure-<nome>.md)
    → cards-desenvolvimento.prompt.md  (gera cards-desenvolvimento-<nome>.md)
    → [execução manual de cada card]
    → auditar-feature.prompt.md        (gera relatório de auditoria)
```

**Prompts auxiliares existentes:** `turbo-prompt.prompt.md` (enriquecer contexto), `escrever-prompt.prompt.md` (criar prompts).

---

## 2. Gargalos Identificados

### G1 — Sem orquestração entre etapas

Cada prompt é independente. O humano precisa invocar manualmente cada um, copiar/colar nomes de arquivos, e garantir a sequência correta. Se errar a ordem ou esquecer um passo, o fluxo quebra.

**Impacto:** Alto. É o gargalo principal. Transforma um fluxo que poderia ser automatizado em 4+ interações manuais separadas.

---

### G2 — Perda de contexto entre sessões

Cada invocação de prompt inicia uma nova conversa. Decisões tomadas em `descrever-feature` (nome da feature, campos, regras) não são carregadas automaticamente em `cards-desenvolvimento`. O agente precisa reler os arquivos gerados — e às vezes refaz perguntas que já foram respondidas.

**Impacto:** Alto. Causa redundância de perguntas e risco de inconsistências se o agente interpretar diferente da primeira sessão.

---

### G3 — Três rodadas de perguntas para uma feature

| Prompt                  | Perguntas típicas                                                         |
| ----------------------- | ------------------------------------------------------------------------- |
| `descrever-feature`     | Problema, atores, ações, dados, regras, casos especiais                   |
| `servicos-azure`        | Infraestrutura existente, e-mails, arquivos, jobs, região, ambiente       |
| `cards-desenvolvimento` | Nome da solution, versão .NET, primeira feature, JWT, serviços existentes |

Muitas perguntas se sobrepõem ou poderiam ser consolidadas. O humano responde de 10 a 20 perguntas espalhadas por 3 sessões.

**Impacto:** Médio-alto. Experiência fragmentada e cansativa para o humano.

---

### G4 — Não existe prompt de execução dos cards

`cards-desenvolvimento` gera um documento com 20+ cards, mas nenhum prompt consome esses cards para implementá-los. O humano precisa copiar cada card e pedir ao agente para executar — ou usar `criar-crud` que ignora completamente os cards.

**Impacto:** Crítico. É a maior lacuna funcional. Os cards são um plano excelente que ninguém executa automaticamente.

---

### G5 — Sem verificação incremental (build após cada card)

Cada card diz "o projeto compila sem erros após a adição", mas nenhum mecanismo roda `dotnet build` automaticamente. Erros se acumulam e só são descobertos no final (ou na auditoria).

**Impacto:** Alto. Um erro no Card 2 (entidade) pode causar falhas em cascata nos Cards 3-24. Quanto mais tarde a detecção, mais difícil a correção.

---

### G6 — Auditoria é somente leitura

`auditar-feature` encontra problemas mas tem uma regra explícita: "Não corrija o código automaticamente". Isso é bom para relatórios, mas exige mais uma etapa manual para corrigir cada violação encontrada.

**Impacto:** Médio. Adiciona um ciclo extra (auditar → corrigir → re-auditar) que poderia ser parcialmente automatizado.

---

### G7 — Sem gestão de estado/progresso

Não há forma de saber "em qual card estou?" ou "quais cards já foram executados?". Se a sessão cair ou o humano pausar, precisa recomeçar a análise do zero.

**Impacto:** Médio. Perda de progresso em features grandes (20+ cards).

---

### G8 — Migration depende de ambiente externo

O Card 12 (migration) exige SQL Server rodando e connection string configurada. Se o ambiente não estiver pronto, o fluxo inteiro para. Não há tratamento para esse cenário.

**Impacto:** Médio. Bloqueio frequente em ambientes recém-criados.

---

## 3. Recursos do GitHub Copilot Subutilizados

### R1 — Custom Agents (`.github/agents/*.agent.md`)

**O que é:** Agentes customizados com personalidade, ferramentas e instruções específicas. Podem ser invocados pelo nome no chat.

**Como usar:** Criar um agente orquestrador que roda o pipeline completo. O humano fala com um único agente que internamente decide qual prompt executar.

---

### R2 — Sub-agentes (`runSubagent`)

**O que é:** Capacidade de um agente delegar tarefas para sub-agentes especializados.

**Como usar:** O orquestrador delega cada etapa para um sub-agente especializado (um para feature, um para cards, um para execução). Cards independentes podem ser executados em paralelo.

---

### R3 — Todo List (`manage_todo_list`)

**O que é:** Lista de tarefas persistente na sessão, visível na UI do VS Code.

**Como usar:** Cada card vira um item da todo list. O agente marca "in-progress" ao iniciar, "completed" ao terminar + build com sucesso. O humano vê o progresso em tempo real.

---

### R4 — Session Memory (`/memories/session/`)

**O que é:** Notas persistentes dentro da sessão atual.

**Como usar:** Salvar decisões do humano (nome da solution, versão .NET, campos, regras) na session memory. Todos os prompts subsequentes leem em vez de perguntar novamente.

---

### R5 — Terminal (`run_in_terminal`)

**O que é:** Execução de comandos no terminal com captura de output.

**Como usar:** Rodar `dotnet build` após cada card. Rodar `dotnet ef migrations add`. Rodar `dotnet run` no final. Detecção automática de erros de compilação.

---

### R6 — Get Errors (`get_errors`)

**O que é:** Verificação de erros de compilação/lint diretamente de arquivos.

**Como usar:** Após criar/editar um arquivo, chamar `get_errors` para verificar sem precisar rodar build completo. Mais rápido para validação incremental.

---

### R7 — Arquivo `.prompt.md` com variáveis

**O que é:** Prompts podem usar variáveis `{{variavel}}` que o VS Code solicita ao usuário antes de executar.

**Como usar:** Um prompt `executar-card.prompt.md` com `{{card_number}}` e `{{feature_name}}` que executa um card específico lendo o arquivo de cards.

---

## 4. Propostas de Melhoria

### Nível 1 — Quick Wins (sem reestruturação)

#### M1 — Criar `executar-cards.prompt.md`

Um prompt que:
1. Recebe o nome da feature
2. Lê o arquivo `cards-desenvolvimento-<nome>.md`
3. Usa `manage_todo_list` para criar um item por card
4. Executa cada card na ordem, marcando progresso
5. Após cada card, roda `dotnet build` via terminal
6. Se falhar, tenta corrigir; se não conseguir, para e reporta
7. Ao final, roda `dotnet run` para verificação completa

**Resolve:** G4, G5, G7.

---

#### M2 — Criar `corrigir-auditoria.prompt.md`

Um prompt que:
1. Recebe o relatório de auditoria (output do `auditar-feature`)
2. Para cada violação ❌, aplica a correção seguindo as instructions
3. Para cada atenção ⚠️, avalia se vale corrigir
4. Roda build incremental após cada correção
5. Re-executa a auditoria no final para confirmar

**Resolve:** G6.

---

#### M3 — Consolidar perguntas iniciais

Adicionar ao `descrever-feature` as perguntas de infra/configuração que hoje estão espalhadas nos outros prompts:

- Nome da solution
- Versão do .NET
- Primeira feature? JWT existe?
- Região Azure e ambiente

Salvar tudo no próprio `feature-<nome>.md` numa seção "Configuração do Projeto". Os prompts seguintes leem em vez de perguntar.

**Resolve:** G2, G3.

---

### Nível 2 — Reestruturação Moderada

#### M4 — Criar agente orquestrador (`.github/agents/pipeline-feature.agent.md`)

Um custom agent que:
1. Recebe a ideia do humano em linguagem natural
2. Executa internamente o fluxo completo:
   - Faz as perguntas consolidadas (M3)
   - Gera `feature-<nome>.md`
   - Gera `infraestrutura-azure-<nome>.md`
   - Gera `cards-desenvolvimento-<nome>.md`
   - Executa cada card com verificação de build
   - Roda a auditoria
   - Corrige violações encontradas
3. Usa `manage_todo_list` para mostrar progresso ao humano
4. Usa session memory para persistir decisões entre etapas

O humano diz: _"Crie a feature de gerenciamento de usuários com cadastro, login, listagem e desativação"_ e o agente faz tudo.

**Resolve:** G1, G2, G3, G4, G5, G6, G7.

---

#### M5 — Adicionar verificação de ambiente antes da execução

Antes de executar os cards, verificar:
- `dotnet --version` está instalado e compatível
- SQL Server está acessível (connection string funciona)
- Projetos existem (se não for primeira feature)

Se o ambiente não estiver pronto, instruir o humano sobre o que configurar antes de continuar.

**Resolve:** G8.

---

### Nível 3 — Melhorias Avançadas

#### M6 — Execução paralela de cards independentes

O documento de cards já especifica dependências (`Depende de: Card X, Card Y`). Um executor inteligente pode:
1. Montar o grafo de dependências
2. Identificar cards que podem rodar em paralelo (ex: Card 4 e Card 5 não dependem um do outro)
3. Usar sub-agentes para executar cards independentes simultaneamente
4. Sincronizar antes de cards que dependem dos anteriores

Isso pode reduzir o tempo de execução em 30-50% para features com 20+ cards.

---

#### M7 — Feedback loop com aprendizado

Após cada feature completa, salvar em `/memories/repo/`:
- Quais cards falharam na primeira tentativa
- Quais violações a auditoria encontrou
- Quanto tempo cada etapa levou

Usar esses dados para melhorar os prompts nas próximas execuções (ex: se o Card de Migration sempre falha, adicionar pré-verificação de ambiente).

---

## 5. Pipeline Proposto (Visão Final)

```
Humano: "Quero uma feature de gerenciamento de pedidos com notificação por e-mail"
    │
    ▼
┌─────────────────────────────────────────────────┐
│         pipeline-feature (Agent)                │
│                                                 │
│  1. Perguntas consolidadas (1 rodada)           │
│     → session memory: decisões salvas           │
│                                                 │
│  2. Gerar feature-<nome>.md                     │
│     → classificar complexidade                  │
│                                                 │
│  3. Gerar infraestrutura-azure-<nome>.md        │
│                                                 │
│  4. Gerar cards-desenvolvimento-<nome>.md       │
│                                                 │
│  5. Verificar ambiente (dotnet, SQL, etc.)      │
│                                                 │
│  6. Executar cards (com todo list + build)      │
│     ├── Cards independentes em paralelo         │
│     ├── dotnet build após cada card             │
│     └── Correção automática se falhar           │
│                                                 │
│  7. Migration (com verificação de conexão)      │
│                                                 │
│  8. Auditoria automática                        │
│     └── Correção automática de violações        │
│                                                 │
│  9. dotnet run (verificação final)              │
│                                                 │
│  10. Salvar aprendizados em repo memory         │
└─────────────────────────────────────────────────┘
    │
    ▼
Feature completa, compilando, auditada ✅
```

---

## 6. Prioridade de Implementação

| # | Melhoria | Esforço | Impacto | Prioridade |
|---|----------|---------|---------|------------|
| M1 | `executar-cards.prompt.md` | Médio | Crítico | **1 — Fazer agora** |
| M3 | Consolidar perguntas | Baixo | Alto | **2 — Fazer agora** |
| M2 | `corrigir-auditoria.prompt.md` | Médio | Alto | **3 — Fazer agora** |
| M4 | Agente orquestrador | Alto | Muito alto | **4 — Próximo passo** |
| M5 | Verificação de ambiente | Baixo | Médio | **5 — Próximo passo** |
| M6 | Execução paralela | Muito alto | Médio | **6 — Futuro** |
| M7 | Feedback loop | Médio | Baixo-médio | **7 — Futuro** |

---

## 7. Resumo Executivo

O pipeline atual é **conceitualmente sólido** — a decomposição em 4 etapas (descrever → infra → cards → auditar) faz sentido e os prompts individuais são bem escritos, com regras claras e saídas estruturadas.

Os problemas não estão nos prompts isolados, mas **nas costuras entre eles**:

1. **Não há automação entre etapas** — o humano é o "orchestrator", o que é lento e sujeito a erros
2. **Não há executor de cards** — o maior artefato do pipeline (o documento de cards) não tem consumidor automático
3. **Não há verificação incremental** — erros só aparecem no final

A boa notícia é que o GitHub Copilot já tem todas as ferramentas necessárias (agents, sub-agents, todo list, session memory, terminal) — só precisam ser conectadas.

**A recomendação central:** criar o `executar-cards.prompt.md` (M1) resolve o gargalo mais crítico imediatamente. Depois, evoluir para o agente orquestrador (M4) que transforma 4+ interações manuais em uma única conversa.
