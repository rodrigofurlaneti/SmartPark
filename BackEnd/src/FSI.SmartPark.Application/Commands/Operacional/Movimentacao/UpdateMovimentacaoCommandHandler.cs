using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Domain.Entities.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Movimentacao;

public sealed class UpdateMovimentacaoCommandHandler
    : IRequestHandler<UpdateMovimentacaoCommand, MovimentacaoResponseDto>
{
    private readonly IMovimentacaoRepository _repo;
    public UpdateMovimentacaoCommandHandler(IMovimentacaoRepository repo) => _repo = repo;

    public async Task<MovimentacaoResponseDto> Handle(UpdateMovimentacaoCommand request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct)
            ?? throw new KeyNotFoundException($"Movimentacao {request.Id} não encontrada.");
        entidade.RegistrarSaida(request.ValorCobrado, request.FormaPagamento);
        if (request.CpfParaNF is not null) entidade.InformarCpfParaNF(request.CpfParaNF);
        await _repo.RegistrarSaida(request.Id, request.ValorCobrado);
        return ToDto(entidade);
    }

    private static MovimentacaoResponseDto ToDto(Movimentacao e) => new MovimentacaoResponseDto(e.Id, e.Ticket, e.Placa, e.DataEntrada, e.DataSaida, e.ValorCobrado, e.FormaPagamento, e.TipoCliente, e.Unidade_Id, !e.DataSaida.HasValue);