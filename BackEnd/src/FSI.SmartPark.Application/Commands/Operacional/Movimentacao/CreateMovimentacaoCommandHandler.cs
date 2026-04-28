using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Movimentacao;

public sealed class CreateMovimentacaoCommandHandler
    : IRequestHandler<CreateMovimentacaoCommand, MovimentacaoResponseDto>
{
    private readonly IMovimentacaoRepository _repo;
    public CreateMovimentacaoCommandHandler(IMovimentacaoRepository repo) => _repo = repo;

    public async Task<MovimentacaoResponseDto> Handle(CreateMovimentacaoCommand request, CancellationToken ct)
    {
        var entidade = new FSI.SmartPark.Domain.Entities.Operacional.Movimentacao(request.Placa, request.UnidadeId, request.EmpresaId);
        if (request.ClienteId.HasValue) entidade.VincularCliente(request.ClienteId.Value, request.NumeroContrato);
        var id = await _repo.Add(entidade, ct);
        var criado = await _repo.GetById(id, ct) ?? throw new InvalidOperationException("Movimentacao não encontrada após inserção.");
        return ToDto(criado);
    }

    private static MovimentacaoResponseDto ToDto(FSI.SmartPark.Domain.Entities.Operacional.Movimentacao e) => new MovimentacaoResponseDto(e.Id, e.Ticket, e.Placa, e.DataEntrada, e.DataSaida, e.ValorCobrado, e.FormaPagamento, e.TipoCliente, e.Unidade_Id, !e.DataSaida.HasValue);
}
