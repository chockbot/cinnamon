using Cinnamon.Core;
using Cinnamon.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    /// <summary>
    /// The database context for the client data store
    /// </summary>
    public class DataStoreDbContext : IdentityDbContext
    {
        #region DbSets
        public DbSet<ActivityTypeModel> ActivityTypes { get; set; }
        public DbSet<ActivityModel> Activities { get; set; }
        public DbSet<ActivityImagesModels> ActivityImages { get; set; }
        public DbSet<ExperienceTypeModel> ExperienceTypes { get; set; }
        public DbSet<WaitListModel> WaitLists { get; set; }
        public DbSet<ExperienceCategoryModel> ExperienceCategories { get; set; }
        public DbSet<UserListModel> UserList { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options"></param>
        public DataStoreDbContext(DbContextOptions<DataStoreDbContext> options) : base(options) { }
        #endregion

        #region Model Creating
        /// <summary>
        /// Configures the database structure and relationships
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API
            modelBuilder.Entity<ActivityTypeModel>().HasKey(a => new { a.Id });
            modelBuilder.Entity<ActivityTypeModel>()
                .HasMany(a => a.Activities)
                .WithOne(a => a.ActivityType)
                .HasForeignKey(s => s.ActivityTypeId);

            modelBuilder.Entity<ExperienceTypeModel>().HasKey(e => new { e.Id });
            modelBuilder.Entity<ExperienceTypeModel>()
                .HasMany(e => e.Activities)
                .WithOne(a => a.ExperienceType)
                .HasForeignKey(a => a.ExperienceTypeId);

            modelBuilder.Entity<ActivityModel>().HasKey(a => new { a.Id });
            modelBuilder.Entity<ActivityModel>()
                .HasOne(a => a.ActivityType)
                .WithMany(a => a.Activities)
                .HasForeignKey(a => a.ActivityTypeId);

            modelBuilder.Entity<ActivityModel>()
                .HasOne(a => a.ExperienceType)
                .WithMany(e => e.Activities)
                .HasForeignKey(a => a.ExperienceTypeId);

            modelBuilder.Entity<WaitListModel>().HasKey(w => new { w.Id });
            modelBuilder.Entity<WaitListModel>().HasIndex(w => w.Guid);

            modelBuilder.Entity<ActivityImagesModels>().HasKey(b => new { b.Id });
            modelBuilder.Entity<UserListModel>().HasKey(x => new { x.Id});

        }
        #endregion

        #region SaveChangesAsync
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var AddedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();
            var UserID = 0;
            try
            {
                //UserID = Framework.Service<ApplicationViewModel>()?.CurrentUser?.UserID ?? 0;
            }
            catch
            {
                UserID = 0;
            }
            var CurrentTime = DateTime.UtcNow;

            AddedEntities.ForEach(E =>
            {
                E.Property("CreatedOn").CurrentValue = CurrentTime;
                //E.Property("CreatedBy").CurrentValue = UserID;
                E.Property("ChangedOn").CurrentValue = CurrentTime;
                //E.Property("ChangedBy").CurrentValue = UserID;
            });

            var EditedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            EditedEntities.ForEach(E =>
            {
                E.Property("ChangedOn").CurrentValue = CurrentTime;
                //E.Property("ChangedBy").CurrentValue = UserID;
            });

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        #endregion
    }
}

