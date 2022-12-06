using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Repository;

public class ApplicationContext : IdentityDbContext
{
    #region data sets declarations

    public DbSet<Activity> Activities {get; set;}

    public DbSet<ActivityAddress> ActivityAddress {get; set;}

    public DbSet<ActivityDescription> ActivityDescriptions {get; set;}

    public DbSet<ActivityImage> ActivityImages {get; set;}

    public DbSet<ActivitySchedule> ActivitySchedules {get; set;}

    public DbSet<ActivitySearchTag> ActivitySearchTags {get; set;}

    public DbSet<Customer> Customers {get; set;}

    public DbSet<ExperienceCategory> ExperienceCategories {get; set;}

    public DbSet<ExperienceType> ExperienceTypes {get; set;}

    public DbSet<FamilyMember> FamilyMembers {get; set;}

    public DbSet<OngoingActivity> OngoingActivities {get; set;}

    public DbSet<PurchaseOrder> PurchaseOrders {get; set;}

    public DbSet<ResendEmail> ResendEmails {get; set;}

    public DbSet<WaitList> WaitLists {get; set;}

    #endregion

    public ApplicationContext(DbContextOptions<ApplicationContext> opts)
        :base(opts)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // activity builder
    }
}