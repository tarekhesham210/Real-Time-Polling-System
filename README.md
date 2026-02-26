
#  Real-Time Polling System API  
### ASP.NET Core (.NET 8)

A scalable and secure **Real-Time Polling System Web API** built using Clean Architecture principles, supporting live vote updates with SignalR and secure authentication using JWT.

---

##  Features

-  **Real-Time Updates**  
  Live poll vote updates using SignalR Groups.

-  **Secure Voting**  
  JWT Authentication (Access & Refresh Tokens).

-  **Clean Architecture**  
  Scalable, maintainable, and separation of concerns.

-  **Validation & Mapping**
  - FluentValidation  
  - AutoMapper  

-  **API Documentation**
  - Swagger UI  
  - Visual Studio HTTP File  

---

## 🛠 Tech Stack

- **Backend:** ASP.NET Core Web API (.NET 8)  
- **Real-Time:** Microsoft SignalR  
- **Database:** SQL Server with Entity Framework Core  
- **Security:** JSON Web Tokens (JWT)  
- **Testing:** Swagger & Visual Studio HTTP Client  

---

## ⚙ Getting Started

### 1️⃣ Update Connection String

Edit `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "YOUR_CONNECTION_STRING"
}
```

---

### 2️⃣ Apply Database Migrations

Open **Package Manager Console** and run:

```
update-database
```

---

### 3️⃣ Run the Project

- Set the API project as **Startup Project**
- Run the application

---

## 🧪 Testing the API

### 🔹 Swagger UI

Available at:

```
https://localhost:{port}/swagger
```

---

###  API.http File

You can test endpoints using the built-in **Visual Studio HTTP Client**.

---

##  Authentication Flow

### 1️⃣ Register or Login

You will receive:

- Access Token (Valid for 15 minutes)
- Refresh Token

---

### 2️⃣ Authorize Requests

Add the Access Token in the request header:

```
Authorization: Bearer <your_access_token>
```

---

### 3️⃣ Refresh Token

Use the Refresh Token endpoint to generate a new Access Token when expired.

---

##  SignalR Test

1. Run the **TestSignalR Console Project**
2. Use the `Vote` endpoint (Make sure to use the same Poll ID in both console and API)
3. View real-time vote results in the console window

---

##  Architecture Overview

This project follows **Clean Architecture** principles:

- Domain Layer  
- Application Layer  
- Infrastructure Layer  
- Presentation Layer (API)  

### Benefits:

- ✔ Separation of Concerns  
- ✔ Testability  
- ✔ Scalability  
- ✔ Maintainability  

---

## Author

**Tarek Hesham**

🔗 LinkedIn:  
https://www.linkedin.com/in/tarek-hesham-706363308  

💻 GitHub:  
https://github.com/tarekhesham210  

---

