# Test Container

## Initial Projects

```sh
dotnet new sln -o TestContainer
cd TestContainer
dotnet new xunit -o Test
dotnet new classlib -o Repository
dotnet add Test reference Repository
```

## Add Testcontainers Package

```sh
cd Test
dotnet add package Testcontainers.PostgreSql --version 3.3.0
```

```sh
cd Test
dotnet add package Testcontainers.Oracle --version 3.3.0
dotnet add package Oracle.ManagedDataAccess.Core --version 3.21.100
```

[dotnet test container references](https://dotnet.testcontainers.org/)

## Start Container

```cs
var postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:13.11-alpine3.17")
            .Build();
        postgreSqlContainer.StartAsync();
```

[postgresql support versioning](https://www.postgresql.org/support/versioning/)

## Start with random assigned host port

```cs
.WithImage("postgres:13.11-alpine3.17")
```

eg.

```cs
var postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:13.11-alpine3.17")
            .WithPortBinding(5432, true)
            .Build();
        postgreSqlContainer.StartAsync();

// retrieve the mapped port with
var randomPort = postgreSqlContainer.GetMappedPublicPort(5432);
```

## Copying directory or files to the container

```cs
.WithResourceMapping(string, string)
```

eg.

```cs
var postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:13.11-alpine3.17")
            .WithResourceMapping(new DirectoryInfo("./Test/InitialDb"), "/docker-entrypoint-initdb.d")
            .Build();
        postgreSqlContainer.StartAsync();
```

[copying-directories-or-files-to-the-container](https://dotnet.testcontainers.org/api/create_docker_container/#copying-directories-or-files-to-the-container)