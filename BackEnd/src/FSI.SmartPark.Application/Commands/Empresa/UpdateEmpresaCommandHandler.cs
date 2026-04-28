using FSI.SmartPark.Application.DTOs.Identidade;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Empresa;

/// <summary>
/// Handler do UpdateEmpresaCommand.
/// Atualiza dados cadastrais de um tenant existente.
/// </summary>
public sealed class UpdateEmpresaCommandHandler
    : IRequestHandler<UpdateEmpresaCommand, EmpresaResponseDto>
{
    private readonly IEmpresaRepository _repo;

    public UpdateEmpresaCommandHandler(IEmpresaRepository repo) => _repo = repo;

    public async Task<EmpresaResponseDto> Handle(
        UpdateEmpresaCommand request,
        CancellationToken ct)
    {
        // 1. Busca a entidade (já filtra IsDeleted = 0)
        var empresa = await _repo.GetById(request.Id, ct)
            ?? throw new KeyNotFoundException($"Empresa {request.Id} não encontrada.");

        // 2. Aplica as regras de domínio na própria entidade (Rich Entity)
        empresa.AtualizarDados(
            request.NomeFantasia,
            request.RazaoSocial,
            request.Email,
            request.Telefone);

        // 3. Persiste as alterações
        await _repo.Update(empresa, ct);

        return EmpresaResponseDto.FromEntity(empresa);
    }
}
