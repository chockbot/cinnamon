using Cinnamon.Core.Module.CinnamonMakerService.Handler;

namespace Cinnamon.Core.Module.CinnamonMakerService;

public interface ICinnamonMakerService 
{
    IRegisterMaker RegisterMaker { get; }
    IConfirmEmailMaker ConfirmEmailMaker { get; }
    ISubmitWaitngList SubmitWaitngList { get; }
    IVerifyEmail VerifyEmail { get; }
}