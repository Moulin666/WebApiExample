using System;

namespace ApplicationCore.Messages
{
    public abstract class MessageResponse : BaseMessage
    {
        public MessageResponse(Guid correlationId) : base() => _correlationId = correlationId;

        public MessageResponse() { }
    }
}
