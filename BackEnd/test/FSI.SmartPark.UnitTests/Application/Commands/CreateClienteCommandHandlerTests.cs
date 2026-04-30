using FluentAssertions;
using FSI.SmartPark.Application.Commands.Comercial.Cliente;
using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Domain.ValueObjects;
using Moq;

namespace FSI.SmartPark.Test.UnitTests.Application.Commands;

public class CreateClienteCommandHandlerTests
{
    private readonly Mock<IClienteRepository> _repoMock = new();
    private readonly CreateClienteCommandHandler _handler;

    public CreateClienteCommandHandlerTests()
    {
        _handler = new CreateClienteCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_DocumentoCpf11Digitos_DeveCriarClienteComCpf()
    {
        _repoMock.Setup(r => r.Add(It.IsAny<Cliente>(), default)).ReturnsAsync(1);
        _repoMock.Setup(r => r.GetById(1, default)).ReturnsAsync(
            CriarEntidade("Joao Silva", "12345678901", false, 1, 1));

        var resultado = await _handler.Handle(
            new CreateClienteCommand("Joao Silva", "12345678901", false, 1), default);

        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be("Joao Silva");
        resultado.DocumentoNumero.Should().Be("12345678901");
        resultado.IsMensalista.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_DocumentoCnpj14Digitos_DeveCriarClienteComCnpj()
    {
        _repoMock.Setup(r => r.Add(It.IsAny<Cliente>(), default)).ReturnsAsync(2);
        _repoMock.Setup(r => r.GetById(2, default)).ReturnsAsync(
            CriarEntidade("Empresa XYZ", "12345678000195", true, 1, 2));

        var resultado = await _handler.Handle(
            new CreateClienteCommand("Empresa XYZ", "12345678000195", true, 1), default);

        resultado.Nome.Should().Be("Empresa XYZ");
        resultado.DocumentoNumero.Should().Be("12345678000195");
        resultado.IsMensalista.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_DocumentoValido_DeveInvocarAddEGetById()
    {
        _repoMock.Setup(r => r.Add(It.IsAny<Cliente>(), default)).ReturnsAsync(1);
        _repoMock.Setup(r => r.GetById(1, default)).ReturnsAsync(
            CriarEntidade("Teste", "12345678901", false, 1, 1));

        await _handler.Handle(new CreateClienteCommand("Teste", "12345678901", false, 1), default);

        _repoMock.Verify(r => r.Add(It.IsAny<Cliente>(), default), Times.Once);
        _repoMock.Verify(r => r.GetById(1, default), Times.Once);
    }

    [Fact]
    public async Task Handle_GetByIdRetornaNull_DeveLancarInvalidOperationException()
    {
        _repoMock.Setup(r => r.Add(It.IsAny<Cliente>(), default)).ReturnsAsync(99);
        _repoMock.Setup(r => r.GetById(99, default)).ReturnsAsync((Cliente?)null);

        var act = () => _handler.Handle(
            new CreateClienteCommand("Teste", "12345678901", false, 1), default);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*não encontrado*");
    }

    private static Cliente CriarEntidade(string nome, string doc, bool mensalista, int empresaId, int id)
    {
        Cliente c = doc.Length == 11
            ? new Cliente(nome, new Cpf(doc), mensalista, empresaId)
            : new Cliente(nome, new Cnpj(doc), mensalista, empresaId);

        typeof(FSI.SmartPark.Domain.Entities.EntityBase)
            .GetProperty("Id")!
            .SetValue(c, id);

        return c;
    }
}
