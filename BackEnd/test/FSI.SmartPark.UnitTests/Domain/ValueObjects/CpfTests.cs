using FluentAssertions;
using FSI.SmartPark.Domain.ValueObjects;

namespace FSI.SmartPark.UnitTests.Domain.ValueObjects;

/// <summary>
/// Testes unitários para o Value Object Cpf.
/// </summary>
public class CpfTests
{
    // ─── Criação válida ───────────────────────────────────────────────────────

    [Theory]
    [InlineData("12345678901")]          // 11 dígitos sem máscara
    [InlineData("123.456.789-01")]       // com máscara — limpo resulta em 11 dígitos
    public void Cpf_NumeroValido_DeveCriarSemExcecao(string numero)
    {
        var act = () => new Cpf(numero);
        act.Should().NotThrow();
    }

    [Fact]
    public void Cpf_DeveLimparMascara_AoArmazenarNumero()
    {
        var cpf = new Cpf("123.456.789-01");
        cpf.Numero.Should().Be("12345678901");
    }

    // ─── Validação ────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("1234567890")]   // 10 dígitos
    [InlineData("123456789012")] // 12 dígitos
    [InlineData("")]
    public void Cpf_NumeroInvalido_DeveLancarArgumentException(string numero)
    {
        var act = () => new Cpf(numero);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*11 dígitos*");
    }

    // ─── Igualdade de Records ─────────────────────────────────────────────────

    [Fact]
    public void Cpf_MesmoNumero_DeveSerIgual()
    {
        var a = new Cpf("12345678901");
        var b = new Cpf("12345678901");
        a.Should().Be(b);
    }

    [Fact]
    public void Cpf_NumerosDiferentes_NaoDeveSerIgual()
    {
        var a = new Cpf("12345678901");
        var b = new Cpf("98765432100");
        a.Should().NotBe(b);
    }
}
