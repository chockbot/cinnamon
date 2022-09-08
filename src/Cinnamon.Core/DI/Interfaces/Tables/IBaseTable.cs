namespace Cinnamon.Core
{
    /// <summary>
    /// Base Interface for Tables Interface
    /// </summary>
    /// <typeparam name="MODEL"></typeparam>
    public interface IBaseTable<MODEL>
        where MODEL : BaseModel, new()
    {
        /// <summary>
        /// Get Data from database
        /// </summary>
        /// <returns></returns>
        Task<MODEL?> GetDataAsync(MODEL model);

        /// <summary>
        /// Get List from Database
        /// </summary>
        /// <returns></returns>
        Task<List<MODEL>> GetAllAsync();

        /// <summary>
        /// Save data to database
        /// </summary>
        /// <returns></returns>
        Task<ResultModel> SaveDataAsync(MODEL model);

        /// <summary>
        /// Delete data from database
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteDataAsync(MODEL model);

        /// <summary>
        /// Check if Data is Used
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckUsedAsync(MODEL model);
    }
}
