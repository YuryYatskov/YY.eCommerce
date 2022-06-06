namespace Ordering.Domain.Common
{
    /// <summary>
    /// The entity base.
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// The entity identifier.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Created by user.
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// The created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Last modified by user.
        /// </summary>
        public string? LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified date.
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }
    }
}