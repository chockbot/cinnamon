using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceType.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceType.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.ExperienceType;

public class ExperienceTypeData: IExperienceTypeData
{
    private readonly IFlurlClient flurlClient;
	public ExperienceTypeData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateExperienceTypeResult>> CreateExperienceType(string name)
    {
        try
        {
            var result = await flurlClient
                .Request("ExperienceType/CreateExperienceType")
                .PostJsonAsync(name)
                .ReceiveJson<CreateExperienceTypeResult>();

            return AppResult<CreateExperienceTypeResult>.CreateSucceeded(result, "Successfully posting create experience type api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateExperienceTypeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateExperienceTypeResult>.CreateFailed(ex, "An error occured when posting create experience type api");
        }
    }

    public async Task<AppResult<GetAllExperienceTypeResult>> GetAllExperienceType()
    {
        try
        {
            var result = await flurlClient
                            .Request("ExperienceType/GetAllExperienceType")
                            .GetJsonAsync<GetAllExperienceTypeResult>();

            return AppResult<GetAllExperienceTypeResult>.CreateSucceeded(result, "Successfully getting get all experience type api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllExperienceTypeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllExperienceTypeResult>.CreateFailed(ex, "An error occured when getting all experience type api");
        }
    }

    public async Task<AppResult<GetExperienceTypeResult>> GetExperienceTypeById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"ExperienceType/GetExperienceType/{id}")
                            .GetJsonAsync<GetExperienceTypeResult>();

            return AppResult<GetExperienceTypeResult>.CreateSucceeded(result, "Successfully getting experience type by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetExperienceTypeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetExperienceTypeResult>.CreateFailed(ex, "An error occured when getting experience type by id api");
        }
    }

    public async Task<AppResult<UpdateExperienceTypeResult>> UpdateExperienceType(UpdateExperienceTypeArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ExperienceType/UpdateExperienceType")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateExperienceTypeResult>();

            return AppResult<UpdateExperienceTypeResult>.CreateSucceeded(result, "Successfully posting update experience type api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateExperienceTypeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateExperienceTypeResult>.CreateFailed(ex, "An error occured when posting update experience type api");
        }
    }
}
