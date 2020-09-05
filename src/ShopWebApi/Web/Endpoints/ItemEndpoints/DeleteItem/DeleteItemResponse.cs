using System;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class DeleteItemResponse : MessageResponse
    {
        public DeleteItemResponse(Guid correlationId) : base(correlationId) { }

        public DeleteItemResponse() { }

        public string Status { get; set; } = "Deleted";
    }
}
