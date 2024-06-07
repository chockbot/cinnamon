using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Extensions;
using System.Security.AccessControl;

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

    public DbSet<PayoutLog> PayoutLogs {get; set;}
    public DbSet<AdminUser> AdminUsers {get; set; }

    public DbSet<BadgeList> BadgeList { get; set; }

    public DbSet<CustomerPricing> CustomerPricings {get; set;}
    public DbSet<ChatHistory> ChatHistories {get; set; }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatMember> ChatMembers { get; set; }
    public DbSet<Favorite> Favorites { get; set; }

    public DbSet<Coupon> Coupons {get; set;}
    public DbSet<ChatConnection> ChatConnections {get; set; }
    public DbSet<ActivityScheduleTime> ActivityScheduleTimes {get; set; }
    public DbSet<ExperienceCreationType> ExperienceCreationTypes {get; set; }
    public DbSet<OngoingActivityScheduleTime> OngoingActivityScheduleTimes {get; set; }
    public DbSet<OteSchedule> OteSchedules {get; set;}
    public DbSet<OteSchedulePricing> OteSchedulePricings {get; set;}
    public DbSet<OteTicket> OteTickets {get; set;}
    public DbSet<TokenGenerated> TokenGenerateds {get; set;}
    public DbSet<OteDate> OteDates {get; set;}
    public DbSet<OteSchedulePricingGroup> OteSchedulePricingGroups {get; set;}
    public DbSet<OteDateOverride> OteDateOverrides {get; set;}
    public DbSet<Announcement> Announcements {get; set;}

    
    public DbSet<OteSharedLink> OteSharedLinks {get; set;}
    
    public DbSet<OteOnlineEvent> OteOnlineEvent { get; set;}
    
    // new disbursement flow
    public DbSet<Disbursement> Disbursements {get; set;}
    public DbSet<DisbursementDetail> DisbursementDetails {get; set;}
    public DbSet<DisbursementBulk> DisbursementBulks {get; set;}
    public DbSet<DisbursementDetailBulk> DisbursementDetailBulks {get; set;}
    public DbSet<DisbursementBulkLog> DisbursementBulkLogs {get; set;}
    public DbSet<DisbursementManual> DisbursementManuals {get; set;}
    public DbSet<ChatUnreadNotification> ChatUnreadNotifications {get; set;}

    public DbSet<DynamicContent> DynamicContents {get; set;}

    public DbSet<ActivitySummary> ActivitySummaries {get; set;}

    public DbSet<DirectStudentInfo> DirectStudentInfos {get; set;}

    public DbSet<DirectStudentSession> DirectStudentSessions {get; set;}

    public DbSet<DirectStudentPayment> DirectStudentPayments {get; set;}

    public DbSet<DirectStudentAttendance> DirectStudentAttendances {get; set;}

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

        modelBuilder.Entity<Activity>()
            .HasMany<Student>(a => a.Students)
            .WithOne(i => i.Activity)
            .HasForeignKey(i => i.ActivityId);

        // add index to handler
        modelBuilder.Entity<Activity>()
            .HasIndex(a => a.Handler);
        
        modelBuilder.Entity<Activity>()
            .HasOne<Customer>(a => a.Customer);

        // add index for purchase order count
        modelBuilder.Entity<Activity>()
            .HasIndex(a => a.PurchaseOrderCount);

        modelBuilder.Entity<Activity>()
            .HasIndex(a => a.IsPublished);

        modelBuilder.Entity<Activity>()
            .HasIndex(a => a.IsDeactivated);

        modelBuilder.Entity<Activity>()
            .HasIndex(a => a.IsNew);

        modelBuilder.Entity<Activity>()
            .HasIndex("PurchaseOrderCount","IsPublished","IsDeactivated", "IsNew");
        
        modelBuilder.Entity<Activity>()
            .HasIndex("IsDeactivated", "Status", "IsPublished", "ForceDisable");
        
        modelBuilder.Entity<Activity>()
            .HasIndex(a => a.ForceDisable);

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

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.CustomerPricing)
            .WithOne(c => c.Customer)
            .HasForeignKey<CustomerPricing>(c => c.CustomerId);

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
        modelBuilder.Entity<Student>().HasIndex(s => s.OngoingActivityId);

        // student attendance
        modelBuilder.Entity<StudentAttendance>()
            .HasOne<Student>(s => s.Student);
        
        // reset password
        modelBuilder.Entity<ResetPassword>().HasIndex(new string[] {"Guid","Token"});

        // failed login
        modelBuilder.Entity<FailedLogin>().HasIndex(new string[] {"Email","LoginDate"});

        // payout account
        modelBuilder.Entity<PayoutAccount>().HasIndex(p => p.CustomerId);

        // purchase order
        modelBuilder.Entity<PurchaseOrder>().HasIndex(p => p.Status);

        //badge 
        modelBuilder.Entity<BadgeList>().HasIndex(c => c.Id);

        // customer pricing
        modelBuilder.Entity<CustomerPricing>().HasOne<Customer>(c => c.Customer);

        //chat
        modelBuilder.Entity<Customer>()
           .HasMany<ChatMember>(a => a.ChatMembers)
           .WithOne(i => i.Customer)
           .HasForeignKey(i => i.CustomerId);

        modelBuilder.Entity<ChatRoom>()
           .HasMany<ChatMember>(a => a.ChatMembers)
           .WithOne(i => i.ChatRoom)
           .HasForeignKey(i => i.ChatRoomId);

        modelBuilder.Entity<Customer>()
            .HasMany<ChatHistory>(a => a.FromChatHistories)
            .WithOne(i => i.FromCustomer)
            .HasForeignKey(i => i.FromUserId);

        modelBuilder.Entity<Customer>()
           .HasMany<ChatHistory>(a => a.ToChatHistories)
           .WithOne(i => i.ToCustomer)
           .HasForeignKey(i => i.ToUserId);

        modelBuilder.Entity<Customer>()
           .HasMany<ChatConnection>(a => a.ChatConnections)
           .WithOne(i => i.Customer)
           .HasForeignKey(i => i.CustomerId);

        // reviews
        modelBuilder.Entity<Reviews>().HasIndex(c => c.Id);

        modelBuilder.Entity<Activity>()
            .HasMany<Reviews>(a => a.Reviews)
            .WithOne(i => i.Activity)
            .HasForeignKey(i => i.ActivityId);

        modelBuilder.Entity<Activity>()
            .HasOne(c => c.ExperienceCreationType)
            .WithMany(c => c.Activities)
            .HasForeignKey(c => c.ExperienceCreationTypeId);

        modelBuilder.Entity<ActivitySchedule>()
          .HasMany<ActivityScheduleTime>(a => a.ActivityScheduleTimes)
          .WithOne(i => i.ActivitySchedule)
          .HasForeignKey(i => i.ActivityScheduleId);

        modelBuilder.Entity<ActivityScheduleTime>()
         .HasMany<OngoingActivityScheduleTime>(a => a.OngoingActivityScheduleTimes)
         .WithOne(i => i.ActivityScheduleTime)
         .HasForeignKey(i => i.ActivityScheduleTimeId);

        // for coupons
        modelBuilder.Entity<Coupon>().HasOne(c => c.Activity).WithMany().IsRequired(false);
        modelBuilder.Entity<Coupon>().HasOne(c => c.Customer);
        modelBuilder.Entity<Coupon>().HasIndex(c => c.Code);
        modelBuilder.Entity<Coupon>().HasIndex(new string[] {"Code", "CustomerId", "ActivityId"});

        // for ote schedule
        modelBuilder.Entity<OteSchedule>().HasOne(o => o.Activity).WithOne(a => a.OteSchedule);
        modelBuilder.Entity<OteSchedulePricing>().HasOne(o => o.OteSchedule).WithMany(o => o.OteSchedulePricing);
        modelBuilder.Entity<OteOnlineEvent>().HasOne(o => o.OteSchedule).WithMany(o => o.OteOnlineEvent);

        // for ote tickets
        modelBuilder.Entity<OteTicket>()
            .HasOne(t => t.Activity);
        modelBuilder.Entity<OteTicket>()
            .HasOne(t => t.OteSchedule);
        modelBuilder.Entity<OteTicket>()
            .HasOne(t => t.OteSchedulePricing);
        modelBuilder.Entity<OteTicket>()
            .HasOne(t => t.Customer);
        modelBuilder.Entity<OteTicket>()
            .HasIndex(t => t.QRCode);
        modelBuilder.Entity<OteTicket>()
            .HasIndex(t => t.Status);
        modelBuilder.Entity<OteTicket>()
            .HasIndex("ActivityId","QRCode","Status");
        modelBuilder.Entity<OteTicket>()
            .HasIndex(t => t.PurchaseOrderId);
        modelBuilder.Entity<OteTicket>()
            .HasIndex(t => t.OteDateId);


        modelBuilder.Entity<TokenGenerated>()
            .HasIndex("Guid");

        modelBuilder.Entity<TokenGenerated>()
            .HasIndex("Guid","Token");

        //AddOns
        modelBuilder.Entity<AddOns>().HasIndex(o => o.Id);

        // for disbursement configs
        modelBuilder.Entity<Disbursement>()
            .HasIndex(d => d.PurchaseOrderId);
        modelBuilder.Entity<Disbursement>()
            .HasIndex(d => d.CustomerId);
        modelBuilder.Entity<DisbursementBulk>()
            .HasIndex(d => d.CustomerId);
        modelBuilder.Entity<DisbursementBulkLog>()
            .HasIndex(d => d.DisbursementBulkId);
        modelBuilder.Entity<DisbursementManual>()
            .HasIndex(d => d.DisbursementId);
            
        // chat unread notification
        modelBuilder.Entity<ChatUnreadNotification>()
            .HasIndex(c => c.ToUserId);
        modelBuilder.Entity<ChatUnreadNotification>()
            .HasIndex(c => c.FromUserId);
        modelBuilder.Entity<ChatUnreadNotification>()
            .HasIndex(c => c.ChatHistoryId);
            
        // announcements
        modelBuilder.Entity<Announcement>()
            .HasIndex(a => a.Status);

        // for ote shared link
        modelBuilder.Entity<OteSharedLink>()
            .HasIndex(o => o.ActivityId);
        modelBuilder.Entity<OteSharedLink>()
            .HasIndex(o => o.OteDateId);
        modelBuilder.Entity<OteSharedLink>()
            .HasIndex("ActivityId","OteDateId");
        modelBuilder.Entity<OteSharedLink>()
            .HasIndex("Guid", "Token");

        // for dynamic content
        modelBuilder.Entity<DynamicContent>()
            .HasIndex(d => d.Identifier);

        // for activity summary and activity
        modelBuilder.Entity<ActivitySummary>()
            .HasIndex(s => s.ActivityId);
        modelBuilder.Entity<ActivitySummary>()
            .HasIndex(s => s.Ongoing);
        modelBuilder.Entity<ActivitySummary>()
            .HasIndex(s => s.TotalParticipants);
        modelBuilder.Entity<ActivitySummary>()
            .HasIndex(s => s.ReviewAccumulated);
        modelBuilder.Entity<ActivitySummary>()
            .HasIndex("Ongoing", "Completed");
        modelBuilder.Entity<ActivitySummary>()
            .HasIndex(s => s.Location);
        modelBuilder.Entity<ActivitySummary>()
            .HasIndex(s => s.Provider);
        modelBuilder.Entity<ActivitySummary>()
            .HasIndex("Ongoing", "Completed", "TotalParticipants");
        modelBuilder.Entity<Activity>()
            .HasIndex(a => a.Guid);

        // for direct student info
        modelBuilder.Entity<DirectStudentInfo>()
            .HasIndex(s => s.ProviderId);
        modelBuilder.Entity<DirectStudentInfo>()
            .HasIndex(s => s.CreatedOn);
        
        // for direct student session
        modelBuilder.Entity<DirectStudentSession>()
            .HasIndex(s => s.ActivityId);
        modelBuilder.Entity<DirectStudentSession>()
            .HasIndex(s => s.ScheduleId);

        // for direct student attendance
        modelBuilder.Entity<DirectStudentAttendance>()
            .HasIndex("Date", "DirectStudentSessionId");

        // for ote date
        modelBuilder.Entity<OteDate>()
            .HasIndex(d => d.Date);
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