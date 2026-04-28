using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Domain.Entities.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Comercial.Cliente;

public sealed class GetAllClientesQueryHandler
    : IRequestHandler<GetAllClientesQuery, IEnumerable<ClienteResponseDto>>
{
    private readonly IClienteRepository _repo;
    public GetAllClientesQueryHandler(IClienteRepository repo) => _repo = repo;

    public async Task<IEnumerable<ClienteResponseDto>> Handle(GetAllClientesQuery request, CancellationToken ct)
    {
        var lista = await _repo.GetAll(ct);
        return lista.Select(ToDto);
    }

    private static ClienteResponseDto ToDto(Cliente e) => new ClienteResponseDto(e.Id, e.Nome, e.DocumentoNumero, e.IsMensalista, e.Ativo, e.Empresa_Id, e.DataInsercao);