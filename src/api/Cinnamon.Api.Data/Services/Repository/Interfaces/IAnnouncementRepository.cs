using Cinnamon.Framework.ApiCommand.ApiData.DTO.Announcement;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IAnnouncementRepository 
{
    Task<AppResult<AnnouncementDTO>> CreateAnnouncement(AnnouncementDTO announcement);
    Task<AppResult<AnnouncementDTO>> UpdateAnnouncement(AnnouncementDTO announcement);
    Task<AppResult<AnnouncementDTO>> DeleteAnnouncement(AnnouncementDTO announcement);
    Task<AppResult<IEnumerable<AnnouncementDTO>>> GetAllAnnouncements();
}