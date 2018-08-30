# ToggleAPI
A simple example ASP.NET Core 2.1 Web API with Swagger and MySQL using VS Code and Linux.

Hi All, 

This is a really simple "how to understand" the basic functionalities about ASP.NET Core 2.1 with VS Code and your enviroment.
Please be my guest to sugestions how to increase this project.

Right now you find here:

```
API PROJECT
    <PackageReference Include="AutoMapper" Version="7.0.1" />
    <PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="5.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.App" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="2.1.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="2.1.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Relational" Version="2.1.1" />
    <PackageReference Include="Microsoft.Extensions.Caching.Redis" Version="2.1.2" />
    <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="2.1.1" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="2.5.0" />
    <PackageReference Include="Swashbuckle.AspNetCore.SwaggerGen" Version="2.5.0" />
    <PackageReference Include="Swashbuckle.AspNetCore.SwaggerUi" Version="2.5.0" />

TESTS PROJECT
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="15.8.0" />
    <PackageReference Include="xunit" Version="2.3.1" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.3.1" />
```
ps.: The Redis part was not implemented yet.

If you would like to try this project you need to get a MySQL Server. 
I'm use a simple docker container with this command

`docker run -p 3306:3306 --name api-mysql -e MYSQL_ROOT_PASSWORD=123 -d mysql`

The database structure wil be created with this command:

`dotnet ef database update`

**Ps:** _You need to run this command inside the ToggleAPI project folder._ `ToggleAPI/ToggleAPI/`

When the project is running you can access the swagger strucurue with this link:

`https://localhost:5001/swagger`
_If the Door is 5001_

**PS.:** _You can use Postman app if you want_

There you need to create an user get the token (I'm using JWT token), this is happen in two steps:

**1 - Create a Json structure and send by like below:**

```
POST api/User/

{
    "firstName": "string",
    "lastName": "string",
    "username": "string",
    "password":"string",
}
```

**2 - Create a Json structure and send by like below:**

```
POST api/User/auth

{
    "username": "string",
    "password": "string",
}
```

After this get the token and include in the Authorization Link.
Important: include the token with the **Bearer** word ahead like the example if you using swagger:

`Bearer eyJhbGciOiJIUzI1N[...]`

Inside the swagger strucutre you will find the API documentation with examples.
~~I will put this information on Wikki here on git in a near future.~~
Página Wiki criada lá você pode entender melhor sobre as APIs e as suas funcionalidades.
Acesse: [Toggle WIKKI API Pages](https://github.com/pdonatilio/ToggleAPI/wiki)
