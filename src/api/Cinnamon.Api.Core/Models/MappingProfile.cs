using AutoMapper;
using DataDto = Cinnamon.Framework.ApiCommand.ApiData.DTO;
using CoreDto = Cinnamon.Framework.ApiCommand.ApiCore.DTO;
using ActivityResults = Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

namespace Cinnamon.Api.Core.Models;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<DataDto.Activity.OteActivityDTO, ActivityResults.OteFindByHandlerResult>();
        CreateMap<DataDto.OteSchedule.OteSchedulePricingDTO, ActivityResults.OteFindByHandlerResult.OtePricing>();
        CreateMap<DataDto.ActivityImage.ActivityImageDTO, ActivityResults.OteFindByHandlerResult.Image>();

        CreateMap<ActivityResults.OteFindByHandlerResult, CoreDto.Activity.OteActivityDTO>();
        CreateMap<ActivityResults.OteFindByHandlerResult.OtePricing, CoreDto.Activity.OtePricingDTO>();
        CreateMap<ActivityResults.OteFindByHandlerResult.Image, CoreDto.Activity.ActivityDTO.ActivityImage>()
            .ForMember(d => d.ImageSrc, o => o.MapFrom(s => s.ImageLocation))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.ImageName));

        CreateMap<ActivityResults.OteTicketDetailsResult, CoreDto.Activity.OteTicketDetailsDTO>();
        CreateMap<ActivityResults.OteTicketDetailsResult.Ticket, CoreDto.Activity.OteTicketDetailsDTO.Ticket>();
    }
}