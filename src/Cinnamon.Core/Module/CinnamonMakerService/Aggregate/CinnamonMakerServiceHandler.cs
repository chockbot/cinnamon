using Cinnamon.Core.Module.CinnamonMakerService.Handler;

namespace Cinnamon.Core.Module.CinnamonMakerService;

public class CinnamonMakerServiceHandler : ICinnamonMakerService 
{
    private readonly IRegisterMaker registerMaker;
    private readonly IConfirmEmailMaker confirmEmailMaker;
    private readonly ISubmitWaitngList submitWaitngList;
    private readonly IVerifyEmail verifyEmail;

    public CinnamonMakerServiceHandler(IRegisterMaker registerMaker, IConfirmEmailMaker confirmEmailMaker,
        ISubmitWaitngList submitWaitngList, IVerifyEmail verifyEmail)
    {
        this.registerMaker = registerMaker;
        this.confirmEmailMaker = confirmEmailMaker;
        this.submitWaitngList = submitWaitngList;
        this.verifyEmail = verifyEmail;
    }

    public IRegisterMaker RegisterMaker { get { return registerMaker; } }
    public IConfirmEmailMaker ConfirmEmailMaker {get { return confirmEmailMaker; } }
    public ISubmitWaitngList SubmitWaitngList {get { return submitWaitngList; } }
    public IVerifyEmail VerifyEmail { get { return verifyEmail; } }
}