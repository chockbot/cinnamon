using Cinnamon.Core.Module.CinnamonMakerService.Handler;

namespace Cinnamon.Core.Module.CinnamonMakerService;

public interface ICinnamonMakerService 
{
    ISubmitWaitngList SubmitWaitngList { get; }
    IVerifyEmail VerifyEmail { get; }
    ISubmitUpdatedActivity SubmitUpdatedActivity { get; }
    IRegisterMaker RegisterMaker { get; }
}