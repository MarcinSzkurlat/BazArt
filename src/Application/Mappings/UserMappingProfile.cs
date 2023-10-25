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
                .ConstructUsing(src => new UserDetailDto(src.Email!, src.StageName, src.Description, src.Address.Country,
                    src.Address.City, src.Address.Street,
                    src.Address.HouseNumber, src.Address.PostalCode));
        }
    }
}
