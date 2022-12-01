using Cinnamon.Core.Models;

namespace Cinnamon.Core;

public interface IResendEmail : IBaseTable<ResendEmailModel> 
{
    Task<ResendEmailModel> GetResendByEmailAsync(string email);
}