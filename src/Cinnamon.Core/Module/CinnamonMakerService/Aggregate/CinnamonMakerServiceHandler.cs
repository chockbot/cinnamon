using Cinnamon.Core.Module.CinnamonMakerService.Handler;

namespace Cinnamon.Core.Module.CinnamonMakerService;

public class CinnamonMakerServiceHandler : ICinnamonMakerService 
{
    private readonly ISubmitWaitngList submitWaitngList;
    private readonly IVerifyEmail verifyEmail;
    private readonly ISubmitUpdatedActivity submitUpdatedActivity;
    private readonly IRegisterMaker registerMaker;

    public CinnamonMakerServiceHandler(ISubmitWaitngList submitWaitngList, IVerifyEmail verifyEmail,
        ISubmitUpdatedActivity submitUpdatedActivity, IRegisterMaker registerMaker)
    {
        this.submitWaitngList = submitWaitngList;
        this.verifyEmail = verifyEmail;
        this.submitUpdatedActivity = submitUpdatedActivity;
        this.registerMaker = registerMaker;
    }

    public ISubmitWaitngList SubmitWaitngList {get { return submitWaitngList; } }
    public IVerifyEmail VerifyEmail { get { return verifyEmail; } }
    public ISubmitUpdatedActivity SubmitUpdatedActivity {get { return submitUpdatedActivity; } }
    public IRegisterMaker RegisterMaker {get { return registerMaker; } }
}