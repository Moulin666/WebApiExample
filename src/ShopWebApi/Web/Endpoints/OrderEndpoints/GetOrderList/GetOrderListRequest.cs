using System;
using ApplicationCore.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Web.Endpoints
{
    public class GetOrderListRequest : MessageRequest
    {
        public int OrdersPerPage { get; set; }
        public int PageIndex { get; set; }

        public DateSpecificationEnum DateSpecification { get; set; } = DateSpecificationEnum.WithoutAnySpecifications;
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DateSpecificationEnum
    {
        WithoutAnySpecifications,

        OnlyFromDate,

        FromAndToDate
    }
}
