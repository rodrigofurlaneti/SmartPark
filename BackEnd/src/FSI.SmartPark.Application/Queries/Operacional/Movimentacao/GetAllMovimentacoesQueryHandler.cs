using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Domain.Entities.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Movimentacao;

public sealed class GetAllMovimentacoesQueryHandler
    : IRequestHandler<GetAllMovimentacoesQuery, IEnumerable<MovimentacaoResponseDto>>
{
    private readonly IMovimentacaoRepository _repo;
    public GetAllMovimentacoesQueryHandler(IMovimentacaoRepository repo) => _repo = repo;

    public async Task<IEnumerable<MovimentacaoResponseDto>> Handle(GetAllMovimentacoesQuery request, CancellationToken ct)
    {
        var lista = await _repo.GetAll(ct);
        return lista.Select(ToDto);
    }

    private static MovimentacaoResponseDto ToDto(Movimentacao e) => new MovimentacaoResponseDto(e.Id, e.Ticket, e.Placa, e.DataEntrada, e.DataSaida, e.ValorCobrado, e.FormaPagamento, e.TipoCliente, e.Unidade_Id, !e.DataSaida.HasValue);