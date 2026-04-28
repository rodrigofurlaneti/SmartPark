using FluentAssertions;
using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.ValueObjects;

namespace FSI.SmartPark.Test.UnitTests.Domain.Entities;

/// <summary>
/// Testes unitários para a entidade Cliente — regras de negócio do domínio.
/// </summary>
public class ClienteTests
{
    // ─── Construção com CPF ───────────────────────────────────────────────────

    [Fact]
    public void Cliente_ComCpfValido_DeveCriarCorretamente()
    {
        var cpf = new Cpf("12345678901");
        var cliente = new Cliente("João Silva", cpf, false, 1);

        cliente.Nome.Should().Be("João Silva");
        cliente.DocumentoNumero.Should().Be("12345678901");
        cliente.IsMensalista.Should().BeFalse();
        cliente.Ativo.Should().BeTrue();
        cliente.Empresa_Id.Should().Be(1);
    }

    [Fact]
    public void Cliente_ComCpfMensalista_DeveCriarCorretamente()
    {
        var cpf = new Cpf("12345678901");
        var cliente = new Cliente("Maria Souza", cpf, true, 2);

        cliente.IsMensalista.Should().BeTrue();
        cliente.Empresa_Id.Should().Be(2);
    }

    // ─── Construção com CNPJ ─────────────────────────────────────────────────

    [Fact]
    public void Cliente_ComCnpjValido_DeveCriarCorretamente()
    {
        var cnpj = new Cnpj("12345678000195");
        var cliente = new Cliente("Empresa ABC Ltda", cnpj, true, 1);

        cliente.Nome.Should().Be("Empresa ABC Ltda");
        cliente.DocumentoNumero.Should().Be("12345678000195");
        cliente.IsMensalista.Should().BeTrue();
        cliente.Ativo.Should().BeTrue();
    }

    // ─── Validações de guard clauses ─────────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Cliente_ComNomeVazio_DeveLancarArgumentException(string nomeInvalido)
    {
        var cpf = new Cpf("12345678901");
        var act = () => new Cliente(nomeInvalido, cpf, false, 1);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Nome*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Cliente_ComEmpresaIdInvalido_DeveLancarArgumentException(int empresaIdInvalido)
    {
        var cpf = new Cpf("12345678901");
        var act = () => new Cliente("Teste", cpf, false, empresaIdInvalido);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*EmpresaId*");
    }

    // ─── Estado inicial ───────────────────────────────────────────────────────

    [Fact]
    public void Cliente_AoCriar_DeveEstarAtivo_E_NaoExcluido()
    {
        var cpf = new Cpf("12345678901");
        var cliente = new Cliente("Teste", cpf, false, 1);

        cliente.Ativo.Should().BeTrue();
        cliente.IsDeleted.Should().BeFalse();
        cliente.DeletedAt.Should().BeNull();
    }

    // ─── SoftDelete herdado ───────────────────────────────────────────────────

    [Fact]
    public void Cliente_AposSoftDelete_DeveEstarExcluido()
    {
        var cpf = new Cpf("12345678901");
        var cliente = new Cliente("Teste", cpf, false, 1);
        cliente.SoftDelete();
        cliente.IsDeleted.Should().BeTrue();
        cliente.DeletedAt.Should().NotBeNull();
    }
}
