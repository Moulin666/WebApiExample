using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using UnitTests.Builders;
using Web;
using Web.Endpoints;
using Xunit;

namespace UnitTests.OrderTests
{
    public class GetOrders
    {
        private readonly AppDbContext _dbContext;
        private readonly IOrderRepository _orderRepository;

        private readonly IMapper _mapper;

        public GetOrders()
        {
            var order = new OrderBuilder().Build();

            _mapper = new MapperConfiguration(c =>
            {
                c.CreateMap<ShopItem, ItemDto>();
                c.CreateMap<Order, OrderDto>()
                    .ForMember(dto => dto.CustomerUsername, options => options.MapFrom(src => src.CustomerId));
            }).CreateMapper();

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new AppDbContext(dbOptions);
            _orderRepository = new OrderRepository(_dbContext);

            _dbContext.Orders.Add(order);
            _dbContext.OrderItems.AddRange(order.OrderItems);

            _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task NotReturnNullIfOrdersArePresent()
        {
            var request = new GetOrderListRequest()
            {
                OrdersPerPage = 10,
                PageIndex = 1
            };

            var handler = new GetOrderListHandler(_orderRepository, _mapper);

            var result = await handler.HandleAsync(request, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
