using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors
{
    public class GetActivityScheduleTimesArgs : IInteractor
    {
        public int ActivityScheduleId { get; set; }
        public int DayOfWeek { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
