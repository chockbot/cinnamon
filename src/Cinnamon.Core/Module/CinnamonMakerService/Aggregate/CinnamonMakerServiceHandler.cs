using Cinnamon.Core.Module.CinnamonMakerService.Handler;

namespace Cinnamon.Core.Module.CinnamonMakerService;

public class CinnamonMakerServiceHandler : ICinnamonMakerService 
{
    private readonly ISubmitWaitngList submitWaitngList;
    private readonly IVerifyEmail verifyEmail;

    public CinnamonMakerServiceHandler(ISubmitWaitngList submitWaitngList, IVerifyEmail verifyEmail)
    {
        this.submitWaitngList = submitWaitngList;
        this.verifyEmail = verifyEmail;
    }

    public ISubmitWaitngList SubmitWaitngList {get { return submitWaitngList; } }
    public IVerifyEmail VerifyEmail { get { return verifyEmail; } }
}