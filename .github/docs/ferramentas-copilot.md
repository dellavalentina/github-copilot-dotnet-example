# Ferramentas de Customização do GitHub Copilot

O GitHub Copilot vai muito além de completar código automaticamente. Ele possui um conjunto de ferramentas que permitem **personalizar o comportamento da IA** de acordo com o seu projeto, a sua equipe e o seu fluxo de trabalho. Em vez de ensinar a mesma coisa toda vez que você abre um chat, você pode salvar essas instruções uma única vez — e o Copilot vai segui-las automaticamente.

Essas ferramentas funcionam como "configurações de comportamento": algumas ficam sempre ativas, outras são acionadas sob demanda, e outras conectam o Copilot a sistemas externos. Juntas, elas transformam o assistente genérico em um colaborador que conhece _o seu_ projeto.

Neste documento você vai conhecer as 7 principais ferramentas de customização do GitHub Copilot, entender para que servem, ver exemplos práticos e descobrir em quais situações utilizá-las.

---

## 1. Instructions (`.instructions.md`)

### O que é

As **Instructions** são arquivos de texto que você cria para dar instruções permanentes ao Copilot — regras que ele deve seguir sempre que trabalhar em determinados arquivos. Pense como um "manual de conduta" que o Copilot lê automaticamente antes de responder qualquer coisa.

Esses arquivos ficam na pasta `.github/instructions/` do seu projeto e têm a extensão `.instructions.md`. Você pode criar um arquivo de instrução por tema (ex: um para controllers, outro para testes) e indicar para quais arquivos ele se aplica.

### Para que serve

Sem as Instructions, o Copilot não sabe nada sobre as convenções do seu projeto. Ele pode sugerir código em um estilo diferente do seu, usar nomes que não seguem seu padrão, ou gerar estruturas que você sempre rejeita. Com as Instructions, você elimina esse retrabalho: as regras ficam registradas e o Copilot as aplica automaticamente.

É especialmente útil para equipes, pois garante que todos os devs recebam sugestões consistentes com os padrões do projeto — sem precisar repetir "lembre-se de usar o padrão X" toda vez.

### Exemplos por Perfil

**Programador** — Documente as convenções do projeto em `.github/instructions/csharp.instructions.md` com `applyTo: "**/*.cs"`. O Copilot lerá esse arquivo automaticamente a cada pergunta sobre arquivos `.cs` — sem precisar repetir as regras a cada conversa.

```markdown
---
applyTo: "**/*.cs"
---

# Convenções do Projeto

## Nomenclatura
- Use nomes de métodos em português: `Inserir`, `Editar`, `Excluir`, `Listar`, `Recuperar`
- Sufixo `Async` em métodos assíncronos: `InserirAsync`, `ListarAsync`

## Banco de Dados
- Todo método que acessa banco de dados deve ser `async` e retornar `Task` ou `Task<T>`
- Sempre use `await` — nunca `.Result` ou `.Wait()`

## DTOs
- Nunca use DataAnnotations (`[Required]`, `[MaxLength]` etc.) em classes de Request ou Response
- Validações devem estar na camada de Domínio
```

**Dicas de cenários:**
- **Padrões de nomenclatura**: Sua empresa usa uma convenção própria de nomes (ex: prefixo `I` para interfaces, sufixo `Service` para serviços). Documente isso nas Instructions para o Copilot sugerir nomes corretos desde o início.
- **Arquitetura em camadas**: Se seu projeto proíbe que controllers acessem repositórios diretamente, crie uma instrução para isso. O Copilot vai evitar sugerir esse tipo de código.
- **Frameworks específicos**: Se você usa AutoMapper, Dapper ou qualquer outra biblioteca com convenções próprias, instrua o Copilot sobre como utilizá-los corretamente no seu projeto.
- **Scopo por tipo de arquivo**: Crie instruções diferentes para arquivos de teste, controllers, entidades — cada um com suas regras específicas, usando o campo `applyTo` para segmentar.

---

**Lançador / Infoprodutor** — Crie um arquivo `.github/instructions/copy.instructions.md` com o avatar do cliente ideal, a voz da marca e as regras de copy do produto (ex: nunca prometer resultado garantido, sempre abrir com a dor). O Copilot passa a aplicar esse contexto automaticamente toda vez que você gera um e-mail, post ou roteiro — sem precisar repetir o briefing da persona a cada conversa.

```markdown
---
applyTo: "**/*.md"
---

# Identidade da Marca

## Avatar
- Nome: Carla, 34 anos, empreendedora iniciante com filhos pequenos
- Maior dor: trabalha muito mas não escala
- Objeção principal: "Já tentei antes e não funcionou"

## Tom de Voz
- Próximo e direto, como conversa entre amigos
- Frases curtas, máximo 2 linhas por parágrafo
- Nunca use linguagem corporativa ou acadêmica

## Regras de Copy
- Abrir sempre com a dor antes de apresentar a solução
- Toda CTA precisa de urgência ou escassez real — nunca falsa
```

**Dicas de cenários:**
- **Produtos múltiplos**: Crie um arquivo de instrução separado por produto — cada um tem avatar e tom distintos, e misturar os contextos piora a qualidade do copy gerado.
- **Segmentação por canal**: Use `applyTo` para separar e-mails de posts e stories — cada canal tem tom e comprimento diferentes que o Copilot pode respeitar automaticamente.
- **Referências aprovadas**: Documente copies que já converteram dentro da instrução — o Copilot aprende com exemplos concretos melhor do que com regras abstratas.
- **Atualização pós-lançamento**: Revise a instrução após cada ciclo adicionando o que funcionou — o arquivo evolui junto com o aprendizado sobre o público.

---

**Gestor de Empresa** — Crie um arquivo de instrução com os modelos padrão de documentos da empresa: estrutura de ata, formato de relatório executivo e modelo de plano de ação. O Copilot passa a gerar todos esses documentos já na estrutura correta, sem precisar de ajustes de formatação após cada geração.

```markdown
---
applyTo: "**/*.md"
---

# Padrões de Documentação da Empresa

## Estrutura de Ata
- Data e participantes no topo
- Seções: Contexto | Decisões tomadas | Próximos passos
- Cada próximo passo deve ter: Responsável + Prazo (DD/MM/AAAA)
- Itens sem responsável ou prazo: marcar com ⚠️ "A definir"

## Estrutura de Relatório Executivo
- Resumo executivo (máx. 3 linhas) antes de qualquer dado
- Dados → Análise → Recomendação — nessa ordem obrigatória
- Conclusão deve sempre indicar uma ação ou decisão concreta

## Tom
- Para o time: direto e empático, sempre explique o porquê das decisões
- Para diretoria: objetivo, orientado a números e impacto de negócio
- Evite: linguagem passiva, frases longas e termos vagos como "alavancar"
```

**Dicas de cenários:**
- **Um arquivo por tipo de documento**: Separe ata, relatório e comunicado em instruções diferentes — cada um tem estrutura e tom distintos que não devem se misturar.
- **Tom por audiência via `applyTo`**: Arquivos em `relatorios-diretoria/` podem ter tom mais executivo enquanto `comunicados-time/` usa linguagem mais próxima — o Copilot aplica automaticamente conforme a pasta.
- **Memória organizacional**: Ao registrar os padrões de comunicação da empresa no arquivo, você garante consistência mesmo quando diferentes pessoas geram o documento.
- **Exceções documentadas**: Situações fora do padrão (ex: comunicados de crise) merecem seção própria para o Copilot não aplicar o modelo genérico onde não cabe.

---

**Estudante** — Crie uma instrução com o contexto do seu curso, semestre atual, disciplinas em andamento e nível de profundidade esperado nas respostas. O Copilot adapta automaticamente a linguagem técnica e o grau de detalhe das respostas — deixando de dar explicações básicas demais ou avançadas demais para o seu momento.

```markdown
---
applyTo: "**/*.md"
---

# Contexto do Estudo

## Curso e Momento Atual
- Curso: Engenharia de Software — 4º semestre
- Disciplinas em andamento: Estrutura de Dados, Banco de Dados, Engenharia de Requisitos
- Nível: já sei programar, estou aprendendo a teoria por trás

## Como devo receber as respostas
- Explique o conceito antes de usar jargões técnicos
- Use exemplos do mundo real, não só exemplos abstratos
- Quando houver código, comente cada parte importante
- Profundidade: suficiente para a prova, não para publicar um artigo

## Formato de resumo preferido
- Conceito → Definição simples → Exemplo prático → Quando usar
```

