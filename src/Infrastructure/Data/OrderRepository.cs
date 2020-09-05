using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Order> GetByIdWithItemsAsync(int id)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include($"{nameof(Order.OrderItems)}.{nameof(OrderItem.OrderedItem)}")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Order>> ListWithItemsAsync(ISpecification<Order> spec)
        {
            var specificationResult = await ApplySpecification(spec);

            return await specificationResult
                .Include(o => o.OrderItems)
                .Include($"{nameof(Order.OrderItems)}.{nameof(OrderItem.OrderedItem)}")
                .ToListAsync();
        }

        public async Task DeleteOrderAsync(Order entity)
        {
            _dbContext.Set<Order>().Remove(entity);
            _dbContext.Set<OrderItem>().RemoveRange(entity.OrderItems);

            await _dbContext.SaveChangesAsync();
        }
    }
}
