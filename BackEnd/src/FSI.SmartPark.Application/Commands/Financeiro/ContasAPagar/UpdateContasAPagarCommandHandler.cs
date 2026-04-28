using FSI.SmartPark.Application.DTOs.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Financeiro.ContasAPagar;

public sealed class UpdateContasAPagarCommandHandler
    : IRequestHandler<UpdateContasAPagarCommand, ContasAPagarResponseDto>
{
    private readonly IContasAPagarRepository _repo;
    public UpdateContasAPagarCommandHandler(IContasAPagarRepository repo) => _repo = repo;

    public async Task<ContasAPagarResponseDto> Handle(UpdateContasAPagarCommand request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct)
            ?? throw new KeyNotFoundException($"ContasAPagar {request.Id} não encontrado.");
        entidade.Pagar();
        await _repo.Update(entidade, ct);
        return ToDto(entidade);
    }

    private static ContasAPagarResponseDto ToDto(FSI.SmartPark.Domain.Entities.Financeiro.ContasAPagar e) => new ContasAPagarResponseDto(e.Id, e.NumeroDocumento, e.DataVencimento, e.ValorTotal, (FSI.SmartPark.Domain.Enums.StatusContasAPagar)e.StatusConta, e.Fornecedor_Id, e.Unidade_Id);
}
