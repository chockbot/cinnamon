using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.Enums;
using System;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors.Results
{
    public class GetChatMembersByChatRoomIdResult
    {
        public ErrorInfo? ErrorInfo { get; set; }
        public Pagination? Pagination { get; set; }
        public IEnumerable<ChatMembers> ChatMembers { get; set; }
    }

    public class ChatMembers
    {
        public int FromUserId { get; set; }
        public string FromLastName { get; set; }
        public string FromFirstName { get; set; }
        public string? FromProfilePath { get; set; }
        public string? FromProfileLink { get; set; }
        public Enums.ChatMemberType ChatMemberType { get; set; }
    }
}
