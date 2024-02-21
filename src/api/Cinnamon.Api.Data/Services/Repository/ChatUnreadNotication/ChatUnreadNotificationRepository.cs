using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatUnreadNotification;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Extensions;

namespace Cinnamon.Api.Data.Services.Repository.ChatUnreadNotification;

public class ChatUnreadNotificationRepository : IChatUnreadNotificationRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public ChatUnreadNotificationRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<ChatUnreadNotificationDTO>> CreateChatUnreadNotification(ChatUnreadNotificationDTO chatUnreadNotificationDto)
    {
        try
        {
            var entity = mapper.Map<Entities.ChatUnreadNotification>(chatUnreadNotificationDto);
            entity.ChatDate = entity.ChatDate.SetKindUtc();

            var result = await dataStore.ChatUnreadNotification.Add(entity);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<ChatUnreadNotificationDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<ChatUnreadNotificationDTO>(result.Result);
            return AppResult<ChatUnreadNotificationDTO>.CreateSucceeded(dto, "Successfully create chat unread notification");
        }
        catch (Exception ex)
        {
            return AppResult<ChatUnreadNotificationDTO>.CreateFailed(ex, "An error occured when creating chat unread notification.");
        }
    }

    public async Task<AppResult<IEnumerable<ChatUnreadNotificationDTO>>> GetUnreadMessages()
    {
        try
        {
            var result = await dataStore.ChatUnreadNotification.GetUnreadMessages();
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<ChatUnreadNotificationDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtos = mapper.Map<IEnumerable<ChatUnreadNotificationDTO>>(result.Result);
            return AppResult<IEnumerable<ChatUnreadNotificationDTO>>.CreateSucceeded(dtos, "Successfully get unread messages.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ChatUnreadNotificationDTO>>.CreateFailed(ex, "An error occured when getting unread messages.");
        }
    }
}