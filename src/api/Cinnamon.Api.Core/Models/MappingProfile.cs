using AutoMapper;
using DataDto = Cinnamon.Framework.ApiCommand.ApiData.DTO;
using CoreDto = Cinnamon.Framework.ApiCommand.ApiCore.DTO;
using ActivityResults = Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using TransactionResults = Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;

namespace Cinnamon.Api.Core.Models;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<DataDto.Activity.OteActivityDTO, ActivityResults.OteFindByHandlerResult>()
            .ForMember(d => d.Schedule, o => o.MapFrom(s => s.OteSchedule));
        CreateMap<DataDto.OteSchedule.OteSchedulePricingDTO, ActivityResults.OteFindByHandlerResult.OtePricing>();
        CreateMap<DataDto.ActivityImage.ActivityImageDTO, ActivityResults.OteFindByHandlerResult.Image>();
        CreateMap<DataDto.OteSchedule.OteScheduleDateDTO, ActivityResults.OteFindByHandlerResult.OteDate>();
        CreateMap<DataDto.OteSchedule.OteScheduleDTO, ActivityResults.OteFindByHandlerResult.OteSchedule>();
        CreateMap<DataDto.OteSchedule.OtePricingGroupDTO, ActivityResults.OteFindByHandlerResult.OtePricingGroupDTO>();
        CreateMap<DataDto.OteSchedule.OteOnlineEventsDTO, ActivityResults.OteFindByHandlerResult.OteOnlineEvent>();

        CreateMap<ActivityResults.OteFindByHandlerResult, CoreDto.Activity.OteActivityDTO>()
            .ForMember(d => d.OteSchedule, o => o.MapFrom(s => s.Schedule));
        CreateMap<ActivityResults.OteFindByHandlerResult.OtePricing, CoreDto.Activity.OtePricingDTO>();
        CreateMap<ActivityResults.OteFindByHandlerResult.Image, CoreDto.Activity.ActivityDTO.ActivityImage>()
            .ForMember(d => d.ImageSrc, o => o.MapFrom(s => s.ImageLocation))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.ImageName));
        CreateMap<ActivityResults.OteFindByHandlerResult.OteDate, CoreDto.Activity.OteDateDTO>();
        CreateMap<ActivityResults.OteFindByHandlerResult.OteSchedule, CoreDto.Activity.OteScheduleDTO>();
        CreateMap<ActivityResults.OteFindByHandlerResult.OtePricingGroupDTO, CoreDto.Activity.OtePricingGroupDTO>();
        CreateMap<ActivityResults.OteFindByHandlerResult.OteOnlineEvent, CoreDto.Activity.OteOnlineEventDTO>();

        CreateMap<ActivityResults.OteTicketDetailsResult, CoreDto.Activity.OteTicketDetailsDTO>();
        CreateMap<ActivityResults.OteTicketDetailsResult.Ticket, CoreDto.Activity.OteTicketDetailsDTO.TicketDetails>();
        
        CreateMap<TransactionResults.OtePurchaseOrderDetailsResult, CoreDto.PurchaseOrder.OtePurchaseOrderDTO>();
        CreateMap<TransactionResults.OtePurchaseOrderDetailsResult.Ticket, CoreDto.PurchaseOrder.OtePurchaseOrderDTO.Ticket>();

        CreateMap<DataDto.Activity.OteOngoingDTO, ActivityResults.CustomerOteResult.CustomerOte>();
        CreateMap<ActivityResults.CustomerOteResult.CustomerOte, CoreDto.Activity.CustomerOteDTO>();
    }
}