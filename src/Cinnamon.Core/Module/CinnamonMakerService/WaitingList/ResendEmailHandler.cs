using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Cinnamon.Core.Common;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;

namespace Cinnamon.Core.Module.CinnamonMakerService.Handler.WaitingList;

public class ResendEmailHandler : IResendEmail 
{
    public AppResult<ResendEmailResult> Execute(ResendEmailArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ResendEmailResult>.CreateFailed(ex, "An error occured in ResendEmailHandler");
        }
    }

    public async Task<AppResult<ResendEmailResult>> ExecuteAsync(ResendEmailArgs args)
    {
        try
        {
            if(string.IsNullOrEmpty(args.Email))
            {
                return AppResult<ResendEmailResult>.CreateFailed(new ApplicationException("Provide valid email"), "Provide valid email");
            }

            // check if email already in waiting list
            var waitRes = await CoreDI.DataStore.WaitList.GetWaitListByEmail(args.Email);
            if(waitRes == null)
            {
                return AppResult<ResendEmailResult>.CreateFailed(new ApplicationException("Email not yet registered"), "Email not yet registered");
            }

            // check if email already resend 

            var guid = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;

            // generate token
            byte[] time = BitConverter.GetBytes(timestamp.ToBinary());
            byte[] key = guid.ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            return AppResult<ResendEmailResult>.CreateFailed(ex, "An error occured in ResendEmailHandler");
        }
    }
}