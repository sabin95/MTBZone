using AutoMapper;
using CartAPI.Data;
using CartAPI.Results;

namespace CartAPI.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart,CartResult>().ReverseMap();
            CreateMap<Item, ItemResult>().ReverseMap();
        }
    }
}
