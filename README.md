# Project Setup and Deployment Instructions

## Prerequisites

1. **.NET 8 SDK**: Install the latest .NET 8 SDK from [Microsoft](https://dotnet.microsoft.com/download).
2. **Visual Studio 2022**: Install Visual Studio 2022 from [Microsoft](https://visualstudio.microsoft.com/). Make sure to include the .NET web development workloads.
2. **Docker**: Install Docker from [Docker](https://www.docker.com/get-started).
3. **Kubernetes**: Install a local Kubernetes cluster (e.g., Kind) from [Kubernetes](https://kubernetes.io/docs/tasks/tools/).
4. **kubectl**: Install kubectl from [Kubernetes](https://kubernetes.io/docs/tasks/tools/install-kubectl/).
5. **Kompose**: Install Kompose from [Kompose](https://kompose.io/).

## Setup and Build

### Clone the Repository
    git clone https://github.com/iman5/app.git
    cd app

### Exploring the solution
1. Launch Visual Studio 2022.
1. Click on Open a project or solution.
1. Navigate to the cloned repository and select the solution file (.sln).
1. Right click on the solution and select "Configure Start up Projects" and select Docker compose as the single selected project if not already selected.

### Running the solution
#### Visual Studio 
Run the application to bring up API and UI projects side by side in docker. Ports will be selected automatically so no need to clear any instances.
#### Docker Compose CLI
    docker-compose build
    docker-compose up



# Documentation
# 1. Building the API

## Programming Languages and Frameworks

- **.NET 8**: The main framework used for building the Web API and other projects.
- **C#**: The primary programming language used for writing the application logic.

## Database and ORM

- **Entity Framework Core**: Object-Relational Mapper (ORM) for .NET, used for data access.
- **SQLite**: A lightweight, file-based relational database used for development and testing.
- **In-Memory Database**: Used as the main db and unit testing.
- **Repository Pattern**: Used for separation of concern and testability.

## Web and API

- **ASP.NET Core**: For creating cross-platform, high-performance modern applications.
- **ASP.NET Core Controllers**: Used to define endpoints and handle HTTP requests and responses.
- **API Versioning**: Implemented to manage different versions of the API.

## Dependency Injection

- **Scoped Services**: Used for managing service lifetimes, ensuring a new instance is created per request.

## Object Mapping

- **AutoMapper**: Mapping library used to map objects between domain models and DTOs.

## Logging and Monitoring

- **Serilog**: A logging library used to log application events and errors.
- **OpenTelemetry**: Used for observability, providing metrics and tracing.

## API Documentation

- **Swagger (Swashbuckle)**: Used for generating interactive API documentation.

## Development Tools

- **Visual Studio 2022**: The IDE used for writing and debugging code.
- **.NET CLI**: A command-line interface for .NET, used for project management and build tasks.

## Testing Frameworks

- **xUnit**: A testing framework used for writing and running unit tests.

## Configuration and Environment Management

- **appsettings.json**: Used for configuring application settings, including database connection strings.
- **ASP.NET Core Configuration**: Used for managing configuration in the application.

## Middleware

- **Middleware Configuration**: Used to manage HTTP request pipeline components, such as exception handling, request response and CORS.

## Source Control

- **Git**: Used for version control.

## Containerization and Orchestration

- **Docker**: Used for containerizing the applications.
- **Kompose**: Used for converting Docker Compose files to Kubernetes configuration.
- **Kubernetes**: Used for container orchestration and deployment.


# 2. Testing
The pattern for test of integration is top-down integration testing. The CustomersController (the top module) gets tested and progresses to lower-level services and repositories.
The pattern for test of acceptance is automated acceptance testing using xUnit to mimic the behavior of an end to end consumer.
Naming convention used for tests is MethodUnderTest_ExpectedBehavior.

## Unit Tests

1. In the Solution Explorer, right-click on the `App.Test.Unit` project.
2. Select `Run Tests`.

## Integration Tests

1. In the Solution Explorer, right-click on the `App.Test.Integration` project.
2. Select `Run Tests`.

## System End-to-End (E2E) Tests

1. In the Solution Explorer, right-click on the `App.Test.SystemE2E` project.
2. Select `Run Tests`.

## Using Test Explorer

1. Open Test Explorer:
   - Go to `Test` > `Windows` > `Test Explorer` in the menu bar.

2. Run All Tests:
   - In Test Explorer, click on `Run All` to execute all tests in the solution.

3. Run Specific Tests:
   - To run specific tests, select the desired tests in Test Explorer and click `Run Selected`.

4. View Test Results:
   - Test results will be displayed in Test Explorer. You can view detailed information about each test, including status (Passed, Failed, Skipped), duration, and error messages for failed tests.

# 3. Instrumentation
## Logging
### ILogger
ILogger interface was used to ensures seamless logging throughout the application.
### Serilog
Serilog was used to implement a variety of sinks, enabling logs to be sent to different targets such as console, files, Seq, databases, and more.
For this project console log was used as a sink as an example.
## Metering
### Prometheus

Prometheus exporter can be used as metrics collection and storage system,for gathering application and system metrics, commented out in the metrics section of program.

### Grafana

Visualization and monitoring tool for displaying metrics collected by Prometheus, including premade dashboards for kubernetes clusters and .net core apps.
## Tracing
### Jaeger:

Distributed tracing system for visualizing request flows and identifying performance bottlenecks, commented out in the metrics section of program.

# 4. Containerization
## Docker Containerization
**Dockerfile**: - Instructions to build the Docker image for the application is included in the root of each project so `app.api` and `app.ui` have their own Dockerfile to be built and pushed to the desired repo.
### Build the Docker image for app.api
    docker build -t app/app.api:latest -f app.api/Dockerfile .

### Build the Docker image for app.ui
    docker build -t app/app.ui:latest -f app.ui/Dockerfile .

### Push the Docker images to a container registry
    docker push app/app.api:latest
    docker push app/app.ui:latest
## Docker Compose
Docker Compose is provided in the root of the solution to run all the micro services in docker at once.

# 5. Kubernetes
## Convert Docker Compose to Kubernetes
Using Kompose to convert the Docker Compose file to Kubernetes resources.

### Step 1: Create Docker Images           
#### Build the Docker images for the Web API and Blazor app: 
    docker build -t app/app.api:latest -f app.api/Dockerfile .
    docker build -t app/app.ui:latest -f app.ui/Dockerfile .
#### Push the images to a container registry like Docker Hub or a private registry like Harbor:
    docker push app/app.api:latest
    docker push app/app.ui:latest


### Step 2: Docker Compose Setup
#### Create a docker-compose.yml file to define the services:
    version: '3.9'
    services:
      app.api:
        image: app/app.api:latest
        ports:
          - "5000:8080"
      app.ui:
        image: app/app.ui:latest
        ports:
          - "6000:8080"
### Step 3: Convert to Kubernetes Resources
    kompose convert -f docker-compose.yml -o k8s/
### Step 4: Deploy to Kubernetes
    minikube start
    kubectl apply -f .
    kubectl get pods
    kubectl get services

# 6. CI/CD
## Overview
A high-level overview of how to use Argo Workflows with DAGs and integrate tools like Gitea, Kaniko, Harbor, Trivy, and Docker Crane to establish a well defined CI/CD pipeline, native to kubernetes by CNCF landscape accepted projects.
## Argo Workflows with DAGs
Argo Workflows is a Kubernetes-native workflow engine that supports both step-based and DAG-based workflows. DAGs (Directed Acyclic Graphs) allows defining dependencies between tasks, enabling parallel execution and complex workflows.
## Integrating Tools

**Gitea:** Gitea is a self-hosted Git service that provides Git hosting, code review, team collaboration, and CI/CD. Gitea can be used to manage your code repositories and integrate it with Argo Workflows to trigger workflows based on code changes.

**Kaniko:** Kaniko is a tool for building container images from a Dockerfile, without requiring a Docker daemon. Kaniko can be used within an Argo Workflow to build container images as part of the CI/CD pipelines.

**Harbor:** Harbor is an open-source container image registry that secures, scans, and manages images. Harbor is used to store and manage the container images built by Kaniko and integrate it with Argo Workflows to deploy these images.

**Trivy:** Trivy is a vulnerability scanner for container images. Trivy is used within an Argo Workflow to scan container images and file system for vulnerabilities before deploying them.

**Docker Crane:** Docker Crane is a tool for managing container images and registries. Docker Crane interacts with Harbor registry to manage container images within Argo Workflows.
## Setup
In the CI/CD folder, argo workflows yaml file declares the entire process to be completed by pushing the repos.

## Workflow Execution 
**Clone Repository:** The workflow starts by cloning the repository from Gitea.
**Check Contents of Cloned Repository:** This step is used for debugging or verification purposes of cloned repos.

**Run Unit Tests:** Unit tests are executed to ensure the basic functionality of individual components.

**Run Integration Tests:** Integration tests are executed to verify interactions between components.

**Build Image:** If all tests pass, Kaniko builds the Docker image.

**Scan Image:** The built image and system files are scanned for vulnerabilities using Trivy.

**Push Image:** The scanned image is pushed to Harbor using Docker Crane.

**Deploy:** The application is deployed using Kubernetes.

**Run Acceptance Tests:** Acceptance tests are executed after deployment to ensure that the system meets the business requirements and works as expected from an end-user perspective.

# 7. App Integration
A Blazor Server App was selected to function as a micro frontend single-page application (SPA). For a mobile-first design approach, Bootstrap and Bootstrap Icons were utilized. The primary CRUD functions were employed to interact with the API. Data Transfer Objects (DTOs) were used in both projects to encapsulate data, and the Data Annotations Validator was implemented to ensure the validation of data inputs.


# Improvements/Suggestions

## Features

1. Response caching
1. Output caching
1. API Rate limiting
1. Compression and bundling

## .NET ASPIRE
This project can be made with .NET Aspire to enhance most of its aspects listed as below.

### Integration

.NET Aspire offers NuGet packages for commonly used services like Redis and Postgres, with standardized interfaces
### Local Orchestration
It provides features for running and connecting multi-project applications and their dependencies in local development environments
### Health Checks and Telemetry
It includes built-in support for health checks, telemetry, and resilience features
### Service Discovery and Configuration
It has built in service discovery and connection string management

