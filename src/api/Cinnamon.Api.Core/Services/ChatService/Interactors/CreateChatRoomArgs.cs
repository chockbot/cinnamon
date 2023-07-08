using Cinnamon.Framework.Interactor;
using System;
using Cinnamon.Framework.Enums;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors
{
    public class CreateChatRoomArgs : IInteractor
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public Enums.ChatType ChatType { get; set; }
        public string GroupName { get; set; }
        public string ChatName { get; set; }
    }
}
