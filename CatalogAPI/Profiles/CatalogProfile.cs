using AutoMapper;
using CatalogAPI.Data;
using CatalogAPI.Results;

namespace CatalogAPI.Profiles
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile() 
        {
            CreateMap<Category, CategoryResult>().ReverseMap();
            CreateMap<Product, ProductResult>().ReverseMap();
        }
    }
}
