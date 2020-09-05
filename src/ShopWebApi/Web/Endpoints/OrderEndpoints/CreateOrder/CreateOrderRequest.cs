using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class CreateOrderRequest : MessageRequest
    {
        [Required]
        public string CustomerUsername { get; set; }

        [Required]
        public List<OrderedItems> Items { get; set; }

        public string Comment { get; set; } = string.Empty;

        public struct OrderedItems
        {
            public int ItemId { get; set; }
            public int Units { get; set; }
        }
    }
}
