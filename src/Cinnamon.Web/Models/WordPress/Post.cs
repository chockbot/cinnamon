namespace Cinnamon.Web.Models.WordPress;      
public class Post
{   
    public int Id { get; set; }
    public string Date { get; set; }
    public RenderedProperty Title { get; set; }
    public RenderedProperty Excerpt { get; set; }
    public int Featured_Media { get; set; }
    public string Link { get; set; }
    public string ImageLink { get; set; }
    public class RenderedProperty
    {
        public string Rendered { get; set; }
    }
}