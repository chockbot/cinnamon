using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class AnnouncementEntity : GenericEntity<Announcement>, IAnnouncement 
{
    public AnnouncementEntity(ApplicationContext applicationContext):base(applicationContext)
    {
        
    }
}