using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Modules.Services
{
    public class WordPressServices
    {
        private const string WordPressAPI = "https://cinnamon.ph/blog/wp-json/wp/v2";

        public async Task<List<WordPressPost>> GetPostsAsync()
        {
            try
            {
                var posts = await $"{WordPressAPI}/posts"
                    .GetJsonAsync<List<WordPressPost>>();
                return posts;
            }
            catch (FlurlHttpException ex)
            {
                // Handle HTTP errors here (e.g., log them)
                throw;
            }
        }
    }
}