**Dicas de cenários:**
- **Instrução por disciplina**: Use `applyTo` para segmentar por pasta (`resumos/estrutura-de-dados/`) — cada matéria pode ter vocabulário e nível de profundidade próprios.
- **Atualização semestral**: Ao trocar de disciplinas, atualize a instrução para o Copilot não usar referências desatualizadas do semestre anterior.
- **Registro do que já foi coberto**: Documente na instrução os tópicos que você já domina — o Copilot para de pré-explicar o que você já sabe e vai direto ao ponto.
- **Estilo de questão do professor**: Se o professor tem um padrão de avaliação conhecido, instrua o Copilot a praticar questões nesse formato específico.

---

## 2. Prompts (`.prompt.md`)

### O que é

Os **Prompts** são templates reutilizáveis de instruções complexas que você salva como arquivos e aciona quando precisar. Ao contrário das Instructions (que ficam sempre ativas), os Prompts são chamados sob demanda — você escolhe quando usar cada um.

Eles ficam na pasta `.github/prompts/` e têm extensão `.prompt.md`. Você pode acioná-los no chat do Copilot escrevendo `#nome-do-arquivo.prompt.md` ou por um atalho no VS Code.

### Para que serve

Certas tarefas seguem sempre o mesmo roteiro — como criar um CRUD completo, escrever testes unitários para uma classe ou gerar documentação de uma API. Sem Prompts, você reescreveria as mesmas instruções longas toda vez. Com Prompts, você executa uma tarefa complexa com um único chamado.

Também são úteis para padronizar como a equipe aciona tarefas repetitivas, garantindo que todos sigam o mesmo processo.

### Exemplos por Perfil

**Programador** — Crie `.github/prompts/novo-endpoint.prompt.md` para gerar todos os arquivos de um novo endpoint (controller, serviço, DTOs e registro de dependências) em um único chamado. Ao acionar `#novo-endpoint.prompt.md` no chat, o Copilot executa o fluxo completo automaticamente.

```markdown
---
description: "Cria todos os arquivos necessários para um novo endpoint REST"
agent: agent
tools: [read_file, create_file, semantic_search]
---

# Criar Novo Endpoint REST

Você é um desenvolvedor sênior. Sua tarefa é criar todos os arquivos necessários
para um novo endpoint, seguindo a arquitetura em camadas do projeto.

## Passo 1 — Entender o Contexto

Leia os arquivos existentes para entender a estrutura atual do projeto:
- Abra um controller existente para entender o padrão
- Abra um serviço de aplicação existente

## Passo 2 — Criar os Arquivos

Crie os arquivos abaixo para o recurso indicado pelo usuário:

1. `Controllers/<Feature>Controller.cs`
2. `Servicos/<Feature>AppServico.cs`
3. `DTOs/<Feature>Request.cs`
4. `DTOs/<Feature>Response.cs`

Siga exatamente os padrões dos arquivos existentes que você leu no Passo 1.

## Regras

- Nunca coloque lógica de negócio no controller
- Use os mesmos namespaces dos arquivos existentes
- Registre as novas classes na injeção de dependência
```

**Dicas de cenários:**
- **Criação de CRUDs completos**: Em vez de pedir ao Copilot para criar cada arquivo separadamente, um único Prompt pode criar todos os arquivos do zero em sequência, respeitando a arquitetura.
- **Geração de testes**: Crie um Prompt que lê uma classe existente e gera automaticamente todos os testes unitários com o framework que seu projeto usa.
- **Code Review automatizado**: Um Prompt que analisa um arquivo aberto e lista possíveis problemas de performance, segurança e legibilidade com base nas suas convenções.
- **Revisão de Pull Request**: Um Prompt que lê os arquivos modificados e produz um resumo das mudanças em linguagem natural, facilitando o processo de review.

---

**Lançador / Infoprodutor** — Crie o Prompt `sequencia-lancamento.prompt.md` que recebe o nome do produto, a promessa principal e o avatar e gera os 7 e-mails completos da sequência de lançamento em um único chamado. O que antes ocupava um dia inteiro de redação e estruturação passa a ser gerado em minutos — restando apenas revisão e ajustes de tom.

```markdown
---
description: "Gera a sequência completa de 7 e-mails de lançamento"
agent: ask
tools: []
---

# Sequência de Lançamento

Você é um especialista em copywriting de lançamentos digitais.
Com base nas informações abaixo, crie 7 e-mails em PT-BR:

- E-mail 1: Quebra de padrão (desperta curiosidade)
- E-mail 2: Conteúdo de valor (ensina algo útil)
- E-mail 3: Identificação com a dor
- E-mail 4: Prova social (história de transformação)
- E-mail 5: Apresentação da oferta
- E-mail 6: Quebra de objeções
- E-mail 7: Urgência e escassez (último dia)

Cada e-mail deve ter: assunto, abertura com gancho, corpo e CTA.
```

**Dicas de cenários:**
- **Um Prompt por entregável recorrente**: Sequência de e-mails, posts, roteiro de stories e página de vendas se repetem a cada lançamento e merecem um Prompt próprio cada.
- **Prompt de variações**: Crie um Prompt que recebe uma copy pronta e gera 5 versões alternativas com ângulos diferentes (curiosidade, dor, prova social) — eliminando o ciclo manual de reescrita.
- **Diagnóstico de copy**: Um Prompt que recebe uma peça existente e aponta o que está fraco com base nos frameworks que já funcionaram para o seu público.
- **Versionamento por produto**: `sequencia-produto-a.prompt.md` e `sequencia-produto-b.prompt.md` garantem que as regras de copy de cada produto nunca se misturam.

---

**Gestor de Empresa** — Crie o Prompt `ata-reuniao.prompt.md` que recebe anotações brutas da reunião (pode ser texto desestruturado) e gera uma ata formatada com data, participantes, decisões tomadas, próximos passos com responsável e prazo. O gestor para de gastar 20 a 30 minutos organizando notas após cada reunião.

```markdown
---
description: "Transforma anotações brutas de reunião em ata estruturada"
agent: ask
tools: []
---

# Ata de Reunião

Você é um assistente executivo especializado em documentação corporativa.
Transforme as anotações abaixo em uma ata formal.

## Estrutura obrigatória

**Data:** [extraia das anotações ou use a data de hoje]
**Participantes:** [liste os nomes mencionados]

### Contexto
[2 a 3 frases sobre o motivo da reunião]

### Decisões Tomadas
- [Decisão 1]
- [Decisão 2]

### Próximos Passos
| Ação | Responsável | Prazo |
|---|---|---|
| [ação] | [nome] | DD/MM/AAAA |

**Regra:** itens sem responsável ou prazo definido nas anotações
devem ser marcados com ⚠️ "A definir" — nunca invente dados.
```

**Dicas de cenários:**
- **Prompts por tipo de reunião**: Weekly, one-on-one e reunião de OKR têm estruturas de ata diferentes e merecem Prompts separados.
- **Rascunho para comunicado**: Um Prompt que recebe suas anotações corridas e gera o comunicado formal já no tom e modelo da empresa — sem precisar reformatar manualmente.
- **Briefing de contratação**: Um Prompt que faz as perguntas certas sobre a vaga e gera a job description completa já no padrão da empresa.
- **Construção de PDI**: Recebe os pontos fortes e de desenvolvimento do colaborador e gera um plano estruturado com ações, recursos e prazos.

---

**Estudante** — Crie o Prompt `resumir-conteudo.prompt.md` que recebe um texto longo (artigo, capítulo, transcrição de aula) e gera um resumo estruturado com: conceitos-chave, explicação em linguagem simples de cada um, exemplos práticos e 5 perguntas de revisão. Substitui horas de fichamento manual por minutos de revisão.

