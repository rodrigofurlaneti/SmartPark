using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Application.Interfaces.Comercial;
using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;

namespace FSI.SmartPark.Application.Services.Comercial;

public class ContratoMensalistaService : IContratoMensalistaService
{
    private readonly IContratoMensalistaRepository _repo;

    public ContratoMensalistaService(IContratoMensalistaRepository repo) => _repo = repo;

    public async Task<ContratoMensalistaResponseDto> Criar(ContratoMensalistaRequestDto dto)
    {
        var contrato = new ContratoMensalista(dto.ClienteId, dto.UnidadeId, dto.Valor);
        var id = await _repo.Inserir(contrato);
        var criado = await _repo.ObterPorId(id);
        return ToDto(criado!);
    }

    public async Task<ContratoMensalistaResponseDto?> ObterPorId(int id)
    {
        var c = await _repo.ObterPorId(id);
        return c is null ? null : ToDto(c);
    }

    public async Task<IEnumerable<ContratoMensalistaResponseDto>> ListarPorCliente(int clienteId)
    {
        var lista = await _repo.ListarTodos();
        return lista.Where(c => c.Cliente_Id == clienteId).Select(ToDto);
    }

    public async Task<IEnumerable<ContratoMensalistaResponseDto>> ListarPorUnidade(int unidadeId)
    {
        var lista = await _repo.ListarTodos();
        return lista.Where(c => c.Unidade_Id == unidadeId).Select(ToDto);
    }

    public async Task Renovar(int id)
    {
        var c = await _repo.ObterPorId(id)
            ?? throw new KeyNotFoundException($"Contrato {id} não encontrado.");
        c.RenovarContrato();
        await _repo.Atualizar(c);
    }

    public async Task Bloquear(int id)
    {
        var c = await _repo.ObterPorId(id)
            ?? throw new KeyNotFoundException($"Contrato {id} não encontrado.");
        c.BloquearContrato();
        await _repo.Atualizar(c);
    }

    private static ContratoMensalistaResponseDto ToDto(ContratoMensalista c) => new(
        c.Id, c.NumeroContrato, c.Cliente_Id, c.Unidade_Id,
        c.Valor, c.Ativo, c.DataVencimento, c.NumeroVagas);
}
