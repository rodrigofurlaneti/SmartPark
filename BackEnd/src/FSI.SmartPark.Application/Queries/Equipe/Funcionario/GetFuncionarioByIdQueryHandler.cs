using FSI.SmartPark.Application.DTOs.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Equipe.Funcionario;

public sealed class GetFuncionarioByIdQueryHandler
    : IRequestHandler<GetFuncionarioByIdQuery, FuncionarioResponseDto?>
{
    private readonly IFuncionarioRepository _repo;
    public GetFuncionarioByIdQueryHandler(IFuncionarioRepository repo) => _repo = repo;

    public async Task<FuncionarioResponseDto?> Handle(GetFuncionarioByIdQuery request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct);
        return entidade is null ? null : ToDto(entidade);
    }

    private static FuncionarioResponseDto ToDto(FSI.SmartPark.Domain.Entities.Equipe.Funcionario e) => new FuncionarioResponseDto(e.Id, e.Pessoa_Id, e.Codigo, e.Salario, e.Status, e.TipoEscala, e.DataAdmissao, e.Cargo_Id, e.Unidade_Id);
}
