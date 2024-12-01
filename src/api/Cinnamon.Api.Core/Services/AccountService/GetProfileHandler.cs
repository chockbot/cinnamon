using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GetProfileHandler : IGetProfileHandler
{
    private readonly ICustomerData customerData;
    private readonly IHttpContextAccessor httpContext;

    public GetProfileHandler(ICustomerData customerData, IHttpContextAccessor httpContext)
    {
        this.customerData = customerData;
        this.httpContext = httpContext;
    }

    public AppResult<GetProfileResult> Execute(GetProfileArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetProfileResult>.CreateFailed(ex, "An error occured in GetProfileHandler");
        }
    }

    public async Task<AppResult<GetProfileResult>> ExecuteAsync(GetProfileArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            int id = customerId != null ? Convert.ToInt32(customerId) : 0;

            var result = await customerData.GetCustomerById(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetProfileResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetProfileResult>.CreateFailed(new ApplicationException("Can't find customer"), "Can't find customer");
            }
            var profile = result.Result.Result;

            return AppResult<GetProfileResult>.CreateSucceeded(new GetProfileResult {
                About            = profile.About,
                Birthdate        = profile.Birthdate,
                PhoneNumber      = profile.PhoneNumber,
                Email            = profile.Email,
                FirstName        = profile.FirstName,
                Id               = profile.Id,
                IsMaker          = profile.IsMaker,
                LastName         = profile.LastName,
                DateJoined       = profile.DateJoined,
                ProfileImagePath = profile.ProfileImg,
                IsVerified       = profile.IsVerified,
                IsVerifiedDate   = profile.IsVerifiedObtainedDate,
                IsOG             = profile.IsOG,
                IsOGDate         = profile.IsOGObtainedDate,
                IsOfficial       = profile.IsOfficial,
                IsOfficialDate   = profile.IsOfficialObtainedDate,
                Handler          = profile.Handler,
                TotalCredits     = profile.TotalCredits,
                ConnectionId     = profile.ConnectionId,
                IsGuest          = profile.IsGuest,
            }, "Successfully get profile");
        }
        catch (Exception ex)
        {
            return AppResult<GetProfileResult>.CreateFailed(ex, "An error occured in GetProfileHandler");
        }
    }
}