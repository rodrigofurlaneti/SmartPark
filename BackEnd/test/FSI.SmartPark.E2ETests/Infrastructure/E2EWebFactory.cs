using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace FSI.SmartPark.Test.E2ETests.Infrastructure;

/// <summary>
/// Factory para testes E2E. Sobe o servidor ASP.NET Core completo em memória
/// contra o banco de dados real (SmartParkDB).
/// </summary>
public class E2EWebFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:SmartPark"] =
                    "Server=financialmanager.cziua2iyy04f.us-west-1.rds.amazonaws.com;" +
                    "Port=3306;Database=SmartParkDB;User=admin;Password=financialmanager123;" +
                    "CharSet=utf8mb4;SslMode=Required;AllowPublicKeyRetrieval=true;" +
                    "ConvertZeroDateTime=True;AllowZeroDateTime=False;ConnectionTimeout=30;"
            });
        });
        builder.UseEnvironment("E2E");
    }
}

/// <summary>
/// Cliente HTTP enriquecido com helpers de autenticação e deserialização.
/// </summary>
public class E2EClient : IDisposable
{
    private readonly E2EWebFactory _factory = new();
    private readonly HttpClient _http;

    public E2EClient()
    {
        _http = _factory.CreateClient();
    }

    // ─── Auth ─────────────────────────────────────────────────────────────────

    public async Task LoginAsync(string login = "admin", string senha = "Admin@123")
    {
        var resp = await _http.PostAsJsonAsync("/api/usuarios/autenticar",
            new { Login = login, Senha = senha });
        resp.EnsureSuccessStatusCode();

        var body = await resp.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>(
            new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

        var token = body.GetProperty("token").GetString()!;
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    // ─── Wrappers HTTP ────────────────────────────────────────────────────────

    public Task<HttpResponseMessage> GetAsync(string uri) => _http.GetAsync(uri);
    public Task<HttpResponseMessage> PostAsync<T>(string uri, T body) => _http.PostAsJsonAsync(uri, body);
    public Task<HttpResponseMessage> PutAsync<T>(string uri, T body) => _http.PutAsJsonAsync(uri, body);
    public Task<HttpResponseMessage> DeleteAsync(string uri) => _http.DeleteAsync(uri);

    public async Task<System.Text.Json.JsonElement> ReadJsonAsync(HttpResponseMessage resp)
    {
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>(
            new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));
    }

    public void Dispose()
    {
        _http.Dispose();
        _factory.Dispose();
    }
}

[CollectionDefinition("E2E")]
public class E2ECollection : ICollectionFixture<E2EClient> { }
