using System.Linq;
using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    public class Activities : BaseDbSet<ActivityModel>, IActivities
    {
        public Activities(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<ActivityModel> Table => mDbContext.Activities;

        public async Task<ActivityModel> GetActivityByIdAsync(int id)
            => await mDbContext.Activities.FirstOrDefaultAsync(i => i.Id == id);

        public async Task<IList<ActivityModel>> GetActivityByCreatedId(int id)
            => await mDbContext.Activities.Where(i => i.CreatedBy == id).ToListAsync();
        
        public async Task<ActivityModel> SlowUpdateActivityAsync(ActivityModel activity)
        {
            var getActivity = await mDbContext.Activities.FirstOrDefaultAsync(i => i.Id == activity.Id);
            if(getActivity == null) return getActivity;

            // update activity fields
            getActivity.ActivityTypeId = activity.ActivityTypeId;
            getActivity.ExperienceTypeId = activity.ExperienceTypeId;
            getActivity.Title = activity.Title;
            getActivity.Location = activity.Location;
            getActivity.Price = activity.Price;
            getActivity.ExperienceCategoryId = activity.ExperienceCategoryId;
            getActivity.IsPublished = activity.IsPublished;

            // update activityImages
            var imageIds = getActivity.ActivityImages.Select(i => i.Id).ToArray();
            var images = await mDbContext.ActivityImages.Where(i => imageIds.Contains(i.Id)).ToListAsync();
            foreach(var item in images)
            {
                var im = activity.ActivityImages.FirstOrDefault(i => i.Id == item.Id);
                // insert new item
                if(im != null)
                {
                    item.ImageLocation = im.ImageLocation;
                    item.ImageName = im.ImageName;
                }
            }

            // address field
            if(activity.Address != null && activity.Address.Id != 0)
            {
                var address = await mDbContext.Addresses.FirstOrDefaultAsync(i => i.Id == activity.Address.Id);
                if(address != null)
                {
                    address.Address1 = activity.Address.Address1;
                    address.Address2 = activity.Address.Address2;
                    address.District = activity.Address.District;
                    address.City = activity.Address.City;
                }
            }

            // description
            if(activity.DescriptionSectionModel != null && activity.DescriptionSectionModel.Id != 0)
            {
                var description = await mDbContext.Descriptions.FirstOrDefaultAsync(i => i.Id == activity.DescriptionSectionModel.Id);
                if(description != null)
                {
                    description.Description = activity.DescriptionSectionModel.Description;
                    description.SpecificsYouWillProvide = activity.DescriptionSectionModel.SpecificsYouWillProvide;
                    description.CustomerBringWithThem = activity.DescriptionSectionModel.CustomerBringWithThem;
                    description.AdditionalRequirements = activity.DescriptionSectionModel.AdditionalRequirements;
                    description.ActivityLevel = activity.DescriptionSectionModel.ActivityLevel;
                    description.SkillLevel = activity.DescriptionSectionModel.SkillLevel;
                    description.MinimumAge = activity.DescriptionSectionModel.MinimumAge;
                    description.CanAdultsJoin = activity.DescriptionSectionModel.CanAdultsJoin;
                }
            }

            // update and add schedules
            if(activity.ScheduleList != null && activity.ScheduleList.Count > 0)
            {
                var schedIds = activity.ScheduleList.Where(i => i.Id > 0).Select(i => i.Id).ToArray();
                var schedules = await mDbContext.Schedules.Where(i => schedIds.Contains(i.Id)).ToListAsync();

                foreach(var item in schedules)
                {
                    var sched = activity.ScheduleList.FirstOrDefault(i => i.Id == item.Id);
                    if(sched != null)
                    {
                        item.Name = sched.Name;
                        item.DateTime = sched.DateTime;
                        item.Price = sched.Price;
                        item.UnitPrice = sched.UnitPrice;
                        item.PerUnit1 = sched.PerUnit1;
                        item.PriceUnit1 = sched.PriceUnit1;
                        item.PerUnit2 = sched.PerUnit2;
                        item.PriceUnit2 = sched.PriceUnit2;
                    }
                }

                // add new schedules
                var newSchedules = activity.ScheduleList.Where(i => i.Id == 0 || i.Id is null);
                foreach(var item in newSchedules)
                {
                    item.ActivityId = activity.Id;
                }
                mDbContext.Schedules.AddRange(newSchedules);
            }

            // search tags
            if(activity.SearchTagsModel != null && activity.SearchTagsModel.Id > 0)
            {
                var searchTag = await mDbContext.SearchTags.FirstOrDefaultAsync(i => i.Id == activity.SearchTagsModel.Id);
                if(searchTag != null)
                {
                    searchTag.SearchTag1 = activity.SearchTagsModel.SearchTag1;
                    searchTag.SearchTag2 = activity.SearchTagsModel.SearchTag2;
                    searchTag.SearchTag3 = activity.SearchTagsModel.SearchTag3;
                    searchTag.SearchTag4 = activity.SearchTagsModel.SearchTag4;
                    searchTag.SearchTag5 = activity.SearchTagsModel.SearchTag5;
                }
            }

            // save the changes
            await mDbContext.SaveChangesAsync();

            // get updated activity
            var updatedActivity = await mDbContext.Activities.FirstOrDefaultAsync(i => i.Id == activity.Id);

            return updatedActivity;
        }
    }
}