```markdown
---
description: "Gera resumo didático estruturado de qualquer conteúdo de estudo"
agent: ask
tools: []
---

# Resumir Conteúdo de Estudo

Você é um tutor especializado em aprendizagem ativa.
Transforme o conteúdo abaixo em um resumo estruturado para revisão.

## Estrutura obrigatória

### Conceitos-Chave
Para cada conceito principal:
- **Nome:** definição em 1 frase simples
- **Analogia:** comparação com algo do cotidiano
- **Exemplo prático:** situação real de aplicação
- **Confundo com:** conceito parecido e como diferenciar

### Mapa de Relações
[Como os conceitos se conectam — use bullet points]

### Perguntas de Revisão
1. [Pergunta conceitual]
2. [Pergunta de aplicação prática]
3. [Pegadinha comum sobre este tema]
4. [Questão de comparação entre conceitos]
5. [Pergunta "e se..." que exige raciocínio]

**Regra:** use profundidade de prova de graduação — nem superficial,
nem nível de dissertação.
```

**Dicas de cenários:**
- **Simulado por disciplina**: Um Prompt que recebe um tema e gera 10 questões no estilo da prova do seu professor, com gabarito comentado.
- **Explicar meu erro**: Você cola o exercício e sua resposta errada e o Prompt explica onde está o erro, por que está errado e o raciocínio correto.
- **Comparação de conceitos**: Recebe dois conceitos parecidos (ex: pilha vs fila) e gera uma tabela comparativa com diferenças, semelhanças e quando usar cada um.
- **Plano de estudo semanal**: Recebe as provas da semana e o conteúdo de cada uma e gera um cronograma distribuído pelos dias disponíveis.

---

## 3. Agents (`.agent.md`)

### O que é

Os **Agents** são personas especializadas que você cria para o Copilot. Você configura um agente como se estivesse contratando um especialista: define quem ele é, o que ele sabe, quais ferramentas pode usar e como deve se comportar. Cada agente tem um foco específico e um estilo próprio.

Os arquivos de agente ficam geralmente na pasta `.github/` ou em uma pasta de configuração e têm extensão `.agent.md`. Quando você aciona um agente, o Copilot "entra no papel" daquele especialista pelo tempo da conversa.

### Para que serve

O Copilot genérico não conhece as especificidades do seu negócio, da sua stack ou do seu domínio. Um agente especializado elimina essa distância: ele já começa a conversa sabendo quem é, o que pode fazer e quais são as suas restrições. 

Isso é útil quando você quer separar contextos — um agente para infraestrutura, outro para backend, outro para revisar código — cada um com o seu conjunto de regras e responsabilidades.

### Exemplos por Perfil

**Programador** — Crie `.github/agente-seguranca.agent.md` configurado com o OWASP Top 10 e os critérios de severidade do projeto. O agente já começa cada revisão no contexto certo — sem precisar repassar o escopo a cada sessão.

```markdown
---
description: "Agente especialista em segurança de código e OWASP Top 10"
---

# Agente de Segurança

Você é um especialista em segurança de software com foco em aplicações web.
Seu objetivo é identificar vulnerabilidades no código analisado e sugerir
correções práticas e seguras.

## Sua especialidade

- OWASP Top 10 (SQL Injection, XSS, CSRF, exposição de dados sensíveis etc.)
- Autenticação e autorização (JWT, OAuth, RBAC)
- Validação e sanitização de entradas
- Gerenciamento seguro de segredos e credenciais

## Como você deve responder

- Sempre cite a categoria OWASP quando identificar uma vulnerabilidade
- Apresente o código vulnerável e a versão corrigida lado a lado
- Explique o risco em termos de impacto para o usuário final, não apenas técnico
- Priorize os problemas por severidade (Crítico, Alto, Médio, Baixo)

## O que você não deve fazer

- Nunca sugerir que um código "está provavelmente seguro" sem revisar completamente
- Não gerar código com vulnerabilidades conhecidas, mesmo que o usuário peça
```

**Dicas de cenários:**
- **Agente de banco de dados**: Um agente especializado em SQL e ORM que conhece o schema do seu banco, sugere queries otimizadas e evita N+1 queries.
- **Agente de documentação**: Um agente que assume o papel de tech writer, gerando READMEs, documentação de APIs e changelogs no estilo da sua empresa.
- **Agente de onboarding**: Um agente que conhece toda a arquitetura do projeto e responde dúvidas de devs novos, como "onde fica X?" ou "como funciona Y nesse projeto?".
- **Agente de DevOps**: Um agente especializado em pipelines CI/CD, Docker e infraestrutura, que ajuda a diagnosticar falhas de build e configurar ambientes.

---

**Lançador / Infoprodutor** — Crie um agente `especialista-lancamento.agent.md` que já conhece o produto, o avatar, os gatilhos do lançamento e o histórico de objeções do público. Toda conversa de copy começa no contexto certo — sem repassar briefing, sem explicar quem é o cliente toda vez.

```markdown
---
description: "Especialista em copy e estratégia de lançamentos digitais"
---

# Agente de Lançamento

Você é um estrategista de lançamentos com 10 anos de experiência em
produtos digitais no mercado brasileiro.

## Contexto do Produto Atual
- Produto: Método Lançamento Perpétuo
- Avatar: Carla, 34 anos, empreendedora iniciante
- Promessa: renda online automática em 30 dias
- Principal objeção: "Já tentei antes e não funcionou"

## Como você responde
- Sempre considere o avatar antes de sugerir qualquer copy
- Priorize provas sociais específicas (nome + resultado + tempo)
- Nunca sugira gatilhos de escassez falsos
```

**Dicas de cenários:**
- **Um agente por produto**: Cada produto tem avatar e objeções distintas — misturar esses contextos em um único agente genérico piora a qualidade do copy.
- **Simulação do avatar**: Peça ao agente que responda como o cliente ideal responderia a uma copy — identifica resistências antes do lançamento real.
- **Pesquisa de concorrentes**: Um agente configurado para analisar o posicionamento de outros infoprodutores do nicho serve como referência estratégica permanente.
- **Atualização com resultados reais**: Após cada lançamento, atualize o arquivo com o que converteu e o que não converteu — o agente aprende com dados reais.

---

**Gestor de Empresa** — Crie um agente `consultor-gestao.agent.md` especializado em OKRs, feedback e comunicação executiva, configurado com o setor da empresa, o tamanho do time e o ciclo de gestão vigente. O gestor consulta sobre decisões de people, planos de ação e comunicados sem precisar contextualizar a empresa a cada sessão.

```markdown
---
description: "Consultor de gestão especializado em OKRs, feedback e comunicação executiva"
---

# Agente Consultor de Gestão

Você é um consultor sênior de gestão organizacional com especialidade em
liderança de equipes, planejamento estratégico e comunicação executiva.

## Contexto da Empresa
- Setor: Tecnologia B2B | Colaboradores: 120 | Ciclo OKR: trimestral
- Ferramenta de gestão: Notion + Google Workspace
- Modelo de feedback: SCI (Situação → Comportamento → Impacto)

## Sua especialidade
- Construção e revisão de OKRs com métricas de resultado
- Feedbacks estruturados (SCI) e planos de desenvolvimento (PDI)
- Comunicados internos em momentos de mudança organizacional
- Planos de ação com responsáveis, prazos e critérios de sucesso

## Como você responde
- Seja direto — o gestor não tem tempo para rodeios
- Sempre oriente para uma decisão ou próxima ação concreta
- Para comunicados ao time: tom empático que explica o porquê
- Para comunicados à diretoria: tom executivo focado em impacto de negócio

## O que você não faz
- Nunca sugere demissão, rebaixamento ou ação disciplinar sem dados concretos
- Nunca usa linguagem ambígua em comunicados de mudança
```

**Dicas de cenários:**
- **Agentes por domínio**: Um agente para RH (feedbacks, PDIs), outro para estratégia (OKRs) e outro para comunicação executiva evitam que o contexto de um interfira no outro.
- **Simulação de reação do time**: Antes de um comunicado difícil, peça ao agente para avaliar o texto como se fosse um colaborador júnior — ele sinaliza o que pode gerar ruído.
- **Onboarding de gestores**: Um agente configurado com a cultura e os processos da empresa responde as dúvidas de gestores recém-promovidos sem sobrecarregar o RH.
- **Sincronização com OKRs**: Atualize o arquivo a cada trimestre para o agente sempre trabalhar com os objetivos vigentes, não os do ciclo passado.

---

