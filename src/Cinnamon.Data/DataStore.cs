using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    /// <summary>
    /// Stores and retriees information about the client application
    /// in an SQLite Database
    /// </summary>
    public class DataStore : BaseCore, IDataStore
    {
        #region Protected Members
        /// <summary>
        /// The database context for the client store
        /// </summary>
        protected DataStoreDbContext mDbContext;
        #endregion

        #region Public Properties
        /// <summary>
        /// Shortcut to access Datacontext externally
        /// Mostly used for testing purposes only
        /// </summary>
        public DataStoreDbContext dbContext => mDbContext;

        public IActivities Activities => new Activities(mDbContext);
        public IActivityTypes ActivityTypes => new ActivityTypes(mDbContext);
        public IExperienceTypes ExperienceTypes => new ExperienceTypes(mDbContext);
        public IWaitList WaitList => new WaitLists(mDbContext);
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dbContext">The database to use</param>
        public DataStore(DataStoreDbContext dbContext)
        {
            //Set Local Member
            mDbContext = dbContext;
        }
        #endregion

        #region Interface Implementation
        /// <summary>
        /// Makes sure the client data store is correctly setup
        /// </summary>
        /// <returns></returns>
        public async Task EnsuredataStoreAsync()
        {
            // Make sure the database exist and is created
            bool result = await mDbContext.Database.EnsureCreatedAsync();

            // Migrate Changes
            await mDbContext.Database.MigrateAsync();
        }

        /// <summary>
        /// Backup current database
        /// </summary>
        /// <returns></returns>
        public Task BackupDatabaseAsync(string s, string d)
        {
            //var source = new SqliteConnection($"Data Source={s}");
            //var destination = new SqliteConnection($"Data Source={d}");
            //source.Open();
            //destination.Open();
            //source.BackupDatabase(destination);
            //source.Close();
            //destination.Close();
            return Task.CompletedTask;
        }
        #endregion
    }
}
