using ApplicationCore.Entities;

namespace UnitTests.Builders
{
    public class ItemBuilder
    {
        public int TestItemId => 10;

        public string TestItemName => "Test Item Name";
        public string TestDescription => "Test Item Description 123 123 123";
        public string TestPictureUri => "https://test.com/testImage.png";

        public decimal TestUnitPrice = 100;

        private ShopItem _item { get; set; }

        public ItemBuilder() => _item = GetItemWithDefaultValues();

        public ShopItem Build() => _item;

        public ShopItem GetItemWithDefaultValues()
        {
            _item = new ShopItem(TestItemName, TestDescription, TestPictureUri, TestUnitPrice);
            return _item;
        }

        public ShopItem GetCustomItem(ShopItem item)
        {
            _item = item;
            return _item;
        }
    }
}
