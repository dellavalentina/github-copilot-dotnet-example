# Live 4: Além do Básico — Agents, Bibliotecas e o Ecossistema Copilot

Guia completo de **onde encontrar skills, instructions, plugins e MCP servers** — os repositórios e diretórios que a comunidade inteira usa para transformar o Copilot num assistente sob medida.

**Os 7 pilares do ecossistema:**

| #   | Pilar               |
| --- | ------------------- |
| 1   | Custom Instructions |
| 2   | Prompt Files        |
| 3   | Custom Agents       |
| 4   | Agent Skills        |
| 5   | MCP Servers         |
| 6   | Agent Plugins       |
| 7   | Hooks               |

---

## 📘 Custom Instructions

### O que é

Arquivos Markdown que definem regras e padrões automaticamente aplicados a cada conversa com o Copilot.

### Tipos

| Arquivo                   | Descrição                                                  |
| ------------------------- | ---------------------------------------------------------- |
| `copilot-instructions.md` | Regras globais do projeto (pasta `.github/`)               |
| `.instructions.md`        | Regras por tipo de arquivo (pasta `.github/instructions/`) |
| `AGENTS.md`               | Formato compatível com múltiplos agentes IA                |

### Comando útil

Digitar `/init` no chat do Copilot para gerar automaticamente.

### Links

| Recurso                     | Link                                                                         |
| --------------------------- | ---------------------------------------------------------------------------- |
| Docs oficiais               | https://code.visualstudio.com/docs/copilot/customization/custom-instructions |
| Exemplos da comunidade      | https://github.com/github/awesome-copilot/tree/main/instructions             |
| Cursor Directory (Rules)    | https://cursor.directory/plugins?tag=rules                                   |

---

## 📘 Prompt Files

### O que é

Arquivos `.prompt.md` que funcionam como slash commands reutilizáveis (ex: `/create-react-form`).

### Onde ficam

`.github/prompts/` ou na pasta do perfil do usuário.

### Comando útil

`/create-prompt` no chat para gerar com IA.

### Links

| Recurso                | Link                                                                  |
| ---------------------- | --------------------------------------------------------------------- |
| Docs oficiais          | https://code.visualstudio.com/docs/copilot/customization/prompt-files |
| Exemplos da comunidade | https://github.com/github/awesome-copilot                             |

---

## 📘 Custom Agents

### O que é

Arquivos `.agent.md` que criam "personas" especializadas com ferramentas e instruções próprias.

### Exemplos de uso

- **Agent de planejamento** — só ferramentas read-only
- **Agent de code review** — foco em segurança
- **Agent de implementação** — ferramentas de escrita

### Conceito de Handoffs

Transição guiada entre agents:

```
Planejar → Implementar → Revisar
```

### Onde ficam

`.github/agents/` ou `~/.copilot/agents/`

### Comando útil

`/create-agent` no chat.

### Links

| Recurso              | Link                                                                   |
| -------------------- | ---------------------------------------------------------------------- |
| Docs oficiais        | https://code.visualstudio.com/docs/copilot/customization/custom-agents |
| Agents da comunidade | https://github.com/github/awesome-copilot/tree/main/agents             |

---

## 📘 Agent Skills

### O que é

