using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AdminService
{
    public class GetAdminUserByEmailHandler : IGetAdminUserByEmailHandler
    {
        private readonly IAdminUserData adminUserData;

        public GetAdminUserByEmailHandler(IAdminUserData adminUserData)
        {
            this.adminUserData = adminUserData;
        }
        public AppResult<GetAdminUserByEmailResult> Execute(GetAdminUserByEmailArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetAdminUserByEmailResult>.CreateFailed(ex, "An error occured in GetAdminUserByEmailHandler");
            }
        }

        public async Task<AppResult<GetAdminUserByEmailResult>> ExecuteAsync(GetAdminUserByEmailArgs args)
        {
            try
            {
                var result = await adminUserData.GetAdminUserByEmail(new Framework.ApiCommand.ApiData.AdminUser.Request.GetAdminUserByEmailArgs
                {
                    Email = args.Email
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<GetAdminUserByEmailResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<GetAdminUserByEmailResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetAdminUserByEmailHandler");
                }

                var adminUser = result.Result.Result;

                return AppResult<GetAdminUserByEmailResult>.CreateSucceeded(new GetAdminUserByEmailResult
                {
                    AdminUserDetail = new GetAdminUserByEmailResult.AdminUser
                    {
                        EmailAddress = adminUser.EmailAddress,
                        FirstName = adminUser.FirstName,
                        LastName = adminUser.LastName,
                        Id = adminUser.Id,
                        IsAdmin = adminUser.IsAdmin,
                    }
                }, "successfully called GetAdminUserByEmailHandler");

            }
            catch (Exception ex)
            {
                return AppResult<GetAdminUserByEmailResult>.CreateFailed(ex, "An error occured in GetAdminUserByEmailHandler");
            }
        }
    }
}
