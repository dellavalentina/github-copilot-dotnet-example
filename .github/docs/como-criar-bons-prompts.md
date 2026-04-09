# Como Criar Bons Prompts

---

## O Problema

Compare os dois prompts abaixo enviados para a mesma IA:

**Prompt ruim**
```
Me ajuda com e-mail
```

**Prompt bom**
```
Você é um especialista em comunicação corporativa. Escreva um e-mail formal de 3 parágrafos
solicitando prorrogação de prazo de entrega para um cliente. Tom respeitoso, sem jargões.
```

A diferença de qualidade no resultado não está na IA — está no que foi pedido. A IA não é burra. Ela só executa o que recebe.

---

## O Que é um Prompt?

Um prompt é qualquer texto que você envia para uma IA como instrução. É a sua forma de se comunicar com o modelo.

**Analogia:** É como dar uma ordem para um estagiário muito inteligente que não conhece o seu contexto. Quanto mais claro você for, melhor ele performa.

### Anatomia de um bom prompt

| Bloco              | O que define                              |
| ------------------ | ----------------------------------------- |
| **Instrução**      | O que fazer                               |
| **Contexto**       | Quem você é e qual é o cenário            |
| **Exemplo**        | Como deve ser o resultado (se necessário) |
| **Saída desejada** | Formato, tamanho, tom                     |

### Prompt de usuário vs. prompt de sistema

- **Prompt de usuário:** o que você digita no chat
- **Prompt de sistema:** instrução de bastidores que molda o comportamento padrão da IA — por exemplo, `"Você é um assistente de atendimento da empresa X"`. Na API, é o `system message`.

---

## Erros Mais Comuns

### Erro 1 — Prompt vago sem contexto

```
Escreve um texto sobre produto
```

A IA não sabe qual produto, para quem, qual canal, qual tom. O resultado será genérico.

> **Regra:** se um humano sem contexto não conseguiria executar o pedido, a IA também não vai.

---

### Erro 2 — Pedir tudo de uma vez

```
Analise este código, refatore, documente, gere testes e explique para o time
```

A IA divide atenção e entrega tudo pela metade. Faça um pedido por vez e encadeie as respostas.

---

### Erro 3 — Ignorar o formato da saída

```
Liste os benefícios do produto
```

Se você queria bullet points para um slide mas não disse isso, a IA pode devolver 3 parágrafos corridos. Sempre diga *como* quer o conteúdo: lista, tabela, parágrafos, JSON, código.

---

### Erro 4 — Não iterar quando o resultado é ruim

Receber uma resposta mediana e aceitar é o erro mais comum. Trate cada resposta como rascunho.

```
Refaça, mas mais direto
```
```
Mantenha o tom, mas reduza para 3 linhas
```

---



## Os Itens de um Bom Prompt


### Papel — *"Você é um..."*

Define o modo de pensar da IA: vocabulário, tom e profundidade.

```
Você é um redator de marketing com 10 anos de experiência em SaaS B2B
```
```
Você é um professor de matemática para alunos do ensino médio
```

Sem papel definido, a IA responde como generalista — funciona, mas não é ótimo.

---

### Contexto — *O cenário e o domínio*

Responde a: onde isso acontece? Para quem? Em qual momento?

```
Estou preparando uma apresentação de 10 minutos para diretores que não conhecem o produto
```

Contexto resolve 80% das ambiguidades.

---

### Tarefa — *O verbo e o objetivo*

Seja específico: não "fale sobre X", mas **liste**, **escreva**, **compare**, **resuma**, **sugira**.

```
Gere 5 opções de título para este artigo
```
```
Resuma o texto abaixo em até 3 frases
```

---

### Formato — *Como deve ser a saída*

```
Responda em formato de tabela com colunas: Vantagem | Desvantagem | Quando usar
```

Especialmente importante quando o output vai direto para slides, e-mails ou documentos.

**Boa prática: defina blocos obrigatórios**

Não basta dizer "use tabela" ou "use lista". Para resultados consistentes, especifique *quais blocos* compõem cada item da saída e em que ordem eles aparecem. Isso é especialmente poderoso em prompts reutilizáveis.

Exemplo — prompt para análise de concorrente:

