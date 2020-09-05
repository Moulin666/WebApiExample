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
    public class UpdateItemHandler : BaseAsyncEndpoint<UpdateItemRequest, UpdateItemResponse>
    {
        private readonly IAsyncRepository<ShopItem> _itemRepository;

        public UpdateItemHandler(IAsyncRepository<ShopItem> itemRepository) => _itemRepository = itemRepository;

        [HttpPut("api/items")]
        [SwaggerOperation(
            Summary = "Update item",
            Description = "Update item by Id",
            OperationId = "items.update",
            Tags = new[] { "ItemEndpoints" })]
        public override async Task<ActionResult<UpdateItemResponse>> HandleAsync(UpdateItemRequest request, CancellationToken cancellationToken = default)
        {
            var response = new UpdateItemResponse(request.CorrelationId());

            var existingItem = await _itemRepository.GetByIdAsync(request.Id);
            if (existingItem is null)
                return NotFound();

            existingItem.Update(request.Name, request.Description, request.Price);
            if (!string.IsNullOrEmpty(request.PictureUri))
                existingItem.UpdatePicture(request.PictureUri);

            await _itemRepository.UpdateAsync(existingItem);

            response.Item = new ItemDto
            {
                Id = existingItem.Id,
                Description = existingItem.Description,
                Name = existingItem.Name,
                PictureUri = existingItem.PictureUri,
                Price = existingItem.Price
            };

            return response;
        }
    }
}