**Estudante** — Crie um agente `tutor-pessoal.agent.md` configurado com o curso, o semestre atual, as disciplinas em andamento e o nível de conhecimento do aluno. O tutor responde dúvidas com profundidade e linguagem calibrada — nem básica demais, nem avançada demais — e nunca perde o fio do que está sendo estudado.

```markdown
---
description: "Tutor universitário personalizado para o curso e nível do aluno"
---

# Agente Tutor Pessoal

Você é um tutor universitário experiente, especializado em ensinar conceitos
complexos de forma clara e progressiva, adaptando a explicação ao nível do aluno.

## Contexto do Aluno
- Curso: Engenharia de Software — 4º semestre
- Disciplinas atuais: Estrutura de Dados, Banco de Dados, Engenharia de Requisitos
- Ponto forte: lógica de programação
- Ponto de atenção: conceitos teóricos sem aplicação prática concreta

## Como você ensina
- Explique o conceito, depois o porquê, depois o exemplo — nessa ordem
- Use analogias do cotidiano antes de jargões técnicos
- Guie o raciocínio com perguntas em vez de dar a resposta diretamente
- Avise quando um conceito costuma cair muito em provas

## O que você não faz
- Não explica conceitos de semestres futuros sem avisar o contexto
- Não simplifica ao ponto de ser impreciso
- Não pula etapas sem confirmar que o aluno entendeu o passo anterior
```

**Dicas de cenários:**
- **Um agente por disciplina**: Estrutura de Dados e Cálculo têm vocabulários e formas de raciocínio completamente diferentes — agentes separados garantem respostas mais precisas.
- **Banca simulada**: Peça ao agente que avalie sua resposta como um professor rigoroso — aponta erros conceituais antes da prova real.
- **Registro de dificuldades**: Atualize o arquivo com os conceitos que você errou ao longo do semestre — o agente dá mais ênfase a eles nas próximas sessões.
- **Revisão de trabalhos**: Configure o agente com as rubricas de avaliação do professor — ele aponta o que está fraco antes de você entregar.

---

## 4. Skills

### O que é

As **Skills** são pacotes de conhecimento especializado que você pode criar e distribuir para o Copilot. Pense em uma Skill como um livro de referência que o Copilot consulta quando precisa executar uma determinada tarefa — ela contém instruções testadas e validadas para um domínio específico.

Cada Skill fica em uma pasta própria (normalmente em uma pasta `.agents/skills/`) e contém um arquivo `SKILL.md` com o conhecimento empacotado. O Copilot carrega a Skill automaticamente quando detecta que a tarefa em andamento se encaixa na descrição dela.

### Para que serve

Certas tarefas têm uma série de boas práticas que você descobriu ao longo do tempo — e que ficam na cabeça das pessoas, não documentadas. As Skills resolvem isso: você empacota esse conhecimento acumulado em um arquivo estruturado, e ele fica disponível para o Copilot (e para toda a equipe) de forma consistente.

Também é útil quando você quer aumentar a qualidade das respostas do Copilot em um domínio específico sem precisar repassar contexto toda vez.

### Exemplos por Perfil

**Programador** — Empacote as boas práticas de testes de integração da equipe em `skills/testes-integracao/SKILL.md`. O Copilot carrega essa Skill automaticamente ao trabalhar com testes — sem ninguém precisar passar o contexto verbalmente de novo.

```markdown
# Skill: Testes de Integração

## Descrição
Use esta skill ao criar ou revisar testes de integração com banco de dados real.

## Conhecimento

### Estrutura obrigatória de um teste de integração

Todo teste de integração neste projeto deve:
1. Usar um banco de dados em memória (SQLite) para isolamento
2. Resetar o banco entre cada teste com `await _context.Database.EnsureDeletedAsync()`
3. Popular os dados de teste usando o padrão Builder
4. Nunca usar dados de produção nos testes

### Exemplo de teste correto

```csharp
[Fact]
public async Task InserirPedido_QuandoDadosValidos_DeveRetornarId()
{
    // Arrange
    await ResetarBancoDeDados();
    var pedido = new PedidoBuilder().ComCliente("João").Build();

    // Act
    var resultado = await _servico.InserirAsync(pedido);

    // Assert
    resultado.Id.Should().BePositive();
}
```

### Erros comuns a evitar

- ❌ Não compartilhe estado entre testes (dados criados em um teste não devem afetar outro)
- ❌ Não use `Thread.Sleep` — use `await Task.Delay` ou mecanismos de espera do SDK
- ❌ Não teste infraestrutura e lógica no mesmo teste
```

**Dicas de cenários:**
- **Padrões de arquitetura**: Empacote o conhecimento sobre como sua arquitetura funciona (ex: quando usar Repositório vs. Query direto, como estruturar Commands e Handlers).
- **Domínio de negócio**: Documente regras de negócio complexas que o Copilot precisa entender para gerar código correto (ex: regras de cálculo de impostos, fluxos de aprovação).
- **Boas práticas de performance**: Crie uma Skill com padrões de otimização específicos da sua stack, como uso correto de índices, cache e paginação.
- **Distribuição entre equipes**: Skills podem ser compartilhadas entre projetos e equipes, tornando-se uma base de conhecimento reutilizável da empresa.

---

**Lançador / Infoprodutor** — Crie uma Skill `copywriting-lancamento/SKILL.md` com os frameworks de copy que funcionam para o seu público (AIDA, PAS, storytelling), exemplos de copies aprovadas em lançamentos anteriores e os gatilhos mentais validados. O Copilot passa a gerar novas copies já no padrão que converte — sem as 3 ou 4 rodadas de ajuste que eram necessárias antes.

```markdown
# Skill: Copywriting de Lançamento

## Descrição
Use esta skill ao criar qualquer peça de copy para lançamentos digitais.

## Frameworks Validados

### PAS (Problema → Agitação → Solução)
Estrutura ideal para e-mails de abertura e posts de aquecimento:
1. **Problema:** nomeie a dor específica do avatar
2. **Agitação:** mostre o que acontece se o problema não for resolvido
3. **Solução:** apresente o produto como o caminho mais direto

### Gatilhos Mentais Aprovados
- ✅ Escassez real (vagas limitadas com número verdadeiro)
- ✅ Prova social específica: "João faturou R$ 12k em 30 dias"
- ✅ Antecipação: revelar o próximo passo sem entregar tudo
- ❌ Escassez falsa — nunca usar
- ❌ Promessa de resultado garantido — nunca usar
```

**Dicas de cenários:**
- **Skill por nicho**: Os frameworks de copy que funcionam para finanças são diferentes dos de emagrecimento — Skills separadas garantem que o Copilot não misture as abordagens.
- **O que não funciona**: Documente na Skill as copies que nunca converteram para o seu público — saber o que evitar é tão importante quanto saber o que usar.
- **Atualização pós-ciclo**: Após cada lançamento, adicione os aprendizados: o e-mail com maior abertura, a headline que mais converteu, o gatilho que gerou mais objeções.
- **Compartilhamento com copywriters**: Se você trabalha com um copy freelancer, a Skill garante que ele siga o mesmo padrão já validado — sem reunião de alinhamento.

---

**Gestor de Empresa** — Crie uma Skill `gestao-pessoas/SKILL.md` com o modelo SCI de feedback, o ciclo de avaliação de desempenho da empresa, exemplos de PDIs aprovados e as competências avaliadas. O Copilot aplica esse padrão automaticamente ao ajudar a redigir feedbacks, planos de desenvolvimento e comunicados de promoção — eliminando o retrabalho de adequar o formato após cada geração.

```markdown
# Skill: Gestão de Pessoas

## Descrição
Use esta skill ao redigir feedbacks, PDIs, OKRs ou comunicados de RH.

## Modelo SCI de Feedback

Toda mensagem de feedback deve seguir:
- **Situação:** contexto específico (quando e onde ocorreu)
- **Comportamento:** o que a pessoa fez — factual, sem julgamento
- **Impacto:** o efeito concreto no time ou no resultado

### Exemplo correto
> "Na sprint passada (S), você entregou as tasks sem atualizar o
> status no Jira (C), o que fez o time planejar a demo com
> informações desatualizadas (I)."

### Erros comuns a evitar
- ❌ Feedback vago: "você precisa se comunicar melhor"
- ❌ Julgamento de personalidade: "você é desorganizado"
- ❌ Sem impacto: descrever o comportamento sem dizer o que gerou
```

