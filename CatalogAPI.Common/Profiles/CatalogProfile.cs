using AutoMapper;
using CatalogAPI.Common.Data;
using CatalogAPI.Common.Results;

namespace CatalogAPI.Common.Profiles
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
