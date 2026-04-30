using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using FSI.SmartPark.Test.IntegrationTests.Infrastructure;

namespace FSI.SmartPark.Test.IntegrationTests.Controllers;

/// <summary>
/// Testes de integração para o endpoint POST /api/usuarios/autenticar.
/// Usa o banco SmartParkDB real no RDS — requer seed do admin.
/// </summary>
[Collection("Integration")]
public class AutenticacaoIntegrationTests(SmartParkFixture fixture)
{
    private readonly HttpClient _client = fixture.Client;

    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    // ─── Happy Path ───────────────────────────────────────────────────────────

    [Fact]
    public async Task Autenticar_CredenciaisValidas_DeveRetornar200ComToken()
    {
        // Arrange — usuário admin inserido no seed do banco
        var payload = new { Login = "admin", Senha = "Admin@123" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/usuarios/autenticar", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        body.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
        body.GetProperty("expiracao").GetString().Should().NotBeNullOrWhiteSpace();
        body.GetProperty("usuario").GetProperty("login").GetString().Should().Be("admin");
    }

    // ─── Senha errada ─────────────────────────────────────────────────────────

    [Fact]
    public async Task Autenticar_SenhaErrada_DeveRetornar401()
    {
        // Arrange
        var payload = new { Login = "admin", Senha = "SenhaErrada999" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/usuarios/autenticar", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─── Login inexistente ────────────────────────────────────────────────────

    [Fact]
    public async Task Autenticar_LoginInexistente_DeveRetornar401()
    {
        // Arrange
        var payload = new { Login = "usuarionaoexiste99", Senha = "Qualquer@123" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/usuarios/autenticar", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─── Payload vazio ────────────────────────────────────────────────────────

    [Fact]
    public async Task Autenticar_LoginVazio_DeveRetornar400()
    {
        // Arrange
        var payload = new { Login = "", Senha = "Admin@123" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/usuarios/autenticar", payload);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized);
    }
}
