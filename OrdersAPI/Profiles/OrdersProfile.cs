using AutoMapper;
using OrdersAPI.Data;
using OrdersAPI.Results;

namespace OrdersAPI.Profiles
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
