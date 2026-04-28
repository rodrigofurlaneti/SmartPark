using FSI.SmartPark.Application.DTOs.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Equipe.Funcionario;

public sealed class CreateFuncionarioCommandHandler
    : IRequestHandler<CreateFuncionarioCommand, FuncionarioResponseDto>
{
    private readonly IFuncionarioRepository _repo;
    public CreateFuncionarioCommandHandler(IFuncionarioRepository repo) => _repo = repo;

    public async Task<FuncionarioResponseDto> Handle(CreateFuncionarioCommand request, CancellationToken ct)
    {
        var entidade = new FSI.SmartPark.Domain.Entities.Equipe.Funcionario(request.PessoaId, request.Salario, request.Escala, request.EmpresaId);
        var id = await _repo.Add(entidade, ct);
        var criado = await _repo.GetById(id, ct) ?? throw new InvalidOperationException("Funcionario não encontrado após inserção.");
        return ToDto(criado);
    }

    private static FuncionarioResponseDto ToDto(FSI.SmartPark.Domain.Entities.Equipe.Funcionario e) => new FuncionarioResponseDto(e.Id, e.Pessoa_Id, e.Codigo, e.Salario, e.Status, e.TipoEscala, e.DataAdmissao, e.Cargo_Id, e.Unidade_Id);
}
