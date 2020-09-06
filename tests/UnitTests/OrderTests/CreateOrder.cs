using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using UnitTests.Builders;
using Web.Endpoints;
using Xunit;

namespace UnitTests.OrderTests
{
    public class CreateOrder
    {
        private readonly AppDbContext _dbContext;
        private readonly IOrderRepository _orderRepository;
        private readonly IAsyncRepository<ShopItem> _itemRepository;

        private readonly IMapper _mapper;

        public CreateOrder()
        {
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
            _itemRepository = new Repository<ShopItem>(_dbContext);
        }

        [Fact]
        public async Task ReturnNewOrderDtoIfOrderCreatedSuccessfully()
        {
            var item = new ItemBuilder().Build();
            _dbContext.ShopItems.Add(item);

            await _dbContext.SaveChangesAsync();

            _dbContext.Entry(item).GetDatabaseValues();

            var request = new CreateOrderRequest()
            {
                CustomerUsername = "TestUsername",
                Comment = "TestComment",
                Items = new List<CreateOrderRequest.OrderedItems>
                {
                    new CreateOrderRequest.OrderedItems
                    {
                        ItemId = item.Id,
                        Units = 3
                    }
                }
            };

            var handler = new CreateOrderHandler(_itemRepository, _orderRepository);
            var result = await handler.HandleAsync(request, CancellationToken.None);

            var expectedOrderDto = new OrderDto
            {
                CustomerUsername = request.CustomerUsername,
                Comment = request.Comment,
                OrderDate = DateTimeOffset.Now,
                TotalPrice = item.Price * 3
            };
            expectedOrderDto.Items.Add(_mapper.Map<ItemDto>(item));

            Assert.Equal(expectedOrderDto.CustomerUsername, result.Value.Order.CustomerUsername);
            Assert.Equal(expectedOrderDto.Comment, result.Value.Order.Comment);
            Assert.Equal(expectedOrderDto.TotalPrice, result.Value.Order.TotalPrice);
            Assert.NotNull(result.Value.Order.Items);
        }
    }
}
