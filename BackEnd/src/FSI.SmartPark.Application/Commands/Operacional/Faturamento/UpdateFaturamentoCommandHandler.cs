using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Faturamento;

public sealed class UpdateFaturamentoCommandHandler
    : IRequestHandler<UpdateFaturamentoCommand, FaturamentoResponseDto>
{
    private readonly IFaturamentoRepository _repo;
    public UpdateFaturamentoCommandHandler(IFaturamentoRepository repo) => _repo = repo;

    public async Task<FaturamentoResponseDto> Handle(UpdateFaturamentoCommand request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct)
            ?? throw new KeyNotFoundException($"Faturamento {request.Id} não encontrado.");
        entidade.FecharTurno(request.ValorTotal, request.ValorDinheiro, request.ValorCartaoDebito,
            request.ValorCartaoCredito, request.ValorRotativo, request.ValorMensalidade,
            request.ValorSemParar, request.ValorSeloDesconto, request.TicketFinal);
        await _repo.Update(entidade, ct);
        return ToDto(entidade);
    }

    private static FaturamentoResponseDto ToDto(FSI.SmartPark.Domain.Entities.Operacional.Faturamento e) => new FaturamentoResponseDto(e.Id, e.NumFechamento, e.NumTerminal, e.Unidade_Id, e.DataAbertura, e.DataFechamento, e.ValorTotal, e.ValorDinheiro, e.ValorCartaoDebito, e.ValorCartaoCredito, e.ValorRotativo, e.ValorSemParar, e.SaldoInicial, e.ValorSangria, e.Usuario_Id);
}
