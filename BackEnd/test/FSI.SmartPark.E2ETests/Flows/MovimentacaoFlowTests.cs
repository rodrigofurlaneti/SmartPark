using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using FSI.SmartPark.Test.E2ETests.Infrastructure;

namespace FSI.SmartPark.Test.E2ETests.Flows;

/// <summary>
/// E2E — Fluxo operacional completo de estacionamento:
/// Abrir Turno → Registrar Entrada → Registrar Saída → Fechar Turno.
/// </summary>
public class MovimentacaoFlowTests : IDisposable
{
    private readonly E2EClient _client = new();
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    // ─── Fluxo Completo: Entrada → Saída ─────────────────────────────────────

    [Fact]
    public async Task FluxoEntradaSaida_DeveFuncionarPontaAPonta()
    {
        // SETUP
        await _client.LoginAsync("admin", "Admin@123");
        var placa = $"E2E{new Random().Next(1000, 9999)}";

        // PASSO 1: Registrar Entrada
        var entradaResp = await _client.PostAsync("/api/movimentacoes", new
        {
            Placa       = placa,
            UnidadeId   = 1,
            EmpresaId   = 1,
            TipoVeiculo = "Carro"
        });
        entradaResp.StatusCode.Should().Be(HttpStatusCode.Created,
            "a entrada do veículo deve gerar status 201");

        var entrada = await entradaResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        var movId = entrada.GetProperty("id").GetInt32();
        movId.Should().BeGreaterThan(0);
        entrada.GetProperty("placa").GetString().Should().Be(placa);

        // DataSaida deve ser null na entrada
        if (entrada.TryGetProperty("dataSaida", out var dsSaida))
            dsSaida.ValueKind.Should().BeOneOf(
                new[] { JsonValueKind.Null, JsonValueKind.Undefined },
                "veículo recém-entrado não deve ter data de saída");

        // PASSO 2: Listar movimentações e confirmar que a nova aparece
        var listaResp = await _client.GetAsync("/api/movimentacoes");
        var lista = await listaResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        var placas = lista.EnumerateArray()
            .Select(e => e.GetProperty("placa").GetString())
            .ToList();
        placas.Should().Contain(placa, "a movimentação registrada deve aparecer na lista");

        // PASSO 3: Registrar Saída
        var saidaResp = await _client.PutAsync($"/api/movimentacoes/{movId}", new
        {
            MovimentacaoId = movId,
            ValorCobrado   = 12.50m,
            EmpresaId      = 1
        });
        saidaResp.StatusCode.Should().Be(HttpStatusCode.OK,
            "a saída do veículo deve gerar status 200");
    }

    // ─── Fluxo de Faturamento: Abrir → Fechar Turno ──────────────────────────

    [Fact]
    public async Task FluxoTurno_AbrirEFecharFaturamento_DeveFuncionarPontaAPonta()
    {
        // SETUP
        await _client.LoginAsync("admin", "Admin@123");

        // PASSO 1: Abrir Turno
        var numFechamento = new Random().Next(1000, 9999);
        var abrirResp = await _client.PostAsync("/api/faturamentos", new
        {
            NumFechamento  = numFechamento,
            ValorAbertura  = 100.00m,
            UnidadeId      = 1,
            EmpresaId      = 1,
            FuncionarioId  = (int?)null
        });
        abrirResp.StatusCode.Should().Be(HttpStatusCode.Created,
            "abertura de turno deve retornar 201");

        var turno = await abrirResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        var faturamentoId = turno.GetProperty("id").GetInt32();
        faturamentoId.Should().BeGreaterThan(0);

        // PASSO 2: Fechar Turno
        var fecharResp = await _client.PutAsync($"/api/faturamentos/{faturamentoId}", new
        {
            FaturamentoId = faturamentoId,
            ValorTotal    = 250.00m,
            EmpresaId     = 1
        });
        fecharResp.StatusCode.Should().Be(HttpStatusCode.OK,
            "fechamento de turno deve retornar 200");
    }

    // ─── Fluxo de erro: Entrada com UnidadeId inválida ───────────────────────

    [Fact]
    public async Task FluxoEntrada_UnidadeIdInvalida_DeveRetornarErro()
    {
        // SETUP
        await _client.LoginAsync("admin", "Admin@123");

        // PASSO 1: Tentar entrada com unidade inexistente (FK violation)
        var resp = await _client.PostAsync("/api/movimentacoes", new
        {
            Placa       = "ERR0001",
            UnidadeId   = 999999,   // não existe
            EmpresaId   = 1,
            TipoVeiculo = "Carro"
        });

        // Espera-se 400, 404 ou 500 (depende do tratamento do FK violation no DB)
        ((int)resp.StatusCode).Should().BeGreaterThanOrEqualTo(400,
            "FK violation deve resultar em erro HTTP ≥ 400");
    }

    public void Dispose() => _client.Dispose();
}
