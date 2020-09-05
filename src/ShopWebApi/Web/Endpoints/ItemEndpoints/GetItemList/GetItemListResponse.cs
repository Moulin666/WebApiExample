using System;
using System.Collections.Generic;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class GetItemListResponse : MessageResponse
    {
        public GetItemListResponse(Guid correlationId) : base(correlationId) { }

        public GetItemListResponse() { }

        public List<ItemDto> Items { get; set; } = new List<ItemDto>();
        public int PageCount { get; set; }
    }
}
