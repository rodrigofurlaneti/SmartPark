using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Domain.Entities.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Estoque;

public sealed class CreateEstoqueCommandHandler
    : IRequestHandler<CreateEstoqueCommand, EstoqueResponseDto>
{
    private readonly IEstoqueRepository _repo;
    public CreateEstoqueCommandHandler(IEstoqueRepository repo) => _repo = repo;

    public async Task<EstoqueResponseDto> Handle(CreateEstoqueCommand request, CancellationToken ct)
    {
        var entidade = new Estoque(request.Nome, request.UnidadeId ?? 0, request.EmpresaId, request.EstoquePrincipal);
        var id = await _repo.Add(entidade, ct);
        var criado = await _repo.GetById(id, ct) ?? throw new InvalidOperationException("Estoque não encontrado após inserção.");
        return ToDto(criado);
    }

    private static EstoqueResponseDto ToDto(Estoque e) => new EstoqueResponseDto(e.Id, e.Nome, e.EstoquePrincipal, e.Unidade_Id);
}
