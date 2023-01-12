namespace Cinnamon.Api.Core.Config;

public class EmailService 
{
    public string GrantType {get; set;}
    public string UserId {get; set;}
    public string ClientId {get; set;}
    public string ClientSecret {get; set;}
    public string DirectoryTenantId {get; set;}
    public string Resource {get; set;}
    public string Email { get; set; }
}