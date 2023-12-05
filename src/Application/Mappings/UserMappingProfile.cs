using Application.Dtos.Search;
using Application.Dtos.User;
using AutoMapper;
using Domain.Models.User;

namespace Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDetailDto>()
                .ConstructUsing(src => new UserDetailDto(src.Id, src.Email!, src.StageName, src.Description, src.Address.Country,
                    src.Address.City, src.Address.Street,
                    src.Address.HouseNumber, src.Address.PostalCode, src.CategoryId, src.Avatar, src.BackgroundImage));

            CreateMap<EditUserDetailsDto, User>()
                .ForPath(dest => dest.Address.Country, opt => opt.MapFrom(src => src.Country))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.HouseNumber, opt => opt.MapFrom(src => src.HouseNumber))
                .ForPath(dest => dest.Address.PostalCode, opt => opt.MapFrom(src => src.PostalCode));

            CreateMap<User, SearchItemDto>()
                .ConstructUsing(src => new SearchItemDto(src.Id, src.StageName!, src.Description!, null, $"/User/{src.Id}"));
        }
    }
}
