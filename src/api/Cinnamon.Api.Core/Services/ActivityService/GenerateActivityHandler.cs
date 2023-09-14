using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GenerateActivityHandler : IGenerateActivityHandler
{
    private readonly IActivityData activityData;

    public GenerateActivityHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<GenerateActivityHandlerResult> Execute(GenerateActivityHandlerArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GenerateActivityHandlerResult>.CreateFailed(ex, "An error occured in GenerateActivityHandler");
        }
    }

    public async Task<AppResult<GenerateActivityHandlerResult>> ExecuteAsync(GenerateActivityHandlerArgs args)
    {
        try
        {
            // remove special characters for creating handler name
            char[] separators = new char[]{';',',','\r','\t','\n','`','~','!','@','#','$','%','^','&','*',
                '(',')','-','_','+','=','\'','{','}','[',']','|','\\',':','?','/','<','>','.'};
            var removedCharacters = args.ActivityName.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var handlerName = string.Join("-",string.Join("",removedCharacters.Where(s => !string.IsNullOrEmpty(s))).Split(" ").Where(s => !string.IsNullOrEmpty(s))).ToLower();

            var queryActivitiesLikeHandlerName = await activityData.GetAllActivities(new Cinnamon.Framework.ApiCommand.ApiData.Activity.Request.GetAllActivities {
                LikeHandler = handlerName
            });
            if(!queryActivitiesLikeHandlerName.Succeeded || queryActivitiesLikeHandlerName.Result == null || !queryActivitiesLikeHandlerName.Result.IsSuccess)
            {
                return AppResult<GenerateActivityHandlerResult>.CreateFailed(
                    new ApplicationException(queryActivitiesLikeHandlerName.Result?.ErrorInfo?.Message), queryActivitiesLikeHandlerName.Message);
            }
            var activitiesHandlers = queryActivitiesLikeHandlerName.Result.Result.Select(a => a.Handler).OrderBy(h => h).ToList();
            if(activitiesHandlers.Count > 0)
            {
                for(int i = activitiesHandlers.Count - 1; i >= 0; i--)
                {
                    var activityHandler = activitiesHandlers[i];
                    var splittedHandler = activityHandler.Split("-").ToList();

                    if(splittedHandler.Count > 0)
                    {
                        var lastIdentifier = splittedHandler.Last();

                        // concatenated string without last identifier eg: chess-chess
                        var concatHandler = string.Join("-",splittedHandler.Take(splittedHandler.Count -1)).ToLower();

                        if(activityHandler == handlerName)
                        {
                            if(int.TryParse(lastIdentifier, out int iResult))
                            {
                                handlerName = $"{concatHandler}-{iResult + 1}";
                                break;
                            }
                            else 
                            {
                                handlerName = $"{handlerName}-1";
                                break;
                            }
                        }

                        if(int.TryParse(lastIdentifier, out int intResult))
                        {
                            if(concatHandler == handlerName)
                            {
                                handlerName = $"{handlerName}-{intResult + 1}";
                                break;
                            }
                        }
                        else 
                        {
                            var concatSplitted = string.Join("-",splittedHandler).ToLower();
                            if(concatSplitted == handlerName)
                            {
                                handlerName = $"{handlerName}-1";
                                break;
                            }
                        }
                    }
                }
            }

            return AppResult<GenerateActivityHandlerResult>.CreateSucceeded(
                new GenerateActivityHandlerResult {GeneratedHandler = handlerName}, "Successfuly generate activity handler");
        }
        catch (Exception ex)
        {
            return AppResult<GenerateActivityHandlerResult>.CreateFailed(ex, "An error occured in GenerateActivityHandler");
        }
    }
}