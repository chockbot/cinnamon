using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cinnamon.Data
{
    public class DataStoreDbContextFactory : IDesignTimeDbContextFactory<DataStoreDbContext>
    {
        public DataStoreDbContext CreateDbContext(string[] args) {
            var options = new DbContextOptionsBuilder<DataStoreDbContext>();
            options.UseNpgsql($"Host=192.168.254.106;Database=CINNAMON;Username=postgres;Password=postgres");
            return new DataStoreDbContext(options.Options);
        }
    }
}
