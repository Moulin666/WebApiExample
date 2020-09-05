using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Web.Endpoints
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CreateOrderHandler : BaseAsyncEndpoint<CreateOrderRequest, CreateOrderResponse>
    {
        private readonly IAsyncRepository<ShopItem> _itemRepository;
        private readonly IAsyncRepository<Order> _orderRepository;

        public CreateOrderHandler(IAsyncRepository<ShopItem> itemRepository, IAsyncRepository<Order> orderRepository)
        {
            _itemRepository = itemRepository;
            _orderRepository = orderRepository;
        }

        [HttpPost("api/orders")]
        [SwaggerOperation(
            Summary = "Create new order",
            Description = "Create new order",
            OperationId = "orders.create",
            Tags = new[] { "OrderEndpoints" })]
        public override async Task<ActionResult<CreateOrderResponse>> HandleAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
        {
            var response = new CreateOrderResponse(request.CorrelationId());

            if (request.Items.Count <= 0)
                return BadRequest("Items can't less than 1");

            var itemsSpecification = new ItemFilterSpecification(request.Items.Select(i => i.ItemId).ToArray());
            var items = await _itemRepository.ListAsync(itemsSpecification);

            if (items.Count <= 0)
                return BadRequest("Items doesn't exists");

            var orderedItems = items.Select(orderedItem =>
            {
                var item = items.First(c => c.Id == orderedItem.Id);
                var units = request.Items.FirstOrDefault(i => i.ItemId == item.Id).Units;
                var itemOrdered = new ItemOrdered(orderedItem.Id, orderedItem.Name, orderedItem.Description, orderedItem.PictureUri);
                var orderItem = new OrderItem(itemOrdered, item.Price, units);

                return orderItem;
            }).ToList();

            var order = new Order(request.CustomerUsername, orderedItems, request.Comment);
            await _orderRepository.AddAsync(order);

            response.Order = new OrderDto
            {
                CustomerUsername = order.CustomerId,
                Comment = order.Comment,
                OrderDate = order.OrderDate,
                TotalPrice = order.GetTotalPrice()
            };

            foreach(var item in order.OrderItems)
            {
                response.Order.Items.Add(new ItemDto
                {
                    Id = item.Id,
                    Description = item.OrderedItem.Description,
                    Name = item.OrderedItem.Name,
                    PictureUri = item.OrderedItem.PictureUri,
                    Price = item.UnitPrice
                });
            }

            return response;
        }
    }
}
