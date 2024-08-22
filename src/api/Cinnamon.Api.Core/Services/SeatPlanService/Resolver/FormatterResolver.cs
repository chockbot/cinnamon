namespace Cinnamon.Api.Core.Services.SeatPlanService.Resolver;

public class FormatterResolver 
{
    public Type ResolveFormatter(string interactorHandler, string driverClassName)
    {
        var interactorHandlerInterface = Type.GetType($"Cinnamon.Api.Core.Services.SeatPlanService.Handlers.{interactorHandler}");
        var driverClassInterface = Type.GetType($"Cinnamon.Api.Core.Services.SeatPlanService.Handlers.{driverClassName}");

        if(interactorHandlerInterface == null || driverClassInterface == null)
        {
            throw new ApplicationException("Can't resolve seat plan formatter driver");
        }

        var seaPlanFormatter = AppDomain.CurrentDomain.
                            GetAssemblies().SelectMany(e => e.GetTypes()).
                            Where(p => interactorHandlerInterface.IsAssignableFrom(p) && driverClassInterface.IsAssignableFrom(p)).
                            FirstOrDefault();

        if(seaPlanFormatter == null)
        {
            throw new ApplicationException("Can't resolve seat plan formatter driver");
        }
        
        return seaPlanFormatter;
    }
}