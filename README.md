# 📱 Social Network API & MVC Application

A layered ASP.NET Core application that simulates a basic social network, featuring user management, profiles, and posts. The project is structured following separation of concerns principles, combining an API backend with an MVC frontend.

---

## 🚀 Technologies

* ASP.NET Core
* Entity Framework Core
* SQL Server (or compatible database)
* JWT Authentication
* MVC Pattern
* C#

---

## 🏗️ Architecture

This solution is organized using a multi-layered architecture:

* **RedeSocial.API** → REST API layer (handles HTTP requests)
* **RedeSocial.MVC** → Frontend using MVC pattern
* **RedeSocial.BLL** → Business Logic Layer (intended for rules and services)
* **RedeSocial.Domain** → Entities, DbContext, and data access

This structure improves maintainability, scalability, and separation of responsibilities.

---

## ✨ Features

* 👤 User management (create, update, delete)
* 📝 Posts creation and interaction
* 🧾 User profiles
* 🔐 JWT-based authentication
* 🔄 Entity Framework migrations

---

## ⚙️ Getting Started

### Prerequisites

* .NET SDK installed
* SQL Server (or another supported database)
* Git

---

### Installation

1. Clone the repository:

```bash
git clone https://github.com/your-username/your-repository.git
```

2. Navigate to the project folder:

```bash
cd your-repository
```

3. Configure your database connection in:

```bash
appsettings.json
```

4. Run database migrations:

```bash
dotnet ef database update
```

5. Run the application:

```bash
dotnet run
```

---

## 🔐 Authentication

The project uses JWT (JSON Web Token) for authentication.

Make sure to configure the token settings in:

```bash
JwtBearerTokenSettings
```

---

## 📌 Project Status

This project was developed for learning purposes and is continuously being improved, including:

* Better separation of business logic
* Implementation of DTOs
* Improved validation and security practices

---

## 📚 Future Improvements

* Add Service Layer with business rules
* Implement Repository Pattern
* Improve authentication and authorization
* Enhance frontend UI/UX
* Add unit and integration tests

---

## 👩‍💻 Author

**Jéssica Galhardi**
Software Developer | QA | .NET Enthusiast

---

## 📄 License

This project is open-source and available for learning purposes.
