using FSI.SmartPark.Application.DTOs.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Domain.Entities.Financeiro;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Financeiro.ContasAPagar;

public sealed class GetAllContasAPagarQueryHandler
    : IRequestHandler<GetAllContasAPagarQuery, IEnumerable<ContasAPagarResponseDto>>
{
    private readonly IContasAPagarRepository _repo;
    public GetAllContasAPagarQueryHandler(IContasAPagarRepository repo) => _repo = repo;

    public async Task<IEnumerable<ContasAPagarResponseDto>> Handle(GetAllContasAPagarQuery request, CancellationToken ct)
    {
        var lista = await _repo.GetAll(ct);
        return lista.Select(ToDto);
    }

    private static ContasAPagarResponseDto ToDto(ContasAPagar e) => new ContasAPagarResponseDto(e.Id, e.NumeroDocumento, e.DataVencimento, e.ValorTotal, (FSI.SmartPark.Domain.Enums.StatusContasAPagar)e.StatusConta, e.Fornecedor_Id, e.Unidade_Id);