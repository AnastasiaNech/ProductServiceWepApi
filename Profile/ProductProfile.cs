using AutoMapper;
using ProductServiceWepApi.Dtos;
using ProductServiceWepApi.Models;

namespace UserMinimalApi.Profiles;

public class ProductsProfile : Profile
{
    public ProductsProfile()
    {
        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductCreateDto, Product>()
            .ForMember(dest => dest.ID,
            opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<ProductUpdateDto, Product>();
    }
}
