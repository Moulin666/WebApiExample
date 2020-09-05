using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class GetOrdersByCustomerIdRequest : MessageRequest
    {
        public string CustomerName { get; set; }
    }
}
