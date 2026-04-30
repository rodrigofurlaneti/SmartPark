using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using FSI.SmartPark.Application.Commands.Suporte.Usuario;
using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using Moq;

namespace FSI.SmartPark.Test.UnitTests.Application.Commands;

/// <summary>
/// Testes unitários para AuthenticateCommandHandler.
/// Isola completamente o repositório via Moq — nenhuma dependência de banco.
/// </summary>
public class AuthenticateCommandHandlerTests
{
    private readonly Mock<IUsuarioRepository> _repoMock = new();
    private readonly AuthenticateCommandHandler _handler;

    // Hash SHA256 de "Senha@123"
    private static readonly string SenhaHash =
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes("Senha@123"))).ToLower();

    public AuthenticateCommandHandlerTests()
    {
        _handler = new AuthenticateCommandHandler(_repoMock.Object);
    }

    private Usuario CriarUsuarioAtivo(string login = "operador") =>
        new(login, SenhaHash, empresaId: 1);

    // ─── Happy Path ───────────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_CredenciaisValidas_DeveRetornarTokenResponseDto()
    {
        // Arrange
        var usuario = CriarUsuarioAtivo();
        _repoMock.Setup(r => r.GetAll(default)).ReturnsAsync([usuario]);

        var command = new AuthenticateCommand("operador", "Senha@123");

        // Act
        var resultado = await _handler.Handle(command, default);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Token.Should().NotBeNullOrWhiteSpace();
        resultado.Expiracao.Should().BeAfter(DateTime.UtcNow);
        resultado.Usuario.Login.Should().Be("operador");
    }

    [Fact]
    public async Task Handle_CredenciaisValidas_TokenDeveExpirarEm8Horas()
    {
        // Arrange
        var usuario = CriarUsuarioAtivo();
        _repoMock.Setup(r => r.GetAll(default)).ReturnsAsync([usuario]);

        // Act
        var antes = DateTime.UtcNow;
        var resultado = await _handler.Handle(new AuthenticateCommand("operador", "Senha@123"), default);

        // Assert — expira em ~8h (tolerância de 1 minuto)
        resultado.Expiracao.Should().BeCloseTo(antes.AddHours(8), TimeSpan.FromMinutes(1));
    }

    // ─── Login inválido ───────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_LoginNaoEncontrado_DeveLancarUnauthorizedAccessException()
    {
        // Arrange — repositório retorna lista vazia
        _repoMock.Setup(r => r.GetAll(default)).ReturnsAsync([]);

        // Act
        var act = () => _handler.Handle(new AuthenticateCommand("naoexiste", "Senha@123"), default);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*Login*");
    }

    // ─── Senha incorreta ─────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_SenhaIncorreta_DeveLancarUnauthorizedAccessException()
    {
        // Arrange
        var usuario = CriarUsuarioAtivo();
        _repoMock.Setup(r => r.GetAll(default)).ReturnsAsync([usuario]);

        // Act
        var act = () => _handler.Handle(new AuthenticateCommand("operador", "SenhaErrada"), default);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*Senha*");
    }

    // ─── Usuário bloqueado ────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_UsuarioBloqueado_DeveLancarUnauthorizedAccessException()
    {
        // Arrange
        var usuario = CriarUsuarioAtivo();
        usuario.Bloquear();
        _repoMock.Setup(r => r.GetAll(default)).ReturnsAsync([usuario]);

        // Act
        var act = () => _handler.Handle(new AuthenticateCommand("operador", "Senha@123"), default);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*bloqueado*");
    }

    // ─── Guard clauses ────────────────────────────────────────────────────────

    [Theory]
    [InlineData("", "Senha@123")]
    [InlineData("   ", "Senha@123")]
    public async Task Handle_LoginVazio_DeveLancarArgumentException(string login, string senha)
    {
        var act = () => _handler.Handle(new AuthenticateCommand(login, senha), default);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Login*");
    }

    [Theory]
    [InlineData("operador", "")]
    [InlineData("operador", "   ")]
    public async Task Handle_SenhaVazia_DeveLancarArgumentException(string login, string senha)
    {
        var act = () => _handler.Handle(new AuthenticateCommand(login, senha), default);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Senha*");
    }

    // ─── Isolamento — repositório chamado exatamente 1 vez ───────────────────

    [Fact]
    public async Task Handle_CredenciaisValidas_DeveConsultarRepositorioUmaVez()
    {
        // Arrange
        var usuario = CriarUsuarioAtivo();
        _repoMock.Setup(r => r.GetAll(default)).ReturnsAsync([usuario]);

        // Act
        await _handler.Handle(new AuthenticateCommand("operador", "Senha@123"), default);

        // Assert
        _repoMock.Verify(r => r.GetAll(default), Times.Once);
    }
}
