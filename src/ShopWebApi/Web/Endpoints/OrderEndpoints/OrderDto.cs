using System;
using System.Collections.Generic;
using ApplicationCore.Entities;
using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace Web.Endpoints
{
    [AutoMap(typeof(Order))]
    public class OrderDto
    {
        [SourceMember("CustomerId")]
        public string CustomerUsername { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public string Comment { get; set; } = string.Empty;

        public List<ItemDto> Items { get; set; } = new List<ItemDto>();

        public decimal TotalPrice { get; set; }
    }
}
