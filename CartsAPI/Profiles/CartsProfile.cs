using AutoMapper;
using CartsAPI.Data;
using CartsAPI.Results;

namespace CartsAPI.Profiles
{
    public class CartsProfile : Profile
    {
        public CartsProfile()
        {
            CreateMap<Cart, CartResult>().ReverseMap();
            CreateMap<Item, ItemResult>().ReverseMap();
        }
    }
}
