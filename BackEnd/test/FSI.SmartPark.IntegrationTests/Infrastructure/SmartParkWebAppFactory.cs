using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace FSI.SmartPark.Test.IntegrationTests.Infrastructure;

/// <summary>
/// Factory que inicializa o servidor ASP.NET Core completo em memória
/// apontando para o SmartParkDB de teste no RDS.
/// </summary>
public class SmartParkWebAppFactory : WebApplicationFactory<Program>
{
    // Connection string para o banco de integração (mesmo RDS do desenvolvimento)
    private const string TestConnectionString =
        "Server=financialmanager.cziua2iyy04f.us-west-1.rds.amazonaws.com;" +
        "Port=3306;Database=SmartParkDB;User=admin;Password=financialmanager123;" +
        "CharSet=utf8mb4;SslMode=Required;AllowPublicKeyRetrieval=true;" +
        "ConvertZeroDateTime=True;AllowZeroDateTime=False;ConnectionTimeout=30;";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            // Sobrescreve a connection string para usar o banco de integração
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:SmartPark"] = TestConnectionString
            });
        });

        builder.UseEnvironment("Integration");
    }
}

/// <summary>
/// Fixture compartilhada entre todos os testes de integração da mesma coleção.
/// Evita subir o servidor web múltiplas vezes.
/// </summary>
public class SmartParkFixture : IDisposable
{
    public SmartParkWebAppFactory Factory { get; } = new();
    public HttpClient Client => Factory.CreateClient();

    public void Dispose() => Factory.Dispose();
}

/// <summary>
/// Agrupa os testes de integração na mesma coleção xUnit para compartilhar o Factory.
/// </summary>
[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<SmartParkFixture> { }
