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
    public class CreateItemHandler : BaseAsyncEndpoint<CreateItemRequest, CreateItemResponse>
    {
        private readonly IAsyncRepository<ShopItem> _itemRepository;

        public CreateItemHandler(IAsyncRepository<ShopItem> itemRepository) => _itemRepository = itemRepository;

        [HttpPost("api/items")]
        [SwaggerOperation(
            Summary = "Create new item",
            Description = "Create new item",
            OperationId = "items.create",
            Tags = new[] { "ItemEndpoints" })]
        public override async Task<ActionResult<CreateItemResponse>> HandleAsync(CreateItemRequest request, CancellationToken cancellationToken = default)
        {
            var response = new CreateItemResponse(request.CorrelationId());

            var newItem = new ShopItem(request.Name, request.Description, request.PictureUri, request.Price);
            newItem = await _itemRepository.AddAsync(newItem);

            response.Item = new ItemDto
            {
                Id = newItem.Id,
                Description = newItem.Description,
                Name = newItem.Name,
                PictureUri = newItem.PictureUri,
                Price = newItem.Price
            };

            return response;
        }
    }
}
