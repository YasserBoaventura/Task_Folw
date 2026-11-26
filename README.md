# TaskFlow

**TaskFlow** é uma plataforma web de gestão de projectos e tarefas, desenvolvida para facilitar a organização de equipas, o acompanhamento do progresso dos projectos e a colaboração entre membros.

O sistema permite criar projectos, gerir membros, distribuir tarefas, acompanhar estados de execução e centralizar a comunicação da equipa.

O projecto está a ser desenvolvido como uma aplicação de arquitectura em camadas, utilizando **ASP.NET Core Web API, C#, Entity Framework Core e MySQL** no backend.

---

## 1. Objectivos do projecto

O principal objectivo do TaskFlow é disponibilizar uma plataforma que permita:

- Criar e gerir projectos.
- Organizar equipas e gerir membros.
- Distribuir tarefas entre os membros de cada projecto.
- Acompanhar o progresso das tarefas.
- Definir prioridades e prazos.
- Visualizar tarefas num quadro Kanban.
- Gerir comentários e notificações.
- Controlar o acesso aos recursos através de autenticação e autorização.
- Manter um histórico organizado das actividades.

## 2. Tecnologias utilizadas

### Backend

| Tecnologia | Utilização |
|---|---|
| C# | Linguagem de programação |
| .NET 10 | Plataforma de desenvolvimento |
| ASP.NET Core Web API | Construção da API REST |
| Entity Framework Core 10 | ORM e acesso à base de dados |
| MySQL 8 | Sistema de gestão de base de dados |
| ASP.NET Core Identity | Gestão de utilizadores |
| JWT | Autenticação e autorização |
| Refresh Tokens | Renovação de sessões |
| Data Annotations | Validação de dados |

### Frontend (planeado)

| Tecnologia | Utilização |
|---|---|
| React | Construção da interface |
| TypeScript | Tipagem estática |
| HTML5 | Estrutura |
| CSS3 | Estilização |

### Ferramentas

- Visual Studio Code
- Git e GitHub
- Postman
- .NET CLI
- Docker (planeado)

## 3. Arquitectura do projecto

O backend segue uma arquitectura em camadas, separando as responsabilidades e facilitando a manutenção, os testes e a evolução do sistema.

```text
TaskFlow/
│
├── backend/
│   │
│   ├── TaskFlow.API/
│   │   ├── Controllers/
│   │   ├── Exceptions/
│   │   ├── Extensions/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── TaskFlow.Application/
│   │   ├── DTOs/
│   │   ├── Exceptions/
│   │   ├── Interfaces/
│   │   │   ├── Repository/
│   │   │   └── Services/
│   │   └── Services/
│   │
│   ├── TaskFlow.Domain/
│   │   ├── Entities/
│   │   └── Enums/
│   │
│   └── TaskFlow.Infrastructure/
│       ├── Data/
│       │   ├── Configurations/
│       │   └── Migrations/
│       ├── Repositories/
│       └── DependencyInjection.cs
│
├── frontend/
│
├── tests/
│
├── TaskFlow.slnx
└── README.md
```

### Responsabilidades das camadas

**TaskFlow.API**

Responsável pela comunicação HTTP, Controllers, autenticação, autorização, middleware e tratamento global de excepções.

**TaskFlow.Application**

Contém a lógica de aplicação, Services, DTOs, interfaces de repositórios e serviços, e regras de validação da aplicação.

**TaskFlow.Domain**

Representa o núcleo do negócio, contendo entidades, enums e regras fundamentais do domínio.

**TaskFlow.Infrastructure**

Responsável pelo acesso à base de dados, Entity Framework Core, configurações das entidades, migrations e implementação dos repositórios.

---

## 4. Funcionalidades do sistema

### 4.1. Autenticação e gestão de utilizadores

- Registo de utilizadores.
- Autenticação através de email e palavra-passe.
- Geração de JWT Access Tokens.
- Renovação de sessões através de Refresh Tokens.
- Logout e invalidação de Refresh Tokens.
- Armazenamento seguro de hashes dos Refresh Tokens.

### 4.2. Gestão de projectos

- Criação de projectos.
- Consulta dos projectos em que o utilizador participa.
- Consulta de detalhes de um projecto.
- Actualização de informações.
- Arquivamento de projectos.
- Controlo de acesso aos projectos.

### 4.3. Gestão de membros

- Adição de membros através do email.
- Gestão de permissões dentro de cada projecto.
- Remoção de membros.
- Associação de utilizadores aos respectivos projectos.

**Papéis dos membros:**

