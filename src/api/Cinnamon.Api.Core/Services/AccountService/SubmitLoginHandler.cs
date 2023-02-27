using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Microsoft.IdentityModel.Tokens;

namespace Cinnamon.Api.Core.Services.AccountService;

public class SubmitLoginHandler : ISubmitLoginHandler
{
    private readonly ICustomerData customerData;
    private readonly ApplicationConfig applicationConfig;
    private readonly IFailedLoginData failedLoginData;
    private readonly IBannedAccountHandler bannedAccountHandler;

    public SubmitLoginHandler(ICustomerData customerData, ApplicationConfig applicationConfig, 
        IBannedAccountHandler bannedAccountHandler, IFailedLoginData failedLoginData)
    {
        this.customerData = customerData;
        this.applicationConfig = applicationConfig;
        this.bannedAccountHandler = bannedAccountHandler;
        this.failedLoginData = failedLoginData;
    }

    public AppResult<SubmitLoginResult> Execute(SubmitLoginArgs args)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            return AppResult<SubmitLoginResult>.CreateFailed(ex, "An error occured in SubmitLoginHandler");
        }
    }

    public async Task<AppResult<SubmitLoginResult>> ExecuteAsync(SubmitLoginArgs args)
    {
        try
        {
            var result = await customerData.CheckCustomerLogin(new Framework.ApiCommand.ApiData.Customer.Request.CheckCustomerLoginArgs {
                Email = args.Email,
                Password = args.Password
            });

            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<SubmitLoginResult>.CreateFailed(
                    new ApplicationException("An error occured when trying to access api"), "An error occured when trying to access api");
            }

            // check banned account
            var bannnedResult = await bannedAccountHandler.ExecuteAsync(new BannedAccountArgs { Email = args.Email });
            if(!bannnedResult.Succeeded || bannnedResult.Result == null)
            {
                return AppResult<SubmitLoginResult>.CreateFailed(new ApplicationException(bannnedResult.Message), bannnedResult.Message);
            }
            if(bannnedResult.Result.IsBanned)
            {
                return AppResult<SubmitLoginResult>.CreateFailed(new ApplicationException("Account temporary banned."), "Account temporary banned.");
            }

            // insert failed logins
            if(result.Succeeded && !result.Result.IsSuccess)
            {
                var insertFailedLogin = await failedLoginData.CreateFailedLogin(new Framework.ApiCommand.ApiData.FailedLogin.Request.CreateFailedLoginArgs {
                    Email = args.Email,
                    LoginDate = DateTime.Now
                });
                if(!insertFailedLogin.Succeeded || insertFailedLogin.Result == null || !insertFailedLogin.Result.IsSuccess)
                {
                    return AppResult<SubmitLoginResult>.CreateFailed(new ApplicationException(insertFailedLogin.Result?.ErrorInfo?.Message), insertFailedLogin.Message);
                }

                return AppResult<SubmitLoginResult>.CreateFailed(new ApplicationException("Invalid username or password"), "Invalid username or password");
            }

            var loginResult = result.Result.Result;

            var claims = new [] {
                new Claim(JwtRegisteredClaimNames.Sub, loginResult.Email),
                new Claim(JwtRegisteredClaimNames.Email, loginResult.Email),
                new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", loginResult.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(applicationConfig.Jwt.Key));
            var signInCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            // expire token in 1 day
            var token = new JwtSecurityToken(applicationConfig.Jwt.Issuer, applicationConfig.Jwt.Audience, 
                                        claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signInCredential);

            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return AppResult<SubmitLoginResult>.CreateSucceeded(new SubmitLoginResult {
                Email = loginResult.Email,
                ExternalLogin = loginResult.ExternalLogin,
                IsMaker = loginResult.IsMaker,
                FirstName = loginResult.FirstName,
                LastName = loginResult.LastName,
                GeneratedToken = generatedToken,
                Id = loginResult.Id
            }, "Successfully checked login credentials");
        }
        catch (Exception ex)
        {
            return AppResult<SubmitLoginResult>.CreateFailed(ex, "An error occured in SubmitLoginHandler");
        }
    }
}