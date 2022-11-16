using Cinnamon.Core.DI.Interfaces.Tables;

namespace Cinnamon.Core
{
    public interface IDataStore
    {
        IActivities Activities { get; }
        IActivityTypes ActivityTypes { get; }
        IActivityImages ActivityImages { get; }
        IExperienceTypes ExperienceTypes { get; }
        IWaitList WaitList { get; }
        IExperienceCategory ExperienceCategory { get; }
        ISchedules Schedules { get; }
        IAddresses Addresses { get; }
        IDescriptions Descriptions { get; }
        ISearchTags SearchTags { get; }
        ICustomer Customer { get; }
        IUser User { get; }
        /// <summary>
        /// Makes sure the client data store is correctly setup
        /// </summary>
        /// <returns></returns>
        Task EnsuredataStoreAsync();

        /// <summary>
        /// Backup the Database
        /// </summary>
        /// <returns></returns>
        Task BackupDatabaseAsync(string source, string destination);
    }
}
