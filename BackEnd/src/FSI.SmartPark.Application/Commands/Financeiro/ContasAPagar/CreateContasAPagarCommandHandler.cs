using FSI.SmartPark.Application.DTOs.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Domain.Entities.Financeiro;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Financeiro.ContasAPagar;

public sealed class CreateContasAPagarCommandHandler
    : IRequestHandler<CreateContasAPagarCommand, ContasAPagarResponseDto>
{
    private readonly IContasAPagarRepository _repo;
    public CreateContasAPagarCommandHandler(IContasAPagarRepository repo) => _repo = repo;

    public async Task<ContasAPagarResponseDto> Handle(CreateContasAPagarCommand request, CancellationToken ct)
    {
        var entidade = new ContasAPagar(request.NumeroDocumento, request.DataVencimento, request.ValorTotal, request.EmpresaId);
        var id = await _repo.Add(entidade, ct);
        var criado = await _repo.GetById(id, ct) ?? throw new InvalidOperationException("ContasAPagar não encontrado após inserção.");
        return ToDto(criado);
    }

    private static ContasAPagarResponseDto ToDto(ContasAPagar e) => new ContasAPagarResponseDto(e.Id, e.NumeroDocumento, e.DataVencimento, e.ValorTotal, (FSI.SmartPark.Domain.Enums.StatusContasAPagar)e.StatusConta, e.Fornecedor_Id, e.Unidade_Id);