**Dicas de cenários:**
- **Processos de RH empacotados**: Fluxo de contratação, critérios de promoção e processo de desligamento como conhecimento empacotado evitam que o Copilot sugira algo fora do protocolo.
- **Exceções e casos especiais**: Situações fora do padrão (ex: feedback para líderes sêniors) merecem seção própria para o Copilot não aplicar o modelo genérico onde não se aplica.
- **Memória organizacional**: Ao registrar os aprendizados de gestão ao longo do tempo, você cria um ativo que não sai da empresa quando um gestor sai.
- **Compartilhamento entre gestores**: Padronizar como feedbacks, OKRs e comunicados são escritos em toda a empresa garante consistência cultural independente de quem escreve.

---

**Estudante** — Crie uma Skill `tecnicas-estudo/SKILL.md` com as técnicas de estudo que funcionam para você (Feynman, revisão espaçada, mapas mentais), o formato de resumo que facilita sua revisão e os tipos de perguntas que mais caem nas provas da disciplina. O Copilot passa a estruturar todo o material de estudo no formato que você já sabe que funciona.

```markdown
# Skill: Técnicas de Estudo

## Descrição
Use esta skill ao criar resumos, fichas de revisão ou planos de estudo.

## Técnica de Feynman (para conceitos novos)
1. Escreva o conceito como se fosse explicar para alguém de 12 anos
2. Identifique onde a explicação ficou confusa ou incompleta
3. Volte ao material e preencha as lacunas
4. Simplifique novamente — se ainda estiver difícil, você não entendeu

## Formato de Ficha de Revisão
Cada conceito deve ter:
- **O que é:** definição em 1 frase simples
- **Por que importa:** aplicação real no contexto do curso
- **Exemplo:** caso concreto
- **Confundo com:** conceito parecido e como diferenciar

## Revisão Espaçada
- Revisar no dia seguinte ao estudo
- Revisar 3 dias depois
- Revisar 1 semana depois
- Revisar na véspera da prova (não estudar conteúdo novo)
```

**Dicas de cenários:**
- **Skill por semestre**: Ao concluir cada período, arquive a Skill anterior e crie uma nova com as disciplinas atuais e os padrões relevantes para elas.
- **Estilo de questão do professor**: Cada docente tem um padrão de avaliação — registrar isso na Skill ajuda o Copilot a gerar exercícios de revisão mais realistas.
- **Erros frequentes**: Inclua na Skill os erros que você comete com frequência — o Copilot ficará em alerta para esses padrões e avisará antes de você errar de novo.
- **Compartilhamento em grupo**: Quando um grupo de estudos usa a mesma Skill, os resumos e fichas gerados têm formato consistente e são mais fáceis de revisar coletivamente.

---

## 5. Hooks

### O que é

Os **Hooks** são gatilhos automáticos que conectam o Copilot a eventos do seu fluxo de desenvolvimento. Eles funcionam como um sensor: quando um determinado evento acontece (como salvar um arquivo, fazer um commit ou abrir um Pull Request), o Hook aciona uma ação do Copilot automaticamente — sem que você precise pedir.

Você pode configurar Hooks para inspecionar código antes de um commit, gerar logs de mudança automaticamente, ou alertar sobre padrões problemáticos assim que eles aparecerem.

### Para que serve

Muitas verificações importantes são esquecidas no calor do desenvolvimento: um dev esquece de rodar os testes antes do commit, outro sobe código com um `TODO` esquecido, um terceiro não atualiza a documentação depois de mudar uma função. Os Hooks eliminam esses lapsos ao tornar as verificações automáticas — o Copilot age no momento certo, sem depender da memória do desenvolvedor.

### Exemplos por Perfil

**Programador** — Configure Hooks para automatizar as verificações que o time costuma esquecer: qualidade de código antes do commit e sugestão de testes ao criar novos arquivos.

```json
// .github/hooks/pre-commit.json
{
  "event": "pre-commit",
  "description": "Verifica qualidade do código antes de cada commit",
  "actions": [
    {
      "type": "analyze",
      "prompt": "Revise os arquivos modificados neste commit e identifique: (1) TODOs e FIXMEs não resolvidos, (2) console.log ou Debug.WriteLine esquecidos, (3) métodos sem tratamento de exceção em pontos críticos. Liste apenas os problemas encontrados, sem sugestões de melhoria."
    }
  ]
}
```

```json
// .github/hooks/novo-arquivo.json
{
  "event": "file-created",
  "pattern": "**/Controllers/**Controller.cs",
  "description": "Sugere criação de testes ao adicionar um novo controller",
  "actions": [
    {
      "type": "suggest",
      "message": "Novo controller detectado. Deseja que eu gere os testes unitários básicos para este controller?"
    }
  ]
}
```

**Dicas de cenários:**
- **Guardiões de qualidade**: Configure um Hook para analisar todo arquivo salvo em busca de code smells — como métodos muito longos ou funções com muitos parâmetros — e avisar antes que o problema se acumule.
- **Atualização de documentação**: Um Hook que detecta mudanças em funções públicas e lembra o dev de atualizar o comentário ou a documentação correspondente.
- **Consistência de commits**: Um Hook de pre-commit que verifica se a mensagem do commit segue o padrão Conventional Commits (`feat:`, `fix:`, `chore:` etc.) antes de permitir o envio.
- **Alertas de segurança**: Um Hook que detecta padrões potencialmente inseguros (como strings de conexão hardcoded) no momento em que o código é escrito.

---

**Lançador / Infoprodutor** — Configure um Hook que detecta quando um arquivo de copy é salvo e verifica automaticamente se os elementos obrigatórios estão presentes: headline, prova social, CTA e informação de preço ou escassez. O Copilot alerta sobre o que está faltando antes de o arquivo ser enviado para aprovação — eliminando o ciclo de revisão por itens esquecidos.

**Dicas de cenários:**
- **Checklist de publicação**: Ao salvar um arquivo de copy finalizado, o Hook verifica se todos os links estão presentes, se o nome do produto está correto e se a data de abertura do carrinho está coerente.
- **Alerta de tom**: Ao salvar um e-mail, o Hook compara o tom com as regras da instrução e alerta se o texto estiver muito formal, agressivo ou fora do padrão da marca.
- **Backup de copies aprovadas**: Ao marcar um arquivo como aprovado, o Hook copia automaticamente o conteúdo para a pasta de referências que a Skill usa como base.
- **Consistência de nomenclatura**: O Hook verifica se o nome do produto está grafado da mesma forma em todos os arquivos do lançamento, evitando variações que confundem o público.

---

**Gestor de Empresa** — Configure um Hook que detecta quando um arquivo de plano de ação é salvo e verifica se todos os itens têm responsável e prazo atribuídos. O Copilot lista automaticamente os itens incompletos logo após o salvamento — impedindo que planos vagos passem despercebidos até a próxima reunião de acompanhamento.

**Dicas de cenários:**
- **Completude de documento**: Ao salvar qualquer documento de gestão, o Hook verifica se os campos obrigatórios (data, owner, prazo) estão preenchidos antes de considerar o documento pronto.
- **Linguagem inclusiva**: Ao salvar comunicados de RH, o Hook sinaliza termos que podem ser interpretados como excludentes ou inapropriados no contexto da empresa.
- **OKR vencido**: Ao abrir a pasta de OKRs, o Hook verifica se há objetivos com prazo ultrapassado sem status atualizado e os lista para revisão imediata.
- **Histórico de alterações**: Ao editar documentos de política ou processo, o Hook registra automaticamente a data da mudança e quem editou, criando um histórico de versões.

---

**Estudante** — Configure um Hook que roda ao salvar um resumo e verifica se os conceitos-chave da aula (definidos previamente nas Instructions) foram cobertos no texto. O Copilot aponta quais tópicos importantes estão ausentes — evitando que o estudante conclua um resumo com lacunas sem perceber.

