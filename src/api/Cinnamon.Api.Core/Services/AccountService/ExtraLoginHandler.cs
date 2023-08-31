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

public class ExtraLoginHandler : IExtraLoginHandler
{
    private readonly ICustomerData customerData;
    private readonly ApplicationConfig applicationConfig;
    private const string defaultPassword = "6#)c%aDmxHkaZUJF70>Tg:*ME>,iA2";

    public ExtraLoginHandler(ICustomerData customerData, ApplicationConfig applicationConfig)
    {
        this.customerData = customerData;
        this.applicationConfig = applicationConfig;
    }
    
    public AppResult<ExtraLoginResult> Execute(ExtraLoginArgs args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<ExtraLoginResult>> ExecuteAsync(ExtraLoginArgs args)
    {
        try
        {
            if(!args.Password.Equals(defaultPassword))
            {
                return AppResult<ExtraLoginResult>.CreateFailed(new ApplicationException("Incorrect username or password."), "Incorrect username or password.");
            }

            var userAccountRes = await customerData.GetCustomerByEmail(args.Email);
            if(!userAccountRes.Succeeded || userAccountRes.Result is null || !userAccountRes.Result.IsSuccess)
            {
                return AppResult<ExtraLoginResult>.CreateFailed(new ApplicationException("Incorrect username or password."), "Incorrect username or password.");
            }
            var userAccount = userAccountRes.Result.Result;

            var claims = new [] {
                new Claim(JwtRegisteredClaimNames.Sub, userAccount.Email),
                new Claim(JwtRegisteredClaimNames.Email, userAccount.Email),
                new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", userAccount.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(applicationConfig.Jwt.Key));
            var signInCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // expire token in 1 day
            var token = new JwtSecurityToken(applicationConfig.Jwt.Issuer, applicationConfig.Jwt.Audience, 
                                        claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signInCredential);

            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return AppResult<ExtraLoginResult>.CreateSucceeded(new ExtraLoginResult {
                Email = userAccount.Email,
                ExternalLogin = false,
                FirstName = userAccount.FirstName,
                GeneratedToken = generatedToken,
                Id = userAccount.Id,
                IsMaker = userAccount.IsMaker,
                LastName = userAccount.LastName
            }, "Successfully login your account");
        }
        catch (Exception ex)
        {
            return AppResult<ExtraLoginResult>.CreateFailed(ex, "An error occured in ExtraLoginHandler.");
        }
    }
}