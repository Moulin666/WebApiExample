using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Web.Endpoints
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GetOrdersByCustomerIdHandler : BaseAsyncEndpoint<GetOrdersByCustomerIdRequest, GetOrdersByCustomerIdResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersByCustomerIdHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

        [HttpGet("api/orders/customer/{CustomerName}")]
        [SwaggerOperation(
            Summary = "Get Order by Customer name",
            Description = "Get Order by Customer name",
            OperationId = "orders.GetByCustomerName",
            Tags = new[] { "OrderEndpoints" })
        ]
        public override async Task<ActionResult<GetOrdersByCustomerIdResponse>> HandleAsync([FromRoute] GetOrdersByCustomerIdRequest request, CancellationToken cancellationToken = default)
        {
            var response = new GetOrdersByCustomerIdResponse(request.CorrelationId());

            var filterSpec = new OrderFilterSpecification(request.CustomerName);
            var orders = await _orderRepository.ListAsync(filterSpec);
            if (orders.Count == 0)
                return NotFound();

            response.Orders = new List<OrderDto>();
            foreach (var order in orders)
            {
                var orderItems = new List<ItemDto>();
                foreach(var item in order.OrderItems)
                {
                    orderItems.Add(new ItemDto
                    {
                        Id = item.Id,
                        Description = item.OrderedItem.Description,
                        Name = item.OrderedItem.Name,
                        PictureUri = item.OrderedItem.PictureUri,
                        Price = item.UnitPrice
                    });
                }

                response.Orders.Add(new OrderDto
                {
                    CustomerUsername = order.CustomerId,
                    Comment = order.Comment,
                    OrderDate = order.OrderDate,
                    Items = orderItems,
                    TotalPrice = order.GetTotalPrice()
                });
            }

            return response;
        }
    }
}
