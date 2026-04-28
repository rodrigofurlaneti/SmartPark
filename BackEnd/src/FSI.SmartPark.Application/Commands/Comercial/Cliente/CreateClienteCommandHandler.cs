using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.Cliente;

public sealed class CreateClienteCommandHandler
    : IRequestHandler<CreateClienteCommand, ClienteResponseDto>
{
    private readonly IClienteRepository _repo;
    public CreateClienteCommandHandler(IClienteRepository repo) => _repo = repo;

    public async Task<ClienteResponseDto> Handle(CreateClienteCommand request, CancellationToken ct)
    {
        // Valida tamanho do documento e cria a entidade de domínio
        var entidade = request.Documento.Length == 11
            ? new FSI.SmartPark.Domain.Entities.Comercial.Cliente(request.Nome, new FSI.SmartPark.Domain.ValueObjects.Cpf(request.Documento), request.IsMensalista, request.EmpresaId)
            : new FSI.SmartPark.Domain.Entities.Comercial.Cliente(request.Nome, new FSI.SmartPark.Domain.ValueObjects.Cnpj(request.Documento), request.IsMensalista, request.EmpresaId);
        var id = await _repo.Add(entidade, ct);
        var criado = await _repo.GetById(id, ct) ?? throw new InvalidOperationException("Cliente não encontrado após inserção.");
        return ToDto(criado);
    }

    private static ClienteResponseDto ToDto(FSI.SmartPark.Domain.Entities.Comercial.Cliente e) => new ClienteResponseDto(e.Id, e.Nome, e.DocumentoNumero, e.IsMensalista, e.Ativo, e.Empresa_Id, e.DataInsercao);
}
