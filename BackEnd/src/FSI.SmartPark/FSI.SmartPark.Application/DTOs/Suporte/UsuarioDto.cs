namespace FSI.SmartPark.Application.DTOs.Suporte;

public record UsuarioRequestDto(
    string Login,
    string Senha,
    int? UnidadeId = null,
    int? FuncionarioId = null
);

public record UsuarioLoginDto(
    string Login,
    string Senha
);

public record UsuarioResponseDto(
    int Id,
    string Login,
    bool Ativo,
    int? UnidadeId,
    int? FuncionarioId
);

public record TokenResponseDto(
    string Token,
    DateTime Expiracao,
    UsuarioResponseDto Usuario
);
