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
    public class DeleteOrderHandler : BaseAsyncEndpoint<DeleteOrderRequest, DeleteOrderResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

        [HttpDelete("api/orders")]
        [SwaggerOperation(
            Summary = "Delete order",
            Description = "Delete order by Id",
            OperationId = "orders.delete",
            Tags = new[] { "OrderEndpoints" })]
        public override async Task<ActionResult<DeleteOrderResponse>> HandleAsync(DeleteOrderRequest request, CancellationToken cancellationToken = default)
        {
            var response = new DeleteOrderResponse(request.CorrelationId());

            var itemToDelete = await _orderRepository.GetByIdWithItemsAsync(request.OrderId);
            if (itemToDelete is null)
                return NotFound();

            await _orderRepository.DeleteOrderAsync(itemToDelete);

            return Ok(response);
        }
    }
}
