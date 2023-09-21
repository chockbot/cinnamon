using AutoMapper;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivityImage;

namespace Cinnamon.Api.Data.Models;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<Activity, OteActivityDTO>()
            .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.ExperienceCategoryId))
            .ForMember(d => d.BarangayCode, o => o.MapFrom(s => s.Address.Region))
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
            .ForMember(d => d.EventName, o => o.MapFrom(s => s.Title));
        
        CreateMap<OteSchedulePricing, OteSchedulePricingDTO>();
        CreateMap<ActivityImage, ActivityImageDTO>();
    }
}