Pastas com `SKILL.md` + scripts/exemplos que o Copilot carrega sob demanda. É um **padrão aberto** ([agentskills.io](https://agentskills.io/)) compatível com vários agentes.

### Diferença de Instructions

| Aspecto   | Skills                          | Instructions          |
| --------- | ------------------------------- | --------------------- |
| Propósito | Tarefas especializadas          | Regras de codificação |
| Conteúdo  | Instruções + scripts + exemplos | Apenas instruções     |
| Escopo    | Sob demanda                     | Sempre aplicado       |
| Padrão    | Aberto (agentskills.io)         | VS Code específico    |

### Onde ficam

`.github/skills/`, `~/.copilot/skills/`, ou `~/.agents/skills/`

### Comando útil

`/create-skill` no chat.

### Agents compatíveis com Agent Skills

GitHub Copilot (VS Code, CLI, Coding Agent) · Claude Code · Gemini CLI · Amp · Kiro · Factory · OpenHands · e mais.

### Links

| Recurso                        | Link                                                                  |
| ------------------------------ | --------------------------------------------------------------------- |
| Docs oficiais                  | https://code.visualstudio.com/docs/copilot/customization/agent-skills |
| Site do padrão aberto          | https://agentskills.io/                                               |
| Repositório Anthropic (114k ⭐) | https://github.com/anthropics/skills                                  |
| Skills da comunidade           | https://github.com/github/awesome-copilot/tree/main/skills            |
| Smithery Skills                | https://smithery.ai/skills                                            |

---

## 📘 MCP Servers

### O que é

Model Context Protocol — protocolo aberto para conectar IA a ferramentas externas (bancos de dados, APIs, browsers etc).

### Como instalar

1. No Extensions view: digitar `@mcp` para ver o gallery
2. No arquivo `mcp.json` (workspace `.vscode/mcp.json` ou user)
3. Via Command Palette: `MCP: Add Server`
4. Via **Smithery CLI**: `npx @smithery/cli@latest setup`

### Exemplos populares

| MCP Server     | O que faz                               |
| -------------- | --------------------------------------- |
| **Playwright** | Automação de browser                    |
| **GitHub MCP** | Interação com repos                     |
| **Azure MCP**  | Recursos Azure                          |
| **Context7**   | Docs atualizadas de qualquer biblioteca |
| **Gmail**      | Gerenciar e-mails (41k installs)        |
| **Instagram**  | Automação de social media (442k)        |
| **Supabase**   | Banco de dados + Edge Functions         |
| **Notion**     | Busca e gestão de workspace             |

### Diretórios de MCP Servers

| Diretório                                                                       | Qtde servers | Diferencial                                            |
| ------------------------------------------------------------------------------- | ------------ | ------------------------------------------------------ |
| [smithery.ai](https://smithery.ai/)                                             | 6.700+       | CLI install, auth gerenciado, observability            |
| [glama.ai/mcp/servers](https://glama.ai/mcp/servers)                            | 21.000+      | Maior diretório, categorias (Remote, Python, DevTools) |
| [mcp.so](https://mcp.so/)                                                       | 19.700+      | Marketplace com Featured/Hosted/Official               |
| [modelcontextprotocol/servers](https://github.com/modelcontextprotocol/servers) | —            | Repo oficial do protocolo (Official + Community)       |
| [punkpeye/awesome-mcp-servers](https://github.com/punkpeye/awesome-mcp-servers) | —            | Lista curada, 40+ categorias                           |
| [opentools.com](https://opentools.com/)                                         | —            | API unificada para MCP tools, qualquer LLM             |
| [cursor.directory (MCPs)](https://cursor.directory/plugins?tag=mcp)             | —            | MCP servers com install direto no Cursor               |

### Links

| Recurso                    | Link                                                                   |
| -------------------------- | ---------------------------------------------------------------------- |
| Docs oficiais              | https://code.visualstudio.com/docs/copilot/customization/mcp-servers   |
| Site do protocolo MCP      | https://modelcontextprotocol.io/                                       |
| Referência de configuração | https://code.visualstudio.com/docs/copilot/reference/mcp-configuration |

---

## 📘 Agent Plugins, Hooks e o Padrão Open Plugins

### Plugins

Bundles pré-empacotados que combinam skills + agents + hooks + MCP servers.

### Open Plugins — O padrão aberto

[open-plugins.com](https://open-plugins.com/) define a estrutura de um plugin:

```
my-plugin/
├── .plugin/plugin.json   # Manifesto (nome, versão)
├── skills/               # Agent Skills (SKILL.md)
├── agents/               # Sub-agents especializados
├── hooks/                # Automação por eventos
├── rules/                # Coding standards (.mdc)
├── .mcp.json             # MCP servers
└── .lsp.json             # Language servers
```

**Como funciona:** Install → Discover → Namespace → Activate

### Marketplaces e diretórios de plugins

| Fonte                                                               | O que tem                                              |
| ------------------------------------------------------------------- | ------------------------------------------------------ |
| [cursor.directory/plugins](https://cursor.directory/plugins)        | Hub da comunidade: plugins, MCP, rules (77k+ devs)     |
| [github/awesome-copilot](https://github.com/github/awesome-copilot) | Plugins, agents, instructions, skills, hooks, cookbook |
| [github/copilot-plugins](https://github.com/github/copilot-plugins) | Coleção oficial de plugins GitHub Copilot              |
| [open-plugins.com](https://open-plugins.com/)                       | Especificação + docs do padrão                         |
| [smithery.ai/skills](https://smithery.ai/skills)                    | Skills publicados na Smithery                          |

**Como instalar (VS Code):**

```bash
# Extensions view
@agentPlugins

# Ou via Copilot CLI
copilot plugin install <nome>@awesome-copilot
```

### Hooks

Comandos shell executados em pontos do ciclo de vida do agent (ex: rodar formatter após cada edição).

### Links

| Recurso                 | Link                                                                   |
| ----------------------- | ---------------------------------------------------------------------- |
| Docs — Agent Plugins    | https://code.visualstudio.com/docs/copilot/customization/agent-plugins |
| Docs — Hooks            | https://code.visualstudio.com/docs/copilot/customization/hooks         |
| Open Plugins (spec)     | https://open-plugins.com/                                              |
| Plugins oficiais GitHub | https://github.com/github/copilot-plugins                              |
| Hooks da comunidade     | https://github.com/github/awesome-copilot/tree/main/hooks              |
| Cursor Directory        | https://cursor.directory/                                              |

---

## 📘 Onde encontrar TUDO

### 🔵 Instructions & Rules (regras de codificação)

| Repositório / Site                                                                | O que tem                                           | Stars |
| --------------------------------------------------------------------------------- | --------------------------------------------------- | ----- |
| [github/awesome-copilot/instructions](https://github.com/github/awesome-copilot)  | Instructions da comunidade para GitHub Copilot      | 29.1k |
| [PatrickJS/awesome-cursorrules](https://github.com/PatrickJS/awesome-cursorrules) | 100+ regras por framework/linguagem (.cursorrules)  | 39k   |
| [cursor.directory (Rules)](https://cursor.directory/plugins?tag=rules)            | Rules com install direto, rankings por popularidade | —     |
| [cursorlist.com](https://cursorlist.com/)                                         | Diretório adicional de cursor rules                 | —     |

**Popular no cursor.directory:** Next.js (33.8k installs), Front End (29.4k), Expo (17.2k), FastAPI (11.1k), Java (7.6k)

### 🟢 Agent Skills (capacidades sob demanda)

| Repositório / Site                                                         | O que tem                                             | Stars |
| -------------------------------------------------------------------------- | ----------------------------------------------------- | ----- |
| [anthropics/skills](https://github.com/anthropics/skills)                  | Skills oficiais: Creative, Dev, Enterprise, Documents | 114k  |
| [agentskills.io](https://agentskills.io/)                                  | Especificação do padrão aberto                        | —     |
| [github/awesome-copilot/skills](https://github.com/github/awesome-copilot) | Skills da comunidade Copilot                          | 29.1k |
| [smithery.ai/skills](https://smithery.ai/skills)                           | Skills publicados na Smithery                         | —     |

**Agents que suportam o padrão Agent Skills:**
VS Code / GitHub Copilot, Claude Code, Gemini CLI, Amp, Qodo, TRAE, Junie, Kiro, Factory, OpenHands, Mux, Databricks Genie, Snowflake Cortex Code, e mais.

### 🟡 MCP Servers (ferramentas externas)

| Diretório                                                                       | Qtde servers | Diferencial                                      |
| ------------------------------------------------------------------------------- | ------------ | ------------------------------------------------ |
| [smithery.ai](https://smithery.ai/)                                             | 6.700+       | CLI install, auth gerenciado, observability      |
| [glama.ai/mcp/servers](https://glama.ai/mcp/servers)                            | 21.000+      | Maior diretório, filtros por categoria           |
| [mcp.so](https://mcp.so/)                                                       | 19.700+      | Marketplace com Featured/Hosted/Official         |
| [modelcontextprotocol/servers](https://github.com/modelcontextprotocol/servers) | —            | Repo oficial do protocolo (Official + Community) |
| [punkpeye/awesome-mcp-servers](https://github.com/punkpeye/awesome-mcp-servers) | —            | Lista curada GitHub, 40+ categorias              |
| [cursor.directory (MCPs)](https://cursor.directory/plugins?tag=mcp)             | —            | MCP servers com install direto                   |
| [opentools.com](https://opentools.com/)                                         | —            | API unificada para usar MCPs com qualquer LLM    |

**MCP servers mais instalados (Smithery):** Instagram (442k), Gmail (41k), Linkup (40k), Exa Search (34k), Slack (22k), Mesh (20k), Blockscout (16k), Context7 (13k)

### 🟣 Plugins & Agents (bundles completos)

| Repositório / Site                                                  | O que tem                                            | Stars |
| ------------------------------------------------------------------- | ---------------------------------------------------- | ----- |
| [cursor.directory/plugins](https://cursor.directory/plugins)        | Hub de plugins: MCP, rules, skills (77k+ developers) | —     |
| [github/awesome-copilot](https://github.com/github/awesome-copilot) | Agents, plugins, hooks, workflows, cookbook          | 29.1k |
| [github/copilot-plugins](https://github.com/github/copilot-plugins) | Plugins oficiais GitHub Copilot                      | 185   |
| [open-plugins.com](https://open-plugins.com/)                       | Especificação do padrão Open Plugins                 | —     |

### 🔴 Repositórios "Awesome" e gerais

| Repositório                                                                       | Descrição                                                                            | Stars |
| --------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------ | ----- |
| [github/awesome-copilot](https://github.com/github/awesome-copilot)               | **A bíblia da comunidade** — agents, instructions, skills, plugins, hooks, workflows | 29.1k |
| [anthropics/skills](https://github.com/anthropics/skills)                         | Skills de referência da Anthropic (funciona no VS Code também)                       | 114k  |
| [PatrickJS/awesome-cursorrules](https://github.com/PatrickJS/awesome-cursorrules) | Maior coleção de regras por framework/linguagem                                      | 39k   |
| [modelcontextprotocol/servers](https://github.com/modelcontextprotocol/servers)   | Repo oficial de MCP servers                                                          | —     |
| [punkpeye/awesome-mcp-servers](https://github.com/punkpeye/awesome-mcp-servers)   | Lista curada de MCP servers                                                          | —     |

### Sites de referência

| Site                                                              | O que tem                                                 |
| ----------------------------------------------------------------- | --------------------------------------------------------- |
| [agentskills.io](https://agentskills.io/)                         | Especificação do padrão aberto Agent Skills               |
| [open-plugins.com](https://open-plugins.com/)                     | Especificação do padrão Open Plugins                      |
| [cursor.directory](https://cursor.directory/)                     | Hub comunitário: plugins, MCPs, rules, events, jobs       |
| [smithery.ai](https://smithery.ai/)                               | Registry de MCP servers com CLI e auth                    |
| [awesome-copilot.github.com](https://awesome-copilot.github.com/) | Website com busca, filtros, Learning Hub e seção de Tools |
| [modelcontextprotocol.io](https://modelcontextprotocol.io/)       | Docs do protocolo MCP                                     |
| [glama.ai/mcp/servers](https://glama.ai/mcp/servers)              | Maior diretório de MCP servers (21k+)                     |
| [mcp.so](https://mcp.so/)                                         | Marketplace de MCP servers (19k+)                         |
| [opentools.com](https://opentools.com/)                           | API unificada para tool use com qualquer LLM              |

### Documentação oficial VS Code

| Página                      | Link                                                                         |
| --------------------------- | ---------------------------------------------------------------------------- |
| Visão geral de customização | https://code.visualstudio.com/docs/copilot/copilot-customization             |
| Custom Instructions         | https://code.visualstudio.com/docs/copilot/customization/custom-instructions |
| Prompt Files                | https://code.visualstudio.com/docs/copilot/customization/prompt-files        |
| Custom Agents               | https://code.visualstudio.com/docs/copilot/customization/custom-agents       |
| Agent Skills                | https://code.visualstudio.com/docs/copilot/customization/agent-skills        |
| MCP Servers                 | https://code.visualstudio.com/docs/copilot/customization/mcp-servers         |
| Agent Plugins               | https://code.visualstudio.com/docs/copilot/customization/agent-plugins       |
| Hooks                       | https://code.visualstudio.com/docs/copilot/customization/hooks               |

### Documentação GitHub

| Página              | Link                                                               |
| ------------------- | ------------------------------------------------------------------ |
| Customizing Copilot | https://docs.github.com/en/copilot/customizing-copilot             |
| Using MCP           | https://docs.github.com/en/copilot/how-tos/provide-context/use-mcp |

### Comunidade

| Canal                | Link                          |
| -------------------- | ----------------------------- |
| Discord Agent Skills | https://discord.gg/MKPE9g8aUy |

---

## Comandos rápidos

```
/init                  → Gerar copilot-instructions.md
/create-instruction    → Criar instruction por tipo de arquivo
/create-prompt         → Criar prompt file reutilizável
/create-agent          → Criar agent customizado
/create-skill          → Criar skill especializada
/create-hook           → Criar hook de automação
@mcp (Extensions)      → Explorar MCP servers disponíveis
@agentPlugins          → Explorar plugins da comunidade
```

---

## 📋 Todos os links

```
📚 REPOSITÓRIOS
• github/awesome-copilot: https://github.com/github/awesome-copilot
• anthropics/skills: https://github.com/anthropics/skills
• github/copilot-plugins: https://github.com/github/copilot-plugins
• PatrickJS/awesome-cursorrules: https://github.com/PatrickJS/awesome-cursorrules
• modelcontextprotocol/servers: https://github.com/modelcontextprotocol/servers
• punkpeye/awesome-mcp-servers: https://github.com/punkpeye/awesome-mcp-servers

🌐 DIRETÓRIOS DE MCP SERVERS
• Smithery (6.700+ MCPs): https://smithery.ai/
• Glama (21.000+ MCPs): https://glama.ai/mcp/servers
• MCP.so (19.700+ MCPs): https://mcp.so/
• OpenTools (API unificada): https://opentools.com/
• Cursor Directory (MCPs): https://cursor.directory/plugins?tag=mcp

🔧 SKILLS, PLUGINS & RULES
• Agent Skills (padrão aberto): https://agentskills.io/
• Open Plugins (padrão): https://open-plugins.com/
• Cursor Directory (plugins): https://cursor.directory/plugins
• Cursor Directory (rules): https://cursor.directory/plugins?tag=rules
• Smithery Skills: https://smithery.ai/skills

🌐 SITES GERAIS
• Awesome Copilot (website): https://awesome-copilot.github.com/
• Learning Hub: https://awesome-copilot.github.com/learning-hub
• Tools: https://awesome-copilot.github.com/tools
• MCP Protocol: https://modelcontextprotocol.io/

📖 DOCS VS CODE
• Customização: https://code.visualstudio.com/docs/copilot/copilot-customization
• Instructions: https://code.visualstudio.com/docs/copilot/customization/custom-instructions
• Prompt Files: https://code.visualstudio.com/docs/copilot/customization/prompt-files
• Custom Agents: https://code.visualstudio.com/docs/copilot/customization/custom-agents
• Agent Skills: https://code.visualstudio.com/docs/copilot/customization/agent-skills
• MCP Servers: https://code.visualstudio.com/docs/copilot/customization/mcp-servers
• Plugins: https://code.visualstudio.com/docs/copilot/customization/agent-plugins
• Hooks: https://code.visualstudio.com/docs/copilot/customization/hooks

📖 DOCS GITHUB
• Customizing Copilot: https://docs.github.com/en/copilot/customizing-copilot

💬 COMUNIDADE
• Discord Agent Skills: https://discord.gg/MKPE9g8aUy
• Discord Smithery: https://discord.gg/sKd9uycgH9
```
https://www.promptingguide.ai/pt/introduction/examples