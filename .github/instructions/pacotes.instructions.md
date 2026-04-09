---
applyTo: "**/*.csproj,**/package.json,**/requirements.txt,**/*.sln*,**/pyproject.toml,**/Cargo.toml,**/go.mod"
---

# Instrução: Seleção de Versões de Pacotes

## Papel

Agente de código especialista em gerenciamento de dependências multi-ecossistema (NuGet, npm, pip, Cargo, Go modules, etc.).

## Objetivo

Ao adicionar, atualizar ou sugerir pacotes em qualquer ecossistema, garantir que as versões escolhidas sejam:
1. As mais recentes **estáveis** disponíveis
2. **Compatíveis entre si** — pacotes do mesmo grupo/família devem compartilhar a mesma major/minor

## Regras

### DEVE
- Antes de definir uma versão, verificar a versão estável mais recente usando CLI do ecossistema (prioridade) ou consulta web (fallback):
  - **NuGet:** `dotnet package search <pacote> --exact-match` ou consultar nuget.org
  - **npm:** `npm view <pacote> version`
  - **pip:** `pip index versions <pacote>` ou consultar pypi.org
- Pacotes de uma mesma família (ex: `Microsoft.EntityFrameworkCore.*`, `@angular/*`, `boto3`+`botocore`) devem usar a mesma major/minor
- Sempre preferir versões **estáveis** (sem sufixos `-preview`, `-rc`, `-beta`, `-alpha`)
- Se o projeto já utilizar versões **prerelease**, perguntar ao usuário antes de decidir entre: migrar para estável (se existir) ou atualizar dentro do canal prerelease

### NÃO DEVE
- Inventar ou adivinhar números de versão sem verificar
- Misturar versões major/minor diferentes dentro de um mesmo grupo de pacotes
- Escolher prerelease silenciosamente quando existe versão estável

## Passos

1. Identificar o ecossistema e gerenciador de pacotes pelo tipo de arquivo
2. Para cada pacote a ser adicionado/atualizado:
   a. Executar o comando CLI correspondente para consultar a versão estável mais recente
   b. Se o CLI falhar ou não estiver disponível, consultar o registro web (nuget.org, npmjs.com, pypi.org, etc.)
3. Agrupar pacotes da mesma família e alinhar na mesma major/minor
4. Se o projeto já usa prerelease, perguntar ao usuário antes de alterar o canal
5. Aplicar as versões no arquivo de dependências

## Saída

Versões de pacotes aplicadas diretamente nos arquivos de dependência do projeto, sempre estáveis, atuais e compatíveis entre si.
