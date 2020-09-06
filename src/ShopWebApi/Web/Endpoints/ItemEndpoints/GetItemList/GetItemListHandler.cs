using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Web.Endpoints
{
    public class GetItemListHandler : BaseAsyncEndpoint<GetItemListRequest, GetItemListResponse>
    {
        private readonly IAsyncRepository<ShopItem> _itemRepository;
        private readonly IMapper _mapper;

        public GetItemListHandler(IAsyncRepository<ShopItem> itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        [HttpGet("api/items")]
        [SwaggerOperation(
            Summary = "Get List Items paged",
            Description = "Get List Items paged with sort by price",
            OperationId = "items.GetItemList",
            Tags = new[] { "ItemEndpoints" })
        ]
        public override async Task<ActionResult<GetItemListResponse>> HandleAsync([FromQuery]GetItemListRequest request, CancellationToken cancellationToken = default)
        {
            var response = new GetItemListResponse(request.CorrelationId());

            var filterSpec = GetFilterSpecification(request);
            var pagedSpec = GetFilterPaginatedSpecification(request);

            var totalItems = await _itemRepository.CountAsync(filterSpec);
            var items = await _itemRepository.ListAsync(pagedSpec);

            response.Items.AddRange(items.Select(_mapper.Map<ItemDto>));

            response.PageCount = int.Parse(Math.Ceiling((decimal)totalItems / request.ItemsPerPage).ToString());

            return Ok(response);
        }

        private ItemFilterSpecification GetFilterSpecification(GetItemListRequest request)
            =>
                request.PriceSpecification ? new ItemFilterSpecification(request.PriceFrom, request.PriceTo) : new ItemFilterSpecification();

        private ItemFilterPaginatedSpecification GetFilterPaginatedSpecification(GetItemListRequest request)
            =>
                request.PriceSpecification ? new ItemFilterPaginatedSpecification(
                    request.ItemsPerPage * (request.PageIndex - 1),
                    request.ItemsPerPage,
                    request.PriceFrom,
                    request.PriceTo) :
                new ItemFilterPaginatedSpecification(
                    request.ItemsPerPage * (request.PageIndex - 1),
                    request.ItemsPerPage);
    }
}
