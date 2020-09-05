using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public class ItemFilterPaginatedSpecification : Specification<ShopItem>
    {
        public ItemFilterPaginatedSpecification(int skip, int take)
           : base()
               => Query.Skip(skip).Take(take);

        public ItemFilterPaginatedSpecification(int skip, int take, decimal? priceFrom, decimal? priceTo)
            : base()
                => Query.Where(i => i.Price >= priceFrom && i.Price <= priceTo).Skip(skip).Take(take);
    }
}
