using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Domain.Entities.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Faturamento;

public sealed class CreateFaturamentoCommandHandler
    : IRequestHandler<CreateFaturamentoCommand, FaturamentoResponseDto>
{
    private readonly IFaturamentoRepository _repo;
    public CreateFaturamentoCommandHandler(IFaturamentoRepository repo) => _repo = repo;

    public async Task<FaturamentoResponseDto> Handle(CreateFaturamentoCommand request, CancellationToken ct)
    {
        var entidade = new Faturamento(request.NumFechamento, request.NumTerminal, request.UnidadeId, request.UsuarioId, request.EmpresaId);
        entidade.DefinirSaldoInicial(request.SaldoInicial);
        var id = await _repo.Add(entidade, ct);
        var criado = await _repo.GetById(id, ct) ?? throw new InvalidOperationException("Faturamento não encontrado após inserção.");
        return ToDto(criado);
    }

    private static FaturamentoResponseDto ToDto(Faturamento e) => new FaturamentoResponseDto(e.Id, e.NumFechamento, e.NumTerminal, e.Unidade_Id, e.DataAbertura, e.DataFechamento ?? DateTime.MinValue, e.ValorTotal, e.ValorDinheiro, e.ValorCartaoDebito, e.ValorCartaoCredito, e.ValorRotativo, e.ValorSemParar, e.SaldoInicial, e.ValorSangria, e.Usuario_Id);