```
Para cada concorrente listado, entregue exatamente 3 blocos, nesta ordem:
1. **Posicionamento** — como eles se descrevem no mercado (1 frase)
2. **Diferencial** — o que os torna únicos em relação a nós (2 a 3 frases)
3. **Ponto fraco** — onde há oportunidade para nos destacar (1 frase)
```

Com blocos obrigatórios e ordem definida, o resultado é previsível, comparável e fácil de usar em documentos ou apresentações.

---

### Restrições — *O que evitar*

```
Não use jargão técnico. Máximo de 150 palavras. Não mencione concorrentes.
```

---

### Regras Explícitas — *O que fazer e o que não fazer*

Uma boa prática poderosa é incluir no prompt uma seção de regras explícitas dividida em dois blocos: o que a IA **deve** fazer e o que ela **não deve** fazer em nenhuma hipótese. Isso elimina comportamentos indesejados e garante consistência, especialmente em prompts reutilizáveis.

**Estrutura recomendada:**

```
### O que você deve fazer
- Responder sempre em português brasileiro
- Usar exemplos práticos para ilustrar cada conceito
- Manter o tom direto e objetivo

### O que você não deve fazer
- Inventar informações que não foram fornecidas
- Usar jargão técnico sem explicação
- Responder com mais de 200 palavras
```

Quanto mais específicas as regras, menor a margem para interpretações incorretas. Use este bloco principalmente quando:

- O prompt será reutilizado várias vezes ou por outras pessoas
- A IA tende a adicionar conteúdo que você não pediu (introduções genéricas, conclusões desnecessárias etc.)
- O resultado precisa seguir um padrão rígido de formato ou tom

---

## Checklist: Audite Seu Prompt Antes de Enviar

Antes de enviar qualquer prompt, classifique cada dimensão abaixo como **resolvido**, **ambíguo** ou **ausente**. Só envie quando todas estiverem resolvidas.

| Dimensão        | Pergunta que você deve responder                              |
| --------------- | ------------------------------------------------------------- |
| **Objetivo**    | O que a IA deve entregar concretamente?                       |
| **Papel**       | Qual especialista a IA deve simular?                          |
| **Contexto**    | Qual o cenário, domínio e para quem?                          |
| **Público**     | Quem vai consumir o resultado?                                |
| **Formato**     | Como deve ser a saída? Quais blocos obrigatórios?             |
| **Restrições**  | O que a IA não deve fazer ou incluir?                         |
| **Regras**      | Há comportamentos que precisam ser garantidos explicitamente? |

> Toda célula **ambígua** ou **ausente** é um ponto onde a IA vai adivinhar — e geralmente erra. Complete antes de enviar.

---

### Exemplo: melhore este prompt

Prompt original:
```
Escreve algo sobre nossa empresa para redes sociais
```

Aplicando os 6 ingredientes:

```
Você é um redator de conteúdo especializado em redes sociais corporativas. Escreva uma legenda
para o LinkedIn apresentando a empresa [Nome], que atua em [setor], para atrair profissionais
de tecnologia. Tom: profissional e acessível. Máximo de 100 palavras. Inclua 1 chamada para ação.
```

---

## Técnica Avançada: Meta Prompt

Meta prompt é usar a própria IA para criar ou melhorar um prompt. Em vez de tentar escrever o prompt perfeito do zero, você descreve sua intenção e pede que a IA monte o prompt por você.

### Quando usar

- Quando você sabe o que quer, mas não sabe como pedir
- Quando precisa de um prompt que será reutilizado várias vezes
- Quando quer documentar prompts padronizados para o seu time

### Como funciona na prática

**Passo 1 — Ponto de partida (prompt ruim)**

```
Escreve um post sobre produto novo
```

Resultado: genérico, sem personalidade, sem direção.

---

**Passo 2 — O meta prompt**

```
Você é um engenheiro de prompts especialista. O prompt abaixo foi escrito de forma vaga e está
gerando resultados genéricos. Reescreva-o com todos os detalhes necessários para que qualquer IA
entenda e execute corretamente na primeira tentativa. Pergunte-me o que precisar para preencher
as lacunas.

Prompt original: "Escreve um post sobre produto novo"
```

A IA vai devolver perguntas (público-alvo, canal, tom, benefício principal etc.) ou já vai gerar uma versão melhorada.

---

