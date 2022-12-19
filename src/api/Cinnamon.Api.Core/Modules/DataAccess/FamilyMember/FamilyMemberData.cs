using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.FamilyMember.Request;
using Cinnamon.Framework.ApiCommand.ApiData.FamilyMember.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.FamilyMember;

public class FamilyMemberData : IFamilyMemberData
{
    private readonly IFlurlClient flurlClient;

    public FamilyMemberData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }
    
    public async Task<AppResult<CreateFamilyMemberResult>> CreateFamilyMember(CreateFamilyMemberArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("FamilyMember/CreateFamilyMember")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateFamilyMemberResult>();

            return AppResult<CreateFamilyMemberResult>.CreateSucceeded(result, "Successfully posting create family member api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateFamilyMemberResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateFamilyMemberResult>.CreateFailed(ex, "An error occured when posting create family member api");
        }
    }

    public async Task<AppResult<CreateManyFamilyMemberResult>> CreateManyFamilyMember(CreateManyFamilyMemberArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("FamilyMember/CreateManyFamilyMember")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateManyFamilyMemberResult>();

            return AppResult<CreateManyFamilyMemberResult>.CreateSucceeded(result, "Successfully posting create many family member api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateManyFamilyMemberResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateManyFamilyMemberResult>.CreateFailed(ex, "An error occured when posting create many family member api");
        }
    }

    public async Task<AppResult<GetAllFamilyMemberResult>> GetAllFamilyMembers(GetAllFamilyMemberArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("FamilyMember/GetAllFamilyMembers")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllFamilyMemberResult>();

            return AppResult<GetAllFamilyMemberResult>.CreateSucceeded(result, "Successfully getting get all family member api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllFamilyMemberResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllFamilyMemberResult>.CreateFailed(ex, "An error occured when getting all family member api");
        }
    }

    public async Task<AppResult<GetFamilyMemberResult>> GetFamilyMemberById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"FamilyMember/GetFamilyMemberById/{id}")
                            .GetJsonAsync<GetFamilyMemberResult>();
            
            return AppResult<GetFamilyMemberResult>.CreateSucceeded(result, "Successfully getting family member by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetFamilyMemberResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetFamilyMemberResult>.CreateFailed(ex, "An error occured when getting family member by id api");
        }
    }

    public async Task<AppResult<GetFamilyMemberByCustomerIdResult>> GetFamilyMemberByCustomerId(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"FamilyMember/GetFamilyMemberByCustomerId/{id}")
                            .GetJsonAsync<GetFamilyMemberByCustomerIdResult>();
            
            return AppResult<GetFamilyMemberByCustomerIdResult>.CreateSucceeded(result, "Successfully getting family member by customer id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetFamilyMemberByCustomerIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetFamilyMemberByCustomerIdResult>.CreateFailed(ex, "An error occured when getting family member by customer id api");
        }
    }

    public async Task<AppResult<UpdateFamilyMemberResult>> UpdateFamilyMember(UpdateFamilyMemberArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("FamilyMember/UpdateFamilyMember")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateFamilyMemberResult>();
            
            return AppResult<UpdateFamilyMemberResult>.CreateSucceeded(result, "Successfully posting update family member api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateFamilyMemberResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateFamilyMemberResult>.CreateFailed(ex, "An error occured when posting update family member api");
        }
    }

    public async Task<AppResult<UpdateManyFamilyMembersResult>> UpdateManyFamilyMember(UpdateManyFamilyMemberArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("FamilyMember/UpdateManyFamilyMember")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateManyFamilyMembersResult>();
            
            return AppResult<UpdateManyFamilyMembersResult>.CreateSucceeded(result, "Successfully posting update many family member api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateManyFamilyMembersResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateManyFamilyMembersResult>.CreateFailed(ex, "An error occured when posting update many family member api");
        }
    }

    public async Task<AppResult<DeleteManyFamilyMembersResult>> DeleteManyFamilyMembers(DeleteManyFamilyMembersArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("FamilyMember/DeleteManyFamilyMember")
                            .PostJsonAsync(args)
                            .ReceiveJson<DeleteManyFamilyMembersResult>();
            
            return AppResult<DeleteManyFamilyMembersResult>.CreateSucceeded(result, "Successfully posting delete many family member api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<DeleteManyFamilyMembersResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DeleteManyFamilyMembersResult>.CreateFailed(ex, "An error occured when posting delete many family member api");
        }
    }
}