using AutoMapper;
using OrdersAPI.Common.Data;
using OrdersAPI.Common.Results;

namespace OrdersAPI.Common.Profiles
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile() 
        {
            CreateMap<Order, OrderResult>().ReverseMap();
            CreateMap<Item, ItemResult>().ReverseMap();
        }
    }
}
