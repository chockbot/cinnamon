using System.Threading.Tasks;

namespace Cinnamon.Core.Interactor;

public interface IInteractorHandler<TInteractor, TInteractorResult> where TInteractor : IInteractor
{
    TInteractorResult Execute(TInteractor interactor);

    Task<TInteractorResult> ExecuteAsync(TInteractor interactor);
}