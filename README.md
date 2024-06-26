# Nome do Projeto

Interface de Integração Hhemo

## Nuget de Referências do Projeto - WebsupplyHHemo.API

- [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/6.0.16) - ASP.NET Core middleware that enables an application to receive an OpenID Connect bearer token..
- [Microsoft.AspNetCore.Identity](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity/2.2.0) - ASP.NET Core Identity is the membership system for building ASP.NET Core web applications, including membership, login, and user data. ASP.NET Core Identity allows you to add login features to your application and makes it easy to customize data about the logged in user.
- [Microsoft.Data.SqlClient](https://www.nuget.org/packages/Microsoft.Data.SqlClient/2.1.4) - Provides the data provider for SQL Server. These classes provide access to versions of SQL Server and encapsulate database-specific protocols, including tabular data stream (TDS)
- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/6.0.16) - Entity Framework Core is a modern object-database mapper for .NET. It supports LINQ queries, change tracking, updates, and schema migrations. EF Core works with SQL Server, Azure SQL Database, SQLite, Azure Cosmos DB, MySQL, PostgreSQL, and other databases through a provider plugin API.
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/6.0.16) - Microsoft SQL Server database provider for Entity Framework Core.
- [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/6.0.16) - Entity Framework Core Tools for the NuGet Package Manager Console in Visual Studio.
- [Microsoft.Extensions.Identity.Stores](https://www.nuget.org/packages/Microsoft.Extensions.Identity.Stores/7.0.5) - ASP.NET Core Identity is the membership system for building ASP.NET Core web applications, including membership, login, and user data. ASP.NET Core Identity allows you to add login features to your application and makes it easy to customize data about the logged in user.
- [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/6.2.3) - Swagger tools for documenting APIs built on ASP.NET Core

## Nuget de Referências do Projeto - WebsupplyHHemo.Interface

- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/13.0.3) - Json.NET is a popular high-performance JSON framework for .NET

## Componentes COM

- **WSComuns.dll**: Componente responsável por funções comuns do Sistema.
- **SgiConnection.dll**: Componente responsável pela conexão com o banco de dados.

## Gateway Utilizado

Descreva aqui o gateway utilizado no projeto. Exemplo:

- **Gateway X**: Descrição do gateway e como ele é utilizado no projeto.

## Como Usar

Descreva aqui como utilizar o projeto, incluindo snippets de código em ASP.NET. Exemplo:

### Instalação

1. Clone o repositório:
    ```bash
    git clone https://github.com/seu-usuario/seu-repositorio.git
    ```
2. Navegue até o diretório do projeto:
    ```bash
    cd seu-repositorio
    ```
3. Restaure os pacotes NuGet:
    ```bash
    dotnet restore
    ```

### Exemplo de Uso

```csharp
using System;
using Newtonsoft.Json;

namespace ExemploProjeto
{
    class Program
    {
        static void Main(string[] args)
        {
            var objeto = new { Nome = "Exemplo", Valor = 123 };
            string json = JsonConvert.SerializeObject(objeto);
            Console.WriteLine(json);
        }
    }
}
