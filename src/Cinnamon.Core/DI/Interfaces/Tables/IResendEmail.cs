using Cinnamon.Core.Models;

namespace Cinnamon.Core;

public interface IResendEmail : IBaseTable<ResendEmailModel> 
{
    Task<ResendEmailModel> GetResendByEmailAsync(string email);
    Task<IList<ResendEmailModel>> GetLisResendEmailAsync(string email, DateTime from, DateTime to);
}