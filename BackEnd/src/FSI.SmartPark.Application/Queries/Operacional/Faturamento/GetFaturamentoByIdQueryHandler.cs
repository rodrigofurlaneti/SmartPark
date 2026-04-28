using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Faturamento;

public sealed class GetFaturamentoByIdQueryHandler
    : IRequestHandler<GetFaturamentoByIdQuery, FaturamentoResponseDto?>
{
    private readonly IFaturamentoRepository _repo;
    public GetFaturamentoByIdQueryHandler(IFaturamentoRepository repo) => _repo = repo;

    public async Task<FaturamentoResponseDto?> Handle(GetFaturamentoByIdQuery request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct);
        return entidade is null ? null : ToDto(entidade);
    }

    private static FaturamentoResponseDto ToDto(FSI.SmartPark.Domain.Entities.Operacional.Faturamento e) => new FaturamentoResponseDto(e.Id, e.NumFechamento, e.NumTerminal, e.Unidade_Id, e.DataAbertura, e.DataFechamento, e.ValorTotal, e.ValorDinheiro, e.ValorCartaoDebito, e.ValorCartaoCredito, e.ValorRotativo, e.ValorSemParar, e.SaldoInicial, e.ValorSangria, e.Usuario_Id);
}
