using FSI.SmartPark.Application.DTOs.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Usuario;

/// <summary>Autentica um usuário e retorna o token de acesso.</summary>
public sealed record AuthenticateCommand(
    string Login,
    string Senha) : IRequest<TokenResponseDto>;
