# 🚀 Enterprise Product Management System l

> **A modern enterprise-grade Product Management System built with .NET 8 Web API, Angular, Azure Cosmos DB, and Azure File Storage.**

A full-stack cloud-integrated application designed to manage product lifecycles through a clean and scalable architecture. The system provides complete **CRUD operations**, stores structured product data in **Azure Cosmos DB**, and maintains JSON document backups in **Azure File Storage**.

---

## ✨ Features

* 📦 Complete Product CRUD operations
* ☁️ Azure Cosmos DB integration
* 📁 Azure File Storage JSON backups
* 🔄 Automatic synchronization between database and file storage
* 📄 Download product JSON documents
* 🛡️ Global exception handling middleware
* 🔐 CORS configuration
* 📚 Swagger / OpenAPI documentation
* 🧩 Clean Architecture principles
* 💉 Dependency Injection
* 🏗️ Repository / Service pattern
* 🎨 Responsive Angular frontend
* 🌐 RESTful API communication

---

## 🏗️ Architecture

```text
┌───────────────────────────────┐
│        Angular Frontend       │
│          Port: 4200           │
└───────────────┬───────────────┘
                │
                │ HTTP / REST API
                ▼
┌───────────────────────────────┐
│       .NET 8 Web API          │
│                               │
│  Controllers                  │
│  Services                     │
│  DTOs                         │
│  Middleware                   │
│  Dependency Injection         │
└───────────────┬───────────────┘
                │
        ┌───────┴────────┐
        │                │
        ▼                ▼
┌───────────────┐  ┌──────────────────┐
│ Azure Cosmos  │  │ Azure File       │
│ DB            │  │ Storage          │
│               │  │                  │
│ Product Data  │  │ Product JSON     │
└───────────────┘  └──────────────────┘
```

---

## 🛠️ Technology Stack

### Backend

| Technology               | Purpose                      |
| ------------------------ | ---------------------------- |
| **ASP.NET Core 8**       | RESTful Web API              |
| **C#**                   | Backend programming language |
| **Azure Cosmos DB**      | NoSQL cloud database         |
| **Azure File Storage**   | JSON document storage        |
| **Swagger / OpenAPI**    | API documentation            |
| **Dependency Injection** | Service management           |
| **Clean Architecture**   | Application architecture     |
| **Custom Middleware**    | Global exception handling    |

### Frontend

| Technology     | Purpose                       |
| -------------- | ----------------------------- |
| **Angular**    | Frontend framework            |
| **TypeScript** | Frontend programming language |
| **HTML5**      | UI structure                  |
| **CSS3**       | Styling                       |
| **REST API**   | Backend communication         |

---

## 📂 Project Structure

```text
ProductManagementSystem/
│
├── Backend/
│   └── ProductManagementAPI/
│       │
│       ├── Controllers/
│       │   └── ProductsController.cs
│       │
│       ├── DTOs/
│       │   ├── CreateProductDto.cs
│       │   └── UpdateProductDto.cs
│       │
│       ├── Interfaces/
│       │   └── IProductService.cs
│       │
│       ├── Middlewares/
│       │   └── ExceptionHandlingMiddleware.cs
│       │
│       ├── Models/
│       │   └── Product.cs
│       │
│       ├── Services/
│       │   └── ProductService.cs
│       │
│       ├── Program.cs
│       └── appsettings.json
│
└── Frontend/
    └── product-management-ui/
        │
        ├── src/
        │   ├── app/
        │   ├── assets/
        │   └── environments/
        │
        ├── angular.json
        ├── package.json
        └── tsconfig.json
```

---

# ⚡ API Endpoints

The backend exposes RESTful endpoints for managing products.

|  Method  | Endpoint                  | Description                                   |
| :------: | ------------------------- | --------------------------------------------- |
|  `POST`  | `/api/Products`           | Create a new product                          |
|   `GET`  | `/api/Products`           | Retrieve all products                         |
|   `GET`  | `/api/Products/{id}`      | Retrieve a product by ID                      |
|   `PUT`  | `/api/Products/{id}`      | Update an existing product                    |
| `DELETE` | `/api/Products/{id}`      | Delete a product                              |
|   `GET`  | `/api/Products/{id}/file` | Download product JSON from Azure File Storage |

---

## 🔄 Product Data Flow

### Create Product

```text
Angular
   │
   ▼
POST /api/Products
   │
   ▼
.NET Web API
   │
   ├──────────────► Azure Cosmos DB
   │                     │
   │                     ▼
   │              Product stored
   │
   └──────────────► Azure File Storage
                         │
                         ▼
                  Product JSON stored
```

### Update Product

```text
Angular
   │
   ▼
PUT /api/Products/{id}
   │
   ▼
.NET Web API
   │
   ├──────────────► Update Cosmos DB
   │
   └──────────────► Update JSON
                         │
                         ▼
                  Azure File Storage
```

### Delete Product

```text
Angular
   │
   ▼
DELETE /api/Products/{id}
   │
   ▼
.NET Web API
   │
   ├──────────────► Delete from Cosmos DB
   │
   └──────────────► Delete JSON file
                         │
                         ▼
                  Azure File Storage
```

---

# ⚙️ Getting Started

## 📋 Prerequisites

