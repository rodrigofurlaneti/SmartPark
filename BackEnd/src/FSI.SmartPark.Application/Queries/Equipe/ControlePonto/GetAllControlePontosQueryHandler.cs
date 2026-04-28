using FSI.SmartPark.Application.DTOs.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Equipe.ControlePonto;

public sealed class GetAllControlePontosQueryHandler
    : IRequestHandler<GetAllControlePontosQuery, IEnumerable<ControlePontoResponseDto>>
{
    private readonly IControlePontoRepository _repo;
    public GetAllControlePontosQueryHandler(IControlePontoRepository repo) => _repo = repo;

    public async Task<IEnumerable<ControlePontoResponseDto>> Handle(GetAllControlePontosQuery request, CancellationToken ct)
    {
        var lista = await _repo.GetAll(ct);
        return lista.Select(ToDto);
    }

    private static ControlePontoResponseDto ToDto(FSI.SmartPark.Domain.Entities.Equipe.ControlePonto e) => new ControlePontoResponseDto(e.Id, e.Funcionario_Id, e.DataRegistro, (FSI.SmartPark.Domain.Enums.TipoRegistroPonto)e.TipoRegistro, e.Unidade_Id);
}
