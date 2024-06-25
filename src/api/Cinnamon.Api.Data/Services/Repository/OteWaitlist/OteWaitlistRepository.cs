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
    public async Task<AppResult<IEnumerable<OteWaitlistDTO>>> GetWaitlistByProvider(int providerId)
    {
        try
        {
            Expression<Func<Entities.OteWaitlist, bool>> filter = a => (a.ProviderId == providerId);

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
    public Task<AppResult<OteWaitlistDTO>> UpdateOteWaitlist(OteWaitlistDTO oteWaitlistDTO)
    {
        throw new NotImplementedException();
    }

    public Task<AppResult<IEnumerable<OteWaitlistDTO>>> GetAllAsync(int? count, int? skip)
    {
        throw new NotImplementedException();
    }

    public Task<AppResult<IEnumerable<OteWaitlistDTO>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

   
}
