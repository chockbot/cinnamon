namespace Cinnamon.Core
{
    public interface IDataStore
    {
        IActivities Activities { get; }
        IActivityTypes ActivityTypes { get; }
        IExperienceTypes ExperienceTypes { get; }
        IWaitList WaitList { get; }
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
