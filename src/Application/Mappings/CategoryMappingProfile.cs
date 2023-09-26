using Application.Dtos.Category;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
