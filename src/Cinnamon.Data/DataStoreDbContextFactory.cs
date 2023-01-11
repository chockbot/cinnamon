using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cinnamon.Data
{
    public class DataStoreDbContextFactory : IDesignTimeDbContextFactory<DataStoreDbContext>
    {
        public DataStoreDbContext CreateDbContext(string[] args) {
            var options = new DbContextOptionsBuilder<DataStoreDbContext>();
            options.UseNpgsql($"Host=db;Database=CINNAMON;Username=postgres;Password=postgres");
            return new DataStoreDbContext(options.Options);
        }
    }
}
