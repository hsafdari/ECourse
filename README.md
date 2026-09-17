# E-Course Platform

E-Course is a full-stack online learning platform built as a practical demonstration of modern .NET backend development, microservice architecture, database design, testing, observability, and frontend development.

The project is designed not only as an online course management system, but also as a production-oriented environment for applying software engineering practices used in real-world distributed systems.

## 🎯 Project Goals

The main goals of this project are to:

- Design and implement independently deployable services using Microservice Architecture.
- Apply Domain-Driven Design (DDD) principles where they provide practical value.
- Separate read and write responsibilities using CQRS.
- Build maintainable application boundaries using Clean Architecture principles.
- Design RESTful APIs with consistent validation, error handling, pagination, filtering, sorting, and API documentation.
- Apply SOLID principles and Dependency Injection throughout the application.
- Design efficient PostgreSQL data access and queries with performance and scalability in mind.
- Implement Unit and Integration Testing for critical application behavior.
- Containerize services and infrastructure using Docker.
- Introduce observability using OpenTelemetry, including distributed tracing, metrics, and structured logging.
- Build automated CI/CD workflows using GitHub Actions.
- Develop the public frontend with React, TypeScript, and Redux.
- Provide a separate administration interface for managing platform content.

## 🏗️ Architecture

The platform follows a service-oriented architecture in which business capabilities are separated into independently maintainable services.

Each service follows clear application boundaries:

Domain
→ Application
→ Infrastructure
→ API

The architecture applies patterns selectively rather than treating them as mandatory abstractions. CQRS, repositories, domain models, and infrastructure abstractions are introduced where they improve maintainability, testability, or separation of concerns.

## 🛠️ Technology Stack

### Backend

- C# / .NET
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- CQRS
- MediatR
- Dependency Injection
- REST APIs
- OpenAPI

### Architecture & Engineering

- Microservice Architecture
- Domain-Driven Design (DDD)
- Clean Architecture
- SOLID principles
- Repository and Unit of Work patterns where appropriate
- Centralized error handling
- RFC 7807 Problem Details
- API versioning
- Pagination, filtering, and sorting
- Idempotency and resilient API design

### Testing

- xUnit
- Unit Testing
- Integration Testing
- Test Doubles / Mocking
- Testcontainers for infrastructure-dependent tests

### Observability

- OpenTelemetry
- Distributed Tracing
- Metrics
- Structured Logging

### DevOps

- Docker
- Docker Compose
- GitHub Actions
- CI/CD

### Frontend

- React
- TypeScript
- Redux

### Administration

- Blazor

## 🚧 Project Status

E-Course is under active development.

The architecture and implementation are being expanded incrementally, with each stage focusing on applying production-oriented .NET and distributed-system practices.

The README and architecture documentation will evolve alongside the implementation.

## 💡 What This Project Demonstrates

This project demonstrates practical experience with:

- Designing ASP.NET Core APIs and service boundaries
- Microservice and Clean Architecture principles
- Domain-Driven Design and CQRS
- PostgreSQL database design and query optimization
- REST API standards and consistent error contracts
- Unit and integration testing
- Docker-based development environments
- Distributed tracing, metrics, and logging with OpenTelemetry
- CI/CD and automated development workflows
- React/TypeScript frontend development


## 📦 Modules

### Current Modules

- **Admin Panel** — Blazor-based administration interface
- **Course Management** — Course groups, courses, content, and related operations
- **User Management** — User and account management
- **Payment Management** — Payment-related workflows
- **Content Management** — Management of learning content and resources

### 🗺️ Roadmap

The platform is being developed incrementally. Planned modules include:

- Quiz Management
- Certificate Management
- Reporting & Analytics
- Notification Management
- Real-time Chat
- Discussion Forums
- Blog Management
- SEO Management

Additional services and infrastructure capabilities will be introduced as the architecture evolves.


## 🚧 Project Status

E-Course is under active development.

The project is being developed incrementally, with a focus on applying production-oriented software engineering practices rather than simply implementing features.

Current development includes API architecture, service boundaries, CQRS, PostgreSQL data access, automated testing, containerization, and observability. New capabilities and documentation are added as the platform evolves.

Contributions, technical feedback, and architecture discussions are welcome.

## 🤝 Feedback & Contact

Technical feedback, suggestions, and discussions about the architecture or implementation are welcome.

You can reach me through:

- [Personal Website](https://huseyin.es)
- [LinkedIn](https://www.linkedin.com/in/hsafdari/)