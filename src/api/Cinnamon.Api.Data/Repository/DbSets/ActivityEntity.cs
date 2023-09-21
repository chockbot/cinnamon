using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;
using System.Data;
using AutoMapper;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ActivityEntity : GenericEntity<Activity>, IActivity
{
    private readonly ApplicationContext applicationContext;

    public ActivityEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<Activity>>> FindActivitiesAsync(Expression<Func<Activity, bool>> expression, string searchValue,
        int? take = 100, int? skip = 0, IEnumerable<Expression<Func<Activity, object>>>? includes = null)
    {
        try
        {
            int limitCount = take.HasValue ? take.Value : int.MaxValue;
            int skipCount = skip.HasValue ? skip.Value : 0;

            var query = applicationContext.Set<Activity>().OrderBy(a => a.Guid).Where(expression);

            if (!string.IsNullOrEmpty(searchValue))
            {
                string[] keywords = searchValue.ToLower().Trim().Split(' ');

                foreach (string keyword in keywords)
                {
                    query = query.Where(a => (string.IsNullOrEmpty(keyword) ? true : a.Title.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.Address1.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.Address2.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.District.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.CityName.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.Subdivision.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.RegionName.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.BarangayName.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.PostalCode.ToLower().Trim().Contains(keyword) ||
                                                  a.ExperienceType.Name.ToLower().Trim().Contains(keyword) ||
                                                  a.ExperienceCategory.Category.ToLower().Trim().Contains(keyword) ||
                                                  a.SubCategory.SubCatergory.ToLower().Trim().Contains(keyword) ||
                                                  a.Customer.FirstName.ToLower().Trim().Contains(keyword) ||
                                                  a.Customer.LastName.ToLower().Trim().Contains(keyword)
                                                  ));
                }
            }

            query = query.Skip(skipCount).Take(limitCount);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var results = query.AsQueryable();
            return AppResult<IEnumerable<Activity>>.CreateSucceeded(results, "Successfully find entities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<Activity>>.CreateFailed(ex, "An error occured when finding entities");
        }
    }

    public async Task<AppResult<IEnumerable<Activity>>> GetPopularActivities(Expression<Func<Activity, bool>> expression, int? take = 100, int? skip = 0, IEnumerable<Expression<Func<Activity, object>>>? includes = null)
    {
        try
        {
            int limitCount = take.HasValue ? take.Value : int.MaxValue;
            int skipCount = skip.HasValue ? skip.Value : 0;

            var query = applicationContext.Set<Activity>().Where(expression); //.OrderByDescending(a => a.PurchaseOrderCount).Skip(skipCount).Take(limitCount);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var results = await query.ToListAsync();
            return AppResult<IEnumerable<Activity>>.CreateSucceeded(results, "Successfully find entities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<Activity>>.CreateFailed(ex, "An error occured when finding entities");
        }
    }

    public async Task<AppResult<IEnumerable<Activity>>> GetRecommendedActivities(int primaryActivityId, int count)
    {
        try
        {
            var activity = await applicationContext.Activities.FindAsync(primaryActivityId);
            if (activity == null)
            {
                return AppResult<IEnumerable<Activity>>.CreateFailed(new ApplicationException("Invalid activity"), "Invalid activity");
            }

            // based first in sub category
            var activitiesSubs = await applicationContext.Activities.Where(a => a.Id != primaryActivityId && 
                                                                                a.SubCategoryId == activity.SubCategoryId && 
                                                                                a.Status == 1 && a.IsPublished == true)
                                    .Include(a => a.Images)
                                    .Include(a => a.Schedules)
                                    .Include(a => a.Address)
                                    .ToListAsync();

            if (activitiesSubs.Count >= count)
            {
                var randomActivities = GenerateRandomActivity(activitiesSubs, count);
                return AppResult<IEnumerable<Activity>>.CreateSucceeded(randomActivities, "Successfully get recommended activities");
            }

            // bas in experience categories
            var activitiesCats = await applicationContext.Activities.Where(a => a.Id != primaryActivityId && 
                                                                                a.ExperienceCategoryId == activity.ExperienceCategoryId &&
                                                                                a.Status == 1 && a.IsPublished == true )
                                    .Include(a => a.Images)
                                    .Include(a => a.Schedules)
                                    .Include(a => a.Address)
                                    .ToListAsync();

            var activities = GenerateRandomActivity(activitiesCats, count);

            return AppResult<IEnumerable<Activity>>.CreateSucceeded(activities, "Successfully get recommended activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<Activity>>.CreateFailed(ex, "An error occured when getting recommended activities");
        }
    }

    private IEnumerable<Activity> GenerateRandomActivity(IEnumerable<Activity> activities, int count)
    {
        if(activities.Count() < count) return activities;
        IList<Activity> results = new List<Activity>();
        var activityList = activities.ToList();

        while(results.Count != count)
        {
            int random = (new Random()).Next(0, activityList.Count);
            var activity = activityList[random];
            results.Add(activity);
            activityList.RemoveAt(random);
        }

        return results;
    }

    public async Task<AppResult<IEnumerable<PopularActivityDTO>>> PopularActivities(int? take, int? skip)
    {
        try
        {
            var takeValue = take ?? int.MaxValue;
            var skipValue = skip ?? 0;
            var dateString = DateTime.Now.ToString("yyyy-MM-dd");

            string query = "with totalStundets as " +
                           "( " +
                               "select ac.\"Id\", Count(ac.\"Id\") \"StudentCount\" " +
                               "from public.\"Activities\" ac " +
                               "join public.\"Students\" st " +
                                   "on ac.\"Id\" = st.\"ActivityId\" " +
                               "where ac.\"IsPublished\" = true and ac.\"IsDeactivated\" = false " +
                                   "and ac.\"IsNew\" = false " +
                               "group by ac.\"Id\" " +
                           "), " +
                           "topActivities as " +
                           "( " +
                               "select * " +
                               "from totalStundets ts " +
                               "order by ts.\"StudentCount\" desc, ts.\"Id\" " +
                               "limit " + takeValue + " offset " + skipValue + " " +
                           "), " +
                           "withRatings as " +
                           "( " +
                               "select ta.*, Count(ar.\"Id\") \"ReviewCount\", " +
                                   "Trunc(Coalesce(Sum(ar.\"Rating\"::decimal) / Count(ar.\"Id\"),0),1) \"Rating\" " + 
                               "from topActivities ta " +
                               "left join public.\"Reviews\" ar " +
                                   "on ta.\"Id\" = ar.\"ActivityId\" " +
                               "group by ta.\"Id\", ta.\"StudentCount\" " +
                           "), " +
                           "withOngoingStudent as " +
                           "( " +
                               "select ac.*, " +
                                   "(select Count(*) from public.\"Students\" st " +
                                   "where st.\"ActivityId\" = ac.\"Id\" and " +
                                   "((st.\"SessionsAttended\" < st.\"NumberOfSessions\") or " +
                                   "(st.\"ExpirationDateEnd\" != '-infinity' " +
                                       "and Date(st.\"ExpirationDateEnd\") > Date('" + dateString + "')) " +
                                    ") " +
                                   ") \"OngoingStudent\" " +
                               "from withRatings ac " +
                           "), " +
                           "withCoverPhoto as " +
                           "( " +
                               "select aw.*, ac.\"Title\", ac.\"CreatedBy\", ac.\"IsNew\", ac.\"Handler\", " +
                                   "ac.\"ExperienceTypeId\", ac.\"Price\", ad.\"CityName\", ad.\"RegionName\", " +
                                   "Row_Number() over (partition by ac.\"Id\" order by ai.\"Order\", ai.\"Id\") \"RowCnt\", " +
                                   "ai.\"ImageLocation\" " +
                               "from withOngoingStudent aw " +
                               "join public.\"Activities\" ac " +
                                   "on aw.\"Id\" = ac.\"Id\" " +
                               "join public.\"ActivityAddress\" ad " +
                                   "on ac.\"Id\" = ad.\"ActivityId\" " +
                               "join public.\"ActivityImages\" ai " +
                                   "on ac.\"Id\" = ai.\"ActivityId\" " +
                           ") " +
                           "select ac.* " +
                           "from withCoverPhoto ac " +
                           "where ac.\"RowCnt\" = 1 " +
                           "order by ac.\"StudentCount\" desc, ac.\"Id\" ";

            IList<PopularActivityDTO> listResult = new List<PopularActivityDTO>();

            using(var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = System.Data.CommandType.Text;

                applicationContext.Database.OpenConnection();

                using(var dr = await command.ExecuteReaderAsync())
                {
                    if(dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        listResult = dt.AsEnumerable().Select(item => new PopularActivityDTO {
                            CityName = item["CityName"].ToString() ?? string.Empty,
                            ExperienceTypeId = Convert.ToInt32(item["ExperienceTypeId"]),
                            Handler = item["Handler"].ToString() ?? string.Empty,
                            Id = Convert.ToInt32(item["Id"]),
                            ImageSrc = item["ImageLocation"].ToString() ?? string.Empty,
                            IsNew = Convert.ToBoolean(item["IsNew"]),
                            MakerId = Convert.ToInt32(item["CreatedBy"]),
                            OngoingStudentCount = Convert.ToInt32(item["OngoingStudent"]),
                            Price = item["Price"].ToString() ?? string.Empty,
                            Rating = Convert.ToDecimal(item["Rating"]),
                            RegionName = item["RegionName"].ToString() ?? string.Empty,
                            ReviewCount = Convert.ToInt32(item["ReviewCount"]),
                            StudentCount = Convert.ToInt32(item["StudentCount"]),
                            Title = item["Title"].ToString() ?? string.Empty
                        }).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<PopularActivityDTO>>.CreateSucceeded(listResult, "Successfully get popular activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PopularActivityDTO>>.CreateFailed(ex, "An error occured when getting popular activities");
        }
    }

    public async Task<AppResult<Activity>> CreateOteActivity(Activity activity, ActivityDescription description, ActivityAddress address, OteSchedule oteSchedule)
    {
        try
        {
            description.Activity = activity;
            address.Activity = activity;
            oteSchedule.Activity = activity;
            var schedulePricing = oteSchedule.OteSchedulePricing.Select(p => {
                p.OteSchedule = oteSchedule;
                return p;
            });

            this.applicationContext.Activities.Add(activity);
            this.applicationContext.ActivityAddress.Add(address);
            this.applicationContext.ActivityDescriptions.Add(description);
            this.applicationContext.OteSchedules.Add(oteSchedule);
            this.applicationContext.OteSchedulePricings.AddRange(schedulePricing);
            await this.applicationContext.SaveChangesAsync();

            return AppResult<Activity>.CreateSucceeded(activity, "One time activity successfully created.");
        }
        catch (Exception ex) 
        {
            return AppResult<Activity>.CreateFailed(ex, "An error occured when creating One time event.");
        }
    }

    public async Task<AppResult<Activity>> UpdateOteActivity(Activity activity, ActivityDescription description, ActivityAddress address, OteSchedule oteSchedule)
    {
        try
        {
            var activityResult = await applicationContext.Activities
                    .Where(a => a.Id == activity.Id)
                    .Include(a => a.ActivityDescription)
                    .Include(a => a.Address)
                    .Include(a => a.OteSchedule)
                    .ThenInclude(p => p.OteSchedulePricing)
                    .FirstOrDefaultAsync();
            
            if(activityResult is not null)
            {
                activityResult.Description = activity.Description;
                activityResult.Title = activity.Title;
                activityResult.ExperienceTypeId = activity.ExperienceTypeId;
                activityResult.Price = activity.Price;
                activityResult.IsPublished = activity.IsPublished;
                activityResult.ExperienceCategoryId = activity.ExperienceCategoryId;
                activityResult.SubCategoryId = activity.SubCategoryId;
                activityResult.Handler = activity.Handler;
                activityResult.IsDeactivated = activity.IsDeactivated;
                activityResult.Status = activity.Status;

                activityResult.ActivityDescription.Description = description.Description;

                activityResult.Address.Address1 = address.Address1;
                activityResult.Address.City = address.City;
                activityResult.Address.CityName = address.CityName;
                activityResult.Address.Barangay = address.Barangay;
                activityResult.Address.BarangayName = address.BarangayName;
                activityResult.Address.Region = address.Region;
                activityResult.Address.RegionName = address.RegionName;
                activityResult.Address.PinnedLocation = address.PinnedLocation;
                activityResult.Address.PostalCode = address.PostalCode;

                activityResult.OteSchedule.From = oteSchedule.From;
                activityResult.OteSchedule.To = oteSchedule.To;
                activityResult.OteSchedule.Recurrences = oteSchedule.Recurrences;

                var updatedPricingList = oteSchedule.OteSchedulePricing.Where(p => p.Id > 0);
                foreach(var item in activityResult.OteSchedule.OteSchedulePricing)
                {
                    var local = updatedPricingList.FirstOrDefault(p => p.Id == item.Id);
                    if(local is not null)
                    {
                        item.Description = local.Description;
                        item.IsAbsorbFees = local.IsAbsorbFees;
                        item.MaxSlots = local.MaxSlots;
                        item.Price = local.Price;
                    }
                }

                var newPricingList = oteSchedule.OteSchedulePricing.Where(p => p.Id == 0);
                foreach(var item in newPricingList)
                {
                    activityResult.OteSchedule.OteSchedulePricing.Add(item);
                }

                await applicationContext.SaveChangesAsync();
            }

            return AppResult<Activity>.CreateSucceeded(activityResult, "One time event successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<Activity>.CreateFailed(ex, "An error occured when updating One time event");
        }
    }

    public async Task<AppResult<Activity>> FindOteByHandler(string handler, bool includeDescription = false, bool includeAddress = false,
        bool includeSchedule = false, bool includePricing = false, bool includeProvider = false)
    {
        try
        {
            var query = applicationContext.Activities.Where(a => a.Handler.ToLower() == handler.ToLower());

            if(includeAddress) query = query.Include(a => a.Address);
            if(includeDescription) query = query.Include(a => a.ActivityDescription);
            if(includeProvider) query = query.Include(a => a.Customer);
            if(includeSchedule && includePricing) query = query.Include(a => a.OteSchedule).ThenInclude(a => a.OteSchedulePricing);
            if(includeSchedule && !includePricing) query = query.Include(a => a.OteSchedule);

            var result = await query.FirstOrDefaultAsync();

            if(result is null)
            {
                return AppResult<Activity>.CreateFailed(new ApplicationException("Can't find ote activity"), "Can't find ote activity");
            }

            return AppResult<Activity>.CreateSucceeded(result, "Successfully find ote activity");
        }
        catch (Exception ex)
        {
            return AppResult<Activity>.CreateFailed(ex, "An error occured when finding ote activity");
        }
    }
}