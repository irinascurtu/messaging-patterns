using AutoMapper;
using System.Runtime.CompilerServices;

namespace ProductsApi.Infrastructure
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Data.Entities.Product, Models.ProductModel>();
            CreateMap<Models.ProductModel, Data.Entities.Product>();

            CreateMap<Data.Entities.Product, Models.ProductTrimmedModel>();

            //V2
        }
    }
}
