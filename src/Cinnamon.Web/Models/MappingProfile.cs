using AutoMapper;
using CoreDto = Cinnamon.Framework.ApiCommand.ApiCore.DTO;
using Entities = Cinnamon.Web.Models.Entities;
using OteEntities = Cinnamon.Web.Models.Ote;

namespace Cinnamon.Web.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CoreDto.Activity.OteActivityDTO, OteEntities.OteActivity>();
        CreateMap<CoreDto.Activity.OtePricingDTO, OteEntities.OtePricing>();
        CreateMap<CoreDto.Activity.ActivityDTO.ActivityImage, Entities.ActivityImage>();

        CreateMap<CoreDto.Customer.CustomerDTO, Entities.CustomerProfile>()
            .ForMember(d => d.ProfilePath, o => o.MapFrom(s => s.ProfileImg));
    }
}