using ApplicationCore.Entities;
using AutoMapper;
using Web.Endpoints;

namespace Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ShopItem, ItemDto>();
            CreateMap<Order, OrderDto>();
        }
    }
}
