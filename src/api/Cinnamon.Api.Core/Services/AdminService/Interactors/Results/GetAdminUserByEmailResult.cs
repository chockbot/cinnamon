using Cinnamon.Framework.ApiCommand.ApiCore;

namespace Cinnamon.Api.Core.Services.AdminService.Interactors.Results
{
    public class GetAdminUserByEmailResult
    {
        public ErrorInfo? ErrorInfo { get; set; }
        public Pagination? Pagination { get; set; }

        public AdminUser AdminUserDetail { get; set; }

        public class AdminUser
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmailAddress { get; set; }
            public bool IsAdmin { get; set; }
        }
    }

}
