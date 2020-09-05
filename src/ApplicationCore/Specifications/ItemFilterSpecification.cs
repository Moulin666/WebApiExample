using System.Linq;
using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public class ItemFilterSpecification : Specification<ShopItem>
    {
        public ItemFilterSpecification() : base()
           => Query.OrderBy(i => i.Id);

        public ItemFilterSpecification(params int[] ids) : base()
            => Query.Where(i => ids.Contains(i.Id));

        public ItemFilterSpecification(decimal? priceFrom, decimal? priceTo) : base()
            => Query.Where(i => i.Price >= priceFrom && i.Price <= priceTo);
    }
}
