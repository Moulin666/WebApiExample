using System;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class CreateOrderResponse : MessageResponse
    {
        public CreateOrderResponse(Guid correlationId) : base(correlationId) { }

        public CreateOrderResponse() { }

        public OrderDto Order { get; set; }
    }
}
