using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Order : BaseEntity
    {
        public string CustomerId { get; private set; }

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;

        public string Comment { get; set; } = string.Empty;

        private readonly List<OrderItem> _orderItems = new List<OrderItem>();

        public Order() { }

        public Order(string customerId, List<OrderItem> items, string comment)
        {
            CustomerId = customerId;
            _orderItems = items;

            if (!string.IsNullOrEmpty(comment))
                Comment = comment;
        }

        public decimal GetTotalPrice()
        {
            var totalPrice = 0m;

            foreach (var item in _orderItems)
                totalPrice += item.UnitPrice * item.Units;

            return totalPrice;
        }
    }
}
