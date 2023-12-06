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
        CreateMap<DataDto.Activity.OteActivityDTO, ActivityResults.OteFindByHandlerResult>();
        CreateMap<DataDto.OteSchedule.OteSchedulePricingDTO, ActivityResults.OteFindByHandlerResult.OtePricing>();
        CreateMap<DataDto.ActivityImage.ActivityImageDTO, ActivityResults.OteFindByHandlerResult.Image>();
        CreateMap<DataDto.OteSchedule.OteDateDTO, ActivityResults.OteFindByHandlerResult.OteDate>();

        CreateMap<ActivityResults.OteFindByHandlerResult, CoreDto.Activity.OteActivityDTO>();
        CreateMap<ActivityResults.OteFindByHandlerResult.OtePricing, CoreDto.Activity.OtePricingDTO>();
        CreateMap<ActivityResults.OteFindByHandlerResult.Image, CoreDto.Activity.ActivityDTO.ActivityImage>()
            .ForMember(d => d.ImageSrc, o => o.MapFrom(s => s.ImageLocation))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.ImageName));
        CreateMap<ActivityResults.OteFindByHandlerResult.OteDate, CoreDto.Activity.OteDateDTO>();

        CreateMap<ActivityResults.OteTicketDetailsResult, CoreDto.Activity.OteTicketDetailsDTO>();
        CreateMap<ActivityResults.OteTicketDetailsResult.Ticket, CoreDto.Activity.OteTicketDetailsDTO.TicketDetails>();
        
        CreateMap<TransactionResults.OtePurchaseOrderDetailsResult, CoreDto.PurchaseOrder.OtePurchaseOrderDTO>();
        CreateMap<TransactionResults.OtePurchaseOrderDetailsResult.Ticket, CoreDto.PurchaseOrder.OtePurchaseOrderDTO.PurchaseTicket>();

        CreateMap<DataDto.Activity.OteOngoingDTO, ActivityResults.CustomerOteResult.CustomerOte>();
        CreateMap<ActivityResults.CustomerOteResult.CustomerOte, CoreDto.Activity.CustomerOteDTO>();
    }
}