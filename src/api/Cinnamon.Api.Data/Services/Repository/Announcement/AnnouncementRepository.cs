using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Announcement;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.Announcement;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public AnnouncementRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<AnnouncementDTO>> CreateAnnouncement(AnnouncementDTO announcement)
    {
        try
        {
            var entity = mapper.Map<Entities.Announcement>(announcement);

            var result = await dataStore.Announcement.Add(entity);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<AnnouncementDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<AnnouncementDTO>(result.Result);
            return AppResult<AnnouncementDTO>.CreateSucceeded(dto, "Successfully create announcement.");
        }
        catch (Exception ex)
        {
            return AppResult<AnnouncementDTO>.CreateFailed(ex, "An error occured when creating announcement.");
        }
    }

    public async Task<AppResult<AnnouncementDTO>> UpdateAnnouncement(AnnouncementDTO announcement)
    {
        try
        {
            var entityRes = await dataStore.Announcement.GetByIdAsync(announcement.Id);
            if(!entityRes.Succeeded || entityRes.Result is null)
            {
                return AppResult<AnnouncementDTO>.CreateFailed(new ApplicationException(entityRes.Message), entityRes.Message);
            }
            var entity = entityRes.Result;

            entity.AdminId = announcement.AdminId > 0 ? announcement.AdminId : entity.AdminId;
            entity.ButtonLabel = announcement.ButtonLabel ?? entity.ButtonLabel;
            entity.Description = announcement.Description ?? entity.Description;
            entity.Link = announcement.Link ?? entity.Link;
            entity.Status = announcement.Status ?? entity.Status;
            entity.Title = announcement.Title ?? entity.Title;

            var result = await dataStore.Announcement.Update(entity);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<AnnouncementDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<AnnouncementDTO>(result.Result);
            return AppResult<AnnouncementDTO>.CreateSucceeded(dto, "Successfully update announcement.");
        }
        catch (Exception ex)
        {
            return AppResult<AnnouncementDTO>.CreateFailed(ex, "An error occured when updating announcement.");
        }
    }

    public async Task<AppResult<AnnouncementDTO>> DeleteAnnouncement(AnnouncementDTO announcement)
    {
        try
        {
            var entity = mapper.Map<Entities.Announcement>(announcement);

            var result = await dataStore.Announcement.Remove(entity);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<AnnouncementDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<AnnouncementDTO>.CreateSucceeded(announcement, "Successfully removed announcement.");
        }
        catch (Exception ex)
        {
            return AppResult<AnnouncementDTO>.CreateFailed(ex, "An error occured when removing announcement.");
        }
    }

    public async Task<AppResult<IEnumerable<AnnouncementDTO>>> GetAllAnnouncements()
    {
        try
        {
            var result = await dataStore.Announcement.GetAllAsync();
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<AnnouncementDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtos = mapper.Map<IEnumerable<AnnouncementDTO>>(result.Result);
            return AppResult<IEnumerable<AnnouncementDTO>>.CreateSucceeded(dtos, "Successfully get announcements.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<AnnouncementDTO>>.CreateFailed(ex, "An error occured when getting announcements.");
        }
    }
}