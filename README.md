# 📈 Stockify API

Welcome to **Stockify** — a fully functional, production-ready RESTful API for managing stocks, portfolios, user accounts, and admin operations.

I built this project to demonstrate my backend development skills in real-world scenarios including **authentication**, **data modeling**, **role-based access control**, and **API documentation** — with clean, maintainable code and modern architecture.

🔗 **Live Demo (Swagger UI)**:  
[https://stockfiy.runasp.net/swagger/index.html](https://stockfiy.runasp.net/swagger/index.html)

🧪 **Admin Test Credentials**:  
If you'd like to test the admin-only endpoints, you can log in with:  
**Email**: `mohamedosman.se@gmail.com`  
**Password**: `Admin_123`

---

## 🧠 Why This Project?

This project reflects how I would design and implement a **secure, scalable, and maintainable Web API** in a real job environment.  
It covers everything a recruiter or hiring manager looks for:

- ✅ Clean architecture using ASP.NET Core
- ✅ JWT-based authentication & authorization
- ✅ Secure user flows: register, login, reset password
- ✅ Realistic domain: stocks, portfolios, and comments
- ✅ Admin-only endpoints with role restrictions
- ✅ EF Core with migrations & SQLite integration
- ✅ Full API documentation using Swagger

---

## 🛠️ Technologies Used

| Area               | Technology                          |
|--------------------|--------------------------------------|
| Language           | C# (.NET 9 SDK)                     |
| Framework          | ASP.NET Core Web API                |
| Authentication     | ASP.NET Core Identity + JWT         |
| Database           | SQLite (EF Core ORM)                |
| Documentation      | Swagger / OpenAPI 3.0               |
| Hosting (Live Demo) | MonsterASP.NET                     |

---

## 🚀 How to Run Locally

### Prerequisites

- [.NET SDK 9 or later](https://dotnet.microsoft.com/)
- [SQLite](https://www.sqlite.org/index.html)

### Steps

```bash
git clone https://github.com/mohamed-osman-se/Stockfiy-APIs
cd Stockfiy-APIs
dotnet restore
dotnet run
