using FSI.SmartPark.Application.DTOs.Equipe;
using FSI.SmartPark.Application.Interfaces.Equipe;
using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;

namespace FSI.SmartPark.Application.Services.Equipe;

public class ControlePontoService : IControlePontoService
{
    private readonly IControlePontoRepository _repo;

    public ControlePontoService(IControlePontoRepository repo) => _repo = repo;

    public async Task<ControlePontoResponseDto> RegistrarPonto(RegistrarPontoRequestDto dto)
    {
        var ponto = new ControlePonto(dto.FuncionarioId, (int)dto.TipoRegistro);
        var id = await _repo.Inserir(ponto);
        var criado = await _repo.ObterPorId(id);
        return ToDto(criado!);
    }

    public async Task<IEnumerable<ControlePontoResponseDto>> ListarPorFuncionario(int funcionarioId, DateTime data)
    {
        var lista = await _repo.ListarTodos();
        return lista
            .Where(p => p.Funcionario_Id == funcionarioId && p.DataRegistro.Date == data.Date)
            .Select(ToDto);
    }

    private static ControlePontoResponseDto ToDto(ControlePonto p) => new(
        p.Id, p.Funcionario_Id, p.DataRegistro,
        (Domain.Enums.TipoRegistroPonto)p.TipoRegistro, p.Unidade_Id);
}
