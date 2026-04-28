using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Domain.Entities.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.ContratoMensalista;

public sealed class UpdateContratoMensalistaCommandHandler
    : IRequestHandler<UpdateContratoMensalistaCommand, ContratoMensalistaResponseDto>
{
    private readonly IContratoMensalistaRepository _repo;
    public UpdateContratoMensalistaCommandHandler(IContratoMensalistaRepository repo) => _repo = repo;

    public async Task<ContratoMensalistaResponseDto> Handle(UpdateContratoMensalistaCommand request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct)
            ?? throw new KeyNotFoundException($"ContratoMensalista {request.Id} não encontrado.");
        entidade.RenovarContrato();
        await _repo.Update(entidade, ct);
        return ToDto(entidade);
    }

    private static ContratoMensalistaResponseDto ToDto(ContratoMensalista e) => new ContratoMensalistaResponseDto(e.Id, e.NumeroContrato, e.Cliente_Id, e.Unidade_Id, e.Valor, e.Ativo, e.DataVencimento, e.NumeroVagas);