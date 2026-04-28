using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.ContratoMensalista;

public sealed class CreateContratoMensalistaCommandHandler
    : IRequestHandler<CreateContratoMensalistaCommand, ContratoMensalistaResponseDto>
{
    private readonly IContratoMensalistaRepository _repo;
    public CreateContratoMensalistaCommandHandler(IContratoMensalistaRepository repo) => _repo = repo;

    public async Task<ContratoMensalistaResponseDto> Handle(CreateContratoMensalistaCommand request, CancellationToken ct)
    {
        var entidade = new FSI.SmartPark.Domain.Entities.Comercial.ContratoMensalista(request.ClienteId, request.UnidadeId, request.Valor, request.EmpresaId);
        var id = await _repo.Add(entidade, ct);
        var criado = await _repo.GetById(id, ct) ?? throw new InvalidOperationException("Contrato não encontrado após inserção.");
        return ToDto(criado);
    }

    private static ContratoMensalistaResponseDto ToDto(FSI.SmartPark.Domain.Entities.Comercial.ContratoMensalista e) => new ContratoMensalistaResponseDto(e.Id, e.NumeroContrato, e.Cliente_Id, e.Unidade_Id, e.Valor, e.Ativo, e.DataVencimento, e.NumeroVagas);
}