| Papel | Responsabilidade |
|---|---|
| Owner | Proprietário e responsável pelo projecto |
| Admin | Gere o projecto, membros e tarefas |
| Member | Participa no projecto e executa tarefas |

Cada projecto terá apenas um Owner.

### 4.4. Gestão de tarefas

- Criação de tarefas.
- Actualização e eliminação de tarefas.
- Atribuição de tarefas aos membros.
- Definição de prioridades.
- Definição de datas de início e conclusão.
- Acompanhamento do estado das tarefas.
- Organização da posição das tarefas no quadro Kanban.

**Estados das tarefas:**

- ToDo
- InProgress
- InReview
- Done

**Níveis de prioridade:**

- Low
- Medium
- High
- Urgent

### 4.5. Comentários

- Adicionar comentários às tarefas.
- Consultar comentários.
- Manter a identificação do autor e a data de criação.

### 4.6. Notificações

- Notificações relacionadas com tarefas.
- Notificações de atribuição de tarefas.
- Notificações relacionadas com alterações nos projectos.
- Notificações de actividades relevantes.

### 4.7. Outras funcionalidades planeadas

- Quadro Kanban com drag and drop.
- Calendário de tarefas.
- Histórico de actividades.
- Controlo de concorrência na actualização de tarefas.
- Dashboard com estatísticas dos projectos.
- Testes automatizados.
- Containerização com Docker.

---

## 5. Regras de negócio

O TaskFlow possui regras de negócio para garantir a integridade dos dados e o controlo de acesso.

1. Cada projecto possui um único proprietário (Owner).
2. Apenas membros de um projecto podem consultar os seus dados.
3. Owner e Admin podem gerir o projecto e todas as tarefas.
4. Members podem criar e editar as suas próprias tarefas ou tarefas que lhes foram atribuídas, respeitando as permissões definidas.
5. Um utilizador só pode ser atribuído a uma tarefa se pertencer ao respectivo projecto.
6. Os membros são adicionados através do email de utilizadores já registados.
7. Ao remover um membro, as tarefas atribuídas a esse membro ficam sem responsável.
8. Projectos arquivados não permitem a criação de novas tarefas nem a adição de novos membros.
9. A data de conclusão não pode ser anterior à data de início.
10. O acesso aos recursos protegidos exige autenticação.

---

## 6. Configuração do ambiente

### Pré-requisitos

Antes de executar o projecto, instala:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [MySQL 8](https://dev.mysql.com/downloads/mysql/)
- [Git](https://git-scm.com/)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Postman](https://www.postman.com/)

### 6.1. Clonar o repositório

```bash
git clone https://github.com/YasserBoaventura/Task_Folw.git
```

Entrar na pasta:

```bash
cd Task_Folw
```

### 6.2. Restaurar dependências

```bash
dotnet restore TaskFlow.slnx
```

### 6.3. Configurar a base de dados

Cria uma base de dados MySQL:

```sql
CREATE DATABASE taskflow_db;
```

A connection string será configurada através do User Secrets, evitando guardar credenciais no código ou no Git.

Na raiz do projecto, executa:

```powershell
dotnet user-secrets init --project backend/TaskFlow.API
```

Configura a ligação à base de dados:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=taskflow_db;User=TEU_UTILIZADOR;Password=TUA_PASSWORD;" --project backend/TaskFlow.API
```

Substitui `TEU_UTILIZADOR` e `TUA_PASSWORD` pelas credenciais locais do teu MySQL.

### 6.4. Configurar JWT

Gera uma chave segura:

```powershell
$jwtKey = [Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(48))
```

Guarda a chave:

```powershell
dotnet user-secrets set "Jwt:SecretKey" "$jwtKey" --project backend/TaskFlow.API
```

Configura o emissor:

```powershell
dotnet user-secrets set "Jwt:Issuer" "TaskFlow" --project backend/TaskFlow.API
```

Configura a audiência:

```powershell
dotnet user-secrets set "Jwt:Audience" "TaskFlow.Client" --project backend/TaskFlow.API
```

**Importante:** Não publiques chaves JWT, passwords ou connection strings com credenciais no GitHub.

---

## 7. Executar as migrations

O Entity Framework Core é utilizado para gerir a estrutura da base de dados.

Para criar uma nova migration:

```powershell
dotnet ef migrations add NomeDaMigration --project backend/TaskFlow.Infrastructure --startup-project backend/TaskFlow.API --output-dir Data/Migrations
```

Para aplicar as migrations:

```powershell
dotnet ef database update --project backend/TaskFlow.Infrastructure --startup-project backend/TaskFlow.API
```

Se ainda não tiveres o EF CLI instalado:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.14
```

Se já estiver instalado, podes actualizar com:

```powershell
