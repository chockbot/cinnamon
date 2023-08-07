namespace Cinnamon.Web.Config;

public class Config 
{
    public string BaseUrl { get; set; }
    public string ApiUrl {get; set;}
    public string Environment {get; set;}
    public Authentication Authentication { get; set; }
}

public class Authentication
{
    public Calendly Calendly { get; set; }
}
public class Calendly
{
    public string MainUrl { get; set; }
    public string AuthUrl { get; set; }
    public string ApiUrl { get; set; }
    public string RedirectUri { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string UpdateRedirectUri { get; set; }
    public string UpdateClientId { get; set; }
    public string UpdateClientSecret { get; set; }
}