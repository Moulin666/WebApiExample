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

            int totalOrders = 0;
            var orders = new List<Order>().AsReadOnly() as IReadOnlyList<Order>;

            // TODO : Need refactor this IFs statements.
            if (request.DateSpecification == DateSpecificationEnum.FromAndToDate)
            {
                if (request.DateFrom > request.DateTo)
                    return BadRequest("Date from can't be less than Date to");

                var filterSpec = new OrderFilterSpecification(request.DateFrom, request.DateTo);
                totalOrders = await _orderRepository.CountAsync(filterSpec);

                var pagedSpec = new OrderFilterPaginatedSpecification(
                    request.OrdersPerPage * (request.PageIndex - 1),
                    request.OrdersPerPage,
                    request.DateFrom,
                    request.DateTo);

                orders = await _orderRepository.ListWithItemsAsync(pagedSpec);
            }
            else if (request.DateSpecification == DateSpecificationEnum.OnlyFromDate)
            {
                if (request.DateFrom > DateTimeOffset.Now)
                    return BadRequest("Date from can't be less than date now");

                var filterSpec = new OrderFilterSpecification(request.DateFrom);
                totalOrders = await _orderRepository.CountAsync(filterSpec);

                var pagedSpec = new OrderFilterPaginatedSpecification(
                    request.OrdersPerPage * (request.PageIndex - 1),
                    request.OrdersPerPage,
                    request.DateFrom);

                orders = await _orderRepository.ListWithItemsAsync(pagedSpec);
            }
            else
            {
                var filterSpec = new OrderFilterSpecification();
                totalOrders = await _orderRepository.CountAsync(filterSpec);

                var pagedSpec = new OrderFilterPaginatedSpecification(
                    request.OrdersPerPage * (request.PageIndex - 1),
                    request.OrdersPerPage);

                orders = await _orderRepository.ListWithItemsAsync(pagedSpec);
            }

            response.Orders.AddRange(orders.Select(_mapper.Map<OrderDto>));
            foreach(var orderResponse in response.Orders)
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
    }
}
