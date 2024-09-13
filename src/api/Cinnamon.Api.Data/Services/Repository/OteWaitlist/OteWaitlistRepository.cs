using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteWaitlist;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;
using System;

namespace Cinnamon.Api.Data.Services.Repository.OteWaitlist;
public class OteWaitlistRepository : IOteWaitlistRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;
    public OteWaitlistRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<OteWaitlistDTO>> GetWaitList(int id)
    {
        try
        {
            var result = await dataStore.OteWaitlist.GetByIdAsync(id);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<OteWaitlistDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<OteWaitlistDTO>(result.Result);
            return AppResult<OteWaitlistDTO>.CreateSucceeded(dto, "Successfully get watlist by id.");
        }
        catch (Exception ex)
        {
            return AppResult<OteWaitlistDTO>.CreateFailed(ex, "An error occured when getting waitlist by id.");
        }
    }

    public async Task<AppResult<OteWaitlistDTO>> CreateOteWaitlist(OteWaitlistDTO oteWaitlistDTO)
    {
        try
        {
            var oteWaitlist = mapper.Map<Entities.OteWaitlist>(oteWaitlistDTO);
            var result = await dataStore.OteWaitlist.Add(oteWaitlist);
            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<OteWaitlistDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var dtoWaitlist = mapper.Map<OteWaitlistDTO>(result.Result);
            return AppResult<OteWaitlistDTO>.CreateSucceeded(dtoWaitlist, "Waitlist successfully created");
        }
        catch (Exception ex)
        {
            return AppResult<OteWaitlistDTO>.CreateFailed(ex, "An error occured when creating ote waitlist");
        }
    }
    public async Task<AppResult<IEnumerable<OteWaitlistDTO>>> GetWaitlistByProvider(int? providerId, int? activityId, 
        IEnumerable<int>? status = null, int? oteDateId = null)
    {
        try
        {
            Expression<Func<Entities.OteWaitlist, bool>> filter = 
                a => (providerId.HasValue ? a.ProviderId == providerId.Value: true)&&
                     (activityId.HasValue ? a.ActivityId == activityId.Value: true) &&
                     (status != null ? status.Contains(a.Status) : true) &&
                     (oteDateId.HasValue ? a.OteDateId == oteDateId.Value : true);

            var result = await dataStore.OteWaitlist.FindAsync(filter);

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<OteWaitlistDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
            var dtoWaitlist = mapper.Map<IEnumerable<OteWaitlistDTO>>(result.Result);
            return AppResult<IEnumerable<OteWaitlistDTO>>.CreateSucceeded(dtoWaitlist, "Successfully get waitlists by provider id");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteWaitlistDTO>>.CreateFailed(ex, "An error occured when getting waitlists by provider id.");
        }
    }
    public async Task<AppResult<OteWaitlistDTO>> UpdateOteWaitlist(OteWaitlistDTO oteWaitlistDTO)
    {
        try
        {
            var waitlistRes = await dataStore.OteWaitlist.GetByIdAsync(oteWaitlistDTO.Id);
            if (!waitlistRes.Succeeded || waitlistRes.Result == null)
            {
                return AppResult<OteWaitlistDTO>.CreateFailed(waitlistRes.Error.Exception, waitlistRes.Message);
            }
            var waitlist = waitlistRes.Result;
            waitlist.Status = oteWaitlistDTO.Status;

            var updatedWaitlistRes = await dataStore.OteWaitlist.Update(waitlist);
            if (!updatedWaitlistRes.Succeeded || updatedWaitlistRes.Result is null)
            {
                return AppResult<OteWaitlistDTO>.CreateFailed(new ApplicationException(updatedWaitlistRes.Message), updatedWaitlistRes.Message);
            }
            var dtoWaitlist = mapper.Map<OteWaitlistDTO>(updatedWaitlistRes.Result);
            return AppResult<OteWaitlistDTO>.CreateSucceeded(dtoWaitlist, "Waitlist successfully updated");
        }
        catch (Exception ex)
        {
            return AppResult<OteWaitlistDTO>.CreateFailed(ex, "An error occured when updating ote waitlist");
        }
    }

    public Task<AppResult<IEnumerable<OteWaitlistDTO>>> GetAllAsync(int? count, int? skip)
    {
        throw new NotImplementedException();
    }

    public Task<AppResult<IEnumerable<OteWaitlistDTO>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<bool>> DeleteOteWaitlist(int Id)
    {
        try
        {
            var oteWaitlist = await dataStore.OteWaitlist.GetByIdAsync(Id);
            if (oteWaitlist.Result == null)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("No ote waitlist to delete"), "No ote waitlist to delete");
            }
            var result = await dataStore.OteWaitlist.Remove(oteWaitlist.Result);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            return AppResult<bool>.CreateSucceeded(true, "Successfully deleted ote waitlist");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured in deleting ote waitlist");
        }
    }
}
