namespace Cinnamon.Core
{
    public interface IDataStore
    {
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
