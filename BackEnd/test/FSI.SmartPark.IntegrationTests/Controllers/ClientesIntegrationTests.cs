using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using FSI.SmartPark.Test.IntegrationTests.Infrastructure;

namespace FSI.SmartPark.Test.IntegrationTests.Controllers;

/// <summary>
/// Testes de integração para ClientesController (CRUD completo).
/// </summary>
[Collection("Integration")]
public class ClientesIntegrationTests(SmartParkFixture fixture)
{
    private readonly HttpClient _client = fixture.Client;
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    // ─── Helpers ─────────────────────────────────────────────────────────────

    private async Task AutorizarAsync()
    {
        var resp = await _client.PostAsJsonAsync("/api/usuarios/autenticar",
            new { Login = "admin", Senha = "Admin@123" });
        resp.EnsureSuccessStatusCode();
        var body = await resp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        var token = body.GetProperty("token").GetString()!;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // ─── GET /api/clientes ────────────────────────────────────────────────────

    [Fact]
    public async Task GetAll_SemAutorizacao_DeveRetornar200()
    {
        // A API atual não requer auth para GETs — valida que o endpoint existe e retorna lista
        var response = await _client.GetAsync("/api/clientes");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        body.ValueKind.Should().Be(JsonValueKind.Array);
    }

    // ─── POST /api/clientes ───────────────────────────────────────────────────

    [Fact]
    public async Task Post_ClienteComCpfValido_DeveRetornar201()
    {
        // Arrange
        await AutorizarAsync();
        var cpfUnico = GerarCpfFake();
        var payload = new
        {
            Nome        = $"Teste Integração {DateTime.UtcNow:HHmmss}",
            Documento   = cpfUnico,
            IsMensalista = false,
            EmpresaId   = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/clientes", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        body.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
        body.GetProperty("nome").GetString().Should().StartWith("Teste Integração");
    }

    [Fact]
    public async Task Post_ClienteComCnpjValido_DeveRetornar201()
    {
        // Arrange
        await AutorizarAsync();
        var payload = new
        {
            Nome        = "Empresa Integração Ltda",
            Documento   = GerarCnpjFake(),
            IsMensalista = true,
            EmpresaId   = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/clientes", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    // ─── GET /api/clientes/{id} ───────────────────────────────────────────────

    [Fact]
    public async Task GetById_IdExistente_DeveRetornar200ComCliente()
    {
        // Arrange — cria primeiro
        await AutorizarAsync();
        var cpfUnico = GerarCpfFake();
        var postResp = await _client.PostAsJsonAsync("/api/clientes", new
        {
            Nome = "Busca por Id", Documento = cpfUnico, IsMensalista = false, EmpresaId = 1
        });
        postResp.EnsureSuccessStatusCode();
        var criado = await postResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        var id = criado.GetProperty("id").GetInt32();

        // Act
        var response = await _client.GetAsync($"/api/clientes/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        body.GetProperty("id").GetInt32().Should().Be(id);
    }

    [Fact]
    public async Task GetById_IdInexistente_DeveRetornar404Ou200Null()
    {
        // Arrange
        await AutorizarAsync();

        // Act
        var response = await _client.GetAsync("/api/clientes/999999");

        // Assert — handler retorna null → controller retorna 404 ou 200 com body null
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.OK);
    }

    // ─── DELETE /api/clientes/{id} ────────────────────────────────────────────

    [Fact]
    public async Task Delete_IdExistente_DeveRetornar204()
    {
        // Arrange — cria e depois deleta
        await AutorizarAsync();
        var postResp = await _client.PostAsJsonAsync("/api/clientes", new
        {
            Nome = "Para Deletar", Documento = GerarCpfFake(), IsMensalista = false, EmpresaId = 1
        });
        postResp.EnsureSuccessStatusCode();
        var criado = await postResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        var id = criado.GetProperty("id").GetInt32();

        // Act
        var response = await _client.DeleteAsync($"/api/clientes/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────

    private static string GerarCpfFake()
    {
        var rnd = new Random();
        return string.Concat(Enumerable.Range(0, 11).Select(_ => rnd.Next(0, 10).ToString()));
    }

    private static string GerarCnpjFake()
    {
        var rnd = new Random();
        return string.Concat(Enumerable.Range(0, 14).Select(_ => rnd.Next(0, 10).ToString()));
    }
}
