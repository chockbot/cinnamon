using AutoMapper;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivityImage;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteTicket;
using OteTicketAlias = Cinnamon.Framework.ApiCommand.ApiData.OteTicket;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.TokenGenerated;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using DisbursementAlias = Cinnamon.Framework.ApiCommand.ApiData.DTO.Disbursement;
using DisbursementReqAlias = Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;

namespace Cinnamon.Api.Data.Models;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<Activity, OteActivityDTO>()
            .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.ExperienceCategoryId))
            .ForMember(d => d.BarangayCode, o => o.MapFrom(s => s.Address.Barangay))
            .ForMember(d => d.BarangayName, o => o.MapFrom(s => s.Address.BarangayName))
            .ForMember(d => d.CityNumber, o => o.MapFrom(s => s.Address.City))
            .ForMember(d => d.CityName, o => o.MapFrom(s => s.Address.CityName))
            .ForMember(d => d.RegionCode, o => o.MapFrom(s => s.Address.Region))
            .ForMember(d => d.RegionName, o => o.MapFrom(s => s.Address.RegionName))
            .ForMember(d => d.HouseNo, o => o.MapFrom(s => s.Address.Address1))
            .ForMember(d => d.PostalCode, o => o.MapFrom(s => s.Address.PostalCode))
            .ForMember(d => d.PinnedLocation, o => o.MapFrom(s => s.Address.PinnedLocation))
            .ForMember(d => d.ScheduleFrom, o => o.MapFrom(s => s.OteSchedule.From))
            .ForMember(d => d.ScheduleTo, o => o.MapFrom(s => s.OteSchedule.To))
            .ForMember(d => d.Recurrences, o => o.MapFrom(s => s.OteSchedule.Recurrences))
            .ForMember(d => d.Pricings, o => o.MapFrom(s => s.OteSchedule.OteSchedulePricing))
            .ForMember(d => d.ProviderId, o => o.MapFrom(s => s.CreatedBy))
            .ForMember(d => d.Owner, o => o.MapFrom(s => s.Customer))
            .ForMember(d => d.EventName, o => o.MapFrom(s => s.Title))
            .ForMember(d => d.OteDates, o => o.MapFrom(s => s.OteSchedule.OteDates));
        
        CreateMap<OteSchedulePricing, OteSchedulePricingDTO>();
        CreateMap<OteSchedulePricingDTO, OteSchedulePricing>();
        CreateMap<OteSchedule, OteScheduleDTO>();
        CreateMap<OteSchedulePricingGroup, OtePricingGroupDTO>();
        CreateMap<ActivityImage, ActivityImageDTO>();
        CreateMap<Customer, CustomerDTO>();
        CreateMap<OteDate, OteScheduleDateDTO>();

        CreateMap<OteTicket, OteTicketDTO>();
        CreateMap<OteTicketDTO, OteTicket>();
        CreateMap<OteTicketAlias.Request.CreateOteTicketArgs, OteTicketDTO>();

        CreateMap<TokenGenerated, TokenGeneratedDTO>();

        // disbursement mapping
        CreateMap<DisbursementAlias.DisbursementDTO, Disbursement>();
        CreateMap<DisbursementAlias.DisbursementDetailDTO, DisbursementDetail>();
        CreateMap<Disbursement, DisbursementAlias.DisbursementDTO>();
        CreateMap<DisbursementDetail, DisbursementAlias.DisbursementDetailDTO>();
        CreateMap<DisbursementReqAlias.CreateDisbursementArgs.DisbursementArgs, DisbursementAlias.DisbursementDTO>();
        CreateMap<DisbursementReqAlias.CreateDisbursementArgs.DisbursementDetailArgs, DisbursementAlias.DisbursementDetailDTO>();

        // disbursement bulk mapping
        CreateMap<DisbursementAlias.DisbursementBulkDTO, DisbursementBulk>();
        CreateMap<DisbursementAlias.DisbursementDetailBulkDTO, DisbursementDetailBulk>();
        CreateMap<DisbursementBulk, DisbursementAlias.DisbursementBulkDTO>();
        CreateMap<DisbursementDetailBulk, DisbursementAlias.DisbursementDetailBulkDTO>();
        CreateMap<DisbursementReqAlias.CreateDisbursementBulkArgs.DisbursementBulkArgs, DisbursementAlias.DisbursementBulkDTO>();
        CreateMap<DisbursementReqAlias.CreateDisbursementBulkArgs.DisbursementDetailBulkArgs, DisbursementAlias.DisbursementDetailBulkDTO>();

        // disbursement bulk log mapping
        CreateMap<DisbursementAlias.DisbursementBulkLogDTO, DisbursementBulkLog>();
        CreateMap<DisbursementBulkLog, DisbursementAlias.DisbursementBulkLogDTO>();
        CreateMap<DisbursementReqAlias.CreateDisbursementBulkLogArgs.DisbursementBulkLogArgs, DisbursementAlias.DisbursementBulkLogDTO>();
        CreateMap<DisbursementAlias.DisbursementManualDTO, DisbursementManual>();
        CreateMap<DisbursementManual, DisbursementAlias.DisbursementManualDTO>();
        CreateMap<DisbursementReqAlias.CreateManualDisbursementArgs, DisbursementAlias.DisbursementManualDTO>();
    }
}