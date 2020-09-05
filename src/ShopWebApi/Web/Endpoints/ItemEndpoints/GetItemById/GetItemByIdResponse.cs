using System;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class GetItemByIdResponse : MessageResponse
    {
        public GetItemByIdResponse(Guid correlationId) : base(correlationId) { }

        public GetItemByIdResponse() { }

        public ItemDto Item { get; set; }
    }
}
