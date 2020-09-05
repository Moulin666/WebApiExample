using Ardalis.GuardClauses;

namespace ApplicationCore.Entities
{
    public class ShopItem : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string PictureUri { get; private set; }

        public decimal Price { get; private set; }

        /// <summary>
        /// Create new item.
        /// </summary>
        /// <param name="name">Item name</param>
        /// <param name="description">Description for item</param>
        /// <param name="pictureUri">URI to picture</param>
        /// <param name="price">Price per item</param>
        public ShopItem(string name, string description, string pictureUri, decimal price)
        {
            Name = name;
            Description = description;
            PictureUri = pictureUri;
            Price = price;
        }

        public void Update(string name, string description, decimal price)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.NegativeOrZero(price, nameof(price));

            Name = name;
            Description = description;
            Price = price;
        }

        public void UpdatePicture(string pictureUri)
        {
            Guard.Against.NullOrEmpty(pictureUri, nameof(pictureUri));

            PictureUri = pictureUri;
        }
    }
}
