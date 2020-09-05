using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class GetOrderByIdRequest : MessageRequest 
    {
        public int OrderId { get; set; }
    }
}
