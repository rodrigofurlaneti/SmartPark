using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Domain.Entities.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Unidade;

public sealed class CreateUnidadeCommandHandler
    : IRequestHandler<CreateUnidadeCommand, UnidadeResponseDto>
{
    private readonly IUnidadeRepository _repo;
    public CreateUnidadeCommandHandler(IUnidadeRepository repo) => _repo = repo;

    public async Task<UnidadeResponseDto> Handle(CreateUnidadeCommand request, CancellationToken ct)
    {
        var entidade = new Unidade(request.Nome, request.NumeroVagas, request.DiaVencimento, request.EmpresaId);
        if (request.CNPJ is not null) entidade.DefinirCNPJ(request.CNPJ);
        var id = await _repo.Add(entidade, ct);
        var criado = await _repo.GetById(id, ct) ?? throw new InvalidOperationException("Unidade não encontrada após inserção.");
        return ToDto(criado);
    }

    private static UnidadeResponseDto ToDto(Unidade e) => new UnidadeResponseDto(e.Id, e.Codigo, e.Nome, e.NumeroVaga, e.Ativa, e.DiaVencimento, e.CNPJ, e.Empresa_Id, e.Funcionario_Id, e.Endereco_Id);