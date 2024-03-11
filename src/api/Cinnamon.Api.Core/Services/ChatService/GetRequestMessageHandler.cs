using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Interactors;
using Cinnamon.Api.Core.Services.ChatService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ChatService;

public class GetRequestMessageHandler : IGetRequestMessageHandler
{
    private readonly ITokenGeneratedData tokenGeneratedData;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    
    public GetRequestMessageHandler(ITokenGeneratedData tokenGeneratedData, IJsonSerializationProvider jsonSerializationProvider)
    {
        this.tokenGeneratedData = tokenGeneratedData;
        this.jsonSerializationProvider = jsonSerializationProvider;
    }

    public AppResult<GetRequestMessageResult> Execute(GetRequestMessageArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<GetRequestMessageResult>> ExecuteAsync(GetRequestMessageArgs args)
    {
        try
        {
            var getTokenRes = await tokenGeneratedData.GetTokenGenerated(args.Guid, args.Token);
            if(!getTokenRes.Succeeded || getTokenRes.Result is null || !getTokenRes.Result.IsSuccess)
            {
                return AppResult<GetRequestMessageResult>.CreateFailed(new ApplicationException("Invalid request. Action not allowed."), "Invalid request. Action not allowed.");
            }

            var generatedToken = getTokenRes.Result.Result;
            if(!generatedToken.TokenType.Equals("MESSAGE-REQUEST", StringComparison.CurrentCultureIgnoreCase))
            {
                return AppResult<GetRequestMessageResult>.CreateFailed(new ApplicationException("Invalid request. Action not allowed."), "Invalid request. Action not allowed.");
            }

            var deserializedPayload = jsonSerializationProvider.Deserialize<TokenPayload>(generatedToken.Payload);
            if(deserializedPayload is null)
            {
                throw new ApplicationException("An error occured. Please contact support.");
            }

            return AppResult<GetRequestMessageResult>.CreateSucceeded(new GetRequestMessageResult {
                FirstName = deserializedPayload.FirstName,
                ImageSrc = deserializedPayload.ImageSrc,
                LastName = deserializedPayload.LastName,
                ProviderId = deserializedPayload.ProviderId
            }, "Message requested successfully provided.");
        }
        catch (Exception ex)
        {
            return AppResult<GetRequestMessageResult>.CreateFailed(ex, "An error occured in GetRequestMessageHandler.");
        }
    }

    record TokenPayload 
    {
        public int ProviderId {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string ImageSrc {get; set;}
    }
}