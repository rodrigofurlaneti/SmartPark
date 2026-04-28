using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Domain.Entities.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Unidade;

public sealed class UpdateUnidadeCommandHandler
    : IRequestHandler<UpdateUnidadeCommand, UnidadeResponseDto>
{
    private readonly IUnidadeRepository _repo;
    public UpdateUnidadeCommandHandler(IUnidadeRepository repo) => _repo = repo;

    public async Task<UnidadeResponseDto> Handle(UpdateUnidadeCommand request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct)
            ?? throw new KeyNotFoundException($"Unidade {request.Id} não encontrada.");
        entidade.AtualizarCapacidade(request.NumeroVagas);
        await _repo.Update(entidade, ct);
        return ToDto(entidade);
    }

    private static UnidadeResponseDto ToDto(Unidade e) => new UnidadeResponseDto(e.Id, e.Codigo, e.Nome, e.NumeroVaga, e.Ativa, e.DiaVencimento, e.CNPJ, e.Empresa_Id, e.Funcionario_Id, e.Endereco_Id);