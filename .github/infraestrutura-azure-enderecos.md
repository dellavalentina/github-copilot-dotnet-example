# Infraestrutura Azure — Endereços de Usuários

---

# Parte 1 — Tecnologias Necessárias

## Contexto

A feature de endereços permite que cada usuário cadastre e gerencie múltiplos endereços, com um deles marcado como principal. Trata-se de uma funcionalidade CRUD sobre dados estruturados, sem necessidade de armazenamento de arquivos, envio de e-mails ou processamento em segundo plano.

---

## Serviços Azure Necessários

### Azure SQL Database

**Para que serve:** Banco de dados relacional na nuvem que armazena os dados do sistema de forma organizada e segura.

**Por que esta feature precisa dele:** Os endereços dos usuários são dados estruturados que se relacionam diretamente com a tabela de usuários. O Azure SQL Database já está provisionado para o projeto e armazenará a nova tabela de endereços com os campos CEP, logradouro, número, complemento, bairro, cidade, estado e a indicação de principal.

**Nível de uso:** Básico — a feature adiciona uma nova tabela com operações CRUD simples e volume proporcional ao número de usuários.

### Azure App Service

**Para que serve:** Hospeda a API do sistema, processando as requisições dos usuários e retornando as respostas.

**Por que esta feature precisa dele:** Os novos endpoints de endereços (cadastrar, editar, listar, desativar, marcar como principal) serão expostos pela API que já roda no App Service. Não há necessidade de escalar ou alterar a configuração existente.

**Nível de uso:** Básico — os endpoints de endereços seguem o mesmo padrão dos endpoints de usuários já existentes.

### Azure Key Vault

**Para que serve:** Armazena com segurança as chaves, senhas e segredos que o sistema utiliza (como a connection string do banco e a chave JWT).

**Por que esta feature precisa dele:** A feature não introduz novos segredos, mas depende dos já existentes (connection string para acessar o banco de dados). O Key Vault já está provisionado.

**Nível de uso:** Básico — sem novos segredos a adicionar.

---

## Resumo de Tecnologias

| Serviço            | Papel na Feature                         | Quando Provisionar |
| ------------------ | ---------------------------------------- | ------------------ |
| Azure SQL Database | Armazena a tabela de endereços           | Já provisionado    |
| Azure App Service  | Hospeda os endpoints da API de endereços | Já provisionado    |
| Azure Key Vault    | Armazena segredos de conexão             | Já provisionado    |

---

---

# Parte 2 — Guia de Criação no Portal Azure

> Esta feature não requer a criação de novos serviços Azure. Todos os recursos necessários já estão provisionados.

A única ação necessária na infraestrutura é a execução da migration do Entity Framework Core para criar a tabela de endereços no banco de dados. Isso será feito como parte do processo de desenvolvimento (via `dotnet ef migrations add` e `dotnet ef database update`), não no Portal Azure.

Nenhuma etapa de provisionamento manual é necessária.
