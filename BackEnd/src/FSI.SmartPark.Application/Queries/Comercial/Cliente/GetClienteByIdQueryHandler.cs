using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Domain.Entities.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Comercial.Cliente;

public sealed class GetClienteByIdQueryHandler
    : IRequestHandler<GetClienteByIdQuery, ClienteResponseDto?>
{
    private readonly IClienteRepository _repo;
    public GetClienteByIdQueryHandler(IClienteRepository repo) => _repo = repo;

    public async Task<ClienteResponseDto?> Handle(GetClienteByIdQuery request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct);
        return entidade is null ? null : ToDto(entidade);
    }

    private static ClienteResponseDto ToDto(Cliente e) => new ClienteResponseDto(e.Id, e.Nome, e.DocumentoNumero, e.IsMensalista, e.Ativo, e.Empresa_Id, e.DataInsercao);