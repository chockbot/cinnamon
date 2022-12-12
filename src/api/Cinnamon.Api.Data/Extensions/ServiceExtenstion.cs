using Cinnamon.Api.Data.Repository.DbSets;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Repository;
using Cinnamon.Api.Data.Services.Repository.ActivityAddress;

namespace Cinnamon.Api.Data.Extensions;

public static class ServiceExtenstion
{
    public static IServiceCollection ExtendServices(this IServiceCollection services) 
    {
        services.AddTransient(typeof(IGenericEntity<>), typeof(GenericEntity<>));
        services.AddTransient<IActivity, ActivityEntity>();
        services.AddTransient<IActivityAddress, ActivityAddressEntity>();
        services.AddTransient<IActivityDescription, ActivityDescriptionEntity>();
        services.AddTransient<IActivityImage, ActivityImageEntity>();
        services.AddTransient<IActivitySchedule, ActivityScheduleEntity>();
        services.AddTransient<IActivitySearchTag, ActivitySearchTagEntity>();
        services.AddTransient<ICustomer, CustomerEntity>();
        services.AddTransient<IExperienceCategory, ExperienceCategoryEntity>();
        services.AddTransient<IExperienceType, ExperienceTypeEntity>();
        services.AddTransient<IFamilyMember, FamilyMemberEntity>();
        services.AddTransient<IOngoingActivity, OngoingActivityEntity>();
        services.AddTransient<IPurchaseOrder, PurchaseOrderEntity>();
        services.AddTransient<IResendEmail, ResendEmailEntity>();
        services.AddTransient<IWaitList, WaitListEntity>();
        services.AddTransient<IDataStore, DataStore>();
        services.AddTransient<Services.Repository.Interfaces.IActivityRepository, Services.Repository.Activity.ActivityRepository>();
        services.AddTransient<Services.Repository.Interfaces.IAddressRepository, Services.Repository.ActivityAddress.AddressRepository>();
        services.AddTransient<Services.Repository.Interfaces.IDescriptionRepository, Services.Repository.ActivityDescription.DescriptionRepository>();
        services.AddTransient<Services.Repository.Interfaces.ICustomerRepository, Services.Repository.Customer.CustomerRepository>();
        services.AddTransient<Services.Repository.Interfaces.IWaitListRepository, Services.Repository.Waitlist.WaitListRepository>();
        services.AddTransient<Services.Repository.Interfaces.IResendEmailRepository, Services.Repository.ResendEmail.ResendEmailRepository>();
        services.AddTransient<Services.Repository.Interfaces.IPurchaseOrderRepository,Services.Repository.PurchaseOrder.PurchaseOrderRepository>();
        services.AddTransient<Services.Repository.Interfaces.IOngoingActivityRepository, Services.Repository.OngoingActivity.OngoingActivityRepository>();
        services.AddTransient<Services.Repository.Interfaces.IFamilyMemberRepository, Services.Repository.FamilyMember.FamilyMemberRepository>();
        services.AddTransient<Services.Repository.Interfaces.IExperienceCategoryRepository, Services.Repository.ExperienceCategory.ExperienceCategoryRepository>();
        services.AddTransient<Services.Repository.Interfaces.ISubCategoryRepository, Services.Repository.ExperienceSubCategory.SubCategoryRepository>();
        services.AddTransient<Services.Repository.Interfaces.IActivityImageRepository, Services.Repository.ActivityImage.ActivityImageRepository>();

        return services;
    }
}