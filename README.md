# Sales Invoice Management System

A premium, enterprise-grade Sales Invoice Management System built with **Clean Architecture**, **ASP.NET Core Web API**, and **Angular 19**.

## 🚀 Features
- **Dashboard Overview**: Paginated list of all invoices with advanced sorting.
- **Dynamic Invoicing**: Create and edit invoices with a dynamic item entry system.
- **Auto-Calculations**: Real-time server-side and client-side calculations for line items and grand totals.
- **Smart Numbering**: Auto-generated invoice numbers following the `YYINV000X` format.
- **Professional Reporting**: Filterable reports by date and invoice number with print-ready views.
- **Modern UI**: Clean, professional "Enterprise Pro" design using Tailwind CSS.

## 🛠️ Tech Stack
- **Backend**: .NET Core 10.0 (Web API)
- **Database**: SQL Server (EF Core)
- **Frontend**: Angular 19, Tailwind CSS
- **Patterns**: Clean Architecture, Repository Pattern, Dependency Injection.

---

## ⚙️ Getting Started

### 1. Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js (LTS)](https://nodejs.org/)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (Included with Visual Studio)

### 2. Database Setup
Open your terminal in the `./backend` directory and run the following command to create the database and tables:
```bash
dotnet ef database update
```
*Note: This command uses the connection string in `appsettings.json`. If you use a different SQL instance, please update the string accordingly.*

### 3. Backend Setup
Navigate to the `./backend/backend` directory and start the API:
```bash
dotnet run
```
The API will be available at: `https://localhost:7271`

### 4. Frontend Setup
Navigate to the `./frontend` directory, install dependencies, and start the development server:
```bash
npm install
npm start
```
Open your browser to: `http://localhost:4200`

---

## 🏗️ Architecture Highlights
- **Application Layer**: Encapsulates business logic, ensuring the core remains decoupled from the infrastructure.
- **Domain Entities**: Clearly defined entities for `Invoice` and `InvoiceItem` with proper EF Core relationship mapping.
- **Repository Pattern**: Abstracted data access to allow for easier unit testing and future-proofing.
- **Reactive Forms**: Sophisticated form handling in Angular with `FormArray` for dynamic line-item management.

---

## 👨‍💻 Developed by
**Athif Saleem**  
*Sales Invoice Management Machine Test Submission*
