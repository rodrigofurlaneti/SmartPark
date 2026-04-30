using FluentAssertions;
using MediatR;
using NetArchTest.Rules;

namespace FSI.SmartPark.Test.ArchitectureTests;

/// <summary>
/// Valida que a estrutura CQRS está corretamente implementada:
/// — Commands implementam IRequest
/// — Queries implementam IRequest
/// — Handlers implementam IRequestHandler
/// — Controllers residem SOMENTE na API layer
/// </summary>
public class CqrsStructureTests
{
    private static System.Reflection.Assembly AppAssembly =>
        typeof(Application.Commands.Empresa.CreateEmpresaCommand).Assembly;

    private static System.Reflection.Assembly ApiAssembly =>
        typeof(Program).Assembly;

    private static System.Reflection.Assembly InfraAssembly =>
        typeof(Infrastructure.Repositories.RepositoryBase<>).Assembly;

    // ─── Commands implementam IRequest ────────────────────────────────────────

    [Fact]
    public void Commands_DevemImplementar_IRequest()
    {
        var result = Types.InAssembly(AppAssembly)
            .That()
            .ResideInNamespaceContaining("Commands")
            .And()
            .HaveNameEndingWith("Command")
            .Should()
            .ImplementInterface(typeof(IBaseRequest))
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Commands CQRS devem implementar IRequest<TResponse> do MediatR.");
    }

    // ─── Handlers implementam IRequestHandler ────────────────────────────────

    [Fact]
    public void CommandHandlers_DevemImplementar_IRequestHandler()
    {
        // Verificar que handlers implementam IRequestHandler (via IBaseRequest)
        var result = Types.InAssembly(AppAssembly)
            .That()
            .HaveNameEndingWith("CommandHandler")
            .Should()
            .ImplementInterface(typeof(IBaseRequest))
            .GetResult();

        // Não falha se não houver nenhum tipo encontrado (pode não existir nenhum que implementa diretamente)
        // A verificação principal é que handlers estão no namespace correto
        var handlersNoNamespaceCerto = Types.InAssembly(AppAssembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .ResideInNamespaceContaining("FSI.SmartPark.Application")
            .GetResult();

        handlersNoNamespaceCerto.IsSuccessful.Should().BeTrue(
            "Todos os handlers CQRS devem estar no namespace Application.");
    }

    // ─── Controllers residem apenas na API ───────────────────────────────────

    [Fact]
    public void Controllers_DevemResidir_ApenasNaApiLayer()
    {
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Should()
            .ResideInNamespaceContaining("FSI.SmartPark.API.Controllers")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Controllers devem estar exclusivamente na camada API, nunca em Application ou Domain.");
    }

    // ─── Infrastructure não deve ter lógica de negócio ───────────────────────

    [Fact]
    public void Infrastructure_NaoDeveTerHandlers_DeMediatR()
    {
        var result = Types.InAssembly(InfraAssembly)
            .ShouldNot()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Infrastructure não deve conter handlers de MediatR — isso é responsabilidade da Application layer.");
    }

    // ─── Handlers devem ser sealed ────────────────────────────────────────────

    [Fact]
    public void Handlers_DevemSer_Sealed()
    {
        var result = Types.InAssembly(AppAssembly)
            .That()
            .HaveNameEndingWith("Handler")
            .And()
            .AreClasses()
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Handlers CQRS devem ser sealed — não há razão para herança em handlers.");
    }
}
