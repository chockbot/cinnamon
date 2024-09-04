using AutoMapper;
using CoreDto = Cinnamon.Framework.ApiCommand.ApiCore.DTO;
using Entities = Cinnamon.Web.Models.Entities;
using OteEntities = Cinnamon.Web.Models.Ote;

namespace Cinnamon.Web.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CoreDto.Activity.OteActivityDTO, OteEntities.OteActivity>()
            .ForMember(d => d.Schedule, o => o.MapFrom(s => s.OteSchedule));
        CreateMap<CoreDto.Activity.OtePricingDTO, OteEntities.OtePricing>();
        CreateMap<CoreDto.Activity.OteDateDTO, OteEntities.OteDate>();
        CreateMap<CoreDto.Activity.OteScheduleDTO, OteEntities.OteSchedule>();
        CreateMap<CoreDto.Activity.ActivityDTO.ActivityImage, Entities.ActivityImage>();

        CreateMap<CoreDto.Customer.CustomerDTO, Entities.CustomerProfile>()
            .ForMember(d => d.ProfilePath, o => o.MapFrom(s => s.ProfileImg));
        
        CreateMap<CoreDto.Customer.ProfileDTO, Entities.CustomerProfile>();

        // activity feed mappings
        CreateMap<CoreDto.Activity.ActivityFeedDTO, Entities.ActivityFeed>();
        CreateMap<CoreDto.Activity.ActivityFeedDTO.Location, Entities.ActivityFeed.Location>();
        CreateMap<CoreDto.Activity.ActivityFeedDTO.Summary, Entities.ActivityFeed.Summary>();
    }
}