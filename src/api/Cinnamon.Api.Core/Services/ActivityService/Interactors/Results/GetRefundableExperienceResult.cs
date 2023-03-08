namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class GetRefundableExperienceResult 
{
    public IEnumerable<RefundableExperience> RefundableExperiences {get; set;}

    public class RefundableExperience 
    {
        public int PurchaseOrderId {get; set;}
        public string Name {get; set;}
    }        
}