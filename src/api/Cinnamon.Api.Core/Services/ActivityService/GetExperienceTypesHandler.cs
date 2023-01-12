using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetExperienceTypesHandler : IGetExperienceTypesHandler
{
    private readonly IExperienceTypeData experienceTypeData;

    public GetExperienceTypesHandler(IExperienceTypeData experienceTypeData)
    {
        this.experienceTypeData = experienceTypeData;
    }

    public AppResult<GetExperienceTypesResult> Execute(GetExperienceTypesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetExperienceTypesResult>.CreateFailed(ex, "An error occured in GetExperienceTypesHandler");
        }
    }

    public async Task<AppResult<GetExperienceTypesResult>> ExecuteAsync(GetExperienceTypesArgs args)
    {
        try
        {
            var result = await experienceTypeData.GetAllExperienceType();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetExperienceTypesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetExperienceTypesResult>.CreateFailed(new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetExperienceTypesHandler");
            }
            var exTypes = result.Result.Result;

            return AppResult<GetExperienceTypesResult>.CreateSucceeded(new GetExperienceTypesResult {
                ExperienceTypes = exTypes.Select(e => {
                    return new GetExperienceTypesResult.ExperienceType {
                        Id = e.Id,
                        Name = e.Name
                    };
                })
            }, "Successfully get experience types");
        }
        catch (Exception ex)
        {
            return AppResult<GetExperienceTypesResult>.CreateFailed(ex, "An error occured in GetExperienceTypesHandler");
        }
    }
}