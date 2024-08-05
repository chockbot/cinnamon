using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Interactors;
using Cinnamon.Api.Core.Services.ChatService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ChatService;

public class RequestMessageHandler : IRequestMessageHandler
{
    private readonly IGetCustomerByIdHandler getCustomerByIdHandler;
    private readonly ITokenGeneratedData tokenGeneratedData;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly ITokenGeneratorProvider tokenGeneratorProvider;

    public RequestMessageHandler(IGetCustomerByIdHandler getCustomerByIdHandler, ITokenGeneratedData tokenGeneratedData,
        IJsonSerializationProvider jsonSerializationProvider, ITokenGeneratorProvider tokenGeneratorProvider)
    {
        this.getCustomerByIdHandler = getCustomerByIdHandler;
        this.tokenGeneratedData = tokenGeneratedData;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.tokenGeneratorProvider = tokenGeneratorProvider;
    }
    
    public AppResult<RequestMessageResult> Execute(RequestMessageArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<RequestMessageResult>> ExecuteAsync(RequestMessageArgs args)
    {
        try
        {
            var customerRes = await getCustomerByIdHandler.ExecuteAsync(new AccountService.Interactors.GetCustomerByIdArgs {
                Id = args.ProviderId
            });
            if(!customerRes.Succeeded || customerRes.Result is null)
            {
                return AppResult<RequestMessageResult>.CreateFailed(new ApplicationException(customerRes.Message), customerRes.Message);
            }
            var customer = customerRes.Result;

            if(!customer.IsMaker)
            {
                return AppResult<RequestMessageResult>.CreateFailed(
                    new ApplicationException("Invalid request. Action not allowed"), "Invalid request. Action not allowed");
            }

            // generate token and guid
            var tokenGenerated = tokenGeneratorProvider.Generator();

            var payload = new {
                ProviderId = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                ImageSrc = customer.ProfileImg
            };
            var serializePayload = jsonSerializationProvider.Serialize(payload);

            var createTokenRes = await tokenGeneratedData.CreateTokenGenerated(new Framework.ApiCommand.ApiData.TokenGenerated.Request.CreateTokenArgs {
                Guid = tokenGenerated.Guid,
                Payload = serializePayload,
                Token = tokenGenerated.Token,
                TokenType = "MESSAGE-REQUEST"
            });
            if(!createTokenRes.Succeeded || createTokenRes.Result is null || !createTokenRes.Result.IsSuccess)
            {
                return AppResult<RequestMessageResult>.CreateFailed(
                    new ApplicationException("An error occured. Please contact support."), "An error occured. Please contact support."); 
            }

            return AppResult<RequestMessageResult>.CreateSucceeded(new RequestMessageResult {
                Guid = tokenGenerated.Guid,
                Token = tokenGenerated.Token
            }, "Successfully requested a message.");
        }
        catch (Exception ex)
        {
            return AppResult<RequestMessageResult>.CreateFailed(ex, "An error occured in RequestMessageHandler.");
        }
    }
}