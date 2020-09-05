using System;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class UpdateItemResponse : MessageResponse
    {
        public UpdateItemResponse(Guid correlationId) : base(correlationId) { }

        public UpdateItemResponse() { }

        public ItemDto Item { get; set; }
    }
}
