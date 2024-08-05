namespace Cinnamon.Web.Models.Entities;

    public class WordPressPost
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public RenderedProperty Title { get; set; }
        public RenderedProperty Content { get; set; }

        public class RenderedProperty
        {
            public string Rendered { get; set; }
        }
    }