# Feature: Gerenciamento de Usuários

## Descrição do Problema

Atualmente o sistema não possui uma forma de gerenciar quem pode acessá-lo. Qualquer pessoa precisa conseguir criar sua própria conta informando seus dados pessoais, e a partir daí ter acesso ao sistema. Uma vez dentro do sistema, o usuário deve poder consultar outros usuários cadastrados, manter seus próprios dados atualizados e, se necessário, desativar sua própria conta.

O sistema precisa distinguir dois tipos de usuário: o **Usuário Comum**, que gerencia apenas sua própria conta, e o **Administrador**, que além de tudo que o Usuário Comum faz, também pode reativar contas que foram desativadas. O próprio usuário escolhe seu perfil no momento em que cria a conta.

Para garantir que não existam contas duplicadas, o sistema deve tratar o e-mail e o CPF como identificadores únicos — nenhum outro usuário pode se cadastrar com os mesmos valores.

---

## Regras de Negócio

### O que o sistema deve fazer

- Qualquer pessoa, sem precisar estar autenticada, pode criar uma conta informando nome completo, CPF, data de nascimento, e-mail, senha e escolhendo um perfil (Usuário Comum ou Administrador).
- Todo usuário autenticado pode visualizar a lista de todos os usuários cadastrados no sistema.
- A listagem de usuários pode ser filtrada por nome, e-mail, CPF ou status da conta (ativa ou inativa).
- Todo usuário autenticado pode visualizar os detalhes completos de qualquer usuário.
- Todo usuário autenticado pode editar os próprios dados: nome completo, CPF, data de nascimento, e-mail e senha.
- Todo usuário autenticado pode desativar a própria conta.
- Um Administrador pode reativar qualquer conta que esteja desativada.

### O que o sistema não deve permitir

- Não é permitido cadastrar dois usuários com o mesmo e-mail.
- Não é permitido cadastrar dois usuários com o mesmo CPF.
- Não é permitido que um usuário altere seu CPF para um valor que já pertença a outra conta.
- Um usuário com a conta desativada não pode acessar o sistema.
- Um usuário não pode editar os dados de outro usuário.
- Um usuário não pode desativar a conta de outro usuário — exceto o Administrador, que pode reativar contas alheias, mas não desativá-las em lugar do dono.
- Um usuário que desativou a própria conta não pode reativá-la por conta própria — somente um Administrador pode fazer isso.

---

## Critérios de Aceite

**Cenário: Cadastro realizado com sucesso**

- **Dado que** um visitante acessa a tela de cadastro
- **Quando** preenche nome completo, CPF, data de nascimento, e-mail, senha e seleciona o perfil desejado
- **Então** o sistema cria a conta e permite que o usuário acesse o sistema

---

**Cenário: Tentativa de cadastro com e-mail já existente**

- **Dado que** já existe uma conta com o e-mail `joao@exemplo.com`
- **Quando** um novo visitante tenta se cadastrar com o mesmo e-mail
- **Então** o sistema impede o cadastro e informa que o e-mail já está em uso

---

**Cenário: Tentativa de cadastro com CPF já existente**

- **Dado que** já existe uma conta com o CPF `123.456.789-00`
- **Quando** um novo visitante tenta se cadastrar com o mesmo CPF
- **Então** o sistema impede o cadastro e informa que o CPF já está em uso

---

**Cenário: Listagem e busca de usuários**

- **Dado que** um usuário está autenticado
- **Quando** acessa a listagem de usuários e aplica um filtro por nome, e-mail, CPF ou status
- **Então** o sistema exibe apenas os usuários que correspondem ao filtro aplicado

---

**Cenário: Edição dos próprios dados**

- **Dado que** um usuário está autenticado
- **Quando** altera um ou mais dos seus dados (nome, CPF, data de nascimento, e-mail ou senha) e salva
- **Então** o sistema atualiza as informações e passa a exibir os novos dados

---

**Cenário: Tentativa de edição com e-mail já usado por outro usuário**

- **Dado que** um usuário autenticado está editando seus dados
- **Quando** informa um e-mail que já pertence a outra conta
- **Então** o sistema impede a alteração e informa que o e-mail já está em uso

---

**Cenário: Tentativa de edição com CPF já usado por outro usuário**

- **Dado que** um usuário autenticado está editando seus dados
- **Quando** informa um CPF que já pertence a outra conta
- **Então** o sistema impede a alteração e informa que o CPF já está em uso

---

**Cenário: Desativação da própria conta**

- **Dado que** um usuário está autenticado
- **Quando** solicita a desativação da própria conta
- **Então** o sistema desativa a conta e o usuário perde o acesso ao sistema imediatamente

---

**Cenário: Tentativa de acesso com conta desativada**

- **Dado que** uma conta está desativada
- **Quando** o titular tenta fazer login
- **Então** o sistema impede o acesso

---

**Cenário: Reativação de conta por Administrador**

- **Dado que** uma conta está desativada e um Administrador está autenticado
- **Quando** o Administrador reativa essa conta
- **Então** o sistema reativa a conta e o titular volta a conseguir acessar o sistema

---

**Cenário: Usuário Comum tenta reativar uma conta**

- **Dado que** uma conta está desativada e um Usuário Comum está autenticado
- **Quando** tenta reativar a conta desativada
- **Então** o sistema impede a ação, pois somente Administradores podem reativar contas
