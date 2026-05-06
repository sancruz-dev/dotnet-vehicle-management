# dotnet-vehicle-management

[![.NET](https://img.shields.io/badge/.NET-7.0-512BD4?style=flat&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-21-DD0031?style=flat&logo=angular&logoColor=white)](https://angular.io/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15+-4169E1?style=flat&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![JWT](https://img.shields.io/badge/JWT-Auth-000000?style=flat&logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![Swagger](https://img.shields.io/badge/Swagger-UI-85EA2D?style=flat&logo=swagger&logoColor=black)](https://swagger.io/)
[![Semgrep](https://img.shields.io/badge/Semgrep-Security-1B2D55?style=flat&logo=semgrep&logoColor=white)](https://semgrep.dev/)
[![MSTest](https://img.shields.io/badge/MSTest-Testing-68217A?style=flat&logo=microsoft&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/core/testing/)

Sistema completo de gerenciamento de veículos com API RESTful em .NET, frontend em Angular e pipeline de análise de segurança com Semgrep.

---

## Sumário

- [Visão Geral](#visão-geral)
- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Funcionalidades](#funcionalidades)
- [Pré-requisitos](#pré-requisitos)
- [Como Rodar](#como-rodar)
- [Endpoints da API](#endpoints-da-api)
- [Análise de Segurança](#análise-de-segurança)
- [Testes](#testes)

---

## Visão Geral

Aplicação full-stack para gerenciamento de veículos com autenticação JWT, controle de acesso baseado em perfis (Adm/Editor), CRUD completo e análise estática de vulnerabilidades integrada ao fluxo de desenvolvimento.

---

## Tecnologias

### Backend
- **.NET 7** com ASP.NET Core Web API
- **Entity Framework Core 7** — Code First com migrations
- **PostgreSQL** — banco de dados relacional
- **JWT (JSON Web Tokens)** — autenticação stateless
- **BCrypt** — hash seguro de senhas
- **Swagger / OpenAPI** — documentação interativa

### Frontend
- **Angular 21** — SPA com NgModule
- **Bootstrap 5** + Bootstrap Icons — UI responsiva
- **HttpClient** com interceptor JWT automático
- **Route Guards** — proteção de rotas autenticadas

### Qualidade e Segurança
- **Semgrep** — análise estática de vulnerabilidades (regras customizadas + ruleset `p/csharp`)
- **MSTest** — testes unitários e de integração
- **Middleware global** de tratamento de exceções

---

## Arquitetura

O projeto segue os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**:

```
dotnet-vehicle-management/
│
├── API/                          # Backend principal
│   ├── Domain/                   # Núcleo do domínio
│   │   ├── Controllers/          # Endpoints da API
│   │   ├── DTOs/                 # Objetos de transferência de dados
│   │   ├── Entities/             # Entidades de negócio
│   │   ├── Enums/                # Enumerações do domínio
│   │   ├── Interfaces/           # Contratos de serviços
│   │   ├── ModelViews/           # Modelos de resposta
│   │   └── Services/             # Regras de negócio
│   ├── infrastructure/           # Implementações concretas
│   │   ├── Auth/                 # Configuração JWT
│   │   ├── DB/                   # DbContext e migrations
│   │   └── ExceptionMiddleware   # Tratamento global de erros
│   ├── Migrations/               # Histórico de migrations EF Core
│   ├── Startup.cs                # Configuração de serviços e middlewares
│   └── Program.cs                # Entry point
│
├── frontend/                     # Frontend Angular
│   └── src/app/
│       ├── core/
│       │   ├── guards/           # AuthGuard
│       │   └── interceptors/     # Interceptor JWT
│       ├── models/               # Interfaces TypeScript
│       ├── pages/
│       │   ├── login/            # Tela de login
│       │   └── veiculos/         # CRUD de veículos
│       └── services/             # AuthService, VeiculoService
│
├── Test/                         # Projeto de testes
│   ├── Domain/                   # Testes de entidades e serviços
│   ├── Helpers/                  # Setup de testes de integração
│   ├── Mocks/                    # Mocks de serviços
│   └── Requests/                 # Testes de endpoints HTTP
│
├── .semgrep.yml                  # Regras customizadas de segurança
└── semgrep-scan.ps1              # Script de análise de vulnerabilidades
```

### Decisões de Design

- **Repository Pattern** via interfaces (`IAdminService`, `IVeiculoService`) desacoplando domínio da infraestrutura
- **Dependency Injection** em toda a cadeia de serviços
- **Role-based Authorization** com perfis `Adm` e `Editor`
- **BCrypt** para hash de senhas, nunca armazenadas em texto puro
- **Middleware de exceções** centralizado, evitando try/catch espalhados pelos controllers

---

## Funcionalidades

### API
- Autenticação com JWT e expiração configurável
- CRUD completo de veículos com paginação
- Gerenciamento de administradores (restrito ao perfil `Adm`)
- Validação de dados em todas as entradas
- Documentação Swagger disponível em `/swagger`

### Frontend
- Login com feedback de erro e loading state
- Listagem de veículos em tempo real
- Cadastro e edição inline via formulário
- Exclusão com confirmação (restrita ao perfil `Adm`)
- Logout com limpeza de sessão
- Redirecionamento automático para login em caso de token expirado

---

## Pré-requisitos

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Node.js 18+](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Python 3.8+](https://www.python.org/) (para o Semgrep)

---

## Como Rodar

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/dotnet-vehicle-management.git
cd dotnet-vehicle-management
```

### 2. Configure o banco de dados

No PostgreSQL, crie o banco:

```sql
CREATE DATABASE db_minimal_api;
```

### 3. Configure a connection string

Em `API/appsettings.json`, ajuste:

```json
"ConnectionStrings": {
  "Postgresql": "Host=localhost;Port=5432;Database=db_minimal_api;Username=postgres;Password=SUA_SENHA"
}
```

### 4. Aplique as migrations e rode a API

```bash
cd API
dotnet ef database update
dotnet run
```

A API estará disponível em `http://localhost:5097`.  
Swagger em `http://localhost:5097/swagger`.

### 5. Rode o frontend

```bash
cd frontend
npm install
ng serve
```

Acesse `http://localhost:4200`.

### Credenciais padrão

| Campo | Valor |
|---|---|
| Email | `admin@teste.com` |
| Senha | `123456` |

---

## Endpoints da API

### Autenticação

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| POST | `/Admins/login` | Autenticação e geração de token JWT | Público |

### Veículos

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/Veiculos/veiculos` | Lista veículos (paginado) | JWT |
| GET | `/Veiculos/veiculo/{id}` | Busca veículo por ID | JWT |
| POST | `/Veiculos/veiculo` | Cadastra novo veículo | JWT |
| PUT | `/Veiculos/veiculo/{id}` | Atualiza veículo | Adm |
| DELETE | `/Veiculos/veiculo/{id}` | Remove veículo | Adm |

### Administradores

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/Admins/admins` | Lista administradores | Adm |
| GET | `/Admins/admins/{id}` | Busca administrador por ID | Adm |
| POST | `/Admins/admin` | Cadastra administrador | Adm |

---

## Análise de Segurança

O projeto utiliza **Semgrep** para análise estática de vulnerabilidades, com regras customizadas e o ruleset oficial `p/csharp`.

### Instalar o Semgrep

```bash
pip install semgrep
```

### Rodar a análise

```powershell
.\semgrep-scan.ps1
```

Dois relatórios JSON são gerados:

| Arquivo | Conteúdo |
|---|---|
| `semgrep-custom-report.json` | Resultado das regras customizadas do projeto |
| `semgrep-full-report.json` | Resultado do ruleset oficial `p/csharp` (27 regras) |

### Regras customizadas

| ID | Severidade | Descrição |
|---|---|---|
| `sql-injection-executesqlraw` | WARNING | Detecta uso de `ExecuteSqlRaw` sem parâmetros |
| `console-writeline-sensitive` | INFO | Detecta `Console.WriteLine` que pode expor dados em produção |

---

## Testes

```bash
cd Test
dotnet test
```

O projeto de testes cobre entidades, serviços e endpoints HTTP com mocks, utilizando **MSTest** e `WebApplicationFactory` para testes de integração.