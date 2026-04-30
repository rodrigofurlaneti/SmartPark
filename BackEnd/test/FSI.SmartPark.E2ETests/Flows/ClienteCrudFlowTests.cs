using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using FSI.SmartPark.Test.E2ETests.Infrastructure;

namespace FSI.SmartPark.Test.E2ETests.Flows;

/// <summary>
/// E2E — Fluxo completo de CRUD de Cliente.
/// Simula a jornada do operador: listar → criar → buscar → deletar.
/// </summary>
public class ClienteCrudFlowTests : IDisposable
{
    private readonly E2EClient _client = new();
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    // ─── Fluxo: Listar → Criar → Buscar → Deletar ────────────────────────────

    [Fact]
    public async Task FluxoCompletoCrudCliente_DeveFuncionarPontaAPonta()
    {
        // SETUP — autenticar como admin
        await _client.LoginAsync("admin", "Admin@123");

        // PASSO 1: Listar clientes (deve retornar array, mesmo que vazio)
        var listaResp = await _client.GetAsync("/api/clientes");
        listaResp.StatusCode.Should().Be(HttpStatusCode.OK, "GET /api/clientes deve responder 200");
        var lista = await listaResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        lista.ValueKind.Should().Be(JsonValueKind.Array, "resultado deve ser uma lista");

        // PASSO 2: Criar novo cliente com CPF único
        var cpf = GerarCpfFake();
        var nomeCliente = $"E2E Cliente {DateTime.UtcNow:HHmmssfff}";
        var criarResp = await _client.PostAsync("/api/clientes", new
        {
            Nome        = nomeCliente,
            Documento   = cpf,
            IsMensalista = false,
            EmpresaId   = 1
        });
        criarResp.StatusCode.Should().Be(HttpStatusCode.Created,
            "POST /api/clientes deve retornar 201 Created");

        var criado = await criarResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        var clienteId = criado.GetProperty("id").GetInt32();
        clienteId.Should().BeGreaterThan(0, "o id gerado pelo banco deve ser positivo");
        criado.GetProperty("nome").GetString().Should().Be(nomeCliente);
        criado.GetProperty("documento").GetString().Should().Be(cpf);

        // PASSO 3: Buscar o cliente recém-criado por ID
        var buscarResp = await _client.GetAsync($"/api/clientes/{clienteId}");
        buscarResp.StatusCode.Should().Be(HttpStatusCode.OK,
            "GET /api/clientes/{id} deve retornar 200 para cliente existente");

        var encontrado = await buscarResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        encontrado.GetProperty("id").GetInt32().Should().Be(clienteId);
        encontrado.GetProperty("nome").GetString().Should().Be(nomeCliente);

        // PASSO 4: Soft-delete do cliente
        var deletarResp = await _client.DeleteAsync($"/api/clientes/{clienteId}");
        deletarResp.StatusCode.Should().Be(HttpStatusCode.NoContent,
            "DELETE /api/clientes/{id} deve retornar 204 No Content");

        // PASSO 5: Confirma que o cliente deletado não aparece mais na lista
        var listaFinalResp = await _client.GetAsync("/api/clientes");
        var listaFinal = await listaFinalResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        var ids = listaFinal.EnumerateArray()
            .Select(e => e.GetProperty("id").GetInt32())
            .ToList();
        ids.Should().NotContain(clienteId,
            "após soft-delete o cliente não deve aparecer nas listagens");
    }

    // ─── Fluxo: Criar cliente mensalista CNPJ ────────────────────────────────

    [Fact]
    public async Task FluxoCriarClienteMensalistaCnpj_DevePersistirCorretamente()
    {
        // SETUP
        await _client.LoginAsync("admin", "Admin@123");

        // CRIAR com CNPJ (empresa mensalista)
        var cnpj = GerarCnpjFake();
        var criarResp = await _client.PostAsync("/api/clientes", new
        {
            Nome        = "Empresa Mensalista E2E",
            Documento   = cnpj,
            IsMensalista = true,
            EmpresaId   = 1
        });
        criarResp.StatusCode.Should().Be(HttpStatusCode.Created);
        var criado = await criarResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);

        // BUSCAR e validar isMensalista
        var id = criado.GetProperty("id").GetInt32();
        var buscarResp = await _client.GetAsync($"/api/clientes/{id}");
        var cliente = await buscarResp.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);
        cliente.GetProperty("isMensalista").GetBoolean().Should().BeTrue();

        // CLEANUP
        await _client.DeleteAsync($"/api/clientes/{id}");
    }

    // ─── Helpers ─────────────────────────────────────────────────────────────

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

    public void Dispose() => _client.Dispose();
}
