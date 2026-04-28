using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Domain.Entities.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Unidade;

public sealed class GetUnidadeByIdQueryHandler
    : IRequestHandler<GetUnidadeByIdQuery, UnidadeResponseDto?>
{
    private readonly IUnidadeRepository _repo;
    public GetUnidadeByIdQueryHandler(IUnidadeRepository repo) => _repo = repo;

    public async Task<UnidadeResponseDto?> Handle(GetUnidadeByIdQuery request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct);
        return entidade is null ? null : ToDto(entidade);
    }

    private static UnidadeResponseDto ToDto(Unidade e) => new UnidadeResponseDto(e.Id, e.Codigo, e.Nome, e.NumeroVaga, e.Ativa, e.DiaVencimento, e.CNPJ, e.Empresa_Id, e.Funcionario_Id, e.Endereco_Id);