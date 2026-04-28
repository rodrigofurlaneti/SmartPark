using FluentAssertions;
using FSI.SmartPark.Domain.ValueObjects;

namespace FSI.SmartPark.UnitTests.Domain.ValueObjects;

/// <summary>
/// Testes unitários para o Value Object Cnpj.
/// </summary>
public class CnpjTests
{
    // ─── Criação válida ───────────────────────────────────────────────────────

    [Theory]
    [InlineData("12345678000195")]         // 14 dígitos sem máscara
    [InlineData("12.345.678/0001-95")]     // com máscara — limpo resulta em 14 dígitos
    public void Cnpj_NumeroValido_DeveCriarSemExcecao(string numero)
    {
        var act = () => new Cnpj(numero);
        act.Should().NotThrow();
    }

    [Fact]
    public void Cnpj_DeveLimparMascara_AoArmazenarNumero()
    {
        var cnpj = new Cnpj("12.345.678/0001-95");
        cnpj.Numero.Should().Be("12345678000195");
    }

    // ─── Validação ────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("1234567800019")]   // 13 dígitos
    [InlineData("123456780001956")] // 15 dígitos
    [InlineData("")]
    public void Cnpj_NumeroInvalido_DeveLancarArgumentException(string numero)
    {
        var act = () => new Cnpj(numero);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*14 dígitos*");
    }

    // ─── Igualdade de Records ─────────────────────────────────────────────────

    [Fact]
    public void Cnpj_MesmoNumero_DeveSerIgual()
    {
        var a = new Cnpj("12345678000195");
        var b = new Cnpj("12345678000195");
        a.Should().Be(b);
    }

    [Fact]
    public void Cnpj_NumerosDiferentes_NaoDeveSerIgual()
    {
        var a = new Cnpj("12345678000195");
        var b = new Cnpj("98765432000196");
        a.Should().NotBe(b);
    }
}
