namespace Cinnamon.Web.Models.Entities;

public class Files {

    public string Token { get; set; }
    public List<File> FileList { get; set; }
}
public class File
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string ThumbnailLink { get; set; }
    public DateTime?  ModifiedDate { get; set; }
}
