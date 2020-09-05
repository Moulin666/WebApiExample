using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class DeleteItemRequest : MessageRequest
    {
        public int ItemId { get; set; }
    }
}
