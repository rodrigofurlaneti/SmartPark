using FSI.SmartPark.Application.DTOs.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Domain.Entities.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Equipe.ControlePonto;

public sealed class CreateControlePontoCommandHandler
    : IRequestHandler<CreateControlePontoCommand, ControlePontoResponseDto>
{
    private readonly IControlePontoRepository _repo;
    public CreateControlePontoCommandHandler(IControlePontoRepository repo) => _repo = repo;

    public async Task<ControlePontoResponseDto> Handle(CreateControlePontoCommand request, CancellationToken ct)
    {
        var entidade = new ControlePonto(request.FuncionarioId, (int)request.TipoRegistro);
        var id = await _repo.Add(entidade, ct);
        var criado = await _repo.GetById(id, ct) ?? throw new InvalidOperationException("ControlePonto não encontrado após inserção.");
        return ToDto(criado);
    }

    private static ControlePontoResponseDto ToDto(ControlePonto e) => new ControlePontoResponseDto(e.Id, e.Funcionario_Id, e.DataRegistro, (FSI.SmartPark.Domain.Enums.TipoRegistroPonto)e.TipoRegistro, e.Unidade_Id);