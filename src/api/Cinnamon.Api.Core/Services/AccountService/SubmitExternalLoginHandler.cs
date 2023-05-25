using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;

namespace Cinnamon.Api.Core.Services.AccountService;

public class SubmitExternalLoginHandler : IExternalLoginHandler
{
    private readonly ICustomerData customerData;
    private readonly IExternalLoginTokenData externalLoginTokenData;
    private readonly ApplicationConfig applicationConfig;

    public SubmitExternalLoginHandler(ICustomerData customerData, ApplicationConfig applicationConfig, IExternalLoginTokenData externalLoginTokenData)
    {
        this.customerData = customerData;
        this.externalLoginTokenData = externalLoginTokenData;
        this.applicationConfig = applicationConfig;
    }

    public AppResult<ExternalLoginResult> Execute(ExternalLoginArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ExternalLoginResult>.CreateFailed(ex, "An error occured in SubmitExternalLoginHandler");
        }
    }

    public async Task<AppResult<ExternalLoginResult>> ExecuteAsync(ExternalLoginArgs args)
    {
        try
        {
            // check if use empty username 
            if(string.IsNullOrEmpty(args.Email))
            {
                if(args.IsEmptyUsername)
                {
                    return await NewLogin(args);
                }
                else 
                {
                    return AppResult<ExternalLoginResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request");
                }
            }

            // check if already have registered account
            var accountRes = await customerData.GetCustomerByEmail(args.Email);
            if(!accountRes.Succeeded || accountRes.Result == null)
            {
                return AppResult<ExternalLoginResult>.CreateFailed(new ApplicationException(accountRes.Message), accountRes.Message);
            }
            if(accountRes.Succeeded && !accountRes.Result.IsSuccess)
            {
                return await NewLogin(args);    
            }

            var customerAccount = accountRes.Result.Result;

            var claims = new [] {
                new Claim(JwtRegisteredClaimNames.Sub, customerAccount.Email),
                new Claim(JwtRegisteredClaimNames.Email, customerAccount.Email),
                new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", customerAccount.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(applicationConfig.Jwt.Key));
            var signInCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // expire token in 1 day
            var token = new JwtSecurityToken(applicationConfig.Jwt.Issuer, applicationConfig.Jwt.Audience, 
                                        claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signInCredential);

            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return AppResult<ExternalLoginResult>.CreateSucceeded(new ExternalLoginResult {
                Email = customerAccount.Email,
                ExternalLogin = customerAccount.ExternalLogin,
                IsMaker = customerAccount.IsMaker,
                FirstName = customerAccount.FirstName,
                LastName = customerAccount.LastName,
                GeneratedToken = generatedToken,
                Id = customerAccount.Id,
                IsEmptyUsername = args.IsEmptyUsername
            }, "Successfully checked login credentials");
        }
        catch (Exception ex)
        {
            return AppResult<ExternalLoginResult>.CreateFailed(ex, "An error occured in SubmitExternalLoginHandler");
        }
    }

    private async Task<AppResult<ExternalLoginResult>> NewLogin(ExternalLoginArgs args)
    {
        try
        {
            // generate token and guid
            var guid = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;

            // generate token
            byte[] time = BitConverter.GetBytes(timestamp.ToBinary());
            byte[] guidKey = guid.ToByteArray();
            var loginToken = Convert.ToBase64String(time.Concat(guidKey).ToArray());
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(loginToken));

            // generate guid token
            var createToken = await externalLoginTokenData.CreateToken(new Framework.ApiCommand.ApiData.ExternalLoginToken.Request.CreateExterLoginTokenArgs {
                DateGenerated = DateTime.Now,
                Email = args.Email,
                Token = loginToken,
                Guid = guid.ToString(),
                FirstName = args.FirstName,
                LastName = args.LastName,
                IsEmptyUsername = args.IsEmptyUsername
            });

            if(!createToken.Succeeded || createToken.Result == null)
            {
                return AppResult<ExternalLoginResult>.CreateFailed(new ApplicationException(createToken.Message), createToken.Message);
            }
            if(createToken.Succeeded && !createToken.Result.IsSuccess)
            {
                return AppResult<ExternalLoginResult>.CreateFailed(
                    new ApplicationException(createToken.Result.ErrorInfo?.Message), "An error occured in SubmitExternalLoginHandler");
            }

            return AppResult<ExternalLoginResult>.CreateSucceeded(new ExternalLoginResult {
                IsNew = true,
                GeneratedNewToken = encodedToken,
                GeneratedNewGuid = createToken.Result.Result.Guid,
                IsEmptyUsername = args.IsEmptyUsername
            }, "Account not yet registered need to create the account using email");
        }
        catch (Exception ex)
        {
            return AppResult<ExternalLoginResult>.CreateFailed(ex, "An error occured in SubmitExternalLoginHandler");
        }
    }
}