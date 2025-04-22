# RahmanMemberVault API

## 🎯 Overview
RahmanMemberVault is a clean‑architecture ASP.NET Core 9.0 service that provides robust member management for your applications. It offers full Create, Read, Update, and Delete (CRUD) operations with built‑in validation, global error handling, and comprehensive documentation, so you can onboard quickly, reduce surprises, and focus on delivering value to your users.

## 📐 Architecture & Technology Stack
- **Clean Architecture** layered design (Core, Application, Infrastructure, API) for maintainability and testability.  
- **.NET 9.0** and **C#** for modern, high‑performance APIs.  
- **Entity Framework Core** with **SQLite** data store (in `App_Data/members.db`) for lightweight, file‑based persistence.  
- **FluentValidation** for expressive, reusable DTO validators.  
- **Serilog** for structured logging to console and daily rolling files (`ExceptionLogs/logs.txt`).  
- **Swagger / OpenAPI** for interactive API exploration (enabled in all environments).  

## 🚀 Key Benefits for Your Team
- **Reliability**: Unit and integration tests guarantee correct behavior as you evolve the system.  
- **Clarity**: Swagger UI makes it trivial for front‑end developers or third‑party integrators to explore and test endpoints.  
- **Supportability**: Global exception handling emits user‑friendly errors and tracking IDs, so issues can be diagnosed quickly.  
- **Ease of Deployment**: GitHub Actions CI/CD pipeline automates building, testing, and deploying to Azure Web Apps.  

## 🛠️ Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download) installed.
- (Optional) SQLite client for inspecting the `members.db` file.

### Clone & Run Locally
```bash
git clone https://github.com/GreenStriker/RahmanMemberVault.git
cd RahmanMemberVault/RahmanMemberVault.Api
dotnet restore
dotnet build
dotnet run
```

The API will be available at `https://localhost:7118/` or `http://localhost:5184/`.

### Configuration
- **Connection String**: In `appsettings.json`, adjust:
  ```json
  "ConnectionStrings": {
    "MemberVaultDb": "Data Source=App_Data/members.db"
  }
  ```
- **Logging Levels** and **AllowedHosts** can also be tuned in `appsettings.json`.

## API Endpoints (v1)
| Method | Endpoint                  | Description                   |
| ------ | ------------------------- | ----------------------------- |
| GET    | `/api/v1/member`          | Retrieve all members          |
| GET    | `/api/v1/member/{id}`     | Retrieve member by ID         |
| POST   | `/api/v1/member`          | Create a new member           |
| PUT    | `/api/v1/member/{id}`     | Update an existing member     |
| DELETE | `/api/v1/member/{id}`     | Delete a member               |

**Example**: Create a member  
```bash
curl -X POST https://localhost:7118/api/v1/member   -H "Content-Type: application/json"   -d '{ "name":"John Doe", "email":"john@example.com", "phoneNumber":"1234567890" }'
```

## Testing
- **Unit tests** in `RahmanMemberVault.Tests.Unit` cover DTO validators and services.  
- **Integration tests** in `RahmanMemberVault.Tests.Integration` exercise end-to-end API behavior.  


## Deployment
CI/CD with GitHub Actions automates build, test, and deployment on pushes to `master`. Artifacts are deployed to your Azure Web App named `rahmanmembervault-api` with zero‑touch rollback capabilities.

## Support & Troubleshooting
- Check structured logs at `ExceptionLogs/logs.txt` (daily files) for raw errors and tracking IDs.    
- For unexpected issues, provide the tracking ID to our support team for swift resolution.

---
*RahmanMemberVault API is designed to be a reliable, transparent, and customer‑focused member management solution helping your team ship features faster with confidence.*