using Dna;
using Cinnamon.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cinnamon.Data
{
    /// <summary>
    /// Extension methods for the <see cref="FrameworkConstruction"/>
    /// </summary>
    public static class FrameworkConstructionExtensions
    {
        public static FrameworkConstruction UseClientDataStore(this FrameworkConstruction construction)
        {
            // Inject our PSQL data store
            construction.Services.AddDbContext<DataStoreDbContext>(options =>
            {
                string connectionDetails = construction.Configuration.GetConnectionString("CinnamonDB");
                //Lazy Loading
                options.UseLazyLoadingProxies();
                // Setup connection string
                options.UseNpgsql(connectionDetails);
            }, contextLifetime: ServiceLifetime.Transient);

            // Add client data store for easy access/use of the backin data store
            // Make it scoped so we can inject the scoped DbContext
#pragma warning disable CS8604 // Possible null reference argument.
            construction.Services.AddTransient<IDataStore>(
                provider => new DataStore(dbContext: provider.GetService<DataStoreDbContext>()));
#pragma warning restore CS8604 // Possible null reference argument.

            // Return framework for chaining
            return construction;
        }
    }
}
