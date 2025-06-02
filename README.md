# L2 Embalagem

Microserviço que embala produtos na menor quantidade de caixas possível, priorizando as de menor tamanho.

## Pontos de destaque

- API REST implementado em ASP.NET, C# e .NET 9;
- Persistência de dados com o SQL Server;
- Preparação do banco de dados com comandos DDL;
- Manipulação de dados com o Dapper, functions, procedimentos armazenados e comandos DML;
- Microserviço e base de dados conteinerizados com o Docker e Docker Compose;
- Endpoints testáveis através do Swagger;
- Autenticação por meio de tokens JWT;
- Testes unitários com o xUnit;

## Como executar

### Pré-requisitos

- Docker Desktop configurado para usar no mínimo 2GB de RAM;

### Instruções

Assegure que o Docker está rodando e siga os seguintes passos:
1. Clone o repositório do projeto:
```bash
git clone https://github.com/marvipi-dev/l2-embalagem.git
```
2. Entre na pasta raiz do projeto (onde se encontra o arquivo compose.yml):
```bash
cd l2-embalagem
```
3. Mude para o branch dev:
```bash
git switch dev
```
4. Compile o projeto com o Docker Compose:
```bash
docker compose build
```
5. Rode o projeto com o Docker Compose:
```bash
docker compose up
```

Com isso, o Docker compilará o API e o subirá dentro de um contêiner e também baixará o banco de dados pré-configurado
do Docker Hub ([link](https://hub.docker.com/r/marvipi/embalagem-repository-sqlserver "link")).

O Swagger poderá então ser acessado pela URL: http://localhost/swagger.

## API
O API segue o padrão REST e possui quatro endpoints:
- Embalar pedidos;
- Exibir pedidos já embalados;
- Cadastrar usuário;
- Autenticar usuário;

![Swagger](res/api.png)

## Requisitos para entrega

1. 🗹 Fazer microserviço em .NET Core ou superior utilizando banco de dados SQL Server;
2. 🗹 Tanto o serviço como o banco de dados deve rodar via docker;
3. 🗹 Deve conter REAME.md com os pré-requisitos (provavelmente apenas o docker) e
   comandos necessários para rodar a aplicação, recomendado utilizar o “docker-compose”
4. 🗹 A API precisa ter swagger e ser possível testar ela a partir do swagger;
5. 🗹 Enviar o código fonte via link do repositório do github;

## Requisitos opcionais

1. 🗹 Segurança na autenticação da API;
2. 🗹 Deve conter teste unitário;