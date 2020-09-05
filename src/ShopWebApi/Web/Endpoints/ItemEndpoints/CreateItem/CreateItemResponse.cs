using System;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class CreateItemResponse : MessageResponse
    {
        public CreateItemResponse(Guid correlationId) : base(correlationId) { }

        public CreateItemResponse() { }

        public ItemDto Item { get; set; }
    }
}
