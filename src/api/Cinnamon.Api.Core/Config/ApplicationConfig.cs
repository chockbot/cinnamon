namespace Cinnamon.Api.Core.Config;

public class ApplicationConfig 
{
    public string FrontendUrl {get; set;}
    public string ApiDataUrl {get; set;}
    public JwtSetting Jwt {get; set;}
    public EmailService EmailService {get; set;}
    public FailedLogin FailedLogin {get; set;}
    public Payment Payment {get; set;}
}