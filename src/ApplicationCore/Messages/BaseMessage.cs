using System;

namespace ApplicationCore.Messages
{
    /// <summary>
    /// Base message class for API.
    /// </summary>
    public abstract class BaseMessage
    {
        protected Guid _correlationId = Guid.NewGuid();
        public Guid CorrelationId() => _correlationId;
    }
}
