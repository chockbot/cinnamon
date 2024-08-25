using System.Linq.Expressions;
using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.SeatPlan;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Extensions;

namespace Cinnamon.Api.Data.Services.Repository.SeatPlan;

public class SeatPlanTemplateRepository : ISeatPlanTemplateRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public SeatPlanTemplateRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<IEnumerable<SeatPlanTemplateDTO>>> GetSeatPlanTemplateAsync(string templateName, int page, int limit)
    {
        try
        {
            Expression<Func<Entities.SeatPlanTemplate, bool>> filter = 
                t => (!string.IsNullOrEmpty(templateName) ? t.Name.Contains(templateName.Trim()) : true );

            var seatPlanTemplate = await dataStore.SeatPlanTemplate.SeatPlanWithoutPayload(filter, page, limit);
            if(!seatPlanTemplate.Succeeded || seatPlanTemplate.Result is null)
            {
                return AppResult<IEnumerable<SeatPlanTemplateDTO>>.CreateFailed(
                    new ApplicationException(seatPlanTemplate.Message), seatPlanTemplate.Message);
            }

            var seatPlanTemplateDTO = mapper.Map<IEnumerable<SeatPlanTemplateDTO>>(seatPlanTemplate.Result);
            return AppResult<IEnumerable<SeatPlanTemplateDTO>>.CreateSucceeded(seatPlanTemplateDTO, "Seat plan template retrieved successfully");   
        }
        catch (System.Exception ex)
        {           
            return AppResult<IEnumerable<SeatPlanTemplateDTO>>.CreateFailed(ex, "An error occured in getting seat plan template");
        }
    }

    public async Task<AppResult<SeatPlanTemplateDTO>> GetSeatPlanTemplateByIdAsync(int id)
    {
        try
        {
            var seatPlanTemplate = await dataStore.SeatPlanTemplate.GetByIdAsync(id);
            if(!seatPlanTemplate.Succeeded || seatPlanTemplate.Result is null)
            {
                return AppResult<SeatPlanTemplateDTO>.CreateFailed(
                    new ApplicationException(seatPlanTemplate.Message), seatPlanTemplate.Message);
            }

            var seatPlanTemplateDTO = mapper.Map<SeatPlanTemplateDTO>(seatPlanTemplate.Result);
            return AppResult<SeatPlanTemplateDTO>.CreateSucceeded(seatPlanTemplateDTO, "Seat plan template retrieved successfully");   
        }
        catch (System.Exception ex)
        {           
            return AppResult<SeatPlanTemplateDTO>.CreateFailed(ex, "An error occured in getting seat plan template");
        }
    }

    public async Task<AppResult<SeatPlanTemplateDTO>> CreateSeatPlanTemplateAsync(SeatPlanTemplateDTO seatPlanTemplateDTO)
    {
        try
        {
            var seatPlanTemplate = mapper.Map<Entities.SeatPlanTemplate>(seatPlanTemplateDTO);
            seatPlanTemplate.UploadedDate = seatPlanTemplate.UploadedDate.SetKindUtc();
            
            var result = await dataStore.SeatPlanTemplate.Add(seatPlanTemplate);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<SeatPlanTemplateDTO>.CreateFailed(
                    new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<SeatPlanTemplateDTO>(result.Result);
            return AppResult<SeatPlanTemplateDTO>.CreateSucceeded(dto, "Seat plan template created successfully");   
        }
        catch (System.Exception ex)
        {           
            return AppResult<SeatPlanTemplateDTO>.CreateFailed(ex, "An error occured in creating seat plan template");
        }
    }

    public async Task<AppResult<SeatPlanTemplateDTO>> UpdateSeatPlanTemplateAsync(SeatPlanTemplateDTO seatPlanTemplateDTO)
    {
        try
        {
            var getSeatPlanRes = await dataStore.SeatPlanTemplate.GetByIdAsync(seatPlanTemplateDTO.Id);
            if(!getSeatPlanRes.Succeeded || getSeatPlanRes.Result is null)
            {
                return AppResult<SeatPlanTemplateDTO>.CreateFailed(
                    new ApplicationException(getSeatPlanRes.Message), getSeatPlanRes.Message);
            }
            var seatPlan = getSeatPlanRes.Result;

            seatPlan.Name = seatPlanTemplateDTO.Name ?? seatPlan.Name;
            seatPlan.Address = seatPlanTemplateDTO.Address ?? seatPlan.Address;
            seatPlan.ImageSrc = seatPlanTemplateDTO.ImageSrc ?? seatPlan.ImageSrc;
            seatPlan.Payload = seatPlanTemplateDTO.Payload ?? seatPlan.Payload;
            seatPlan.Enabled = seatPlanTemplateDTO.Enabled;

            var result = await dataStore.SeatPlanTemplate.Update(seatPlan);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<SeatPlanTemplateDTO>.CreateFailed(
                    new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<SeatPlanTemplateDTO>(result.Result);
            return AppResult<SeatPlanTemplateDTO>.CreateSucceeded(dto, "Seat plan template updated successfully");   
        }
        catch (System.Exception ex)
        {           
            return AppResult<SeatPlanTemplateDTO>.CreateFailed(ex, "An error occured in updating seat plan template");
        }
    }
}