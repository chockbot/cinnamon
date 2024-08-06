namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatRoom;

public class ChatMemberDTO 
{
    public int Id {get; set;}
    public int ChatRoomId {get; set;}
    public int CustomerId {get; set;}
    public bool HasLeft {get; set;}
    public int ChatMemberType {get; set;}
}