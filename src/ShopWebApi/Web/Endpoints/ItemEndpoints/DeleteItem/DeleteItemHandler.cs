using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Web.Endpoints
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DeleteItemHandler : BaseAsyncEndpoint<DeleteItemRequest, DeleteItemResponse>
    {
        private readonly IAsyncRepository<ShopItem> _itemRepository;

        public DeleteItemHandler(IAsyncRepository<ShopItem> itemRepository) => _itemRepository = itemRepository;

        [HttpDelete("api/items")]
        [SwaggerOperation(
            Summary = "Delete item",
            Description = "Delete item by Id",
            OperationId = "items.delete",
            Tags = new[] { "ItemEndpoints" })]
        public override async Task<ActionResult<DeleteItemResponse>> HandleAsync(DeleteItemRequest request, CancellationToken cancellationToken = default)
        {
            var response = new DeleteItemResponse(request.CorrelationId());

            var itemToDelete = await _itemRepository.GetByIdAsync(request.ItemId);
            if (itemToDelete is null)
                return NotFound();

            await _itemRepository.DeleteAsync(itemToDelete);

            return Ok(response);
        }
    }
}
