# Web APIs Demo


## Overview

This demo project consists of 2 Web APIs to manage device registration and statistics.

- **DeviceRegistrationAPI**: Handles the core registration logic and interacts directly with the database.
- **StatisticsAPI**: Handles device registration via DeviceRegistrationAPI and serves statistics.
- **PostgreSQL**: Stores user and device.

## Tech Stack

- **Framework**: .NET 10 web api
- **Database**: PostgreSQL

## Getting Started

Clone the repository locally

### Deploy with Docker Compose

**Prerequisites**: Docker and Docker Compose installed

> Prebuilt images are available on DockerHub if you don’t want to build locally.
> - `fkauff/device-registration-api:latest` 
> - `fkauff/statistics-api:latest`


1. Configure environment:
    ```sh
    cp .env.example .env
    # Edit .env with your values
    ```

2. Start services:
    ```sh
    docker-compose up --build
    ```

**Services will be available at**:
- Postgres: `localhost:5432`
- DeviceRegistrationAPI: `http://localhost:5000`
- StatisticsAPI: `http://localhost:5001`

### Deploy with Kubernetes

**Prerequisites**: Kubernetes cluster with `kubectl` configured

1. Create namespace:
    ```sh
    kubectl apply -f k8s/00-namespace.yaml
    ```

2. Deploy database and configuration:
    ```sh
    kubectl apply -f k8s/01-secrets.yaml
    kubectl apply -f k8s/02-configmap.yaml
    kubectl apply -f k8s/03-postgres.yaml
    ```

3. Deploy APIs:
    ```sh
    kubectl apply -f k8s/04-network-policy.yaml
    kubectl apply -f k8s/05-rbac.yaml
    kubectl apply -f k8s/06-device-registration.yaml
    kubectl apply -f k8s/07-statistics.yaml
    ```

> 