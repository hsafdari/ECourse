

using System.Runtime.CompilerServices;

namespace ECourse.Services.Course.Domain.Common
{
    public abstract class BaseEntity<TId> where TId : notnull
    {
        protected BaseEntity()
        {
            
        }
        protected BaseEntity(TId id)
        {
            Id = id;
        }
        public TId Id { get; protected set; }=default!;
        public override bool Equals(object? obj)
        {
            if (obj is not BaseEntity<TId> other)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            if (IsTransient() || other.IsTransient())
            {
                return false;
            }

            return EqualityComparer<TId>.Default.Equals(
                Id,
                other.Id);
        }

        public override int GetHashCode()
        {
            return IsTransient()
                ? RuntimeHelpers.GetHashCode(this)
                : HashCode.Combine(GetType(), Id);
        }
        //what does it mean?
        public static bool operator ==(
        BaseEntity<TId>? left,
        BaseEntity<TId>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(
            BaseEntity<TId>? left,
            BaseEntity<TId>? right)
        {
            return !Equals(left, right);
        }
        private bool IsTransient()
        {
            return EqualityComparer<TId>.Default.Equals(
            Id,
            default!);
        }       

    }
}
