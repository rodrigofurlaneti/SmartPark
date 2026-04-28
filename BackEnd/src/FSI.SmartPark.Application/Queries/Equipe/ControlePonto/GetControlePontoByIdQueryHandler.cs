using FSI.SmartPark.Application.DTOs.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Equipe.ControlePonto;

public sealed class GetControlePontoByIdQueryHandler
    : IRequestHandler<GetControlePontoByIdQuery, ControlePontoResponseDto?>
{
    private readonly IControlePontoRepository _repo;
    public GetControlePontoByIdQueryHandler(IControlePontoRepository repo) => _repo = repo;

    public async Task<ControlePontoResponseDto?> Handle(GetControlePontoByIdQuery request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct);
        return entidade is null ? null : ToDto(entidade);
    }

    private static ControlePontoResponseDto ToDto(FSI.SmartPark.Domain.Entities.Equipe.ControlePonto e) => new ControlePontoResponseDto(e.Id, e.Funcionario_Id, e.DataRegistro, (FSI.SmartPark.Domain.Enums.TipoRegistroPonto)e.TipoRegistro, e.Unidade_Id);
}