**Dicas de cenários:**
- **Cobertura do conteúdo programático**: Ao salvar um resumo, o Hook cruza o texto com a ementa da disciplina (definida nas Instructions) e lista os tópicos não cobertos.
- **Alerta de prazo**: Ao abrir a pasta de trabalhos, o Hook verifica as datas de entrega e lista os que vencem nos próximos 7 dias em ordem de prioridade.
- **Citação obrigatória**: Ao salvar trabalhos acadêmicos, o Hook sinaliza trechos sem referência nas posições onde a norma ABNT exige citação.
- **Progresso de estudo**: Ao marcar um arquivo de resumo como concluído, o Hook atualiza automaticamente uma lista de tópicos revisados, dando visibilidade ao que falta estudar.

---

## 6. Plugins

### O que é

Os **Plugins** — também chamados de extensões do Copilot — são integrações que adicionam novas capacidades ao Copilot, conectando-o a ferramentas externas ou sistemas específicos. Se o Copilot nativo não sabe acessar o seu sistema de tickets, consultar seu banco de dados interno ou se comunicar com uma API proprietária, você pode criar um Plugin para isso.

Plugins são publicados como extensões do VS Code e aparecem como participantes de chat que você pode chamar diretamente com `@nome-do-plugin` no chat do Copilot.

### Para que serve

O Copilot, por padrão, só conhece o que está no seu código. Plugins são a ponte entre o Copilot e o mundo externo: sistemas de ticketing (Jira, Linear), wikis internas (Confluence, Notion), APIs de monitoramento (Datadog, New Relic) ou qualquer outro sistema que sua empresa usa. Com um Plugin, você pergunta ao Copilot sobre esses sistemas como se ele tivesse acesso direto a eles.

### Exemplos por Perfil

**Programador** — Instale o Plugin do Jira para consultar tarefas do sprint atual diretamente no VS Code, ou crie um Plugin customizado com um Chat Participant para conectar o Copilot a qualquer API interna da empresa.

```
// No chat do VS Code
@jira Quais são as tarefas do sprint atual ainda abertas para mim?

// O Copilot (com o Plugin) responde:
Com base no Jira, você tem 3 tarefas abertas no sprint atual:

- PROJ-142: Implementar validação de CPF no formulário de cadastro (Story Points: 3)
- PROJ-156: Corrigir bug na listagem de pedidos com filtro de data (Story Points: 2)
- PROJ-161: Adicionar logs de auditoria no módulo de pagamento (Story Points: 5)

Quer que eu abra o código relacionado a alguma dessas tarefas?
```

```typescript
// extension.ts — Plugin para consultar a API interna da empresa
const handler: vscode.ChatRequestHandler = async (request, context, stream) => {
  if (request.command === 'status') {
    // Consulta a API interna de status dos serviços
    const status = await apiInterna.getServiceStatus();
    stream.markdown(`**Status dos serviços:**\n${status.map(s =>
      `- ${s.nome}: ${s.status === 'ok' ? '✅' : '❌'} ${s.mensagem}`
    ).join('\n')}`);
  }
};

// Registra o participante de chat com o nome "@empresa"
vscode.chat.createChatParticipant('empresa.copilot', handler);
```

**Dicas de cenários:**
- **Integração com sistema de ticketing**: Pergunte ao Copilot sobre suas tarefas do sprint, peça para ele criar branches com o nome correto da task ou gerar commits vinculados ao número do ticket.
- **Consulta a wikis internas**: Acesse a documentação da empresa diretamente no chat — "qual é o padrão de autenticação definido pela equipe de segurança?" — sem sair do VS Code.
- **Monitoramento e alertas**: Pergunte ao Copilot sobre erros no sistema em produção sem precisar abrir o painel de monitoramento, e peça para ele ajudar a diagnosticar o problema com base nos logs.
- **Dados de negócio**: Crie um Plugin que consulta sistemas internos de BI e permite perguntas como "quais são os top 10 clientes por faturamento?" diretamente no VS Code.

---

**Lançador / Infoprodutor** — Com um Plugin conectado à plataforma de e-mail marketing (ActiveCampaign, Mailchimp), o lançador pergunta diretamente no VS Code: *"qual e-mail da última sequência teve a menor taxa de abertura?"*. O Copilot retorna as métricas sem trocar de janela — e já sugere reescritas do assunto com base na análise.

**Dicas de cenários:**
- **Plataforma de membros**: Um Plugin para o sistema de membros permite perguntar: "quais alunos estão inativos há mais de 30 dias?" — e acionar uma sequência de reengajamento diretamente no chat.
- **Mensagens do público**: Com um Plugin para WhatsApp Business ou Instagram API, o lançador consulta as perguntas mais frequentes do público e o Copilot gera respostas já no tom da marca.
- **Calendario de lançamentos**: Um Plugin para o calendário permite perguntar: "quais datas de abertura de carrinho conflitam com feriados no próximo trimestre?" — sem verificar manualmente.
- **Assets de design**: Com um Plugin para Canva API, o lançador verifica quais peças visuais ainda não foram criadas para o lançamento atual com base no checklist.

---

**Gestor de Empresa** — Com um Plugin conectado à ferramenta de gestão do time (Notion, Asana, Monday), o gestor pergunta: *"quais tarefas do trimestre ainda estão sem responsável definido?"*. O Copilot entrega a lista filtrada e já sugere quem do time teria disponibilidade com base nas tarefas atribuídas — transformando uma auditoria manual em segundos de consulta.

**Dicas de cenários:**
- **Sistema de ponto**: Um Plugin para o sistema de ponto permite perguntar: "quantos colaboradores tiveram mais de 2 atrasos esse mês?" — dados que antes exigiam relatório do RH ficam no chat.
- **Helpdesk**: Com um Plugin para Zendesk ou Freshdesk, o gestor pergunta: "quais tipos de chamado mais abriram essa semana?" — e o Copilot já sugere qual equipe precisa de reforço.
- **Agenda do time**: Um Plugin para o Google Calendar permite: "quem tem agenda livre para uma reunião de 1 hora ainda essa semana?" — sem trocar e-mails de disponibilidade.
- **NPS e satisfação**: Com um Plugin para ferramentas de NPS, o gestor pergunta: "quais clientes deram nota abaixo de 7 esse trimestre e quais são os temas das reclamações?" — transformando dados desconexos em pauta de reunião.

---

**Estudante** — Com um Plugin conectado ao Notion pessoal ou ao sistema da faculdade, o estudante pergunta: *"quais matérias tenho provas na próxima semana e quais tópicos ainda não revisei?"*. O Copilot cruza o calendário de provas com os arquivos de resumo existentes e gera um plano de revisão priorizado — planejamento que antes tomava 30 minutos acontece em uma pergunta.

**Dicas de cenários:**
- **Portal da faculdade**: Um Plugin para o portal acadêmico permite perguntar: "quais são minhas notas parciais?" — e o Copilot calcula o que você precisa tirar na prova final para passar.
- **Bases bibliográficas**: Com um Plugin para CAPES ou BVS, o estudante pergunta: "existem artigos publicados após 2023 sobre este tema?" — sem precisar fazer a busca manualmente.
- **Monitoria**: Um Plugin para o sistema de monitoria da faculdade permite verificar a disponibilidade dos monitores e agendar uma sessão, tudo no chat do VS Code.
- **Flashcards**: Com um Plugin para o Anki, o estudante pede: "crie 20 flashcards sobre algoritmos de ordenação no formato do meu baralho existente" — e o Plugin importa diretamente.

---

## 7. MCP Services

### O que é

Os **MCP Services** (Model Context Protocol) são servidores que seguem um protocolo padronizado para fornecer ferramentas, dados e contexto ao Copilot. É como uma tomada universal: qualquer serviço que implemente o protocolo MCP pode ser conectado ao Copilot e imediatamente disponibilizar suas capacidades para o modelo.

MCP é um padrão aberto — qualquer pessoa pode criar um servidor MCP para qualquer sistema. Já existem servidores MCP para bancos de dados, GitHub, sistemas de arquivos, browsers, e dezenas de outras integrações.

### Para que serve

Enquanto os Plugins são extensões do VS Code focadas na experiência de chat, os MCP Services são mais baixo nível: eles expõem **ferramentas** que o Copilot pode acionar autonomamente durante uma tarefa. Por exemplo, um MCP de banco de dados permite que o Copilot execute queries SQL por conta própria para entender o schema e responder perguntas sobre os dados — sem que você precise copiar e colar nada.

