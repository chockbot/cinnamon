using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Extensions;

namespace Cinnamon.Api.Data.Repository;

public class ApplicationContext : IdentityDbContext
{
    #region data sets declarations

    public DbSet<Activity> Activities {get; set;}

    public DbSet<ActivityAddress> ActivityAddress {get; set;}

    public DbSet<ActivityDescription> ActivityDescriptions {get; set;}

    public DbSet<ActivityImage> ActivityImages {get; set;}

    public DbSet<ActivitySchedule> ActivitySchedules {get; set;}

    public DbSet<Customer> Customers {get; set;}

    public DbSet<ExperienceCategory> ExperienceCategories {get; set;}

    public DbSet<ExperienceType> ExperienceTypes {get; set;}

    public DbSet<FamilyMember> FamilyMembers {get; set;}

    public DbSet<OngoingActivity> OngoingActivities {get; set;}

    public DbSet<PurchaseOrder> PurchaseOrders {get; set;}

    public DbSet<ResendEmail> ResendEmails {get; set;}

    public DbSet<WaitList> WaitLists {get; set;}

    public DbSet<SubCategory> SubCategory { get; set; }

    public DbSet<SearchTags> SearchTags { get; set; }

    public DbSet<ExternalLoginToken> ExternalLoginTokens {get; set;}

    public DbSet<Student> Students {get; set;}

    public DbSet<StudentAttendance> StudentAttendances {get; set;}

    public DbSet<ResetPassword> ResetPasswords {get; set;}
    public DbSet<Region> Regions {get; set; }
    public DbSet<City> Cities {get; set; }
    public DbSet<Barangay> Barangays {get; set; }

    public DbSet<FailedLogin> FailedLogins {get; set;}

    public DbSet<RequestRefund> RequestRefunds {get; set;}

    public DbSet<PayoutAccount> PayoutAccounts {get; set;}

    #endregion

    public ApplicationContext(DbContextOptions<ApplicationContext> opts)
        :base(opts)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // activity builder
        modelBuilder.Entity<Activity>()
            .HasMany<ActivityImage>(a => a.Images)
            .WithOne(i => i.Activity)
            .HasForeignKey(i => i.ActivityId);

        modelBuilder.Entity<Activity>()
            .HasMany<ActivitySchedule>(a => a.Schedules)
            .WithOne(s => s.Activity)
            .HasForeignKey(s => s.ActivityId);

        modelBuilder.Entity<Activity>()
            .HasOne<ActivityAddress>(a => a.Address)
            .WithOne(ad => ad.Activity)
            .HasForeignKey<ActivityAddress>(ad => ad.ActivityId);

        modelBuilder.Entity<Activity>()
            .HasOne<ActivityDescription>(a => a.ActivityDescription)
            .WithOne(d => d.Activity)
            .HasForeignKey<ActivityDescription>(d => d.ActivityId);

        modelBuilder.Entity<Activity>()
            .HasOne<SearchTags>(a => a.SearchTag)
            .WithOne(s => s.Activity)
            .HasForeignKey<SearchTags>(s => s.ActivityId);
        
        // add index to handler
        modelBuilder.Entity<Activity>()
            .HasIndex(a => a.Handler);
        
        modelBuilder.Entity<Activity>()
            .HasOne<Customer>(a => a.Customer);

        // experience type
        modelBuilder.Entity<ExperienceType>()
            .HasMany<Activity>(e => e.Activities)
            .WithOne(a => a.ExperienceType)
            .HasForeignKey(a => a.ExperienceTypeId);

        // waitlist
        modelBuilder.Entity<WaitList>().HasIndex(w => w.Guid);

        // customer
        modelBuilder.Entity<Customer>().HasIndex(c => c.UserId);
        modelBuilder.Entity<Customer>().HasIndex(c => c.Email);
        modelBuilder.Entity<Customer>().HasIndex(c => c.Handler);

        modelBuilder.Entity<Customer>()
            .HasMany<FamilyMember>(c => c.FamilyMembers)
            .WithOne(f => f.Customer)
            .HasForeignKey(f => f.CustomerId);

        modelBuilder.Entity<Customer>()
            .HasMany<OngoingActivity>(c => c.OngoingActivities)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);

        // ongoingActivity
        modelBuilder.Entity<OngoingActivity>().HasIndex(o => o.PurchaseOrderId);
        modelBuilder.Entity<OngoingActivity>()
            .HasOne<ActivitySchedule>(o => o.Schedule);

        // resendEmail
        modelBuilder.Entity<ResendEmail>().HasIndex(r => r.Email);
        modelBuilder.Entity<ResendEmail>().HasIndex(new string[] { "Email", "DateResend" });

        // exter login tokens
        modelBuilder.Entity<ExternalLoginToken>().HasIndex(e => e.Token);
        modelBuilder.Entity<ExternalLoginToken>().HasIndex(new string[] {"Token", "Guid"});

        // student
        modelBuilder.Entity<Student>()
            .HasOne<Customer>(s => s.Customer);
        
        modelBuilder.Entity<Student>()
            .HasOne<Activity>(s => s.Activity);
        
        modelBuilder.Entity<Student>()
            .HasOne<ActivitySchedule>(s => s.Schedule);

        modelBuilder.Entity<Student>()
            .HasIndex(s => s.FamilyMemberId);

        // student attendance
        modelBuilder.Entity<StudentAttendance>()
            .HasOne<Student>(s => s.Student);
        
        // reset password
        modelBuilder.Entity<ResetPassword>().HasIndex(new string[] {"Guid","Token"});

        // failed login
        modelBuilder.Entity<FailedLogin>().HasIndex(new string[] {"Email","LoginDate"});
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentDate = DateTime.Now.SetKindUtc();
        var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);

        foreach (var entity in addedEntities) 
        {
            if (entity.Properties.Any(p => p.Metadata.Name == "CreatedOn")) 
            {
                entity.Property("CreatedOn").CurrentValue = currentDate;
            }
        }

        var updatedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
        foreach (var entity in updatedEntities) 
        {   
            if(entity.Properties.Any(p => p.Metadata.Name == "ChangedOn"))
            {
                entity.Property("ChangedOn").CurrentValue = currentDate;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}