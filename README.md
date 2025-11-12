# 🗳️ Voting System (.NET + Next.js)

Sistema completo de votação inspirado no paredão do Big Brother Brasil, desenvolvido com **.NET**, **Next.js**, **PostgreSQL** e **RabbitMQ**, utilizando **Docker** e **Docker Compose** para orquestração dos serviços.

[![.NET](https://img.shields.io/badge/.NET%208-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com)
[![Next.js](https://img.shields.io/badge/Next.js-000000?style=flat-square&logo=next.js&logoColor=white)](https://nextjs.org)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat-square&logo=postgresql&logoColor=white)](https://www.postgresql.org)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=flat-square&logo=rabbitmq&logoColor=white)](https://www.rabbitmq.com)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)](https://www.docker.com)

---

## 📋 Sobre o Projeto

Este projeto implementa um **sistema de votação em tempo real**, utilizando uma arquitetura de microserviços em .NET, com **processamento assíncrono via RabbitMQ** e **frontend moderno em Next.js**.

### ⚙️ Funcionalidades

- ✅ Votação entre participantes  
- ✅ API RESTful em .NET 8  
- ✅ Comunicação assíncrona via RabbitMQ  
- ✅ Banco de dados em PostgreSQL  
- ✅ Frontend em Next.js 15 + React 19  
- ✅ Deploy e orquestração com Docker  
- ✅ Migrações automáticas e seed inicial  

---

## 🧱 Arquitetura

A arquitetura segue o padrão **API + Worker + Frontend**, utilizando RabbitMQ como middleware de mensageria para garantir desacoplamento, resiliência e alta performance no processamento dos votos.

Além disso, o back-end foi desenvolvido seguindo os princípios da Clean Architecture, garantindo separação clara de responsabilidades, testabilidade e facilidade de manutenção.
```
┌──────────────┐       ┌───────────────────────┐
│   Frontend   │──────▶│     API (.NET 8)     │
│  Next.js 15  │       │ Recebe votos via HTTP│
└──────────────┘       │ Publica na fila MQ   │
                       └──────────┬────────────┘
                                  │ RabbitMQ
                                  ▼
                       ┌────────────────────────┐
                       │  Worker (.NET 8)       │
                       │ Consome votos da fila  │
                       │ Persiste no PostgreSQL │
                       └──────────┬─────────────┘
                                  │
                                  ▼
                       ┌────────────────────────┐
                       │   PostgreSQL 15        │
                       │ Armazena votos e dados │
                       └────────────────────────┘
```

---

## 🚀 Início Rápido

### 🔧 Pré-requisitos

- Docker
- Docker Compose

  - Ambiente de DEV
      - .NET 8 SDK
      -  Node.js 20+

---

### ⚙️ Estrutura de Docker Compose

O sistema é dividido em 3 partes:

| Arquivo | Descrição |
|----------|------------|
| `docker-compose.infra.yml` | Banco + RabbitMQ |
| `docker-compose.backend.yml` | API + Worker |
| `docker-compose.frontend.yml` | Frontend Next.js |

---

### 🧩 Comandos principais (Makefile) (Linux/Wsl)

| Comando | Descrição |
|----------|------------|
| `make up-infra` | Sobe PostgreSQL + RabbitMQ |
| `make up-back` | Sobe API + Worker |
| `make up-front` | Sobe Front | 
| `make up-all` | Sobe tudo de uma vez |
| `make down-all` | Derruba tudo |
| `make logs-api` | Mostra logs da API |
| `make logs-worker` | Mostra logs do Worker |
| `make logs-front` | Motra logs do Front-end

```bash
# Subir tudo de uma vez (INFRA + BACK + FRONT)
make up-all

# ---- Se pre ferir pode subir separadamente ----

# Subir infraestrutura (Postgres + RabbitMQ)
make up-infra

# Subir backend (.NET API + Worker)
make up-back

# Subir frontend (Next.js)
make up-front
```

### 🪟 Comandos principais (Docker Compose) (Windows e Linux)

No Windows, utilize os comandos abaixo diretamente no terminal (cmd ou PowerShell), pois o `make` não está disponível nativamente:

| Comando | Descrição |
|----------|------------|
| `docker compose -f docker-compose.infra.yml up -d` | Sobe PostgreSQL + RabbitMQ |
| `docker compose -f docker-compose.infra.yml down` | Derruba PostgreSQL + RabbitMQ |
| `docker compose -f docker-compose.backend.yml up --build -d` | Sobe API + Worker |
| `docker compose -f docker-compose.backend.yml down` | Derruba API + Worker |
| `docker compose -f docker-compose.frontend.yml up --build -d` | Sobe Frontend |
| `docker compose -f docker-compose.frontend.yml down` | Derruba Frontend |
| `docker compose -f docker-compose.backend.yml logs -f api` | Mostra logs da API |
| `docker compose -f docker-compose.backend.yml logs -f worker` | Mostra logs do Worker |
| `docker compose -f docker-compose.frontend.yml logs -f frontend` | Mostra logs do Frontend |

**Para subir todos os serviços:**

```cmd
docker compose -f docker-compose.infra.yml up -d
docker compose -f docker-compose.backend.yml up --build -d
docker compose -f docker-compose.frontend.yml up --build -d
```


**Acesse:**

- 🌐 **Frontend**: http://localhost:3000  
- 🔌 **API .NET**: http://localhost:5000/swagger  
- 🐰 **RabbitMQ UI**: http://localhost:15672 (user: `voting_user`, pass: `voting_pass`)  
- 🗄️ **Banco de Dados**: porta `5444` (PostgreSQL)

---

## ⚙️ Variáveis de Ambiente

### API (.NET)

```env
ConnectionStrings__DefaultConnection=Host=voting_pgsql;Port=5432;Database=voting_db;Username=voting_user;Password=voting_pass
RabbitMQ__HostName=voting_rabbitmq
RabbitMQ__Port=5672
RabbitMQ__UserName=voting_user
RabbitMQ__Password=voting_pass
```

### Frontend (Next.js)

```yaml
environment:
  NEXT_PUBLIC_API_URL: "http://localhost:5000"
```

---

## 🧩 Estrutura do Projeto


```bash
voting-system/
├── back-end/                     # Backend (.NET 8)
│   ├── VotingSystem.Api/         # API principal
│   ├── VotingSystem.Worker/      # Worker de mensagens (RabbitMQ Consumer)
│   ├── VotingSystem.Domain/      # Entidades, agregados e regras de domínio
│   ├── VotingSystem.Application/ # Casos de uso e serviços de aplicação
│   └── VotingSystem.Infra.Data/  # Contexto do banco, repositórios e migrations
│   └── VotingSystem.Infra.Ioc/   # Inversão de dependência
│
├── front-end/                    # Frontend (Next.js 15 + React 19 | ShadCn)
│   ├── public/                   # Recursos estáticos
│   ├── src/                      # Código-fonte principal
│   └── package.json              # Dependências e scripts do frontend
│
├── docker-compose.infra.yml      # Infraestrutura (PostgreSQL + RabbitMQ)
├── docker-compose.backend.yml    # Backend (API + Worker)
├── docker-compose.frontend.yml   # Frontend (Next.js)
│
├── Makefile                      # Comandos de automação (up, down, logs, etc.)
└── README.md                     # Documentação principal do projeto
```

---

## 🧰 Desenvolvimento Local

Para executar o projeto localmente (sem Docker completo), é necessário ter as dependências e serviços básicos configurados corretamente.

### 🧩 Pré-requisitos

Antes de começar, certifique-se de ter instalado em sua máquina:

- 🟦 **.NET 8 SDK** — [Download aqui](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
  Usado para compilar e executar a API e o Worker.
- 🟩 **Node.js 20+** — [Download aqui](https://nodejs.org/)  
  Necessário para rodar o frontend (Next.js).
- 🐳 **Docker + Docker Compose** — para subir o PostgreSQL e RabbitMQ.

---

### 🚀 Passo a Passo

#### 1️⃣ Subir a infraestrutura (PostgreSQL + RabbitMQ)

```bash
# apenas no Linux/WSL
make up-infra
```
ou
```bash
docker compose -f docker-compose.infra.yml up -d
```

Isso criará e executará os containers de **banco de dados** e **mensageria**, necessários para o backend funcionar.

### 2️⃣ Executar as migrações do banco de dados

A API já está configurada para rodar a migration sempre que for executada, mas caso necessário:

Execute o comando abaixo:

```bash
cd back-end
dotnet ef database update --project VotingSystem.Api

```

💡 Este comando cria as tabelas e insere dados iniciais (seed) no banco configurado em appsettings.json.

#### 3️⃣ Iniciar o backend (.NET 8)

Em terminais separados:

```bash
# API principal
cd back-end
dotnet run --project VotingSystem.Api
```

```bash
# Worker (consumidor RabbitMQ)
cd back-end
dotnet run --project VotingSystem.Worker
```
### Atenção
Caso tenha rodado o docker do back-end, pode dar erro pela porta 5000 está ocupada.

### 🐞 Debug

Durante o desenvolvimento, é recomendado configurar um ambiente de debug para facilitar a inspeção do código e a depuração de erros.

#### 🔧 Opção 1 — VS Code

Para quem utiliza Visual Studio Code, é necessário instalar o .NET 8 SDK e o DevKit para VS Code, que fornece integração com o debugger do .NET e IntelliSense aprimorado.

💡 Após instalar, abra o diretório back-end e use o atalho F5 para iniciar a depuração.

#### 💻 Opção 2 — Visual Studio 2022 (apenas Windows) (Recomendado)

Se estiver no Windows, com o Visual Studio instalado, você pode abrir diretamente a solução: backend/VotingSystem.sln

#### 3️⃣ Iniciar o frontend (Next.js 15)

Em outro terminal:

```bash
cd front-end
npm install       # apenas na primeira vez
npm run dev
```

---

### 🌐 Acessos

- **Frontend:** [http://localhost:3000](http://localhost:3000)  
- **API:** [http://localhost:5000](http://localhost:5000)  
- **RabbitMQ Management:** [http://localhost:15672](http://localhost:15672) (usuário: `voting_user` / senha: `voting_pass`)

---

💡 *Dica:* se preferir rodar tudo com Docker Compose, basta usar o comando abaixo:

```bash
make up-all
```
ou


```cmd
docker compose -f docker-compose.infra.yml up -d
docker compose -f docker-compose.backend.yml up --build -d
docker compose -f docker-compose.frontend.yml up --build -d
```
---

## 🧑‍💻 Autor

**emersonv25**
