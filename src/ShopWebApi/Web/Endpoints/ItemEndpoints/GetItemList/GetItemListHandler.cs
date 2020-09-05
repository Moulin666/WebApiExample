using System;
using System.Collections.Generic;
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

            int totalItems = 0;
            var items = new List<ShopItem>().AsReadOnly() as IReadOnlyList<ShopItem>;

            if (request.PriceSpecification)
            {
                if (request.PriceFrom > request.PriceTo)
                    return BadRequest("Price from can't be less than Price to");

                var filterSpec = new ItemFilterSpecification(request.PriceFrom, request.PriceTo);
                totalItems = await _itemRepository.CountAsync(filterSpec);

                var pagedSpec = new ItemFilterPaginatedSpecification(
                    request.ItemsPerPage * (request.PageIndex - 1),
                    request.ItemsPerPage,
                    request.PriceFrom,
                    request.PriceTo);

                items = await _itemRepository.ListAsync(pagedSpec);
            }
            else
            {
                var filterSpec = new ItemFilterSpecification();
                totalItems = await _itemRepository.CountAsync(filterSpec);

                var pagedSpec = new ItemFilterPaginatedSpecification(
                    request.ItemsPerPage * (request.PageIndex - 1),
                    request.ItemsPerPage);

                items = await _itemRepository.ListAsync(pagedSpec);
            }

            response.Items.AddRange(items.Select(_mapper.Map<ItemDto>));

            response.PageCount = int.Parse(Math.Ceiling((decimal)totalItems / request.ItemsPerPage).ToString());

            return Ok(response);
        }
    }
}
