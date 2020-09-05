using Ardalis.GuardClauses;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Shapshot
    /// </summary>
    public class ItemOrdered
    {
        public int ItemId { get; set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string PictureUri { get; private set; }

        public ItemOrdered(int itemId, string name, string description, string pictureUri)
        {
            Guard.Against.OutOfRange(itemId, nameof(itemId), 1, int.MaxValue);
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrEmpty(pictureUri, nameof(pictureUri));
            Guard.Against.NullOrEmpty(description, nameof(description));

            ItemId = itemId;
            Name = name;
            Description = description;
            PictureUri = pictureUri;
        }

        private ItemOrdered() { }
    }
}
