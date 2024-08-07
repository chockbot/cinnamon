using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class TopBookedCustomersHandler : ITopBookedCustomersHandler
{
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IOteTicketData oteTicketData;

    public TopBookedCustomersHandler(IGetActivityHandler getActivityHandler, 
        IOteFindByHandler oteFindByHandler, IOteTicketData oteTicketData)
    {
        this.getActivityHandler = getActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.oteTicketData = oteTicketData;
    }

    public AppResult<TopBookedCustomersResult> Execute(TopBookedCustomersArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<TopBookedCustomersResult>> ExecuteAsync(TopBookedCustomersArgs args)
    {
        try
        {
            var activityRes = await getActivityHandler.ExecuteAsync(new GetActivityArgs {
                ActivityId = args.ActivityId
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<TopBookedCustomersResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }

            var oteRes = await oteFindByHandler.ExecuteAsync(new OteFindByHandlerArgs {
                Handler = activityRes.Result.Handler,
                IncludePricing = true,
                IncludeSchedule = true
            });
            if(!oteRes.Succeeded || oteRes.Result is null)
            {
                return AppResult<TopBookedCustomersResult>.CreateFailed(new ApplicationException(oteRes.Message), oteRes.Message);
            }

            var selectedSched = oteRes.Result.OteDates.FirstOrDefault(d => d.Date.Date == args.BookedDate.Date);
            if(selectedSched is null)
            {
                return AppResult<TopBookedCustomersResult>.CreateFailed(
                    new ApplicationException("Unable to determine selected booked date."), "Unable to determine selected booked date.");
            }

            var totalBookedRes = await oteTicketData.BookedCustomers(new Framework.ApiCommand.ApiData.OteTicket.Request.BookedCustomersArgs {
                ActivityId = args.ActivityId,
                DateId = selectedSched.Id,
            });
            if(!totalBookedRes.Succeeded || totalBookedRes.Result is null || !totalBookedRes.Result.IsSuccess)
            {
                return AppResult<TopBookedCustomersResult>.CreateFailed(
                    new ApplicationException("Unable to determine total booked count."), "Unable to determine total booked count.");
            }
            int totalBooked = totalBookedRes.Result.Result.Count();

            var topBookedRes = await oteTicketData.BookedCustomers(new Framework.ApiCommand.ApiData.OteTicket.Request.BookedCustomersArgs {
                ActivityId = args.ActivityId,
                DateId = selectedSched.Id,
                Limit = 10,
            });
            if(!topBookedRes.Succeeded || topBookedRes.Result is null || !topBookedRes.Result.IsSuccess)
            {
                return AppResult<TopBookedCustomersResult>.CreateFailed(
                    new ApplicationException("Unable to get top booked customers."), "Unable to get top booked customers.");
            }

            return AppResult<TopBookedCustomersResult>.CreateSucceeded(new TopBookedCustomersResult {
                TopBooked = topBookedRes.Result.Result.Select(b => new TopBookedCustomersResult.BookedCustomer {
                    Email = b.Email,
                    FirstName = b.FirstName,
                    LastName = b.LastName,
                    ProfileImage = b.ProfileImage
                }),
                TotalBooked = totalBooked,
                TotalParticipants = 9
            }, "Successfully get top booked customers.");
        }
        catch (Exception ex)
        {
            return AppResult<TopBookedCustomersResult>.CreateFailed(ex, "An error occured in TopBookedCustomersHandler.");
        }
    }
}