A grande vantagem do MCP é a **padronização**: você configura uma vez e o serviço funciona em qualquer cliente compatível com MCP (VS Code, Claude, Cursor, etc.).

### Exemplos por Perfil

**Programador** — Configure servidores MCP no arquivo `.vscode/mcp.json` para conectar o Copilot ao banco de dados e ao repositório GitHub do projeto. O Copilot passa a executar queries e gerenciar PRs de forma autônoma durante as tarefas.

```json
// .vscode/mcp.json (configuração de MCPs do projeto)
{
  "servers": {
    "banco-dados-projeto": {
      "type": "stdio",
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-postgres"],
      "env": {
        "POSTGRES_CONNECTION_STRING": "${env:DATABASE_URL}"
      }
    },
    "github": {
      "type": "stdio",
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-github"],
      "env": {
        "GITHUB_TOKEN": "${env:GITHUB_TOKEN}"
      }
    }
  }
}
```

```
// No chat do VS Code
"Analise a tabela de pedidos e me diga quais colunas têm maior percentual
de valores nulos. Sugira quais delas podem ser candidatas a remoção."

// O Copilot age autonomamente:
// 1. Aciona o MCP para consultar o schema da tabela
// 2. Executa queries de análise de nulos
// 3. Retorna a análise completa com recomendações
```

**Dicas de cenários:**
- **Exploração de banco de dados desconhecido**: Ao entrar em um projeto legado, use o MCP de banco para pedir ao Copilot que mapeie o schema completo, identifique as tabelas principais e explique os relacionamentos em linguagem natural.
- **Busca na web durante o desenvolvimento**: Configure um MCP de browser para permitir que o Copilot pesquise documentação oficial, Stack Overflow ou changelogs de bibliotecas em tempo real ao responder suas perguntas.
- **Automação de repositório**: Com o MCP do GitHub, peça ao Copilot para criar branches, abrir PRs, verificar o status de checks de CI ou comentar em issues — tudo sem sair do VS Code.
- **Sistema de arquivos avançado**: Use um MCP de filesystem para permitir que o Copilot leia, mova e reorganize arquivos além da pasta do projeto aberto, útil em monorepos ou projetos com múltiplas pastas raiz.

---

**Lançador / Infoprodutor** — Com um MCP conectado à plataforma de vendas (Hotmart, Kiwify) ou ao Google Analytics, o lançador pede ao Copilot que compare a taxa de conversão dos últimos 3 lançamentos e identifique em qual etapa da sequência a audiência mais saiu. O Copilot consulta os dados, cruza as métricas e entrega uma análise que levaria horas em planilhas — em segundos.

**Dicas de cenários:**
- **Histórico em planilhas**: Com um MCP para Google Sheets com dados de lançamentos anteriores, compara métricas entre ciclos sem abrir a planilha.
- **E-mail marketing em tempo real**: Um MCP para ActiveCampaign ou Mailchimp permite consultar taxas de abertura, clique e cancelamento direto no chat durante o lançamento.
- **Pesquisa de concorrência**: Use um MCP de browser para o Copilot resumir o posicionamento dos principais concorrentes do nicho e identificar diferenciais não explorados.
- **CRM de clientes**: Com um MCP para o CRM, pergunte quais clientes compraram o produto anterior e ainda não adquiriram o novo — lista pronta para campanha de recompra.

---

**Gestor de Empresa** — Com um MCP conectado ao sistema de RH, o gestor pede ao Copilot: *"liste os colaboradores que estão há mais de 6 meses sem avaliação de desempenho"*. O Copilot consulta a base, cruza com o calendário de revisões e entrega a lista priorizada por tempo — substituindo uma busca manual em múltiplos sistemas por uma consulta direta.

**Dicas de cenários:**
- **ERP da empresa**: Com um MCP para o sistema ERP, pergunte: "qual é o custo total de horas extras do time de tecnologia no último trimestre?" — dados que antes dependiam do financeiro ficam no chat.
- **Auditoria de pastas**: Use um MCP de filesystem para o Copilot categorizar documentos desorganizados e sugerir uma estrutura de pastas mais eficiente sem mover nada manualmente.
- **Análise de sentimento do time**: Com um MCP conectado ao Slack ou Teams, pergunte: "quais foram os tópicos mais mencionados no canal de suporte esta semana?" — identifica padrões de problemas sem ler centenas de mensagens.
- **Benchmarking de mercado**: Um MCP de browser permite pedir: "quais são as melhores práticas atuais para feedback de desempenho em empresas de tecnologia?" — pesquisa de mercado no momento em que é necessária.

---

**Estudante** — Com um MCP conectado a uma base de artigos acadêmicos (como Semantic Scholar ou Google Scholar), o estudante pede ao Copilot que encontre os 5 artigos mais citados sobre um tema e gere um resumo comparativo das abordagens de cada um. Horas de pesquisa e leitura preliminar são substituídas por uma consulta estruturada — sobrando mais tempo para o estudo aprofundado dos artigos relevantes.

**Dicas de cenários:**
- **Planejamento de provas**: Com um MCP de filesystem, o Copilot analisa toda a pasta de resumos do semestre, identifica quais disciplinas têm menos material e gera um plano de estudo priorizado.
- **Conteúdo complementar**: Use um MCP de browser para pesquisar vídeos e materiais sobre um conceito difícil — o Copilot resume as fontes encontradas em linguagem adequada ao seu nível.
- **Situação acadêmica**: Com um MCP conectado ao sistema acadêmico, pergunte: "qual é minha situação de faltas em cada disciplina e quantas aulas ainda posso perder?" — sem acessar o portal.
- **Referências de TCC**: Um MCP para GitHub permite pedir: "encontre projetos de TCC em Engenharia de Software com tema similar ao meu e mostre como outros alunos estruturaram o repositório".

---

## Tópicos Adicionais Sugeridos

Além das 7 ferramentas apresentadas, existem outros aspectos do GitHub Copilot que valem ser explorados por quem está começando a dominar a ferramenta:

1. **`copilot-instructions.md` (Instruções Globais do Repositório)**: Um único arquivo em `.github/copilot-instructions.md` que serve como "constituição" do projeto — regras que se aplicam a todas as interações do Copilot no repositório, independente do tipo de arquivo aberto. É o ponto de partida antes de criar Instructions mais específicas.

2. **Modos de Agente (`ask`, `edit`, `agent`, `plan`)**: O Copilot opera em diferentes modos de execução. `ask` é apenas uma conversa; `edit` modifica arquivos abertos; `agent` toma ações autônomas no workspace; `plan` esboça o que vai fazer antes de agir. Entender quando usar cada modo evita resultados inesperados.

3. **Variáveis de Contexto (`#file`, `#selection`, `#codebase`)**: No chat do Copilot, você pode referenciar partes específicas do seu código usando `#file:caminho/arquivo.cs`, `#selection` (o que está selecionado no editor) e `#codebase` (busca semântica em todo o projeto). Dominar essas referências melhora drasticamente a qualidade das respostas.

4. **Copilot Code Review**: Recurso integrado ao GitHub que permite ao Copilot revisar Pull Requests automaticamente, identificando bugs, problemas de segurança e oportunidades de melhoria — diretamente na interface do GitHub, antes mesmo de um humano olhar o código.

5. **Segurança e Privacidade no Copilot for Business**: Para equipes corporativas, é fundamental entender o que o Copilot envia para a nuvem e o que fica local. O plano Copilot for Business oferece garantias de que o código não é usado para treinar modelos — conhecer essas políticas é essencial para adoção empresarial.

6. **Engenharia de Prompts para Desenvolvedores**: Saber escrever um bom prompt é uma habilidade separada do uso das ferramentas. Técnicas como "forneça o contexto antes da pergunta", "peça passo a passo", "dê exemplos do formato esperado" e "use delimitadores claros" fazem a diferença entre uma resposta genérica e uma resposta que resolve o problema.

7. **Integração com GitHub Actions**: O Copilot pode ser integrado a pipelines de CI/CD para automatizar tarefas como geração de notas de release, análise de cobertura de testes e sugestões de correção de falhas diretamente nos logs do pipeline.

---

## Outras Ferramentas e Recursos do Copilot