Before running the project, make sure the following are installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Node.js](https://nodejs.org/)
* Angular CLI
* Visual Studio 2022 or VS Code
* An active Azure account
* Azure Cosmos DB account
* Azure Storage Account with File Storage enabled

---

## 1️⃣ Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/ProductManagementSystem.git

cd ProductManagementSystem
```

---

## 2️⃣ Configure Azure Services

Open:

```text
Backend/ProductManagementAPI/appsettings.json
```

Configure your Azure Cosmos DB and Azure File Storage credentials.

```json
{
  "CosmosDb": {
    "EndpointUri": "YOUR_COSMOS_ENDPOINT",
    "PrimaryKey": "YOUR_COSMOS_KEY",
    "DatabaseName": "ProductDB",
    "ContainerName": "Products"
  },

  "AzureFileStorage": {
    "ConnectionString": "YOUR_AZURE_STORAGE_CONNECTION_STRING",
    "ShareName": "product-jsons"
  }
}
```

> ⚠️ **Security:** Never commit real Azure connection strings, account keys, or other secrets to GitHub.

For production applications, use environment variables, Azure Key Vault, or another secure secret-management solution.

---

# ▶️ Running the Backend

Navigate to the backend project:

```bash
cd Backend/ProductManagementAPI
```

Restore dependencies:

```bash
dotnet restore
```

Run the API:

```bash
dotnet run
```

The API will start on the configured local HTTPS/HTTP port.

### Swagger

Once the API is running, open the Swagger UI:

```text
https://localhost:7127/swagger
```

> The port may vary depending on your `launchSettings.json` configuration.

---

# ▶️ Running the Angular Frontend

Open a new terminal and navigate to:

```bash
cd Frontend/product-management-ui
```

Install dependencies:

```bash
npm install
```

Start the Angular development server:

```bash
ng serve
```

Open your browser:

```text
http://localhost:4200
```

---

# ☁️ Azure Integration

The application uses two Azure services for product persistence.

### Azure Cosmos DB

Cosmos DB stores the primary product records.

```text
Product
 ├── id
 ├── name
 ├── description
 ├── price
 ├── category
 └── other product properties
```

### Azure File Storage

A JSON representation of each product is maintained in Azure File Storage.

```text
product-jsons/
│
├── product-001.json
├── product-002.json
├── product-003.json
└── ...
```

This provides a secondary document-based representation of the product data.

---

# 🛡️ Exception Handling

The application includes centralized exception handling through custom middleware.

```text
Request
   │
   ▼
Controller
   │
   ▼
Service
   │
   ├── Success ───────► Response
   │
   └── Exception
          │
          ▼
 Global Exception Middleware
          │
          ▼
 Standardized JSON Error Response
```

This prevents duplicate error-handling logic throughout individual controllers.

---

# 🔐 Security

The application follows basic security practices including:

* CORS configuration
* Secure configuration of cloud credentials
* Centralized exception handling
* DTO-based API communication
* Separation of business logic from controllers
* No hard-coded production secrets

For production deployment, additional security measures should be implemented, including:

* Authentication & Authorization
* Azure Key Vault
* HTTPS enforcement
* Rate limiting
* Input validation
* Secure logging
* Managed Identity where applicable

---

# 🧩 Design Patterns & Principles

The project follows several software engineering principles:

### Clean Architecture

Separates responsibilities between different application layers.

### Dependency Injection

Services are registered through the .NET dependency injection container.

### Repository / Service Pattern

Business logic is separated from API controllers.

### DTO Pattern

Data Transfer Objects are used to control API request and response models.

### Middleware

Cross-cutting concerns such as exception handling are implemented through middleware.

---

# 📊 Application Workflow

```text
                  ┌──────────────────┐
                  │  Angular Client  │
                  └────────┬─────────┘
                           │
                           ▼
                  ┌──────────────────┐
                  │   REST API       │
                  │    .NET 8        │
                  └────────┬─────────┘
                           │
                  ┌────────┴────────┐
                  │                 │
                  ▼                 ▼
          ┌──────────────┐   ┌───────────────┐
          │ Cosmos DB    │   │ Azure File    │
          │              │   │ Storage       │
          │ Product Data │   │ JSON Backup   │
          └──────────────┘   └───────────────┘
```

---

# 🧪 Testing the API

You can test the API using:

* Swagger UI
* Postman
* Angular frontend
* REST Client extensions

Example request:

```http
POST /api/Products
Content-Type: application/json
```

Example payload:

```json
{
  "name": "Laptop",
  "description": "Business laptop",
  "price": 150000,
  "category": "Electronics"
}
```

---

# 🚀 Future Enhancements

Potential improvements include:

* 🔐 JWT Authentication
* 👥 Role-based authorization
* 📊 Advanced dashboard analytics
* 🔎 Product search and filtering
* 📄 Pagination
* 📈 Reporting
* ☁️ Azure App Service deployment
* 🔑 Azure Key Vault integration
* 🐳 Docker containerization
* 🔄 CI/CD with GitHub Actions
* 🧪 Automated unit and integration testing
* 📱 Improved responsive UI
* 📝 Audit logging

---

# 👨‍💻 Author

**Muhammad Talha Hakeem**

🎓 BS Computer Science Student
💻 Full-Stack Developer
🚀 Interested in .NET, Angular, Cloud Computing & Enterprise Application Development

---

## ⭐ Support

If you find this project useful, consider giving the repository a ⭐ on GitHub.

---

## 📄 License

This project is intended for educational and development purposes.
