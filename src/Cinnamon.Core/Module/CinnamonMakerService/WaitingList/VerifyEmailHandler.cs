using Cinnamon.Core.Common;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;

namespace Cinnamon.Core.Module.CinnamonMakerService.Handler.WaitingList;

public class VerifyEmailHandler : IVerifyEmail 
{
    public AppResult<VerifyEmailResult> Execute(VerifyEmail args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<VerifyEmailResult>> ExecuteAsync(VerifyEmail args)
    {
        try
        {
            if(string.IsNullOrEmpty(args.Token) || string.IsNullOrEmpty(args.UserId) || string.IsNullOrEmpty(args.Email))
            {
                return AppResult<VerifyEmailResult>.CreateFailed(new ApplicationException("Provide valid token and userId"), "Provide valid token and userId");
            }

            // invalid guid
            var waitingRes = (await CoreDI.DataStore.WaitList.GetAllAsync()).Where(w => w.Guid == args.UserId).FirstOrDefault();
            if(waitingRes == null)
            {
                return AppResult<VerifyEmailResult>.CreateFailed(new ApplicationException("Provide valid token and userId"), "Provide valid token and userId");
            }

            // invalid token
            if(waitingRes.Token != args.Token)
            {
                return AppResult<VerifyEmailResult>.CreateFailed(new ApplicationException("Provide valid token and userId"), "Provide valid token and userId");
            }
            //invalid email
            if (waitingRes.Email != args.Email)
            {
                return AppResult<VerifyEmailResult>.CreateFailed(new ApplicationException("Provide valid token and userId"), "Provide valid token and userId");
            }

            waitingRes.IsVerified = true;
            var updated = await CoreDI.DataStore.WaitList.SaveDataAsync(waitingRes);
            if(!updated.Message.ToLower().Contains("saved"))
            {
                return AppResult<VerifyEmailResult>.CreateFailed(
                    new ApplicationException("An error occured when updating wait list"), "An error occured when updating wait list");
            }

            return AppResult<VerifyEmailResult>.CreateSucceeded(new VerifyEmailResult { WaitingModel = waitingRes }, "Email successfully verified");
        }
        catch (Exception ex)
        {
            return AppResult<VerifyEmailResult>.CreateFailed(ex, "An error occured during VerifyEmailHandler");
        }
    }
}