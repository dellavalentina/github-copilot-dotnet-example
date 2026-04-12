---
description: "Corrigir violações encontradas pela auditoria de uma feature. Lê o relatório de auditoria, aplica correções seguindo as instructions do projeto, roda build incremental após cada correção e sugere re-auditoria ao final."
agent: agent
tools: [read, edit, search, execute, todo, vscode]
---

# Corrigir Violações da Auditoria

Atue como um Desenvolvedor Sênior que corrige violações de código identificadas por um processo de auditoria. Sua responsabilidade é aplicar correções que alinhem o código aos padrões definidos nas instructions do projeto — sem introduzir mudanças desnecessárias.

---

## Passo 1 — Carregar o Relatório de Auditoria

Pergunte ao usuário o nome da feature usando `vscode_askQuestions` (ex: `usuarios`, `pedidos`).

O relatório de auditoria pode estar em:

- Um arquivo `.github/auditoria-<feature>.md`
- Na conversa atual (output anterior do prompt `auditar-feature`)

Se não encontrar o relatório em nenhum desses locais, peça ao usuário que forneça o conteúdo do relatório ou execute primeiro o prompt `auditar-feature`.

---

## Passo 2 — Carregar os Padrões de Referência

Antes de corrigir qualquer coisa, leia **obrigatoriamente**:

- `.github/copilot-instructions.md`
- `.github/instructions/dominio.instructions.md`
- `.github/instructions/infra.instructions.md`
- `.github/instructions/aplicacao.instructions.md`
- `.github/instructions/datatransfer.instructions.md`
- `.github/instructions/api.instructions.md`
- `.github/instructions/ioc.instructions.md`
- `.github/instructions/jobs.instructions.md` (se a feature tiver jobs)

Identifique o nome da solution lendo o arquivo `.slnx` na raiz do projeto.

---

## Passo 3 — Planejar as Correções

Analise o relatório de auditoria e classifique os itens:

### Violações (❌) — Correção obrigatória

Para cada violação, identifique:

- Qual arquivo precisa ser alterado
- Qual regra do instructions foi violada
- Qual é a correção necessária

### Atenções (⚠️) — Avaliar caso a caso

Para cada atenção, avalie:

- A mudança melhora a conformidade com os padrões?
- Há risco de quebrar algo ao corrigir?
- Se a correção for segura e alinhada aos padrões, inclua no plano.
- Se houver risco ou não for claramente necessária, pule e informe o usuário.

### Criar Todo List

Crie um item na todo list para cada correção planejada usando `manage_todo_list`:

- Formato: `Corrigir <Camada> — <Artefato>: <Resumo do problema>`
- Ordene por camada, da mais base à mais alta: Domínio → Infra → DataTransfer → Aplicação → Api → IoC

---

## Passo 4 — Aplicar Correções

Para cada correção planejada, na ordem da todo list:

1. Marque como `in-progress` na todo list.
2. Leia o arquivo atual que precisa ser corrigido.
3. Aplique a correção conforme a instruction da camada correspondente.
4. Execute `dotnet build <NomeSolution>.slnx` para verificar que não introduziu novos erros.
5. Se o build falhar:
   - Analise o erro.
   - Ajuste a correção.
   - Rode build novamente (máximo **3 tentativas** por correção).
6. Se o build passar, marque como `completed` na todo list.
7. Se não conseguir corrigir após 3 tentativas, marque o item como bloqueado e avance para o próximo.

**Ordem de correção obrigatória:** Corrija primeiro as camadas base (Domínio, Infra) antes das camadas superiores (Aplicação, Api). Isso evita cascata de erros — uma correção na entidade pode resolver automaticamente problemas nos DTOs ou no AppServico.

---

## Passo 5 — Verificação Final

Após aplicar todas as correções:

1. Execute `dotnet build <NomeSolution>.slnx` no solution completo.
2. Apresente um resumo:
   - Violações corrigidas: X/Y
   - Atenções corrigidas: X/Y
   - Itens bloqueados: X (se houver, explique por quê)
   - Build final: ✅ ou ❌
3. Sugira ao usuário executar o prompt `auditar-feature` novamente para confirmar que todas as violações foram resolvidas.

---

## Regras de Correção

- ⛔ **Não refatore além do necessário** — corrija apenas o que o relatório aponta
- ⛔ **Não adicione funcionalidade** — correção é alinhar ao padrão, não melhorar
- ⛔ **Não altere a lógica de negócio** — se a violação sugere que a lógica de negócio está errada, reporte ao usuário em vez de alterar por conta própria
- ⛔ **Não corrija o que está marcado como ✅ Conforme** — não toque no que já está certo
- ✅ Siga estritamente as instructions da camada ao corrigir
- ✅ Rode build após cada correção individual — nunca acumule correções sem verificar
- ✅ Mantenha a todo list atualizada para o usuário acompanhar o progresso
