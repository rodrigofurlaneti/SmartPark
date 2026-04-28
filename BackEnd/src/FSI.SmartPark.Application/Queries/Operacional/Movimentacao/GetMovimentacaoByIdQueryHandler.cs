using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Domain.Entities.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Movimentacao;

public sealed class GetMovimentacaoByIdQueryHandler
    : IRequestHandler<GetMovimentacaoByIdQuery, MovimentacaoResponseDto?>
{
    private readonly IMovimentacaoRepository _repo;
    public GetMovimentacaoByIdQueryHandler(IMovimentacaoRepository repo) => _repo = repo;

    public async Task<MovimentacaoResponseDto?> Handle(GetMovimentacaoByIdQuery request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct);
        return entidade is null ? null : ToDto(entidade);
    }

    private static MovimentacaoResponseDto ToDto(Movimentacao e) => new MovimentacaoResponseDto(e.Id, e.Ticket, e.Placa, e.DataEntrada, e.DataSaida, e.ValorCobrado, e.FormaPagamento, e.TipoCliente, e.Unidade_Id, !e.DataSaida.HasValue);