using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.SystemService.Handlers;
using Cinnamon.Api.Core.Services.SystemService.Interactors;
using Cinnamon.Api.Core.Services.SystemService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.SystemService;

public class GenerateSitemapHandler : IGenerateSitemapHandler
{
    private readonly IGetAllActivitiesHandler getAllActivitiesHandler;
    private readonly ApplicationConfig applicationConfig;
    private readonly IGetAllCustomersHandler getAllCustomersHandler;
    private readonly ILogger logger;

    public GenerateSitemapHandler(IGetAllActivitiesHandler getAllActivitiesHandler, ApplicationConfig applicationConfig,
        IGetAllCustomersHandler getAllCustomersHandler, ILogger<GenerateSitemapHandler> logger)
    {
        this.getAllActivitiesHandler = getAllActivitiesHandler;
        this.applicationConfig = applicationConfig;
        this.getAllCustomersHandler = getAllCustomersHandler;
        this.logger = logger;
    }
    
    public AppResult<GenerateSitemapResult> Execute(GenerateSitemapArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GenerateSitemapResult>.CreateFailed(ex, "An error occured in GenerateSitemapHandler");
        }
    }

    public async Task<AppResult<GenerateSitemapResult>> ExecuteAsync(GenerateSitemapArgs args)
    {
        try
        {
            var path = applicationConfig.Sitemap.SitemapPath;
            var currentDate = DateTime.Now;
            var dateString = currentDate.ToString("yyyy-MM-dd");

            logger.LogInformation("-- starting generating sitemap... --");
            using(StreamWriter sw = new StreamWriter(path))
            {
                await sw.WriteLineAsync("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http:www.w3.org/1999/xhtml\">");
                // write static urls
                await sw.WriteLineAsync("<url>");
                await sw.WriteLineAsync("<loc>https://cinnamon.ph/</loc>");
                await sw.WriteLineAsync($"<lastmod>{dateString}</lastmod>");
                await sw.WriteLineAsync("<priority>0.8</priority>");
                await sw.WriteLineAsync("</url>");
                await sw.WriteLineAsync("<url>");
                await sw.WriteLineAsync("<loc>https://cinnamon.ph/explore</loc>");
                await sw.WriteLineAsync($"<lastmod>{dateString}</lastmod>");
                await sw.WriteLineAsync("<priority>0.8</priority>");
                await sw.WriteLineAsync("</url>");
                await sw.WriteLineAsync("<url>");
                await sw.WriteLineAsync("<loc>https://cinnamon.ph/onboarding</loc>");
                await sw.WriteLineAsync($"<lastmod>{dateString}</lastmod>");
                await sw.WriteLineAsync("<priority>0.8</priority>");
                await sw.WriteLineAsync("</url>");
                await sw.WriteLineAsync("<url>");
                await sw.WriteLineAsync("<loc>https://cinnamon.ph/help</loc>");
                await sw.WriteLineAsync($"<lastmod>{dateString}</lastmod>");
                await sw.WriteLineAsync("<priority>0.8</priority>");
                await sw.WriteLineAsync("</url>");
                await sw.WriteLineAsync("<url>");
                await sw.WriteLineAsync("<loc>https://cinnamon.ph/help/faq</loc>");
                await sw.WriteLineAsync($"<lastmod>{dateString}</lastmod>");
                await sw.WriteLineAsync("<priority>0.8</priority>");
                await sw.WriteLineAsync("</url>");
                await sw.WriteLineAsync("<url>");
                await sw.WriteLineAsync("<loc>https://cinnamon.ph/help/terms</loc>");
                await sw.WriteLineAsync($"<lastmod>{dateString}</lastmod>");
                await sw.WriteLineAsync("<priority>0.8</priority>");
                await sw.WriteLineAsync("</url>");
                await sw.WriteLineAsync("<url>");
                await sw.WriteLineAsync("<loc>https://cinnamon.ph/help/privacy</loc>");
                await sw.WriteLineAsync($"<lastmod>{dateString}</lastmod>");
                await sw.WriteLineAsync("<priority>0.8</priority>");
                await sw.WriteLineAsync("</url>");

                // activities sitemap
                var activitiesRes = await getAllActivitiesHandler.ExecuteAsync(new ActivityService.Interactors.GetAllActivitiesArgs {
                    IncludeActivityDescription = true,
                    IsActive = true,
                    ForceDisable = false
                });
                if(activitiesRes.Succeeded && activitiesRes.Result != null)
                {
                    var activities = activitiesRes.Result.Activities.ToList();
                    for(int i =0, cnt = activities.Count(); i < cnt; i++)
                    {
                        await sw.WriteLineAsync("<url>");
                        await sw.WriteLineAsync($"<loc>https://cinnamon.ph/explore/{activities[i].Handler}</loc>");
                        await sw.WriteLineAsync($"<lastmod>{dateString}</lastmod>");
                        await sw.WriteLineAsync("<priority>0.8</priority>");
                        await sw.WriteLineAsync("</url>");
                    }
                }

                // customers sitemap
                var customerRes = await getAllCustomersHandler.ExecuteAsync(new AccountService.Interactors.GetAllCustomersArgs {

                });
                if(customerRes.Succeeded && customerRes.Result != null)
                {
                    var customers = customerRes.Result.Customers.ToList();
                    for(int i = 0, cnt = customers.Count; i < cnt; i++)
                    {
                        if(customers[i].IsMaker)
                        {
                            await sw.WriteLineAsync("<url>");
                            await sw.WriteLineAsync($"<loc>https://cinnamon.ph/maker/profile/{customers[i].Handler}</loc>");
                            await sw.WriteLineAsync($"<lastmod>{dateString}</lastmod>");
                            await sw.WriteLineAsync("<priority>0.8</priority>");
                            await sw.WriteLineAsync("</url>");
                        }
                    }
                }

                await sw.WriteLineAsync("</urlset>");
            }
            logger.LogInformation("-- Done generating sitemap --");
            return AppResult<GenerateSitemapResult>.CreateSucceeded(new GenerateSitemapResult {}, "Success");
        }
        catch (Exception ex)
        {
            logger.LogError("-- Error when generating sitemap " + ex.Message + " --");
            return AppResult<GenerateSitemapResult>.CreateFailed(ex, "An error occured in GenerateSitemapHandler");
        }
    }
}