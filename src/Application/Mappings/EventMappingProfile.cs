using Application.Dtos.Event;
using AutoMapper;

namespace Application.Mappings
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<Domain.Models.Event.Event, EventDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Domain.Models.Event.Event, EventDetailsDto>()
                .ConstructUsing(src => new EventDetailsDto(src.Name, src.Description, src.ImageUrl, src.Category.Name,
                    src.CreatedBy.StageName, src.CreatedById, src.EventDetail.Country, src.EventDetail.City,
                    src.EventDetail.Street, src.EventDetail.HouseNumber, src.EventDetail.PostalCode,
                    src.EventDetail.StartingDate, src.EventDetail.EndingDate, src.EventDetail.Created));

            CreateMap<CreateEventDto, Domain.Models.Event.Event>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForPath(dest => dest.EventDetail.Country, opt => opt.MapFrom(src => src.Country))
                .ForPath(dest => dest.EventDetail.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.EventDetail.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.EventDetail.HouseNumber, opt => opt.MapFrom(src => src.HouseNumber))
                .ForPath(dest => dest.EventDetail.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForPath(dest => dest.EventDetail.StartingDate, opt => opt.MapFrom(src => src.StartingDate))
                .ForPath(dest => dest.EventDetail.EndingDate, opt => opt.MapFrom(src => src.EndingDate));

            CreateMap<EditEventDto, Domain.Models.Event.Event>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForPath(dest => dest.EventDetail.Country, opt => opt.MapFrom(src => src.Country))
                .ForPath(dest => dest.EventDetail.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.EventDetail.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.EventDetail.HouseNumber, opt => opt.MapFrom(src => src.HouseNumber))
                .ForPath(dest => dest.EventDetail.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForPath(dest => dest.EventDetail.StartingDate, opt => opt.MapFrom(src => src.StartingDate))
                .ForPath(dest => dest.EventDetail.EndingDate, opt => opt.MapFrom(src => src.EndingDate));
        }
    }
}
