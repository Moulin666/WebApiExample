using System.Collections.Generic;
using ApplicationCore.Entities;

namespace UnitTests.Builders
{
    public class OrderBuilder
    {
        public ItemOrdered TestItemOrdered { get; private set; }
        private Order _order { get; set; }

        public int TestItemId => 10;

        public string TestItemName => "Test Item Name";
        public string TestDescription => "Test Item Description 123 123 123";
        public string TestPictureUri => "https://test.com/testImage.png";

        public decimal TestUnitPrice = 100;
        public int TestUnits = 5;

        public string TestCustomerId => "TestCustomer";
        public string TestComment => "Test comment for order";

        public OrderBuilder()
        {
            TestItemOrdered = new ItemOrdered(TestItemId, TestItemName, TestDescription, TestPictureUri);
            _order = GetOrderWithDefaultValues();
        }

        public Order Build() => _order;

        public Order GetOrderWithDefaultValues()
        {
            var orderItem = new OrderItem(TestItemOrdered, TestUnitPrice, TestUnits);
            var itemList = new List<OrderItem>() { orderItem };

            _order = new Order(TestCustomerId, itemList, TestComment);
            return _order;
        }

        public Order GetOrderWithNoItems()
        {
            _order = new Order(TestCustomerId, new List<OrderItem>(), TestComment);
            return _order;
        }

        public Order GetOrderWithCustomItems(List<OrderItem> items)
        {
            _order = new Order(TestCustomerId, items, TestComment);
            return _order;
        }
    }
}
