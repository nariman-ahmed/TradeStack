# 📈 TradeStack

**TradeStack** is a clean and scalable backend API for managing stocks and market insights, built with ASP.NET Core Web API. It allows users to manage stocks and user comments, offering a clean RESTful structure, modular architecture, and modern development practices. Built using C#, Entity Framework Core, and SQL Server.

---

## ⚙️ Tech Stack

- **Framework:** ASP.NET Core Web API  
- **Language:** C#  
- **Architecture:** MVC  
- **Database:** SQL Server  
- **ORM:** Entity Framework Core  
- **Query Language:** LINQ  
- **Documentation:** Swagger (OpenAPI)  

---

## 🧩 Features

- ✅ CRUD operations for **Stocks**
- ✅ CRUD operations for **Comments** (each stock can have multiple comments)
- ✅ One-to-many relationship between Stocks and Comments
- ✅ Input validation using **Data Annotations**
- ✅ Use of **DTOs** to structure requests/responses
- ✅ **Filtering**, **sorting**, and **pagination** for stock listings
- ✅ Integrated **Swagger UI** for API testing and documentation

---

## 📁 Project Structure

- /Controllers --> API endpoints for Stocks and Comments
- /Models --> EF Core entity models
- /Data --> DbContext and configurations
- /DTOs --> Data Transfer Objects for cleaner API contracts
- /appsettings.json --> DB configuration and setting
- /Program.cs --> App entry point and service registration
