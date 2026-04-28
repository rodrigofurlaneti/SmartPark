using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.Cliente;

public sealed class UpdateClienteCommandHandler
    : IRequestHandler<UpdateClienteCommand, ClienteResponseDto>
{
    private readonly IClienteRepository _repo;
    public UpdateClienteCommandHandler(IClienteRepository repo) => _repo = repo;

    public async Task<ClienteResponseDto> Handle(UpdateClienteCommand request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct)
            ?? throw new KeyNotFoundException($"Cliente {request.Id} não encontrado.");
        entidade.AlterarStatus(true);
        await _repo.Update(entidade, ct);
        return ToDto(entidade);
    }

    private static ClienteResponseDto ToDto(FSI.SmartPark.Domain.Entities.Comercial.Cliente e) => new ClienteResponseDto(e.Id, e.Nome, e.DocumentoNumero, e.IsMensalista, e.Ativo, e.Empresa_Id, e.DataInsercao);
}
