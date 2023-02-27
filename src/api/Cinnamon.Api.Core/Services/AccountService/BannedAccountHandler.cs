using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class BannedAccountHandler : IBannedAccountHandler
{
    private readonly IFailedLoginData failedLoginData;
    private readonly ApplicationConfig applicationConfig;

    public BannedAccountHandler(IFailedLoginData failedLoginData, ApplicationConfig applicationConfig)
    {
        this.failedLoginData = failedLoginData;
        this.applicationConfig = applicationConfig;
    }
    
    public AppResult<BannedAccountResult> Execute(BannedAccountArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<BannedAccountResult>.CreateFailed(ex, "An error occured in BannedAccountHandler");
        }
    }

    public async Task<AppResult<BannedAccountResult>> ExecuteAsync(BannedAccountArgs args)
    {
        try
        {
            var fromDate = DateTime.Today;
            var toDate = DateTime.Now;

            var failedLoginsRes = await failedLoginData.GetFailedLogins(new Framework.ApiCommand.ApiData.FailedLogin.Request.GetFailedLoginsArgs {
                DateFrom = fromDate.ToString("yyyyMMddHHmmss"),
                DateTo = toDate.ToString("yyyyMMddHHmmss"),
                Email = args.Email
            });
            if(!failedLoginsRes.Succeeded || failedLoginsRes.Result == null || !failedLoginsRes.Result.IsSuccess)
            {
                return AppResult<BannedAccountResult>.CreateFailed(new ApplicationException(failedLoginsRes.Result?.ErrorInfo?.Message), failedLoginsRes.Message);
            }

            if(failedLoginsRes.Result.Result.Count() < applicationConfig.FailedLogin.MaxRetry)
            {
                return AppResult<BannedAccountResult>.CreateSucceeded(new BannedAccountResult {
                    IsBanned = false
                }, "Success checking banned account");
            }

            var lastLogin = failedLoginsRes.Result.Result.Last();
            var lastDatetimeLogin = lastLogin.LoginDate;
            
            lastDatetimeLogin = lastDatetimeLogin.AddMinutes(applicationConfig.FailedLogin.UnbannedInMinutes);
            bool isBanned = lastDatetimeLogin > DateTime.Now;

            // remove banned entries
            if(!isBanned)
            {
                var removedEntries = await failedLoginData.RemoveFailedLogins(new Framework.ApiCommand.ApiData.FailedLogin.Request.RemoveFailedLoginsArgs {
                    RemoveFailedLogins = failedLoginsRes.Result.Result.Select(l => {
                        return new Framework.ApiCommand.ApiData.FailedLogin.Request.RemoveFailedLoginsArgs.RemoveFailedLogin {
                            Id = l.Id
                        };
                    })
                });
                if(!removedEntries.Succeeded || removedEntries.Result == null || !removedEntries.Result.IsSuccess)
                {
                    return AppResult<BannedAccountResult>.CreateFailed(
                        new ApplicationException("An error occured when cleaning failed logins"), "An error occured in BannedAccountHandler");
                }
            }

            return AppResult<BannedAccountResult>.CreateSucceeded(new BannedAccountResult {
                IsBanned = isBanned
            }, "Success checking banned account");
        }
        catch (Exception ex)
        {
            return AppResult<BannedAccountResult>.CreateFailed(ex, "An error occured in BannedAccountHandler");
        }
    }
}