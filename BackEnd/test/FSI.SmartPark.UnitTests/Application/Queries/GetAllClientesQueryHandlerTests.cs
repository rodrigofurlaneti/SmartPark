using FluentAssertions;
using FSI.SmartPark.Application.Queries.Comercial.Cliente;
using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Domain.ValueObjects;
using Moq;

namespace FSI.SmartPark.Test.UnitTests.Application.Queries;

public class GetAllClientesQueryHandlerTests
{
    private readonly Mock<IClienteRepository> _repoMock = new();
    private readonly GetAllClientesQueryHandler _handler;

    public GetAllClientesQueryHandlerTests()
    {
        _handler = new GetAllClientesQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_RepositorioComDoisClientes_DeveRetornarDoisDtos()
    {
        var clientes = new[]
        {
            new Cliente("Joao",  new Cpf("12345678901"), false, 1),
            new Cliente("Maria", new Cpf("98765432100"), true,  1),
        };
        _repoMock.Setup(r => r.GetAll(default)).ReturnsAsync(clientes);

        var resultado = (await _handler.Handle(new GetAllClientesQuery(), default)).ToList();

        resultado.Should().HaveCount(2);
        resultado[0].Nome.Should().Be("Joao");
        resultado[1].Nome.Should().Be("Maria");
        resultado[1].IsMensalista.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_RepositorioVazio_DeveRetornarListaVazia()
    {
        _repoMock.Setup(r => r.GetAll(default)).ReturnsAsync([]);

        var resultado = await _handler.Handle(new GetAllClientesQuery(), default);

        resultado.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_DeveMappearDocumentoCorretamente()
    {
        var cliente = new Cliente("Empresa", new Cnpj("12345678000195"), true, 2);
        _repoMock.Setup(r => r.GetAll(default)).ReturnsAsync([cliente]);

        var resultado = (await _handler.Handle(new GetAllClientesQuery(), default)).Single();

        resultado.DocumentoNumero.Should().Be("12345678000195");
        resultado.EmpresaId.Should().Be(2);
    }

    [Fact]
    public async Task Handle_DeveConsultarRepositorioExatamenteUmaVez()
    {
        _repoMock.Setup(r => r.GetAll(default)).ReturnsAsync([]);

        await _handler.Handle(new GetAllClientesQuery(), default);

        _repoMock.Verify(r => r.GetAll(default), Times.Once);
    }
}
