using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class CreateItemRequest : MessageRequest
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string PictureUri { get; set; }
    }
}
