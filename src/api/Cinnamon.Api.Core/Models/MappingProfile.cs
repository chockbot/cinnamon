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

        CreateMap<ActivityResults.OteFindByHandlerResult, CoreDto.Activity.OteActivityDTO>();
        CreateMap<ActivityResults.OteFindByHandlerResult.OtePricing, CoreDto.Activity.OtePricingDTO>();
    }
}