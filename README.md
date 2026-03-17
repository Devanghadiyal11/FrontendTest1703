# Clinic Queue Management System (CMS) - Frontend

This project is the frontend for a full-stack, multi-tenant Clinic Management System, built with ASP.NET Core MVC. It provides a user interface for different roles within a clinic, including Admins, Doctors, Receptionists, and Patients, to manage clinic operations efficiently.

## Features

- **Role-Based Access Control:** Custom dashboards and functionalities for four distinct user roles:
    - **Admin:** Manages users (doctors, receptionists, patients) and views clinic information.
    - **Doctor:** Views the daily patient queue and adds prescriptions or medical reports for appointments.
    - **Receptionist:** Manages the daily patient queue, updating patient status as they move through the clinic.
    - **Patient:** Books new appointments, views their appointment history, and accesses their prescriptions and medical reports.
- **Authentication:** Secure login system using cookies and JWTs for session management.
- **Dynamic Dashboards:** Each role is presented with a dashboard that summarizes relevant information and provides quick access to key actions.
- **Appointment Management:** Patients can book, view, and manage their appointments.
- **Queue Management:** Receptionists and Doctors can manage the flow of patients through the clinic with a real-time queue system.

## Technologies Used

- **Framework:** ASP.NET Core MVC (.NET 8)
- **Language:** C#
- **Frontend:** HTML, CSS, Bootstrap 5
- **JavaScript:** jQuery, jQuery Validation
- **API Communication:** `HttpClient` for making requests to the backend REST API.
- **Authentication:** Cookie-based authentication with session management.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Backend API

This frontend application connects to a live backend API. No local backend setup is required. The API endpoint is:
```
https://cmsback.sampaarsh.cloud/
```

### Running the Application

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/Devanghadiyal11/FrontendTest1703.git
    cd FrontendTest1703
    ```

2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Run the project:**
    ```bash
    dotnet run
    ```

4.  **Open in your browser:**
    Navigate to `http://localhost:5043`.

## Project Structure

- `/Controllers`: Contains the MVC controllers that handle user requests and business logic.
- `/Views`: Contains the `.cshtml` Razor views for the application's UI.
    - `/Views/Shared`: Shared layout files and partial views.
    - `/Views/Home`: Dashboard views for different roles.
- `/Models`: Contains the C# classes that represent the data structures of the application.
- `/Services`: Contains services like `AuthService` for handling authentication logic.
- `/wwwroot`: Contains static assets like CSS, JavaScript, and third-party libraries.
- `Program.cs`: The main entry point of the application, where services and middleware are configured.
