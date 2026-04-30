using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using FSI.SmartPark.Test.IntegrationTests.Infrastructure;

namespace FSI.SmartPark.Test.IntegrationTests.Controllers;

/// <summary>
/// Testes de integração para MovimentacoesController.
/// Fluxo: Entrada → Saída.
/// </summary>
[Collection("Integration")]
public class MovimentacoesIntegrationTests(SmartParkFixture fixture)
{
    private readonly HttpClient _client = fixture.Client;
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    private async Task AutorizarAsync()
    {
        var resp = await _client.PostAsJsonAsync("/api/usuarios/autenticar",
            new { Login = "admin", Senha = "Admin@123" });
        resp.EnsureSuccessStatusCode();
        var body = await resp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        var token = body.GetProperty("token").GetString()!;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // ─── POST /api/movimentacoes — Registrar Entrada ──────────────────────────

    [Fact]
    public async Task RegistrarEntrada_PlacaValida_DeveRetornar201()
    {
        // Arrange
        await AutorizarAsync();
        var placa = $"TST{new Random().Next(1000, 9999)}";
        var payload = new
        {
            Placa      = placa,
            UnidadeId  = 1,
            EmpresaId  = 1,
            TipoVeiculo = "Carro"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/movimentacoes", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        body.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
        body.GetProperty("placa").GetString().Should().Be(placa);
        body.TryGetProperty("dataSaida", out var ds);
        // DataSaida deve ser null na entrada
        (ds.ValueKind == JsonValueKind.Null || ds.ValueKind == JsonValueKind.Undefined)
            .Should().BeTrue();
    }

    // ─── GET /api/movimentacoes — Lista ──────────────────────────────────────

    [Fact]
    public async Task GetAll_DeveRetornar200ComLista()
    {
        // Arrange
        await AutorizarAsync();

        // Act
        var response = await _client.GetAsync("/api/movimentacoes");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        body.ValueKind.Should().Be(JsonValueKind.Array);
    }

    // ─── PUT /api/movimentacoes/{id} — Registrar Saída ───────────────────────

    [Fact]
    public async Task RegistrarSaida_MovimentacaoAberta_DeveRetornar200()
    {
        // Arrange — cria uma entrada primeiro
        await AutorizarAsync();
        var placa = $"SAI{new Random().Next(1000, 9999)}";
        var entrada = await _client.PostAsJsonAsync("/api/movimentacoes", new
        {
            Placa = placa, UnidadeId = 1, EmpresaId = 1, TipoVeiculo = "Moto"
        });
        entrada.EnsureSuccessStatusCode();
        var entradaBody = await entrada.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        var id = entradaBody.GetProperty("id").GetInt32();

        var saidaPayload = new
        {
            MovimentacaoId = id,
            ValorCobrado   = 15.50m,
            EmpresaId      = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/movimentacoes/{id}", saidaPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
