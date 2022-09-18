using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    public abstract class BaseDbSet<MODEL> : BaseCore, IBaseTable<MODEL>
      where MODEL : BaseModel, new()
    {
        #region Protected Members
        /// <summary>
        /// Database context
        /// </summary>
        protected DataStoreDbContext mDbContext;

        /// <summary>
        /// Table Interface
        /// </summary>
        protected abstract DbSet<MODEL> Table { get; }
        #endregion

        #region Constructor
        public BaseDbSet(DataStoreDbContext dbContext)
        {
            mDbContext = dbContext;
        }
        #endregion

        #region Interface Implementation
        /// <summary>
        /// Check if Data is Used
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<bool> CheckUsedAsync(MODEL model)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Delete Data from Database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteDataAsync(MODEL model)
        {
            model.LoadVirtualProperties();
            try
            {
                Table.Remove(model);
                await mDbContext.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get All Items in Table
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<MODEL>> GetAllAsync()
        {
            var result = await Table.ToListAsync();
            return result;
        }

        /// <summary>
        /// Get Data from Database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<MODEL?> GetDataAsync(MODEL model)
        {
            return await Table.FindAsync(GetKey(model));
        }

        /// <summary>
        /// Save Data to Database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> SaveDataAsync(MODEL model)
        {
            //Check if LazyLoader Property Exist
            if (model.HasProperty("LazyLoader"))
            {
                try
                {
                    mDbContext.Update(model);
                    await mDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (ex.Message != "An error occurred while updating the entries. See the inner exception for details.")
                        return ResultModel.error(ex.Message);
                    else
                        return ResultModel.error(ex.InnerException?.Message ?? "An unknown error occured.");
                }
                return ResultModel.success("Saved");
            }
            //Clear Change Tracker
            var check = await Table.FindAsync(GetKey(model));
            try
            {
                if (check == null)
                {
                    Table.Add(model);
                }
                else
                {
                    //Remove Tracking for check
                    mDbContext.Entry(check).State = EntityState.Detached;
                    Table.Update(model);
                }
                await mDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.Message != "An error occurred while updating the entries. See the inner exception for details.")
                    return ResultModel.error(ex.Message);
                else
                    return ResultModel.error(ex.InnerException?.Message ?? "An unknown error occured.");
            }
            return ResultModel.success("Saved");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get Key value of DataModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private object? GetKey(MODEL model)
        {
            var entityType = mDbContext.Model.FindEntityType(typeof(MODEL));
            var keys = entityType?.FindPrimaryKey();

            if (keys != null)
            {
                var key = model.GetType()?.GetProperty(keys.Properties[0].Name)?.GetValue(model);
                return key;
            }
            else {
                return null; 
            }
        }
        #endregion
    }
}
