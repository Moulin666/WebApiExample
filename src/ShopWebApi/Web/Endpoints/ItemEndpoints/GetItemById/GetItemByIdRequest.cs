using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class GetItemByIdRequest : MessageRequest
    {
        public int ItemId { get; set; }
    }
}
