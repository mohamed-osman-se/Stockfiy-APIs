# 📈 Stockify API

Welcome to the **Stockify API** — a robust RESTful API for managing stock data, user portfolios, authentication, and administrative operations. Designed for developers who want to build trading platforms, stock dashboards, or personal investment tools.

---

## 🔗 Live Demo

> 🧪 [Add your Swagger/Production URL here]

---

## 📃 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Getting Started](#-getting-started)
- [API Endpoints](#-api-endpoints)
- [Authentication](#-authentication)
- [Contributing](#-contributing)
- [License](#-license)
- [Contact](#-contact)

---

## ✅ Features

- 🔐 JWT-based Authentication & Authorization
- 👤 User Registration, Login, and Password Recovery
- 📊 Stock Management (CRUD)
- 💼 Portfolio Tracking
- 💬 Commenting System
- 🛠️ Admin-only operations (like user deletion)
- 📄 Fully documented via Swagger/OpenAPI 3.0

---

## 💻 Tech Stack

- **Backend:** ASP.NET Core Web API
- **Auth:** ASP.NET Core Identity + JWT
- **Docs:** Swagger (OpenAPI 3.0)
- **Database:** SQL Server / Any EF Core-compatible DB

---

## 🚀 Getting Started

### Prerequisites

- [.NET 7/8/9 SDK](https://dotnet.microsoft.com/)
- [SQLite] or any EF Core-supported DB

### Run the project locally

```bash
git clone https://github.com/mohamed-osman-se/Stockfiy-APIs
cd Stockfiy-APIs
dotnet restore
dotnet run
