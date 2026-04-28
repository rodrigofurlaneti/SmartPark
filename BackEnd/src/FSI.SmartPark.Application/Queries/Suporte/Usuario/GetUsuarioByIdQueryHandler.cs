using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Domain.Entities.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Suporte.Usuario;

public sealed class GetUsuarioByIdQueryHandler
    : IRequestHandler<GetUsuarioByIdQuery, UsuarioResponseDto?>
{
    private readonly IUsuarioRepository _repo;
    public GetUsuarioByIdQueryHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<UsuarioResponseDto?> Handle(GetUsuarioByIdQuery request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct);
        return entidade is null ? null : ToDto(entidade);
    }

    private static UsuarioResponseDto ToDto(Usuario e) => new UsuarioResponseDto(e.Id, e.Login, e.Ativo, e.Unidade_Id, e.Funcionario_Id);