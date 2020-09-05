using System.ComponentModel.DataAnnotations;
using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class UpdateItemRequest : MessageRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string PictureUri { get; set; }
    }
}
