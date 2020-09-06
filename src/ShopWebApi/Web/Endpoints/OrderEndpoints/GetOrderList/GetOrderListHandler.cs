using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Web.Endpoints
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GetOrderListHandler : BaseAsyncEndpoint<GetOrderListRequest, GetOrderListResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderListHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet("api/orders")]
        [SwaggerOperation(
            Summary = "Get List Orders paged",
            Description = "Get List Orders paged with sort by date",
            OperationId = "items.GetOrderList",
            Tags = new[] { "OrderEndpoints" })
        ]
        public override async Task<ActionResult<GetOrderListResponse>> HandleAsync([FromQuery] GetOrderListRequest request, CancellationToken cancellationToken = default)
        {
            var response = new GetOrderListResponse(request.CorrelationId());

            var filterSpec = GetFilterSpecification(request);
            var pagedSpec = GetFilterPaginatedSpecification(request);

            var totalOrders = await _orderRepository.CountAsync(filterSpec);
            var orders = await _orderRepository.ListWithItemsAsync(pagedSpec);

            response.Orders.AddRange(orders.Select(_mapper.Map<OrderDto>));
            foreach (var orderResponse in response.Orders)
            {
                var order = orders.FirstOrDefault(o => o.CustomerId == orderResponse.CustomerUsername);

                foreach (var item in order.OrderItems)
                {
                    orderResponse.Items.Add(new ItemDto
                    {
                        Id = item.Id,
                        Description = item.OrderedItem.Description,
                        Name = item.OrderedItem.Name,
                        PictureUri = item.OrderedItem.PictureUri,
                        Price = item.UnitPrice
                    });
                }
            }

            response.PageCount = int.Parse(Math.Ceiling((decimal)totalOrders / request.OrdersPerPage).ToString());

            return Ok(response);
        }

        private OrderFilterSpecification GetFilterSpecification(GetOrderListRequest request)
        {
            if (request.DateSpecification == DateSpecificationEnum.FromAndToDate)
                return new OrderFilterSpecification(request.DateFrom, request.DateTo);
            else if (request.DateSpecification == DateSpecificationEnum.OnlyFromDate)
                return new OrderFilterSpecification(request.DateFrom);
            else
                return new OrderFilterSpecification();
        }

        private OrderFilterPaginatedSpecification GetFilterPaginatedSpecification(GetOrderListRequest request)
        {
            if (request.DateSpecification == DateSpecificationEnum.FromAndToDate)
                return new OrderFilterPaginatedSpecification(request.OrdersPerPage * (request.PageIndex - 1), request.OrdersPerPage, request.DateFrom, request.DateTo);
            else if (request.DateSpecification == DateSpecificationEnum.OnlyFromDate)
                return new OrderFilterPaginatedSpecification(request.OrdersPerPage * (request.PageIndex - 1), request.OrdersPerPage, request.DateFrom);
            else
                return new OrderFilterPaginatedSpecification(request.OrdersPerPage * (request.PageIndex - 1), request.OrdersPerPage);
        }
    }
}