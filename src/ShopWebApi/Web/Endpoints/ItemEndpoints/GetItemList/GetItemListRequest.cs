using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class GetItemListRequest : MessageRequest
    {
        public int ItemsPerPage { get; set; }
        public int PageIndex { get; set; }

        public bool PriceSpecification { get; set; } = false;
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }
}