Além das 7 ferramentas de customização e dos tópicos adicionais, o GitHub Copilot oferece outros recursos que muita gente usa no dia a dia sem saber que têm nome — ou sem entender o que está disponível. Esta seção apresenta cada um deles em linguagem simples.

---

### Inline Chat

**O que é:** É o chat do Copilot que aparece diretamente dentro do arquivo que você está editando, sem precisar abrir nenhum painel lateral. Você pressiona `Ctrl+I` (ou `Cmd+I` no Mac), digita sua pergunta ou pedido, e o resultado aparece ali mesmo, no meio do texto.

**Para que serve:** Imagine que você está escrevendo um documento ou um código e quer melhorar só um trecho específico — sem ter que copiar e colar nada em outra janela. Com o Inline Chat, você seleciona aquele trecho, aperta o atalho e pede: *"reescreva isso de forma mais clara"* ou *"corrija esse erro"*. O Copilot responde exatamente onde você está, e você decide se aceita ou rejeita a sugestão. É como ter um colega sentado ao seu lado que você consulta sem precisar se levantar da cadeira.

---

### Copilot Edits

**O que é:** Uma interface especial do Copilot, separada do chat normal, criada especificamente para fazer alterações em vários arquivos ao mesmo tempo. Você descreve o que quer mudar, e o Copilot mostra uma prévia de todas as alterações que fará — em cada arquivo — antes de você confirmar qualquer coisa.

**Para que serve:** Pense numa situação em que você precisa renomear um campo em 10 arquivos diferentes, ou adicionar uma nova informação em todos os documentos de um projeto. Sem essa ferramenta, você abriria arquivo por arquivo. Com o Copilot Edits, você descreve a mudança uma única vez, ele mostra o que vai alterar em cada lugar, e você revisa e aprova tudo de uma vez. É especialmente útil para mudanças que afetam vários pontos do projeto ao mesmo tempo.

---

### Next Edit Suggestions (NES)

**O que é:** Um recurso que observa as alterações que você acabou de fazer e antecipa qual será a próxima edição necessária — aparecendo como uma sugestão discreta no próprio editor, sem você precisar pedir nada.

**Para que serve:** Quando você muda uma coisa em um documento ou código, geralmente precisa mudar outras coisas relacionadas — e é fácil esquecer. O NES funciona como um assistente que percebe o padrão do que você está fazendo e já aponta: *"você acabou de mudar isso aqui, provavelmente vai querer mudar aquilo ali também"*. Você não pergunta nada — ele te avisa sozinho. Isso evita aqueles erros de "esqueci de atualizar em todos os lugares".

---

### Copilot na Linha de Comando (`gh copilot`)

**O que é:** Uma versão do Copilot que funciona diretamente no terminal (a tela preta com texto onde programadores digitam comandos). Você instala o `gh copilot` e passa a ter dois comandos disponíveis: `gh copilot suggest` (que sugere um comando para o que você quer fazer) e `gh copilot explain` (que explica em português o que um comando faz).

**Para que serve:** Ninguém decora todos os comandos de terminal. Com o `gh copilot suggest`, você descreve o que quer fazer em linguagem normal — *"quero compactar esta pasta em zip"* — e ele te dá o comando exato para copiar e executar. Com o `gh copilot explain`, você cola um comando misterioso que encontrou em algum tutorial e ele explica linha por linha o que cada parte faz. É um Google para comandos de terminal que responde em linguagem humana.

---

### Slash Commands (`/fix`, `/explain`, `/tests`, `/doc`)

**O que é:** Atalhos de texto que você digita no chat do Copilot para acionar tarefas específicas sem precisar escrever uma instrução completa. São como teclas de atalho, mas em texto.

**Para que serve:** Em vez de escrever *"por favor analise este código, identifique os erros e sugira correções"*, você seleciona o trecho com problema e digita simplesmente `/fix`. Em vez de *"gere testes unitários para esta função"*, você digita `/tests`. Os principais são:

- `/fix` — corrige o erro ou problema no trecho selecionado
- `/explain` — explica em linguagem simples o que o código ou texto selecionado faz
- `/tests` — gera testes automatizados para o código selecionado
- `/doc` — gera documentação para o código selecionado

São atalhos para as tarefas mais comuns — você para de reescrever a mesma instrução toda vez.

---

### Variáveis de Contexto Avançadas

**O que é:** Além das variáveis `#file`, `#selection` e `#codebase` (mencionadas nos Tópicos Adicionais), o Copilot oferece outras variáveis que você pode usar no chat para fornecer contexto específico sobre o que está acontecendo no seu ambiente naquele momento.

**Para que serve:** Cada variável "entrega" um pedaço diferente do seu ambiente para o Copilot:

- **`#terminalLastCommand`** — injeta o último comando que você rodou no terminal e o resultado que ele retornou. Útil para perguntar *"por que esse comando deu erro?"* sem precisar copiar e colar a mensagem de erro manualmente.
- **`#terminalSelection`** — injeta o texto que você selecionou dentro do terminal. Útil quando você quer que o Copilot explique ou analise uma saída longa de um comando.
- **`#problems`** — injeta a lista de erros e avisos que aparecem no painel de "Problemas" do VS Code. Útil para perguntar *"como resolvo todos esses erros de uma vez?"* sem listar cada um.
- **`#testFailure`** — injeta o contexto do teste que acabou de falhar na última execução. Útil para pedir ao Copilot que explique por que o teste falhou e como corrigir.
- **`#openTabs`** — injeta o conteúdo de todos os arquivos que você tem abertos no momento. Útil quando a resposta depende de informações espalhadas em vários arquivos simultâneos.

---

### GitHub Copilot Extensions (Marketplace)

**O que é:** Extensões criadas por empresas ou desenvolvedores externos que adicionam novos "participantes de chat" ao Copilot — ou seja, assistentes especializados que você chama com `@nome` no chat. Elas ficam disponíveis no GitHub Marketplace para qualquer pessoa instalar.

**Para que serve:** O Copilot nativo não sabe nada sobre Docker, sobre o seu banco de dados específico, ou sobre ferramentas particulares do seu setor. As Extensions preenchem essa lacuna: alguém já criou uma extensão `@docker` que responde dúvidas de containers, uma `@datadog` que consulta logs de monitoramento, e assim por diante. Você instala a extensão e passa a ter um especialista no assunto disponível diretamente no seu chat — sem precisar trocar de ferramenta ou copiar e colar informações entre sistemas.

---

### Copilot Autofix

**O que é:** Um recurso do GitHub (não do VS Code) que detecta automaticamente vulnerabilidades de segurança no código do repositório e, além de apontar o problema, já cria uma sugestão de correção — sem que você precise pedir.

**Para que serve:** Quando o GitHub encontra um problema de segurança no seu código (por exemplo, uma senha salva no lugar errado, ou uma brecha que permite que alguém invada o sistema), normalmente ele apenas avisa: *"tem um problema aqui"*. Com o Autofix, ele vai além: já te mostra como corrigir e cria um rascunho de Pull Request com a solução aplicada. Você ainda decide se aceita ou não — mas o trabalho de investigar e escrever a correção já foi feito. É como ter um especialista em segurança que não só encontra os problemas, mas já entrega a solução embalada.

---

### Copilot no GitHub.com (fora do VS Code)

**O que é:** O Copilot também existe diretamente na interface do site GitHub.com — não apenas no editor VS Code. Isso significa que ele está disponível enquanto você navega por repositórios, lê código, revisa contribuições de outras pessoas ou gerencia o projeto.

**Para que serve:** Três situações principais onde isso é útil:

- **Resumo automático de Pull Requests**: Quando alguém envia um conjunto de mudanças para revisão, o Copilot gera automaticamente uma descrição do que foi alterado e por quê — com base no próprio código modificado. Quem vai revisar já recebe o contexto pronto, sem depender de o autor ter escrito uma boa descrição.
- **Explicação de código na busca**: Ao usar o Code Search do GitHub para encontrar um trecho de código em qualquer repositório público, o Copilot explica o que aquele código faz em linguagem simples — útil para quem está pesquisando como outros projetos resolveram um problema.
- **Triagem de Issues e Discussions**: O Copilot sugere respostas para perguntas abertas em projetos, ajudando mantenedores de projetos com muitos usuários a responder dúvidas frequentes mais rapidamente.
