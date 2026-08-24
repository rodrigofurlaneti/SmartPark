using System;

namespace SmartPark.Domain.Primitives
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; protected set; }
        public bool IsActive { get; private set; }
        protected Entity(Guid id)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            CreatedAt = DateTime.Now;
            IsActive = true;
        }
        public bool Equals(Entity? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;
            return other.Id == Id && Id != Guid.Empty;
        }
        public override bool Equals(object? obj) => obj is Entity entity && Equals(entity);
        public override int GetHashCode() => HashCode.Combine(GetType(), Id);
        public static bool operator ==(Entity? first, Entity? second)
            => first is null ? second is null : first.Equals(second);
        public static bool operator !=(Entity? first, Entity? second) => !(first == second);
    }
}