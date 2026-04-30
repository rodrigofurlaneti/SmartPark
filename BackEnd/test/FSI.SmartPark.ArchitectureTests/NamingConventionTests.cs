using FluentAssertions;
using NetArchTest.Rules;

namespace FSI.SmartPark.Test.ArchitectureTests;

/// <summary>
/// Valida convenções de nomenclatura para manter consistência CQRS em todo o projeto.
/// </summary>
public class NamingConventionTests
{
    private static System.Reflection.Assembly AppAssembly =>
        typeof(Application.Commands.Empresa.CreateEmpresaCommand).Assembly;

    // ─── Commands ─────────────────────────────────────────────────────────────

    [Fact]
    public void Commands_DevemTerminarCom_Command()
    {
        var result = Types.InAssembly(AppAssembly)
            .That()
            .ResideInNamespaceContaining("Commands")
            .And()
            .AreNotAbstract()
            .And()
            .DoNotHaveNameEndingWith("Handler")        // exclui handlers
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Todos os records de Command devem terminar com 'Command' para clareza CQRS.");
    }

    [Fact]
    public void CommandHandlers_DevemTerminarCom_CommandHandler()
    {
        var result = Types.InAssembly(AppAssembly)
            .That()
            .ResideInNamespaceContaining("Commands")
            .And()
            .AreNotAbstract()
            .And()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Handlers de Command devem terminar com 'CommandHandler'.");
    }

    // ─── Queries ──────────────────────────────────────────────────────────────

    [Fact]
    public void Queries_DevemTerminarCom_Query()
    {
        var result = Types.InAssembly(AppAssembly)
            .That()
            .ResideInNamespaceContaining("Queries")
            .And()
            .AreNotAbstract()
            .And()
            .DoNotHaveNameEndingWith("Handler")
            .Should()
            .HaveNameEndingWith("Query")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Todos os records de Query devem terminar com 'Query' para clareza CQRS.");
    }

    [Fact]
    public void QueryHandlers_DevemTerminarCom_QueryHandler()
    {
        var result = Types.InAssembly(AppAssembly)
            .That()
            .ResideInNamespaceContaining("Queries")
            .And()
            .AreNotAbstract()
            .And()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Handlers de Query devem terminar com 'QueryHandler'.");
    }

    // ─── Repositories ─────────────────────────────────────────────────────────

    [Fact]
    public void Repositories_DevemTerminarCom_Repository()
    {
        var result = Types.InAssembly(typeof(Infrastructure.Repositories.RepositoryBase<>).Assembly)
            .That()
            .ResideInNamespaceContaining("Repositories")
            .And()
            .AreNotAbstract()
            .Should()
            .HaveNameEndingWith("Repository")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Todas as implementações de repositório devem terminar com 'Repository'.");
    }

    // ─── Interfaces de domínio ────────────────────────────────────────────────

    [Fact]
    public void InterfacesDeRepositorio_DevemComecarCom_I()
    {
        var result = Types.InAssembly(typeof(Domain.Entities.EntityBase).Assembly)
            .That()
            .ResideInNamespaceContaining("Interfaces")
            .And()
            .AreInterfaces()
            .Should()
            .HaveNameStartingWith("I")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Interfaces devem seguir a convenção .NET de começar com 'I'.");
    }

    // ─── Entidades de domínio ─────────────────────────────────────────────────

    [Fact]
    public void EntidadesDeDominio_DevemHerdarDeEntityBase()
    {
        var result = Types.InAssembly(typeof(Domain.Entities.EntityBase).Assembly)
            .That()
            .ResideInNamespaceContaining("Entities")
            .And()
            .AreNotAbstract()
            .And()
            .AreClasses()
            .Should()
            .Inherit(typeof(Domain.Entities.EntityBase))
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Todas as entidades de domínio devem herdar de EntityBase para garantir " +
            "Soft Delete, auditoria e igualdade por identidade.");
    }
}
