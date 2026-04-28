using FluentAssertions;
using FSI.SmartPark.Domain.Entities;

namespace FSI.SmartPark.UnitTests.Domain;

/// <summary>
/// Testes unitários para EntityBase — cobertura de Soft Delete, Restore e igualdade.
/// </summary>
public class EntityBaseTests
{
    // Subclasse concreta para testar EntityBase (que é abstract)
    private sealed class DummyEntity : EntityBase
    {
        public DummyEntity() { }
        public DummyEntity(int id) { Id = id; }
    }

    // ─── SoftDelete ───────────────────────────────────────────────────────────

    [Fact]
    public void SoftDelete_DeveMarcarIsDeleted_ComoTrue()
    {
        var entity = new DummyEntity();
        entity.SoftDelete();
        entity.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void SoftDelete_DeveDefinirDeletedAt_ComDataAtual()
    {
        var antes = DateTime.UtcNow;
        var entity = new DummyEntity();
        entity.SoftDelete();
        entity.DeletedAt.Should().NotBeNull();
        entity.DeletedAt!.Value.Should().BeOnOrAfter(antes);
    }

    // ─── Restore ─────────────────────────────────────────────────────────────

    [Fact]
    public void Restore_AposExclusao_DeveMarcarIsDeleted_ComoFalse()
    {
        var entity = new DummyEntity();
        entity.SoftDelete();
        entity.Restore();
        entity.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Restore_AposExclusao_DeveLimparDeletedAt()
    {
        var entity = new DummyEntity();
        entity.SoftDelete();
        entity.Restore();
        entity.DeletedAt.Should().BeNull();
    }

    // ─── Equals ───────────────────────────────────────────────────────────────

    [Fact]
    public void Equals_MesmoId_DeveRetornarTrue()
    {
        var a = new DummyEntity(1);
        var b = new DummyEntity(1);
        a.Equals(b).Should().BeTrue();
    }

    [Fact]
    public void Equals_IdsDiferentes_DeveRetornarFalse()
    {
        var a = new DummyEntity(1);
        var b = new DummyEntity(2);
        a.Equals(b).Should().BeFalse();
    }

    [Fact]
    public void Equals_IdZero_DeveRetornarFalse_PoisEntidadeNaoPersistedAinda()
    {
        var a = new DummyEntity(0);
        var b = new DummyEntity(0);
        a.Equals(b).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_MesmoId_DeveRetornarMesmoHashCode()
    {
        var a = new DummyEntity(42);
        var b = new DummyEntity(42);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    // ─── DataInsercao ────────────────────────────────────────────────────────

    [Fact]
    public void Construtor_DeveDefinirDataInsercao_ComDataAtual()
    {
        var antes = DateTime.UtcNow;
        var entity = new DummyEntity();
        entity.DataInsercao.Should().BeOnOrAfter(antes);
    }

    [Fact]
    public void Construtor_IsDeleted_DeveSerFalse_PorPadrao()
    {
        var entity = new DummyEntity();
        entity.IsDeleted.Should().BeFalse();
        entity.DeletedAt.Should().BeNull();
    }
}
