namespace Cinnamon.Api.Data.Repository.Entities
{
    public class OngoingActivityScheduleTime : BaseEntity
    {
        public int ActivityScheduleTimeId { get; set; }
        public int PurchaseOrderId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime ScheduleDate {get; set;}
        public virtual ActivityScheduleTime ActivityScheduleTime { get; set; }
    }
}
