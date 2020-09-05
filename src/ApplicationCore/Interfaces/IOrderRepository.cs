using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Interfaces
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<Order> GetByIdWithItemsAsync(int id);

        Task<IReadOnlyList<Order>> ListWithItemsAsync(ISpecification<Order> spec);

        Task DeleteOrderAsync(Order entity);
    }
}
