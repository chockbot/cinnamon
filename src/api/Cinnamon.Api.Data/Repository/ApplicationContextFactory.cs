using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cinnamon.Api.Data.Repository;

public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var opts = new DbContextOptionsBuilder<ApplicationContext>();
        opts.UseNpgsql($"Host=cinnamondevserver.postgres.database.azure.com;Database=postgres;Username=adminuser;Password=Cinnamon_01");
        return new ApplicationContext(opts.Options);
    }
}