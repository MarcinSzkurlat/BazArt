using Application.Dtos.Product;
using Application.Dtos.Search;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Domain.Models.Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Domain.Models.Product, ProductDetailDto>()
                .ConstructUsing(src => new ProductDetailDto(src.Name, src.Description, src.Price, src.IsForSell,
                    src.Quantity, src.ImageUrl, src.CreatedBy.StageName, src.CreatedBy.Email!, src.CreatedById, src.CategoryId, src.Category.Name, src.Created));

            CreateMap<CreateProductDto, Domain.Models.Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<EditProductDto, Domain.Models.Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());
            
            CreateMap<Product, SearchItemDto>()
                .ConstructUsing(src => new SearchItemDto(src.Id, src.Name, src.Description, src.ImageUrl, $"/Product/{src.Id}"));
        }
    }
}
