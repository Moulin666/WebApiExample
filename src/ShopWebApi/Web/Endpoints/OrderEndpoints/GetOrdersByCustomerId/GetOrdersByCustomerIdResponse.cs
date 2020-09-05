using System;
using System.Collections.Generic;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class GetOrdersByCustomerIdResponse : MessageResponse
    {
        public GetOrdersByCustomerIdResponse(Guid correlationId) : base(correlationId) { }

        public GetOrdersByCustomerIdResponse() { }

        public List<OrderDto> Orders { get; set; }
    }
}
