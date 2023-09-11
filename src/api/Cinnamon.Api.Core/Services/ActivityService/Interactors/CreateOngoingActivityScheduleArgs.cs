using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors
{
    public class CreateOngoingActivityScheduleArgs : IInteractor
    {
        public DateTime ScheduleDate { get; set; }
        public int ActivityScheduleTimeId { get; set; }
        public int PurchaseOrderId { get; set; }
        public bool IsCompleted { get; set; }
        public int CreatedBy { get; set; }
    }
}
