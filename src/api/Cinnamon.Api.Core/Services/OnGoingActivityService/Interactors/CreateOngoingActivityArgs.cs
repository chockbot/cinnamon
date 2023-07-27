using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OngoingActivityService.Interactors;

public class CreateOngoingActivityArgs : IInteractor
{
    public int CustomerId {get; set;}
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public int PurchaseOrderId {get; set;}
    public IEnumerable<Student> Students {get; set;}
    public string SelectedPeriod { get; set; }

    public class Student 
    {
        public int FamilyMemberId {get; set;}
        public string Name {get; set;}
    }
}