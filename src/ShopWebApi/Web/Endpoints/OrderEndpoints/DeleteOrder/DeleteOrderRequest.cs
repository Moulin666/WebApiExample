using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class DeleteOrderRequest : MessageRequest
    {
        public int OrderId { get; set; }
    }
}
