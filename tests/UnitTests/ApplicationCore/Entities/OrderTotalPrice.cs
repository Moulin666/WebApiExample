using System.Collections.Generic;
using ApplicationCore.Entities;
using UnitTests.Builders;
using Xunit;

namespace UnitTests.ApplicationCore.Entities
{
    public class OrderTotalPrice
    {
        private decimal _testPricePerUnit = 150;

        [Fact]
        public void IsZeroForOrderWithNoItems()
        {
            var order = new OrderBuilder().GetOrderWithNoItems();

            Assert.Equal(0, order.GetTotalPrice());
        }

        [Fact]
        public void IsCorrectWithOneUnit()
        {
            var builder = new OrderBuilder();
            var items = new List<OrderItem>
            {
                new OrderItem(builder.TestItemOrdered, _testPricePerUnit, 1)
            };

            var order = new OrderBuilder().GetOrderWithCustomItems(items);

            Assert.Equal(_testPricePerUnit, order.GetTotalPrice());
        }

        [Fact]
        public void IsCorrectWithSixUnit()
        {
            var builder = new OrderBuilder();
            var order = builder.GetOrderWithDefaultValues();

            Assert.Equal(builder.TestUnitPrice * builder.TestUnits, order.GetTotalPrice());
        }
    }
}
