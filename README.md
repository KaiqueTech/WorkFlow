# 🚀 WorkFlow

## 📂 Estrutura do Projeto

A solução segue os princípios da **Clean Architecture**, garantindo separação de responsabilidades, baixo acoplamento e alta testabilidade.

Além disso, o projeto utiliza o padrão **CQRS (Command Query Responsibility Seg﻿regation)** para separar operações de escrita e leitura, **sem o uso de MediatR**, mantendo a comunicação direta entre os serviços da aplicação para maior simplicidade e controle.

WorkFlow
│
├── Workflow.Api
│ ├── Controllers
│ ├── Middleware
│ ├── Program.cs
│ └── appsettings.json
│
├── Workflow.Application
│ ├── Abstractions
│ ├── Commands
│ ├── Queries
│ ├── DTOs
│ ├── Interfaces
│ ├── Exceptions
│ ├── Settings
│ └── DependencyInjection
│
├── Workflow.Domain
│ ├── Models
│ ├── Enums
│ ├── Constants
│ ├── Interfaces
│ └── Exceptions
│
├── Workflow.Infra
│ ├── Configurations
│ ├── Migrations
│ └── DependencyInjection
│
├── Workflow.Web
│ └── (Frontend Angular)
│
├── Workflow.Tests
│ └── Testes unitários
│
├── docker-compose.yaml
└── WorkFlow.slnx




### 🔹 Descrição das Camadas

- **Workflow.Api** → expõe endpoints REST e configura autenticação, middlewares e controllers.  
- **Workflow.Application** → contém casos de uso, Commands & Queries (CQRS), DTOs e regras de orquestração.  
- **Workflow.Domain** → núcleo do sistema com entidades e regras de negócio.  
- **Workflow.Infra** → acesso a banco de dados, configurações e implementações técnicas.  
- **Workflow.Web** → interface frontend da aplicação.  
- **Workflow.Tests** → testes automatizados para garantir qualidade e estabilidade.  

---

## 🧠 Padrões e Abordagens Utilizadas

- Clean Architecture  
- SOLID Principles  
- CQRS (sem MediatR)  
- Repository Pattern  
- Dependency Injection  
- Separation of Concerns  

---

## 📚 Referência de Arquitetura

Este projeto foi inspirado no **Mini Hub de Catálogo**, utilizado como base conceitual para consolidar práticas modernas de desenvolvimento backend.

Os principais conceitos adotados incluem:

- arquitetura em camadas e separação de responsabilidades  
- autenticação segura e controle de acesso  
- uso estruturado do Entity Framework Core  
- consultas eficientes e organização do domínio  
- auditoria e rastreabilidade de operações  
- boas práticas de segurança e organização de código  

A implementação foi adaptada e evoluída para atender às necessidades específicas do **WorkFlow**, mantendo foco em escalabilidade e clareza arquitetural.

---

# ▶️ Como Rodar o Projeto

## ✅ Pré-requisitos

Instale:

- .NET SDK 8+  
- Docker Desktop  
- Git  
- Node.js 18+  
- Angular CLI  

Instalar Angular CLI (caso não tenha):

```bash
npm install -g @angular/cli

# clonar repositório
git clone https://github.com/KaiqueTech/WorkFlow.git
cd WorkFlow

# criar container docker
docker-compose up -d

# restaurar dependências
dotnet restore

# compilar solução
dotnet build

# aplicar migrations
dotnet ef database update --project Workflow.Infra --startup-project Workflow.Api

# rodar API
dotnet run --project Workflow.Api

# rodar frontend
cd Workflow.Web
npm install
ng serve

#Observações

O projeto utiliza CQRS sem MediatR, mantendo simplicidade e controle direto dos casos de uso.

Docker é recomendado para padronizar o ambiente.

A arquitetura facilita testes, manutenção e evolução futura.
