using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.AdminUser
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly IDataStore dataStore;

        public AdminUserRepository(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }
        public async Task<AppResult<AdminUserDTO>> GetAdminUserByEmailAsync(string email)
        {
            try
            {
                var result = await dataStore.AdminUser.FindAsync(a => a.EmailAddress.ToLower().Trim() == email.ToLower().Trim());

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<AdminUserDTO>.CreateFailed(result.Error.Exception, result.Message);
                }

                var adminUser = result.Result.ToList();

                var adminUserDTO = new AdminUserDTO();

                if (adminUser.Count > 0)
                {
                    var user = adminUser.First();

                    adminUserDTO = new AdminUserDTO
                    {
                        EmailAddress = user.EmailAddress,
                        FirstName    = user.FirstName,
                        LastName     = user.LastName,
                        Id           = user.Id,
                        IsAdmin      = true
                    };
                }
                else
                {
                    adminUserDTO.IsAdmin = false;
                }
                 

                return AppResult<AdminUserDTO>.CreateSucceeded(adminUserDTO, "successfully retrieved admin user by email");
            }
            catch (Exception ex)
            {
                return AppResult<AdminUserDTO>.CreateFailed(ex, "An error occured when getting admin user by email");
            }
        }
    }
}
