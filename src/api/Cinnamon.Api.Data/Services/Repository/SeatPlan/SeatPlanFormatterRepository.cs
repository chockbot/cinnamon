using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.SeatPlan;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.SeatPlan;

public class SeatPlanFormatterRepository : ISeatPlanFormatterRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public SeatPlanFormatterRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<IEnumerable<SeatPlanFormatterDTO>>> GetSeatPlanFormatterAsync()
    {
        try
        {
            var seatPlanFormatter = await dataStore.SeatPlanFormatter.GetAllAsync();
            if(!seatPlanFormatter.Succeeded || seatPlanFormatter.Result is null)
            {
                return AppResult<IEnumerable<SeatPlanFormatterDTO>>.CreateFailed(
                    new ApplicationException(seatPlanFormatter.Message), seatPlanFormatter.Message);
            }

            var seatPlanFormatterDTO = mapper.Map<IEnumerable<SeatPlanFormatterDTO>>(seatPlanFormatter);
            return AppResult<IEnumerable<SeatPlanFormatterDTO>>.CreateSucceeded(seatPlanFormatterDTO, "Seat plan formatter retrieved successfully");   
        }
        catch (System.Exception ex)
        {           
            return AppResult<IEnumerable<SeatPlanFormatterDTO>>.CreateFailed(ex, "An error occured in getting seat plan formatter");
        }
    }   
}