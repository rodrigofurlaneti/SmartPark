using FSI.SmartPark.Domain.Enums;
namespace FSI.SmartPark.Application.DTOs.Equipe;

public record EquipeRequestDto(
    string Nome,
    int UnidadeId,
    TipoHorario TipoHorario,
    int? TipoEquipeId = null
);

public record EquipeResponseDto(
    int Id,
    string Nome,
    bool Ativo,
    int UnidadeId,
    TipoHorario TipoHorario,
    int? EncarregadoId,
    int? SupervisorId,
    DateTime? DataFim
);
