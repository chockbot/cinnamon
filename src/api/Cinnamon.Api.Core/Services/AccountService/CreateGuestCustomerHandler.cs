using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Cinnamon.Api.Core.Config;

namespace Cinnamon.Api.Core.Services.AccountService;

public class CreateGuestCustomerHandler : ICreateGuestCustomerHandler
{
    private readonly ICustomerData customerData;
    private readonly ApplicationConfig applicationConfig;

    public CreateGuestCustomerHandler(ICustomerData customerData, ApplicationConfig applicationConfig)
    {
        this.customerData = customerData;
        this.applicationConfig = applicationConfig;
    }

    public AppResult<CreateGuestCustomerResult> Execute(CreateGuestCustomerArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateGuestCustomerResult>.CreateFailed(ex, "An error occurred in CreateGuestCustomerHandler");
        }
    }

    public async Task<AppResult<CreateGuestCustomerResult>> ExecuteAsync(CreateGuestCustomerArgs args)
    {
        try
        {
            var createGuestResult = await customerData.CreateGuestCustomer(new Framework.ApiCommand.ApiData.Customer.Request.CreateGuestCustomerArgs
            {
                FirstName = args.FirstName,
                LastName = args.LastName,
                Email = args.Email,
                Birthdate = args.Birthdate,
                PhoneNumber = args.PhoneNumber,
                About = args.About,
                ProfilePath = args.ProfilePath,
                Handler = args.Handler,
                HasAcceptedTerms = args.HasAcceptedTerms,
            });

            if (!createGuestResult.Succeeded || createGuestResult.Result == null)
            {
                return AppResult<CreateGuestCustomerResult>.CreateFailed(
                    new ApplicationException(createGuestResult.Message), createGuestResult.Message);
            }

            var createdGuestCustomer = createGuestResult.Result.Result;

            var claims = new [] {
                new Claim(JwtRegisteredClaimNames.Sub, createdGuestCustomer.Email),
                new Claim(JwtRegisteredClaimNames.Email, createdGuestCustomer.Email),
                new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", createdGuestCustomer.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(applicationConfig.Jwt.Key));
            var signInCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            // expire token in 1 day
            var token = new JwtSecurityToken(applicationConfig.Jwt.Issuer, applicationConfig.Jwt.Audience, 
                                        claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signInCredential);

            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return AppResult<CreateGuestCustomerResult>.CreateSucceeded(new CreateGuestCustomerResult { SessionToken = generatedToken }, "Successfully created guest customer");
        }
        catch (Exception ex)
        {
            return AppResult<CreateGuestCustomerResult>.CreateFailed(ex, "An error occurred in CreateGuestCustomerHandler");
        }
    }
}