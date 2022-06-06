namespace EventBus.Messages.Events
{
    /// <summary>
    /// The integration base event.
    /// </summary>
    public class IntegrationBaseEvent
    {
        /// <summary>
        /// An identifier event.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// A creation date.
        /// </summary>
        public DateTime CreationDate { get; }

        /// <summary>
        /// Initialization.
        /// </summary>
        public IntegrationBaseEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="id"> An identifier event. </param>
        /// <param name="creationDate"> A creation date. </param>
        public IntegrationBaseEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}