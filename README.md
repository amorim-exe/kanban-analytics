# 🚀 Revo - Sistema de Gestão de Projetos com Kanban e Analytics

Um sistema moderno e impressionante de gestão de projetos com Kanban Board interativo, Dashboard de Analytics em tempo real, e arquitetura enterprise-grade.

## ✨ Features

- **🎨 Frontend Moderno**: React + TypeScript + Tailwind CSS com design impactante
- **📊 Dashboard Analytics**: Gráficos em tempo real com Recharts
- **🎯 Kanban Board**: Drag & drop com @dnd-kit
- **⚡ Real-time Updates**: SignalR para atualizações instantâneas
- **🗄️ SQL Server**: Banco de dados robusto com Entity Framework Core
- **💾 Redis Cache**: Cache distribuído para performance
- **📬 Background Tasks**: Sistema de filas para processamento assíncrono
- **🏗️ Clean Architecture**: Separação clara de responsabilidades (Domain, Application, Infrastructure)
- **🔧 .NET 10**: Última versão do .NET com ASP.NET Core Web API

## 🏗️ Arquitetura

```
Revo/
├── Revo.Domain/          # Entidades e lógica de negócio
├── Revo.Application/     # DTOs, Interfaces e validações
├── Revo.Infrastructure/  # Repositories, Cache, Background Services
├── Revo.API/            # Web API com SignalR
├── Revo.Frontend/       # React + TypeScript
└── docker-compose.yml   # SQL Server + Redis
```

## 🚀 Como Rodar

### Pré-requisitos

- .NET 10 SDK
- Node.js 18+
- Docker e Docker Compose

### 1. Iniciar Infraestrutura

```bash
docker-compose up -d
```

Isso vai iniciar:
- SQL Server na porta 1433
- Redis na porta 6379

### 2. Configurar e Rodar Backend

```bash
cd Revo.API
dotnet restore
dotnet run
```

A API estará disponível em `http://localhost:5000`

Swagger UI: `http://localhost:5000/swagger`

### 3. Configurar e Rodar Frontend

```bash
cd Revo.Frontend
npm install
npm run dev
```

O frontend estará disponível em `http://localhost:5173`

## 📦 Tecnologias

### Backend
- **.NET 10** - Framework principal
- **ASP.NET Core Web API** - API REST
- **Entity Framework Core** - ORM
- **SQL Server** - Banco de dados
- **Redis** - Cache distribuído
- **SignalR** - Real-time updates
- **MediatR** - Pattern CQRS (preparado para implementação)

### Frontend
- **React 18** - UI Library
- **TypeScript** - Type safety
- **Vite** - Build tool
- **Tailwind CSS** - Styling
- **Recharts** - Gráficos
- **@dnd-kit** - Drag & drop
- **Axios** - HTTP client
- **@microsoft/signalr** - SignalR client

## 🎯 Funcionalidades

### Dashboard
- Cards com métricas principais (Total, Concluídas, Em Progresso, Atrasadas)
- Gráfico de Pizza: Tarefas por Status
- Gráfico de Barras: Tarefas por Prioridade
- Gráfico de Barras: Progresso por Projeto

### Kanban Board
- 4 colunas: A Fazer, Em Progresso, Em Revisão, Concluído
- Drag & drop entre colunas
- Ordenação dentro das colunas
- Visualização de prioridade, data de vencimento, horas estimadas
- Indicadores de comentários e anexos
- Atualização em tempo real via SignalR

### Projetos
- CRUD completo de projetos
- Cores personalizáveis
- Filtro por projeto
- Métricas por projeto

### Tarefas
- CRUD completo de tarefas
- Prioridades (Baixa, Média, Alta, Crítica)
- Data de vencimento
- Estimativa de horas
- Log de horas trabalhadas
- Atribuição a usuários (preparado)
- Comentários e anexos (preparado)

## 🔧 Configuração

### Backend (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=RevoDb;User Id=sa;Password=YourStrong@Password;TrustServerCertificate=True;"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

### Frontend (src/services/api.ts)

```typescript
const API_BASE_URL = 'http://localhost:5000/api';
```

## 📝 Próximos Passos

- [ ] Implementar autenticação JWT
- [ ] Adicionar upload de arquivos
- [ ] Implementar comentários em tarefas
- [ ] Adicionar histórico de atividades
- [ ] Implementar notificações
- [ ] Adicionar testes unitários
- [ ] Deploy em produção

## 🎨 Design

O sistema foi desenhado para impressionar recrutadores com:
- Interface moderna e responsiva
- Animações suaves
- Cores vibrantes e gradientes
- Dashboard com visualizações de dados
- Experiência de usuário fluida com drag & drop
- Atualizações em tempo real

## 📄 Licença

Este projeto foi criado para fins de demonstração e portfólio.

---

Desenvolvido com ❤️ usando .NET 10, React e as melhores práticas de arquitetura.
