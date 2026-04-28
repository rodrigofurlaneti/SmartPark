using FSI.SmartPark.Application.DTOs.Identidade;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Empresa;

/// <summary>
/// Handler do CreateEmpresaCommand.
/// Responsável por orquestrar a criação de um novo tenant (Empresa).
/// </summary>
public sealed class CreateEmpresaCommandHandler
    : IRequestHandler<CreateEmpresaCommand, EmpresaResponseDto>
{
    private readonly IEmpresaRepository _repo;

    public CreateEmpresaCommandHandler(IEmpresaRepository repo) => _repo = repo;

    public async Task<EmpresaResponseDto> Handle(
        CreateEmpresaCommand request,
        CancellationToken ct)
    {
        // 1. Cria a entidade rica com validações de domínio
        var empresa = new Domain.Entities.Identidade.Empresa(
            request.NomeFantasia,
            request.Cnpj,
            request.RazaoSocial,
            request.Email,
            request.Telefone);

        // 2. Persiste via Repository
        var id = await _repo.Add(empresa, ct);

        // 3. Retorna o registro recém-criado
        var criada = await _repo.GetById(id, ct)
            ?? throw new InvalidOperationException($"Empresa Id {id} não encontrada após inserção.");

        return EmpresaResponseDto.FromEntity(criada);
    }
}
