using FluentAssertions;
using FSI.SmartPark.Application.Queries.Comercial.Cliente;
using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Domain.ValueObjects;
using Moq;

namespace FSI.SmartPark.Test.UnitTests.Application.Queries;

public class GetClienteByIdQueryHandlerTests
{
    private readonly Mock<IClienteRepository> _repoMock = new();
    private readonly GetClienteByIdQueryHandler _handler;

    public GetClienteByIdQueryHandlerTests()
    {
        _handler = new GetClienteByIdQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_IdExistente_DeveRetornarClienteResponseDto()
    {
        var cliente = new Cliente("Ana Costa", new Cpf("12345678901"), false, 1);
        SetId(cliente, 5);
        _repoMock.Setup(r => r.GetById(5, default)).ReturnsAsync(cliente);

        var resultado = await _handler.Handle(new GetClienteByIdQuery(5), default);

        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(5);
        resultado.Nome.Should().Be("Ana Costa");
        resultado.DocumentoNumero.Should().Be("12345678901");
    }

    [Fact]
    public async Task Handle_IdInexistente_DeveRetornarNull()
    {
        _repoMock.Setup(r => r.GetById(999, default)).ReturnsAsync((Cliente?)null);
        var resultado = await _handler.Handle(new GetClienteByIdQuery(999), default);
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task Handle_DeveConsultarRepositorioComIdCorreto()
    {
        _repoMock.Setup(r => r.GetById(7, default)).ReturnsAsync((Cliente?)null);
        await _handler.Handle(new GetClienteByIdQuery(7), default);
        _repoMock.Verify(r => r.GetById(7, default), Times.Once);
    }

    private static void SetId(object entity, int id) =>
        typeof(FSI.SmartPark.Domain.Entities.EntityBase)
            .GetProperty("Id")!
            .SetValue(entity, id);
}
