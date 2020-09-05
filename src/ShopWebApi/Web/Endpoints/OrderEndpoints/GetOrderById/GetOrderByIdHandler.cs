using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Web.Endpoints
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GetOrderByIdHandler : BaseAsyncEndpoint<GetOrderByIdRequest, GetOrderByIdResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

        [HttpGet("api/orders/{OrderId}")]
        [SwaggerOperation(
            Summary = "Get Order by Id",
            Description = "Get Order by Id",
            OperationId = "orders.GetByOrderId",
            Tags = new[] { "OrderEndpoints" })
        ]
        public override async Task<ActionResult<GetOrderByIdResponse>> HandleAsync([FromRoute] GetOrderByIdRequest request, CancellationToken cancellationToken = default)
        {
            var response = new GetOrderByIdResponse(request.CorrelationId());

            var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId);
            if (order is null)
                return NotFound();

            response.Order = new OrderDto
            {
                CustomerUsername = order.CustomerId,
                Comment = order.Comment,
                OrderDate = order.OrderDate,
                TotalPrice = order.GetTotalPrice()
            };

            foreach (var item in order.OrderItems)
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
