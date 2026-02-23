# AccountLimit.API

AccountLimit é uma Web API (.NET 8) para controle e autorização de transações PIX, avaliando o limite disponível de uma conta e decidindo se a transação pode ou não ser autorizada.

O projeto foi estruturado com Clean Architecture, aplicando conceitos de DDD (Domain-Driven Design) e usando o padrão MVC na camada de apresentação, com foco em manutenibilidade, separação de responsabilidades e testabilidade.

##  Funcionalidades

-  Gestão de limites: cadastrar, consultar, atualizar e remover limites por conta/identificação do cliente.
-  Autorização de transações PIX: valida se o valor solicitado respeita o limite disponível e retorna o resultado de autorização.
-  Persistência no DynamoDB.
-  Documentação da API via Swagger (OpenAPI).
-  Arquitetura em camadas com Clean Architecture + DDD, facilitando testes unitários e evolução do domínio.
-  Front-End feito em angular - https://github.com/devgferreira/AccountLimit-front-end
---

## 🛠️ Tecnologias Utilizadas

| Tecnologia        | Descrição |
|------------------|---------|
| **.NET 8**       | Plataforma principal para desenvolvimento da API. |
| **DynamoDB**   | Banco de dados NoSQL totalmente gerenciado pela AWS, baseado em chave-valor e documentos, altamente escalável, com baixa latência e ideal para aplicações distribuídas e de alta performance. |
| **Swagger**      | Documentação interativa da API. |
| **JWT**      | Token de autenticação. |




## 📦 Pré-requisitos

- ✅ [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ Um IDE compatível com .NET (recomendado: **Visual Studio 2025** ou **Visual Studio Code**)
- ✅ DynamoDB
---

## 🚀 Como Rodar o Projeto

### 1. Configurar a AWS e DynamoDB

#### Criar uma tabela no DynamoDB

1. Acesse:
👉 https://console.aws.amazon.com/dynamodbv2

       Clique em Create table
       Nome da tabela: gestor-de-limites(caso você coloque outro nome vai ter que ajustar via codigo.)
       Chave de partição: pk -  string
       Chave de classificação: sk - string
      
#### Criar usuário no IAM

1. Acesse:
👉 https://console.aws.amazon.com/iam

        Vá em Users (Usuários)
        Clique em Create user
        Nome do usuário: dynamodb-app-user(ou outro se preferir)
   
2. Adicionar permissões para DynamoDB

Escolha uma das opções:

- Opção simples (recomendada para dev):

Selecione:

    AmazonDynamoDBFullAccess

Em produção, recomenda-se criar uma policy personalizada com permissões mínimas necessárias.

#### Gerar Access Keys
1. Acesse o usuário criado
2. Vá até a aba Security credentials
3. Clique em Create access key
4. Escolha:
   
       Application running outside AWS
 5. Confirme:
    
    A AWS irá gerar:
      
        AWS_ACCESS_KEY_ID
        AWS_SECRET_ACCESS_KEY
    O AWS_SECRET_ACCESS_KEY será exibido apenas uma vez. Salve em local seguro.

#### Definir Região (AWS_REGION)

No canto superior direito do console AWS, identifique sua região.

Exemplo para São Paulo:

        sa-east-1

### 2. Configurar as variáveis de ambiente

Crie um arquivo `.env` dentro do seguinte diretório:

- `AccountLimit.API/.env`

Com o seguinte conteúdo:

```env
AWS_ACCESS_KEY_ID=
AWS_SECRET_ACCESS_KEY=
AWS_REGION=

JWT_KEY=
JWT_ISSUER=
JWT_AUDIENCE=

```


# Estrutura do Projeto

O projeto segue uma arquitetura **limpa e modular**, separando responsabilidades em camadas distintas:

## 📦 API
Responsável por expor endpoints e lidar com solicitações HTTP.

- **Controller**: Controladores de API, responsáveis por receber requisições e retornar respostas.

## 📦 Application
Camada de aplicação, responsável por lógica de integração, DTOs e serviços.
 
- **DTO**: Objetos de Transferência de Dados usados para comunicar entre camadas.  
- **Interface**: Contratos para os serviços da aplicação.  
- **Service**: Contém lógica de negócios de alto nível e orquestra chamadas aos repositórios.
- 
## 📦 Domain
Camada de domínio, responsável pelas regras de negócio essenciais.

- **Commom**: Elementos compartilhados entre o domnínio.
- **Entities**: Entidades de domínio.
- **Interface**: Contratos para os repositórios de domínio.
- **ValueObjects**: Objeto do domínio que representa um conceito definido exclusivamente pelos seus valores.

## 📦 Infra.Data
Camada de persistência, responsável pelo acesso ao banco de dados.

- **Entities**: Entidades do DynamoDB;
- **Mapping**: Mapeadores entre a entidade de dominio e a entidade do DynamoDB
- **Repository**: Contém a lógica de consulta às tabelas do banco.

## 📦 Infra.Ioc
Responsável por gerenciar a injeção de dependências do projeto.

## 📦 Teste
Responsável por gerenciar os teste da aplicação.



# Authenticate.API

Authenticate.API é uma Web API (.NET 8) responsável pela autenticação de usuários e geração de tokens JWT (JSON Web Token) para autorização de acesso a serviços protegidos.

A API centraliza o processo de login, validação de credenciais e emissão de tokens seguros, permitindo que outras aplicações utilizem autenticação baseada em Bearer Token.


##  Funcionalidades

-  Geração de token com base no login e registro.
-  Persistência em memoria.
-  Documentação da API via Swagger (OpenAPI).
---

## 🛠️ Tecnologias Utilizadas

| Tecnologia        | Descrição |
|------------------|---------|
| **.NET 8**       | Plataforma principal para desenvolvimento da API. |
| **Swagger**      | Documentação interativa da API. |
| **JWT**      | Token de autenticação. |


## 📦 Pré-requisitos

- ✅ [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ Um IDE compatível com .NET (recomendado: **Visual Studio 2025** ou **Visual Studio Code**)
---

## 🚀 Como Rodar o Projeto

Sempre que a aplicação é iniciada um usuário padrão é criado: Username: analista1, Password: analista123 e Role: ANALISTA_FRAUDE.

Por padrão os end-points da controlar LimitManagement da AccountLimit.API só podem ser disparado caso o usuário tenha a role: ANALISTA_FRAUDE.

### 1. Configurar as variáveis de ambiente

Crie um arquivo `.env` dentro do seguinte diretório:

- `Authenticate.API/.env`

Com o seguinte conteúdo:

```env
JWT_KEY=
JWT_ISSUER=
JWT_AUDIENCE=
```
# Estrutura do Projeto

## 📦 API
Responsável por expor endpoints e lidar com solicitações HTTP.

- **Controller**: Controladores de API, responsáveis por receber requisições e retornar respostas.
- **Models**: Entidades de domínio.
- **Repository**: Contém a lógica de consulta às tabelas do banco.
- **Security**: Contém a lógica de geração de token
- **Service**: Contém lógica de negócios de alto nível e orquestra chamadas aos repositórios.


