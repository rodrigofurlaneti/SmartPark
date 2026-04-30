using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using FSI.SmartPark.Test.E2ETests.Infrastructure;

namespace FSI.SmartPark.Test.E2ETests.Flows;

/// <summary>
/// E2E — Fluxo de autenticação.
/// Cenários: login válido, credenciais erradas, campo obrigatório vazio.
/// </summary>
public class LoginFlowTests : IDisposable
{
    private readonly E2EClient _client = new();
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    // ─── Fluxo completo de login bem-sucedido ─────────────────────────────────

    [Fact]
    public async Task FluxoLogin_CredenciaisValidas_DeveObterTokenEAcessarAPI()
    {
        // Passo 1: Autenticar
        var loginResp = await _client.PostAsync("/api/usuarios/autenticar",
            new { Login = "admin", Senha = "Admin@123" });

        loginResp.StatusCode.Should().Be(HttpStatusCode.OK, "credenciais do admin seed devem funcionar");

        var loginBody = await loginResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        var token = loginBody.GetProperty("token").GetString();
        token.Should().NotBeNullOrWhiteSpace("o servidor deve retornar um token válido");

        var expiracao = loginBody.GetProperty("expiracao").GetString();
        expiracao.Should().NotBeNullOrWhiteSpace();
        DateTime.Parse(expiracao!).Should().BeAfter(DateTime.UtcNow, "o token não deve estar expirado");

        // Passo 2: Com o token obtido, acessar um endpoint protegido
        await _client.LoginAsync(); // configura o header Authorization
        var clientesResp = await _client.GetAsync("/api/clientes");
        clientesResp.StatusCode.Should().Be(HttpStatusCode.OK,
            "um token válido deve permitir acesso aos recursos da API");
    }

    // ─── Credenciais inválidas ────────────────────────────────────────────────

    [Fact]
    public async Task FluxoLogin_SenhaErrada_DeveBloquearAcesso()
    {
        // Passo 1: Login com senha errada
        var loginResp = await _client.PostAsync("/api/usuarios/autenticar",
            new { Login = "admin", Senha = "SenhaErrada!" });

        loginResp.StatusCode.Should().Be(HttpStatusCode.Unauthorized,
            "credenciais inválidas não devem gerar token");

        var body = await loginResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        body.TryGetProperty("erro", out var erro);
        erro.GetString().Should().NotBeNullOrWhiteSpace("deve retornar mensagem de erro");
    }

    // ─── Login com usuário inexistente ────────────────────────────────────────

    [Fact]
    public async Task FluxoLogin_UsuarioInexistente_DeveRetornar401()
    {
        var resp = await _client.PostAsync("/api/usuarios/autenticar",
            new { Login = "fantasma99", Senha = "Admin@123" });

        resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    public void Dispose() => _client.Dispose();
}
