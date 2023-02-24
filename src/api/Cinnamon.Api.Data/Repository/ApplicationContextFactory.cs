using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cinnamon.Api.Data.Repository;

public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var opts = new DbContextOptionsBuilder<ApplicationContext>();
        opts.UseNpgsql($"Host=localhost;Database=CINNAMON;Username=postgres;Password=P@ssw0rd");
        return new ApplicationContext(opts.Options);
    }
}