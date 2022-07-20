using System;

namespace Shared.Domain.SeedWork
{
    /// <summary>
    /// Base abstract class Entity.
    /// Provides useful generic properties and methods to those who inherit it
    /// </summary>
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Base CreatedAt property
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Base UpdatedAt property
        /// </summary>
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// Base DeletedAt property
        /// </summary>
        public DateTimeOffset? DeletedAt { get; set; }

        private int? _requestedHashCode;

        /// <summary>
        /// Base Id property
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Verifica se há mudança
        /// </summary>
        /// <returns></returns>
        public bool IsTransient()
        {
            return Id == default;
        }

        /// <summary>
        /// Comparisons between generic objects
        /// </summary>
        /// <param name="obj">Generic Objects</param>
        /// <returns>Returns Boolean variable containing the result of the comparison</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (GetType() != obj.GetType())
                return false;
            var item = (Entity)obj;
            if (item.IsTransient() || IsTransient())
                return false;

            return item.Id == Id;
        }

        /// <summary>
        /// Get Hash Code
        /// </summary>
        /// <returns>Hash Code int type</returns>
        /// XOR for random distribution. See: http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx
        public override int GetHashCode()
        {
            if (IsTransient()) return base.GetHashCode();

            if (!_requestedHashCode.HasValue)
                _requestedHashCode = Id.GetHashCode() ^ 31;

            return _requestedHashCode.Value;
        }

        /// <summary>
        /// Robust purchasing between entities
        /// If left entity equals null, compare the right entity with null
        /// If left entity diferent null, compare the left entity with right
        /// </summary>
        /// <param name="left">Left entity use to compare</param>
        /// <param name="right">Right entity use to compare</param>
        /// <returns>Return true if they are different </returns>
        /// <returns>Return false if they are equal </returns>
        public static bool operator ==(Entity left, Entity right)
        {
            return left?.Equals(right) ?? Equals(right, null);
        }

        /// <summary>
        /// Simple comparison between entities
        /// </summary>
        /// <param name="left">Left entity use to compare</param>
        /// <param name="right">Right entity use to compare</param>
        /// <returns>Return true if they are different </returns>
        /// <returns>Return false if they are equal </returns>
        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
