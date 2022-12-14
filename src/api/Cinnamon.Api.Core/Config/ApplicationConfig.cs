namespace Cinnamon.Api.Core.Config;

public class ApplicationConfig 
{
    public string ApiDataUrl {get; set;}
    public JwtSetting Jwt {get; set;}
    public EmailService EmailService {get; set;}
}