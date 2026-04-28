namespace FSI.SmartPark.Application.DTOs.Comercial;

public record SeloResponseDto(
    int Id,
    int Sequencial,
    int EmissaoSeloId,
    DateTime? Validade,
    bool Expirado
);
