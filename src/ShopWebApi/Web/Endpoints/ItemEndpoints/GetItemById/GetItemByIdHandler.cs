using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Web.Endpoints
{
    public class GetItemByIdHandler : BaseAsyncEndpoint<GetItemByIdRequest, GetItemByIdResponse>
    {
        private readonly IAsyncRepository<ShopItem> _itemRepository;

        public GetItemByIdHandler(IAsyncRepository<ShopItem> itemRepository) => _itemRepository = itemRepository;

        [HttpGet("api/items/{ItemId}")]
        [SwaggerOperation(
            Summary = "Get Item by Id",
            Description = "Get Item by Id",
            OperationId = "items.GetByItemId",
            Tags = new[] { "ItemEndpoints" })
        ]
        public override async Task<ActionResult<GetItemByIdResponse>> HandleAsync([FromRoute]GetItemByIdRequest request, CancellationToken cancellationToken = default)
        {
            var response = new GetItemByIdResponse(request.CorrelationId());

            var item = await _itemRepository.GetByIdAsync(request.ItemId);
            if (item is null)
                return NotFound();

            response.Item = new ItemDto
            {
                Id = item.Id,
                Description = item.Description,
                Name = item.Name,
                PictureUri = item.PictureUri,
                Price = item.Price
            };
            return Ok(response);
        }
    }
}
