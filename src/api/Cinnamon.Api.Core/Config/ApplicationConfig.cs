namespace Cinnamon.Api.Core.Config;

public class ApplicationConfig 
{
    public Disbursement Disbursement {get; set;}
    public string FrontendUrl {get; set;}
    public string ApiDataUrl {get; set;}
    public JwtSetting Jwt {get; set;}
    public EmailService EmailService {get; set;}
    public FailedLogin FailedLogin {get; set;}
    public Payment Payment {get; set;}
    public Activity Activity { get; set; }
    public Sitemap Sitemap {get; set;}
    public ExpiringStudentNotification ExpiringActivityNotification { get; set; }
    public ActivitySummary ActivitySummary {get; set;}
    public EventReminder EventReminder {get; set;}
    public EventThankYou EventThankYou {get; set;}
    public MailChimpConfig MailChimpConfig {get; set;}
}