using System;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class GetOrderByIdResponse : MessageResponse
    {
        public GetOrderByIdResponse(Guid correlationId) : base(correlationId) { }

        public GetOrderByIdResponse() { }

        public OrderDto Order { get; set; }
    }
}
