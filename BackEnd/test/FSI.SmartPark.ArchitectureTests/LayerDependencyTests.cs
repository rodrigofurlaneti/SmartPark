using FluentAssertions;
using NetArchTest.Rules;

namespace FSI.SmartPark.Test.ArchitectureTests;

/// <summary>
/// Validação das regras de dependência entre camadas da Clean Architecture.
///
///  Domain   ← Application ← Infrastructure
///                    ↑
///                   API
///
/// Regras fundamentais:
///  • Domain NÃO pode depender de nenhuma outra camada interna.
///  • Application PODE depender apenas de Domain.
///  • Infrastructure PODE depender de Domain (e Application para DTOs).
///  • API PODE depender de todas as camadas.
/// </summary>
public class LayerDependencyTests
{
    private const string DomainNs         = "FSI.SmartPark.Domain";
    private const string ApplicationNs    = "FSI.SmartPark.Application";
    private const string InfrastructureNs = "FSI.SmartPark.Infrastructure";
    private const string ApiNs            = "FSI.SmartPark.API";

    // ─── Domain — deve ser totalmente independente ────────────────────────────

    [Fact]
    public void Domain_NaoDeveDependerde_Application()
    {
        var result = Types.InAssembly(typeof(Domain.Entities.EntityBase).Assembly)
            .ShouldNot()
            .HaveDependencyOn(ApplicationNs)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "o Domain é o núcleo e não pode depender da Application layer. " +
            $"Violações: {FormatViolations(result)}");
    }

    [Fact]
    public void Domain_NaoDeveDependerde_Infrastructure()
    {
        var result = Types.InAssembly(typeof(Domain.Entities.EntityBase).Assembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNs)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Domain não deve conhecer Infrastructure. Violações: {FormatViolations(result)}");
    }

    [Fact]
    public void Domain_NaoDeveDependerde_API()
    {
        var result = Types.InAssembly(typeof(Domain.Entities.EntityBase).Assembly)
            .ShouldNot()
            .HaveDependencyOn(ApiNs)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Domain não deve conhecer API. Violações: {FormatViolations(result)}");
    }

    // ─── Application — depende apenas do Domain ───────────────────────────────

    [Fact]
    public void Application_NaoDeveDependerde_Infrastructure()
    {
        var result = Types.InAssembly(typeof(Application.Commands.Empresa.CreateEmpresaCommand).Assembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNs)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Application não deve referenciar Infrastructure (inversão de dependência). " +
            $"Violações: {FormatViolations(result)}");
    }

    [Fact]
    public void Application_NaoDeveDependerde_API()
    {
        var result = Types.InAssembly(typeof(Application.Commands.Empresa.CreateEmpresaCommand).Assembly)
            .ShouldNot()
            .HaveDependencyOn(ApiNs)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Application não deve depender de API. Violações: {FormatViolations(result)}");
    }

    // ─── Helper ───────────────────────────────────────────────────────────────

    private static string FormatViolations(TestResult result)
    {
        if (result.IsSuccessful) return "nenhuma";
        var types = result.FailingTypeNames ?? [];
        return string.Join(", ", types.Take(5)) + (types.Count > 5 ? $" (+{types.Count - 5} mais)" : "");
    }
}
