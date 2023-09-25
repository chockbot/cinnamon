using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService
{
    public class GetExperienceCreationTypeHandler : IGetExperienceCreationTypeHandler
    {
        private readonly IExperienceCreationTypeData experienceCreationTypeData;
        public GetExperienceCreationTypeHandler(IExperienceCreationTypeData experienceCreationTypeData)
        {
            this.experienceCreationTypeData = experienceCreationTypeData;
        }
        public AppResult<GetExperienceCreationTypeResult> Execute(GetExperienceCreationTypeArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetExperienceCreationTypeResult>.CreateFailed(ex, "An error occured in GetExperienceCreationTypeHandler");
            }
        }

        public async Task<AppResult<GetExperienceCreationTypeResult>> ExecuteAsync(GetExperienceCreationTypeArgs args)
        {
            try
            {
                var result = await experienceCreationTypeData.GetExperienceCreationTypes(new Framework.ApiCommand.ApiData.ExperienceCreationType.Request.GetExperienceCreationTypeArgs
                {

                });

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<GetExperienceCreationTypeResult>.CreateFailed(new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetExperienceCreationTypeHandler");
                }

                var creationTypes = result.Result.Result;

                return AppResult<GetExperienceCreationTypeResult>.CreateSucceeded(new GetExperienceCreationTypeResult
                {
                    ExperienceCreationTypes = creationTypes.Select(c => new GetExperienceCreationTypeResult.ExperienceCreationType
                    {
                        Id = c.Id,
                        Description = c.Description,
                        ImagePath = c.ImagePath,
                        IsActive = c.IsActive,
                        Name = c.Name
                    })
                }, "Successfully get experience creation types");
            }
            catch (Exception ex)
            {
                return AppResult<GetExperienceCreationTypeResult>.CreateFailed(ex, "An error occured in GetExperienceCreationTypeHandler");
            }
        }
    }
}
