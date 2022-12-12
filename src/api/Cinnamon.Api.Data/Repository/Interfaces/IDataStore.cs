namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IDataStore
{
    IActivity Activity { get; }
    IActivityAddress ActivityAddress { get; }
    IActivityDescription ActivityDescription { get; }
    IActivityImage ActivityImage { get; }
    IActivitySchedule ActivitySchedule { get; }
    IActivitySearchTag ActivitySearchTag { get; }
    ICustomer Customer { get; }
    IExperienceCategory ExperienceCategory { get; }
    IExperienceType ExperienceType { get; }
    IFamilyMember FamilyMember { get; }
    IOngoingActivity OngoingActivity { get; }
    IPurchaseOrder PurchaseOrder { get; }
    IResendEmail ResendEmail { get; }
    IWaitList WaitList { get; }
    ISubCategory SubCategory { get; } 

    Task EnsureMigrate();

    Task SeedData();
}