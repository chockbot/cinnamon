# Cinnamon Project

## Tech Stack Used:

- .Net 6 using Blazor
- Postgres for Database

## Getting Started

Build Javascript Library
Run below command inside JsLib folder

```
npm run build
```

Start the Project inside Docker Container

```
docker-compose up -d
```

Start the Database for Local Development

```
cd database
docker-compose up -d
```

## Production Commands

Start the prod container instance  
**Make sure that .env.prod file is created**  
**Make sure SSL Certificates are in proper folder**

```
docker-compose --env-file .env.prod up -d
```
