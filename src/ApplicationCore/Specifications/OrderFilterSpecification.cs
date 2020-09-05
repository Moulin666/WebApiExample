using System;
using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public class OrderFilterSpecification : Specification<Order>
    {
        public OrderFilterSpecification() : base()
            => Query.OrderByDescending(o => o.OrderDate);

        public OrderFilterSpecification(string customerId) : base()
        {
            Query.Include(o => o.OrderItems);
            Query.Include($"{nameof(Order.OrderItems)}.{nameof(OrderItem.OrderedItem)}");

            Query.Where(o => o.CustomerId == customerId).OrderByDescending(i => i.OrderDate);
        }

        public OrderFilterSpecification(DateTimeOffset? dateTimeFrom) : base()
            => Query.Where(i => i.OrderDate >= dateTimeFrom);

        public OrderFilterSpecification(DateTimeOffset? dateTimeFrom, DateTimeOffset? dateTimeTo) : base()
            => Query.Where(i => i.OrderDate >= dateTimeFrom && i.OrderDate <= dateTimeTo);
    }
}
