    namespace Cinnamon.Web.Models.WordPress;
    public class MediaPost
    {
        public int Id { get; set; }
        public RenderedProperty Guid { get; set; }
        public class RenderedProperty
        {
            public string Rendered { get; set; }
        }
    }