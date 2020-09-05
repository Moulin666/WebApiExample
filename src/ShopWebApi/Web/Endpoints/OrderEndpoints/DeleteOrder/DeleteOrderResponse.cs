using System;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class DeleteOrderResponse : MessageResponse
    {
        public DeleteOrderResponse(Guid correlationId) : base(correlationId) { }

        public DeleteOrderResponse() { }

        public string Status { get; set; } = "Deleted";
    }
}