**Passo 3 — O prompt otimizado**

```
Você é um redator de conteúdo especializado em lançamentos de produto para redes sociais.
Escreva um post de Instagram anunciando o lançamento de [nome do produto], destacando o principal
benefício para [público-alvo]. Tom: animado e acessível. Máximo 150 palavras. Inclua 1 chamada
para ação e 3 hashtags relevantes.
```

O contraste entre o prompt do Passo 1 e o resultado do Passo 3 mostra o poder da técnica.

---

## Colocando Tudo em Prática

Exemplo completo: **escrever um e-mail de cobrança para um cliente em atraso**.

---

**Versão 1 — sem ingredientes**

```
Escreve um e-mail de cobrança
```

Resultado: vago, tom inadequado ou muito genérico.

---

**Versão 2 — adicionando papel e contexto**

```
Você é um analista financeiro de uma empresa de software. Preciso de um e-mail para um cliente
que está com uma fatura em aberto há 15 dias.
```

---

**Versão 3 — adicionando formato e restrições**

```
Você é um analista financeiro de uma empresa de software. Escreva um e-mail para um cliente
que está com uma fatura de R$ 4.800 em aberto há 15 dias. Tom: profissional, cordial, sem
ameaças. Estrutura: 1 parágrafo de contexto, 1 com os dados da fatura, 1 com o próximo passo.
Máximo de 150 palavras.
```

---

**Versão 4 — refinamento via conversa**

```
Bom. Agora refaça com um tom um pouco mais firme — o cliente já foi avisado antes.
Mantenha a estrutura.
```

Cada iteração adiciona clareza. O prompt evolui junto com o seu entendimento do que você precisa.

---

**Versão 5 — com regras explícitas**

```
Você é um analista financeiro de uma empresa de software. Escreva um e-mail para um cliente
que está com uma fatura de R$ 4.800 em aberto há 15 dias. Tom: firme, mas profissional.
Estrutura: 1 parágrafo de contexto, 1 com os dados da fatura, 1 com o próximo passo.
Máximo de 150 palavras.

### O que você deve fazer
- Citar o número da fatura e o valor exato
- Deixar claro o prazo para regularização
- Incluir o contato para dúvidas

### O que você não deve fazer
- Usar linguagem ameaçadora ou agressiva
- Mencionar consequências jurídicas
- Adicionar saudações genéricas como "Espero que esteja bem"
```

Com regras explícitas, o resultado é previsível e reutilizável — mesmo que outra pessoa use o prompt, o e-mail sairá dentro dos padrões esperados.

---

## As 4 Regras de Ouro

1. **Seja específico** — contexto e papel fazem toda a diferença. Se um humano sem contexto não conseguiria executar seu pedido, a IA também não vai.

2. **Itere sempre** — o primeiro resultado raramente é o melhor. Trate cada resposta como rascunho. Refine, oriente, ajuste o tom, o formato, o foco.

3. **Use a IA para melhorar seus prompts** — o meta prompt é seu melhor atalho. Para prompts que você vai reutilizar, peça à própria IA para refiná-los antes de salvar.

4. **Defina regras explícitas** — inclua no prompt o que a IA deve e não deve fazer. Quanto mais específicas as regras, menor a margem para interpretações incorretas e resultados fora do padrão.

---

## Recursos para Aprofundamento

- [Prompt Engineering Guide](https://www.promptingguide.ai) — referência técnica abrangente
- [Learn Prompting](https://learnprompting.org) — tutoriais práticos e gratuitos
- [OpenAI Playground](https://platform.openai.com/playground) — para praticar com diferentes modelos
- **GitHub Copilot + `.prompt.md`** — para quem desenvolve: versione seus melhores prompts como código

---

## Referência Rápida

| Ingrediente    | Pergunta-chave                   |
| -------------- | -------------------------------- |
| **Papel**      | Quem a IA deve ser?              |
| **Contexto**   | Qual é o cenário?                |
| **Tarefa**     | O que exatamente ela deve fazer? |
| **Formato**    | Como deve ser a saída?           |
| **Restrições**       | O que ela não deve fazer?                  |
| **Regras Explícitas** | O que a IA deve e não deve fazer?         |

**4 Regras de Ouro:** Seja específico · Itere sempre · Use meta prompt · Defina regras explícitas
