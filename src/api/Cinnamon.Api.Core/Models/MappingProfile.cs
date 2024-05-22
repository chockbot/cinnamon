using AutoMapper;
using DataDto = Cinnamon.Framework.ApiCommand.ApiData.DTO;
using CoreDto = Cinnamon.Framework.ApiCommand.ApiCore.DTO;
using ActivityResults = Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using DashboardResults = Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using TransactionResults = Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using DirectStudentResult = Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;

namespace Cinnamon.Api.Core.Models;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<DataDto.Activity.OteActivityDTO, ActivityResults.OteFindByHandlerResult>()
            .ForMember(d => d.Schedule, o => o.MapFrom(s => s.OteSchedule))
            .ForMember(d => d.OnlineEvent, o => o.MapFrom(s => s.OteOnlineEvent));
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
        CreateMap<TransactionResults.OtePurchaseOrderDetailsResult.Ticket, CoreDto.PurchaseOrder.OtePurchaseOrderDTO.PurchaseTicket>();

        CreateMap<DataDto.Activity.OteOngoingDTO, ActivityResults.CustomerOteResult.CustomerOte>();
        CreateMap<ActivityResults.CustomerOteResult.CustomerOte, CoreDto.Activity.CustomerOteDTO>();

        // for activity feed mappings
        CreateMap<DataDto.Activity.ActivityFeedDTO, ActivityResults.ActivityFeedResult.ActivityFeed>();
        CreateMap<DataDto.Activity.ActivityFeedDTO.Location, ActivityResults.ActivityFeedResult.Location>();
        CreateMap<DataDto.Activity.ActivityFeedDTO.Summary, ActivityResults.ActivityFeedResult.Summary>();
        CreateMap<ActivityResults.ActivityFeedResult.ActivityFeed, CoreDto.Activity.ActivityFeedDTO>();
        CreateMap<ActivityResults.ActivityFeedResult.Location, CoreDto.Activity.ActivityFeedDTO.Location>();
        CreateMap<ActivityResults.ActivityFeedResult.Summary, CoreDto.Activity.ActivityFeedDTO.Summary>();

        // for activity feed popular activity mappings
        CreateMap<DataDto.Activity.ActivityFeedDTO, ActivityResults.PopularActivitiesResult.ActivityFeed>();
        CreateMap<DataDto.Activity.ActivityFeedDTO.Location, ActivityResults.PopularActivitiesResult.Location>();
        CreateMap<DataDto.Activity.ActivityFeedDTO.Summary, ActivityResults.PopularActivitiesResult.Summary>();
        CreateMap<ActivityResults.PopularActivitiesResult.ActivityFeed, CoreDto.Activity.ActivityFeedDTO>();
        CreateMap<ActivityResults.PopularActivitiesResult.Location, CoreDto.Activity.ActivityFeedDTO.Location>();
        CreateMap<ActivityResults.PopularActivitiesResult.Summary, CoreDto.Activity.ActivityFeedDTO.Summary>();

        // for ote already booked dates
        CreateMap<DataDto.Activity.OteAlreadyBookDate, ActivityResults.OteAlreadyBookedResult.OteAlreadyBooked>();
        CreateMap<ActivityResults.OteAlreadyBookedResult.OteAlreadyBooked, CoreDto.Activity.OteAlreadyBookedDTO>();

        // fote ote schedule dates
        CreateMap<DataDto.OteDate.OteDateDTO, ActivityResults.OteScheduleDatesResult.OteDateSchedule>();
        CreateMap<ActivityResults.OteScheduleDatesResult.OteDateSchedule, CoreDto.Activity.OteScheduleDateDTO>();
        CreateMap<DataDto.OteDate.OteDateDTO, ActivityResults.OteTicketBookedCountResult.FirstScheduleDate>();
        CreateMap<ActivityResults.OteTicketBookedCountResult.FirstScheduleDate, CoreDto.Activity.OteTicketBookCountDTO.BookFirstScheduleDTO>();
        CreateMap<ActivityResults.OteTicketBookedCountResult, CoreDto.Activity.OteTicketBookCountDTO>()
            .ForMember(d => d.FirstSchedule, o => o.MapFrom(s => s.FirstOteDate));


        //for direct students
        CreateMap<DataDto.DirectStudent.DirectStudentInfoDTO, DashboardResults.CreateDirectStudentsResult.CreateDirectStudentInfo>();
        CreateMap<DataDto.DirectStudent.DirectStudentSessionDTO, DashboardResults.CreateDirectStudentsResult.CreateDirectStudentSession>();
        CreateMap<DataDto.DirectStudent.DirectStudentPaymentDTO, DashboardResults.CreateDirectStudentsResult.CreateDirectStudentPayment>();
        CreateMap<DataDto.DirectStudent.DirectStudentDTO, DashboardResults.CreateDirectStudentsResult.CreateDirectStudent>()
            .ForMember(d => d.CreateDirectStudentInfo, o => o.MapFrom(o => o.DirectStudentInfo))
            .ForMember(d => d.CreateDirectStudentSession, o => o.MapFrom(o => o.DirectStudentSession))
            .ForMember(d => d.CreateDirectStudentPayment, o => o.MapFrom(o => o.DirectStudentPayment));
        CreateMap<DashboardResults.CreateDirectStudentsResult.CreateDirectStudentInfo, CoreDto.DirectStudents.DirectStudentInfoDTO>();
        CreateMap<DashboardResults.CreateDirectStudentsResult.CreateDirectStudentSession, CoreDto.DirectStudents.DirectStudentSessionDTO>();
        CreateMap<DashboardResults.CreateDirectStudentsResult.CreateDirectStudentPayment, CoreDto.DirectStudents.DirectStudentPaymentDTO>();
        CreateMap<DashboardResults.CreateDirectStudentsResult.CreateDirectStudent, CoreDto.DirectStudents.DirectStudentsDTO>()
            .ForMember(d => d.DirectStudentInfo, o => o.MapFrom(o => o.CreateDirectStudentInfo))
            .ForMember(d => d.DirectStudentSession, o =>  o.MapFrom(o => o.CreateDirectStudentSession))
            .ForMember(d => d.DirectStudentPayment, o => o.MapFrom(o => o.CreateDirectStudentPayment));

        CreateMap<DataDto.DirectStudent.DirectStudentInfoDTO, DirectStudentResult.DirectStudentsInfoResult.DirectStudentInfo>();
        CreateMap<DirectStudentResult.DirectStudentsInfoResult.DirectStudentInfo, CoreDto.DirectStudents.DirectStudentInfoDTO>();
    }
}