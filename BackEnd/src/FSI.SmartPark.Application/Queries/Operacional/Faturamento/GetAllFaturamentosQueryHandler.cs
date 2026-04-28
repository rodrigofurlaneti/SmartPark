using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Faturamento;

public sealed class GetAllFaturamentosQueryHandler
    : IRequestHandler<GetAllFaturamentosQuery, IEnumerable<FaturamentoResponseDto>>
{
    private readonly IFaturamentoRepository _repo;
    public GetAllFaturamentosQueryHandler(IFaturamentoRepository repo) => _repo = repo;

    public async Task<IEnumerable<FaturamentoResponseDto>> Handle(GetAllFaturamentosQuery request, CancellationToken ct)
    {
        var lista = await _repo.GetAll(ct);
        return lista.Select(ToDto);
    }

    private static FaturamentoResponseDto ToDto(FSI.SmartPark.Domain.Entities.Operacional.Faturamento e) => new FaturamentoResponseDto(e.Id, e.NumFechamento, e.NumTerminal, e.Unidade_Id, e.DataAbertura, e.DataFechamento, e.ValorTotal, e.ValorDinheiro, e.ValorCartaoDebito, e.ValorCartaoCredito, e.ValorRotativo, e.ValorSemParar, e.SaldoInicial, e.ValorSangria, e.Usuario_Id);
}
