using Cinnamon.Framework.ApiCommand.ApiCore.System.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface ISystemApiHandler 
{
    Task<AppResult<GetServerDateResult>> GetServerDate();
    Task<AppResult<GetAnnouncementsResult>> GetAnnouncements();
} 
