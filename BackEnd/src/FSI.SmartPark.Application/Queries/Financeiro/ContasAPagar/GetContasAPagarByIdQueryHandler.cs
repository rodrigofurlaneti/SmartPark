using FSI.SmartPark.Application.DTOs.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Domain.Entities.Financeiro;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Financeiro.ContasAPagar;

public sealed class GetContasAPagarByIdQueryHandler
    : IRequestHandler<GetContasAPagarByIdQuery, ContasAPagarResponseDto?>
{
    private readonly IContasAPagarRepository _repo;
    public GetContasAPagarByIdQueryHandler(IContasAPagarRepository repo) => _repo = repo;

    public async Task<ContasAPagarResponseDto?> Handle(GetContasAPagarByIdQuery request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct);
        return entidade is null ? null : ToDto(entidade);
    }

    private static ContasAPagarResponseDto ToDto(ContasAPagar e) => new ContasAPagarResponseDto(e.Id, e.NumeroDocumento, e.DataVencimento, e.ValorTotal, (FSI.SmartPark.Domain.Enums.StatusContasAPagar)e.StatusConta, e.Fornecedor_Id, e.Unidade_Id);