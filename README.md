# GovSpend — Federal Spending Dashboard

GovSpend is an ASP.NET Core 10 Razor Pages application that retrieves and visualizes publicly available federal spending data from the USAspending.gov API.

The project demonstrates full-stack .NET development, REST API integration, database persistence, dependency injection, automated testing, and Docker-based deployment.

## Features

- Live federal agency spending data
- Searchable federal agency dashboard
- Top Five Agencies spending visualization using Chart.js
- Individual agency details pages
- SQL Server CRUD operations using Entity Framework Core
- Read-only Production Demo Mode
- Automated application tests
- Docker containerization and cloud deployment

## Technology Stack

- C# / .NET 10
- ASP.NET Core Razor Pages
- Entity Framework Core
- SQL Server Express
- REST APIs / HttpClient
- Chart.js
- xUnit
- Docker
- GitHub
- Render

## Architecture

GovSpend uses a service-oriented architecture with dependency injection.

The application retrieves federal spending data through an API service, processes it in Razor Page models, and displays it through Razor views.

In local development, Entity Framework Core supports database operations. In Production Demo Mode, database modification features are disabled to provide a read-only public demonstration.

## Running Locally

Requirements:
- .NET 10 SDK
- SQL Server Express for database functionality

```powershell
dotnet restore
dotnet build
dotnet test
dotnet run --project GovSpend.Web
```

## Deployment

The application is containerized with Docker and deployed to Render through GitHub.

**Live Demo:** https://govspend-demo.onrender.com/

## Data Source

Federal spending data is provided by the public USAspending.gov API.

## Project Purpose

GovSpend was developed as a portfolio project to strengthen practical skills in ASP.NET Core, C#, Entity Framework Core, REST API integration, software testing, and application deployment.