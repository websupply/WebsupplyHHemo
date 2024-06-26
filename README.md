![Logo do Projeto](topo.jpg)

# Interface de Integração Websupply x Hhemo

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

- [TOTVS - Protheus](http://h118996.protheus.cloudtotvs.com.br:4050/rest/) - API Totvs - Protheus onde será feito todo o consumo e integração

## Como Usar

Será necessário atualizar as dll's dentro do servidor em questão ou utilizar o projeto de Testes (Próximo Tópico).

### Instalação

1. Clone o repositório:
    ```bash
    git clone https://github.com/lgjhunzecher/WebsupplyHHemo.git
    ```
2. Navegue até o diretório do projeto:
    ```bash
    cd [repositório da solution]
    ```
3. Restaure os pacotes NuGet:
    ```bash
    dotnet restore
    ```

### Exemplo de Uso

```asp
<!-- Fornecedor -->
<%
    Dim wsWebsupply
    set wsWebsupply = Server.CreateObject("WebsupplyHHemo.Interface.Metodos.FornecedorMetodo")
    wsWebsupply.intCodForWebsupply = 0;
    wsWebsupply.strCodForProtheus = string.Empty;
    wsWebsupply.strCodLojaProtheus = string.Empty;
    if wsWebsupply.ConsomeWS() = True then
        response.write "Deu bom"
    else
        response.write wsWebsupply.strMensagem
    end if
%>

<!-- Centros de Custo -->
<%
    Dim wsWebsupply
    set wsWebsupply = Server.CreateObject("WebsupplyHHemo.Interface.Metodos.CentroCustoMetodo")
    if wsWebsupply.ConsomeWS() = True then
        response.write "Deu bom"
    else
        response.write wsWebsupply.strMensagem
    end if
%>

<!-- Condição de Pagamento -->
<%
    Dim wsWebsupply
    set wsWebsupply = Server.CreateObject("WebsupplyHHemo.Interface.Metodos.CondicaoPagtoMetodo")
    if wsWebsupply.ConsomeWS() = True then
        response.write "Deu bom"
    else
        response.write wsWebsupply.strMensagem
    end if
%>

<!-- Forma de Pagamento -->
<%
    Dim wsWebsupply
    set wsWebsupply = Server.CreateObject("WebsupplyHHemo.Interface.Metodos.FormaPagtoMetodo")
    if wsWebsupply.ConsomeWS() = True then
        response.write "Deu bom"
    else
        response.write wsWebsupply.strMensagem
    end if
%>

<!-- Natureza -->
<%
    Dim wsWebsupply
    set wsWebsupply = Server.CreateObject("WebsupplyHHemo.Interface.Metodos.NaturezaMetodo")
    if wsWebsupply.ConsomeWS() = True then
        response.write "Deu bom"
    else
        response.write wsWebsupply.strMensagem
    end if
%>

<!-- Plano de Conta -->
<%
    Dim wsWebsupply
    set wsWebsupply = Server.CreateObject("WebsupplyHHemo.Interface.Metodos.PlanoContaMetodo")
    if wsWebsupply.ConsomeWS() = True then
        response.write "Deu bom"
    else
        response.write wsWebsupply.strMensagem
    end if
%>

<!-- Tipo de Operação -->
<%
    Dim wsWebsupply
    set wsWebsupply = Server.CreateObject("WebsupplyHHemo.Interface.Metodos.TipoOperacaoMetodo")
    if wsWebsupply.ConsomeWS() = True then
        response.write "Deu bom"
    else
        response.write wsWebsupply.strMensagem
    end if
%>

<!-- Unidade de Medida -->
<%
    Dim wsWebsupply
    set wsWebsupply = Server.CreateObject("WebsupplyHHemo.Interface.Metodos.UnidadeMedidaMetodo")
    if wsWebsupply.ConsomeWS() = True then
        response.write "Deu bom"
    else
        response.write wsWebsupply.strMensagem
    end if
%>

<!-- Usuários / Unidades -->
<%
    Dim wsWebsupply
    set wsWebsupply = Server.CreateObject("WebsupplyHHemo.Interface.Metodos.UnidadesFiliaisMetodo")
    if wsWebsupply.ConsomeWS() = True then
        response.write "Deu bom"
    else
        response.write wsWebsupply.strMensagem
    end if
%>

<!-- Usuários -->
<%
    Dim wsWebsupply
    set wsWebsupply = Server.CreateObject("WebsupplyHHemo.Interface.Metodos.UsuarioMetodo")
    if wsWebsupply.ConsomeWS() = True then
        response.write "Deu bom"
    else
        response.write wsWebsupply.strMensagem
    end if
%>

<!-- Usuários / Unidades -->
<%
    Dim wsWebsupply
    set wsWebsupply = Server.CreateObject("WebsupplyHHemo.Interface.Metodos.UsuarioUnidadeMetodo")
    if wsWebsupply.ConsomeWS() = True then
        response.write "Deu bom"
    else
        response.write wsWebsupply.strMensagem
    end if
%>

