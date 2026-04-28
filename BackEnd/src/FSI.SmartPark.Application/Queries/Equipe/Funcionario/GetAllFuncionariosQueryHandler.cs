using FSI.SmartPark.Application.DTOs.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Domain.Entities.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Equipe.Funcionario;

public sealed class GetAllFuncionariosQueryHandler
    : IRequestHandler<GetAllFuncionariosQuery, IEnumerable<FuncionarioResponseDto>>
{
    private readonly IFuncionarioRepository _repo;
    public GetAllFuncionariosQueryHandler(IFuncionarioRepository repo) => _repo = repo;

    public async Task<IEnumerable<FuncionarioResponseDto>> Handle(GetAllFuncionariosQuery request, CancellationToken ct)
    {
        var lista = await _repo.GetAll(ct);
        return lista.Select(ToDto);
    }

    private static FuncionarioResponseDto ToDto(Funcionario e) => new FuncionarioResponseDto(e.Id, e.Pessoa_Id, e.Codigo, e.Salario, e.Status, e.TipoEscala, e.DataAdmissao, e.Cargo_Id, e.Unidade_Id);