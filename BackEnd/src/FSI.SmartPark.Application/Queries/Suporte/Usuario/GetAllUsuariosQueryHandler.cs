using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Domain.Entities.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Suporte.Usuario;

public sealed class GetAllUsuariosQueryHandler
    : IRequestHandler<GetAllUsuariosQuery, IEnumerable<UsuarioResponseDto>>
{
    private readonly IUsuarioRepository _repo;
    public GetAllUsuariosQueryHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<IEnumerable<UsuarioResponseDto>> Handle(GetAllUsuariosQuery request, CancellationToken ct)
    {
        var lista = await _repo.GetAll(ct);
        return lista.Select(ToDto);
    }

    private static UsuarioResponseDto ToDto(Usuario e) => new UsuarioResponseDto(e.Id, e.Login, e.Ativo, e.Unidade_Id, e.Funcionario_Id);