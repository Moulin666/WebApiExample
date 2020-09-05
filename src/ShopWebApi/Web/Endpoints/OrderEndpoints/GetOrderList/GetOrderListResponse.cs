using System;
using System.Collections.Generic;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class GetOrderListResponse : MessageResponse
    {
        public GetOrderListResponse(Guid correlationId) : base(correlationId) { }

        public GetOrderListResponse() { }

        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
        public int PageCount { get; set; }
    }
}
