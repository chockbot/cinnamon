namespace Cinnamon.Api.Data.Repository.Entities;

public class ChatUnreadNotification : BaseEntity 
{
    public int ChatHistoryId {get; set;}
    public int FromUserId {get; set;}
    public int ToUserId {get; set;}
    public DateTime ChatDate {get; set;}
    public string CustomerEmail {get; set;}
    public string Repeated {get; set;}
}