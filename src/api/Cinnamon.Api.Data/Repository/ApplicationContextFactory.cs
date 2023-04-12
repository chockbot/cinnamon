using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cinnamon.Api.Data.Repository;

public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var opts = new DbContextOptionsBuilder<ApplicationContext>();
        opts.UseNpgsql($"Host=10.192.197.239;Database=CINNAMON;Username=postgres;Password=postgres");
        return new ApplicationContext(opts.Options);
    }
}