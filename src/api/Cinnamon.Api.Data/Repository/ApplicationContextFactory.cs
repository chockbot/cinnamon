using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cinnamon.Api.Data.Repository;

public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var opts = new DbContextOptionsBuilder<ApplicationContext>();
        opts.UseNpgsql($"Server=10.127.70.79;Port=5432;Database=CINNAMON;User Id=postgres;Password=postgres;");
        return new ApplicationContext(opts.Options);
    }
}