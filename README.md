# PennyPlanner Backend

Backend application made for the project which is a part of my bachelor's thesis at FER. Written as an ASP.NET web API application.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Running the Application](#running-the-application)
- [Database Migrations](#database-migrations)
- [API Documentation](#api-documentation)

## Prerequisites

Before you begin, ensure you have met the following requirements:

- .NET 6 SDK installed on your machine
- A code editor (Visual Studio recommended)

## Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/fbuljan/PennyPlanner-backend.git
    cd PennyPlanner-backend
    ```

2. Restore the required packages:

    ```bash
    dotnet restore
    ```

## Running the Application

1. Build the application:

    ```bash
    dotnet build
    ```

2. Run the application:

    ```bash
    dotnet run
    ```

    The application should now be running at `https://localhost:7086`.

## Database Migrations

1. Add a new migration (if needed):

    ```bash
    dotnet ef migrations add MigrationName
    ```

2. Apply migrations:

    ```bash
    dotnet ef database update
    ```

## API Documentation

The API documentation can be accessed at `https://localhost:7086/swagger/index.html` when the application is running.
