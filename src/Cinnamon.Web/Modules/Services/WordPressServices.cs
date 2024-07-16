using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Models.WordPress;
using Cinnamon.Web.Modules.Services.Handlers;

namespace Cinnamon.Web.Modules.Services;

    public class WordPressServices : IWordPressServicesHandler
    {
        private readonly IFlurlClient flurlClient;
        private readonly ILogger _logger;

        public WordPressServices(IFlurlClientFactory flurlFac, Config.Config config, ILogger<WordPressServices> logger)
        {
            flurlClient = flurlFac.Get("https://cinnamon.ph/blog/wp-json/wp/v2/");
            _logger = logger;
        }

        public async Task<AppResult<List<Post>>> GetPostsAsync()
        {
            try
            {
                var result = await flurlClient.Request("/posts").GetJsonAsync<List<Post>>();
                return AppResult<List<Post>>.CreateSucceeded(result, "Wordpress API connection is successful");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<List<Post>>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<List<Post>>.CreateFailed(ex, "An error occurred when getting WordPress API");
            }
        }

        public async Task<AppResult<MediaPost>> GetMediaAsync(int mediaId)
        {
            try
            {
                var result = await flurlClient.Request($"/media/{mediaId}").GetJsonAsync<MediaPost>();
                return AppResult<MediaPost>.CreateSucceeded(result, "Wordpress MediaPost API connection is successful");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<MediaPost>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<MediaPost>.CreateFailed(ex, "An error occurred when getting WordPress MediaPost API");
            }
        }
    }