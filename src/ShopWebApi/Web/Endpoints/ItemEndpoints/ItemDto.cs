namespace Web.Endpoints
{
    public class ItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUri { get; set; }

        public decimal Price { get; set; }
    }
}
