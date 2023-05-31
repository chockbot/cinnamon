using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GenerateCustomerHandler : IGenerateCustomerHandler
{
    private readonly ICustomerData customerData;

    public GenerateCustomerHandler(ICustomerData customerData)
    {
        this.customerData = customerData;
    }

    public AppResult<GenerateCustomerHandlerResult> Execute(GenerateCustomerHandlerArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GenerateCustomerHandlerResult>.CreateFailed(ex, "An error occured in GenerateCustomerHandler");
        }
    }

    public async Task<AppResult<GenerateCustomerHandlerResult>> ExecuteAsync(GenerateCustomerHandlerArgs args)
    {
        try
        {
            // remove special characters for creating handler name
            char[] separators = new char[]{';',',','\r','\t','\n','`','~','!','@','#','$','%','^','&','*',
                '(',')','-','_','+','=','\'','{','}','[',']','|','\\',':','?','/','<','>'};
            var removedCharacters = $"{args.Handler}".Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var handlerName = string.Join("-",string.Join("",removedCharacters.Where(s => !string.IsNullOrEmpty(s))).Split(" ").Where(s => !string.IsNullOrEmpty(s))).ToLower();

            var queryCustomerHandler = await customerData.GetAllCustomers(new Framework.ApiCommand.ApiData.Customer.Request.GetAllCustomersArgs {
                HandlerLike = handlerName
            });
            if(!queryCustomerHandler.Succeeded || queryCustomerHandler.Result == null || !queryCustomerHandler.Result.IsSuccess)
            {
                return AppResult<GenerateCustomerHandlerResult>.CreateFailed(
                    new ApplicationException(queryCustomerHandler.Result?.ErrorInfo?.Message), queryCustomerHandler.Message);
            }

            var customerHandlers = queryCustomerHandler.Result.Result.Select(c => c.Handler).OrderBy(s => s).ToList();
            if(customerHandlers.Count > 0)
            {
                for(int i = customerHandlers.Count - 1; i >= 0; i--)
                {
                    var nameHandler = customerHandlers[i];
                    var splittedHandler = nameHandler.Split("-").ToList();

                    if(splittedHandler.Count > 0)
                    {
                        var lastIdentifier = splittedHandler.Last();

                        // concatenated string without last identifier eg: lim-lim
                        var concatHandler = string.Join("-",splittedHandler.Take(splittedHandler.Count -1)).ToLower();

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

            return AppResult<GenerateCustomerHandlerResult>.CreateSucceeded(new GenerateCustomerHandlerResult {GeneratedHandler = handlerName}, "Successfuly generate handler name");
        }
        catch (Exception ex)
        {
            return AppResult<GenerateCustomerHandlerResult>.CreateFailed(ex, "An error occured in GenerateCustomerHandler");
        }
    }
}