using Application.Dtos.Product;
using AutoMapper;

namespace Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Domain.Models.Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CreatedBy.StageName));

            CreateMap<Domain.Models.Product, ProductDetailDto>()
                .ConstructUsing(src => new ProductDetailDto(src.Name, src.Description, src.Price, src.IsForSell,
                    src.Quantity, src.ImageUrl, src.CreatedBy.StageName, src.CreatedById, src.Category.Name));

            CreateMap<CreateProductDto, Domain.Models.Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<EditProductDto, Domain.Models.Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());
        }
    }
}
