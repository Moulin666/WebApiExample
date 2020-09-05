using System;
using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public class OrderFilterPaginatedSpecification : Specification<Order>
    {
        public OrderFilterPaginatedSpecification(int skip, int take)
           : base()
               => Query.Skip(skip).Take(take);

        public OrderFilterPaginatedSpecification(int skip, int take, DateTimeOffset? dateTimeFrom) : base()
            => Query.Where(i => i.OrderDate >= dateTimeFrom).Skip(skip).Take(take);

        public OrderFilterPaginatedSpecification(int skip, int take, DateTimeOffset? dateTimeFrom, DateTimeOffset? dateTimeTo) : base()
            => Query.Where(i => i.OrderDate >= dateTimeFrom && i.OrderDate <= dateTimeTo).Skip(skip).Take(take);
    }
}
