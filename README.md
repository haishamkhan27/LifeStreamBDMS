# 🩸 LifeStream - Blood Donation Management System

LifeStream is a web-based blood donation management system built using **ASP.NET Core MVC**. It is designed to streamline the interaction between blood donors, hospitals, and administrators through an easy-to-use, secure platform.

## 🚀 Features

- 🩸 Donor registration and profile management
- 💉 Blood request submission and tracking
- 🏥 Admin and hospital panel for managing requests and donors
- 🔐 Role-based authentication and authorization (Admin, Hospital, Donor)
- 🖼️ Clean, responsive homepage with modern UI design
- 🧾 Form validation and access control for data accuracy and security

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core MVC (C#)
- **Frontend:** Razor Views, HTML, CSS, Bootstrap, JavaScript
- **Database:** SQL Server using Entity Framework Core (Code First)
- **Authentication:** ASP.NET Identity (Role-based login system)
- **Development Environment:** Visual Studio 2022

## 🧠 Project Architecture

The system is built using the **Model-View-Controller (MVC)** architecture to separate concerns and improve scalability:

- **Models** – Represent data structures like Donor, Request, and User
- **Views** – Razor-based pages for displaying forms, tables, and UI
- **Controllers** – Handle user requests and interact with models

## 🔐 Authentication & Authorization

The application uses **ASP.NET Core Identity** for secure login and user management.

- **Role-Based Access:**  
  - Admin: Full control over system
  - Hospital: Manage requests and donor records
  - Donor: Register, login, and request/offer blood

- **Password Hashing, Sessions, and Access Control** are handled securely

## 📷 UI Overview

- Homepage with custom background image and modern card-based design
- Dynamic tables, form validations, and Bootstrap-based layout
- Responsive design for both desktop and mobile screens

## 🏁 How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/LifeStream-BloodDonationSystem.git
