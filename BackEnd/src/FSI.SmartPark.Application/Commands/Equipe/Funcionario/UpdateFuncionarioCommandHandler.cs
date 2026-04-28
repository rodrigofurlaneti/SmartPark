using FSI.SmartPark.Application.DTOs.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Equipe.Funcionario;

public sealed class UpdateFuncionarioCommandHandler
    : IRequestHandler<UpdateFuncionarioCommand, FuncionarioResponseDto>
{
    private readonly IFuncionarioRepository _repo;
    public UpdateFuncionarioCommandHandler(IFuncionarioRepository repo) => _repo = repo;

    public async Task<FuncionarioResponseDto> Handle(UpdateFuncionarioCommand request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct)
            ?? throw new KeyNotFoundException($"Funcionario {request.Id} não encontrado.");
        entidade.AlterarSalario(request.Salario);
        await _repo.Update(entidade, ct);
        return ToDto(entidade);
    }

    private static FuncionarioResponseDto ToDto(FSI.SmartPark.Domain.Entities.Equipe.Funcionario e) => new FuncionarioResponseDto(e.Id, e.Pessoa_Id, e.Codigo, e.Salario, e.Status, e.TipoEscala, e.DataAdmissao, e.Cargo_Id, e.Unidade_Id);
}
