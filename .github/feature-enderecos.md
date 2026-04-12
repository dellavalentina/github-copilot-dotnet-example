# Feature: Endereços de Usuários

## Descrição do Problema

No sistema atual, as informações dos usuários cadastrados se limitam a dados pessoais como nome, CPF e e-mail. No entanto, há a necessidade de que cada usuário possa informar seus endereços — por exemplo, para entregas, correspondências ou qualquer outra finalidade que o sistema venha a exigir no futuro.

Cada usuário poderá cadastrar quantos endereços desejar, mas apenas um deles pode ser marcado como o endereço principal. Isso facilita situações em que o sistema precisa saber rapidamente qual é o endereço preferido do usuário sem que ele precise escolher toda vez.

Quando o usuário cadastra seu primeiro endereço, o sistema deve marcá-lo automaticamente como principal, já que é o único disponível. Se o usuário decidir que outro endereço deve ser o principal, basta marcar o novo — o sistema automaticamente remove a marcação do endereço que era principal antes, garantindo que nunca exista mais de um endereço principal ao mesmo tempo.

O usuário também pode ficar sem nenhum endereço cadastrado — não há obrigatoriedade de ter pelo menos um. Da mesma forma, se o usuário desativar ou remover o endereço principal, nenhum outro endereço é promovido automaticamente — o usuário fica sem endereço principal até que escolha um novo.

---

## Regras de Negócio

### O que o sistema deve fazer

- Permitir que um usuário cadastre múltiplos endereços
- Exigir que os campos CEP, logradouro, número, bairro, cidade e estado sejam informados ao cadastrar um endereço
- Permitir que o campo complemento seja opcional
- Marcar automaticamente o primeiro endereço de um usuário como principal
- Permitir que o usuário altere qual endereço é o principal a qualquer momento
- Ao marcar um novo endereço como principal, remover automaticamente a marcação de principal do endereço anterior
- Permitir que o usuário edite os dados de um endereço já cadastrado
- Permitir que o usuário desative um endereço (o endereço deixa de aparecer, mas não é removido permanentemente)
- Permitir listar todos os endereços de um determinado usuário, com suporte a paginação

### O que o sistema não deve permitir

- Não é permitido que um usuário tenha mais de um endereço marcado como principal ao mesmo tempo
- Não é permitido cadastrar um endereço sem os campos obrigatórios (CEP, logradouro, número, bairro, cidade, estado)
- Não é permitido cadastrar um endereço para um usuário que não existe ou que esteja desativado
- Não é permitido que um usuário acesse ou gerencie endereços de outro usuário

---

## Critérios de Aceite

**Cenário: Cadastrar o primeiro endereço de um usuário**

- **Dado que** o usuário autenticado não possui nenhum endereço cadastrado
- **Quando** ele cadastrar um novo endereço informando todos os campos obrigatórios
- **Então** o endereço deve ser salvo e marcado automaticamente como principal

**Cenário: Cadastrar um endereço adicional**

- **Dado que** o usuário autenticado já possui pelo menos um endereço cadastrado
- **Quando** ele cadastrar um novo endereço informando todos os campos obrigatórios
- **Então** o endereço deve ser salvo sem a marcação de principal (o principal anterior permanece inalterado)

**Cenário: Marcar um endereço como principal**

- **Dado que** o usuário possui mais de um endereço cadastrado e um deles é o principal
- **Quando** ele marcar outro endereço como principal
- **Então** o novo endereço passa a ser o principal e o anterior perde a marcação automaticamente

**Cenário: Tentar cadastrar endereço sem campo obrigatório**

- **Dado que** o usuário está tentando cadastrar um novo endereço
- **Quando** ele não informar um dos campos obrigatórios (CEP, logradouro, número, bairro, cidade ou estado)
- **Então** o sistema deve rejeitar o cadastro informando qual campo está faltando

**Cenário: Tentar cadastrar endereço para usuário desativado**

- **Dado que** um usuário está desativado no sistema
- **Quando** houver tentativa de cadastrar um endereço para ele
- **Então** o sistema deve rejeitar a operação informando que o usuário está desativado

**Cenário: Desativar o endereço principal**

- **Dado que** o usuário possui um endereço marcado como principal
- **Quando** ele desativar esse endereço
- **Então** o endereço é desativado e nenhum outro endereço é promovido a principal automaticamente

**Cenário: Editar dados de um endereço**

- **Dado que** o usuário possui um endereço cadastrado
- **Quando** ele editar os dados desse endereço (por exemplo, alterar o número ou o complemento)
- **Então** os dados atualizados devem ser salvos mantendo o estado de principal inalterado

**Cenário: Listar endereços de um usuário**

- **Dado que** o usuário possui endereços cadastrados
- **Quando** ele solicitar a listagem dos seus endereços
- **Então** o sistema deve retornar a lista paginada dos endereços, indicando qual é o principal

---

## Configuração do Projeto

| Configuração | Valor |
| --- | --- |
| Nome da solution | GerenciamentoUsuarios |
| Target framework | net10.0 |
| Primeira feature do projeto? | Não |
| Autenticação JWT configurada? | Sim |
| Serviços de integração existentes | Nenhum |
| Região Azure | Brazil South |
| Ambiente | Desenvolvimento |
