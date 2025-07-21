# Course Management System

## Live Demo

The application is deployed and available at: [http://coursedemo-alb-1005453574.us-east-1.elb.amazonaws.com/](http://coursedemo-alb-1005453574.us-east-1.elb.amazonaws.com/)

## Architecture

- **Backend**: .NET 9.0
- **Frontend**: React 19
- **Database**: PostgreSQL with EF Core
- **Containerization**: Docker & Docker Compose
- **Deployment**: AWS (ECS, RDS, ALB)

## Quick Start (Recommended - Docker)

### Prerequisites

- [Docker](https://www.docker.com/get-started) and Docker Compose
- Git

### Running the Application

1. **Clone the repository**

   ```bash
   git clone https://github.com/BogdanJas/demoCourse.git
   cd CourseDemo
   ```

2. **Start all services**

   ```bash
   docker-compose up --build
   ```

3. **Access the application**

   - **Frontend**: http://localhost:3000
   - **Backend API**: http://localhost:8080
   - **API Documentation**: http://localhost:8080/swagger (in development mode)
   - **Database**: localhost:5432 (PostgreSQL)

4. **Stop the application**
   ```bash
   docker-compose down
   ```

## Local Development Setup

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- Git

### Database Setup

1. **Install and start PostgreSQL**

   - Create a database named `CourseDemo`
   - Create a database user with appropriate credentials
   - Grant permissions to the user

   ```sql
   CREATE DATABASE "CourseDemo";
   CREATE USER your_username WITH PASSWORD 'your_password';
   GRANT ALL PRIVILEGES ON DATABASE "CourseDemo" TO your_username;
   ```

### Backend Setup

1. **Navigate to backend directory**

   ```bash
   cd Backend/CourseDemo
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Run database migrations**

   ```bash
   cd CourseDemo.WebAPI
   dotnet ef database update
   ```

4. **Start the backend**

   ```bash
   dotnet run --project CourseDemo.WebAPI
   ```

   The API will be available at: http://localhost:5000 or https://localhost:5001

### Frontend Setup

1. **Navigate to frontend directory**

   ```bash
   cd react-frontend
   ```

2. **Install dependencies**

   ```bash
   npm install
   ```

3. **Update API configuration for local development**

   Edit `src/utils/constants.js` and change the `baseUrl`:

   ```javascript
   export const API_CONFIG = {
     baseUrl: "http://localhost:5000/api", // Changed from AWS endpoint
     endpoints: {
       courses: "/courses",
       courseStatus: (id) => `/courses/${id}/status`,
       course: (id) => `/courses/${id}`,
     },
   };
   ```

4. **Start the frontend**

   ```bash
   npm start
   ```

   The frontend will be available at: http://localhost:3000

## Changes Required for Local Development

### Frontend Configuration

- **File**: `react-frontend/src/utils/constants.js`
- **Change**: Update `API_CONFIG.baseUrl` from AWS endpoint to local endpoint
- **From**: `"http://coursedemo-alb-1005453574.us-east-1.elb.amazonaws.com/api"`
- **To**: `"http://localhost:5000/api"` (or your backend port)

### Database Configuration

The application uses three different connection strings:

- **Production**: Points to AWS RDS (in `appsettings.json`)
- **Development**: Points to localhost (in `appsettings.Development.json`)
- **Docker**: Points to the database container (in `appsettings.Docker.json`)

## Running Tests

### Backend Tests

```bash
cd Backend/CourseDemo
dotnet test
```

## Database Schema

The application uses EF Core with the following entities:

- **Courses**: Title, Description, Duration, Status, PublishedAt, CreatedAt, UpdatedAt
- **Statuses**: Enum-like entity (Draft=1, Published=2, Archived=3)

Database migrations are applied automatically on startup:

- Development: Uses `EnsureCreatedAsync()`
- Production: Uses `MigrateAsync()`

## Project Structure

```
CourseDemo/
├── Backend/CourseDemo/
│   ├── CourseDemo/
│   │   ├── Entities/                  # Domain Entities
│   │   ├── Enums/                     # Domain Enums
│   │   └── Interfaces/                # Repository Interfaces
│   ├── CourseDemo.Application/
│   │   ├── DTOs/                      # DTOs
│   │   ├── Services/                  # Services
│   │   └── Validators/                # Input Validation
│   ├── CourseDemo.Infrastructure/
│   │   ├── Data/                      # DbContext & Configurations
│   │   ├── Migrations/                # EF Core Migrations
│   │   └── Repositories/              # Repository Implementations
│   ├── CourseDemo.WebAPI/
│   │   ├── Controllers/               # API Controllers
│   │   ├── Extensions/                # Service Registration
│   │   └── Middleware/                # Custom Middleware
│   └── CourseDemo.Tests/              # Unit Tests
├── react-frontend/                    # React Frontend
│   ├── src/
│   │   ├── components/                # React Components
│   │   ├── services/                  # API Services
│   │   └── utils/                     # Utilities & Constants
└── docker-compose.yml                # Docker Orchestration
```

## API Endpoints

- `GET /api/courses` - Get all courses
- `GET /api/courses/{id}` - Get course by ID
- `POST /api/courses` - Create new course
- `PUT /api/courses/{id}` - Update course
- `PATCH /api/courses/{id}/status` - Update course status

## Environment Variables

For production deployment, consider using environment variables:

```bash
# Database
DATABASE_HOST=your-db-host
DATABASE_NAME=CourseDemo
DATABASE_USER=your-username
DATABASE_PASSWORD=your-secure-password

# API
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
```
