using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;
using System.Data;
using AutoMapper;
using Npgsql;

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

    public async Task<AppResult<IEnumerable<ActivityFeedDTO>>> PopularActivities(int? take, int? skip, int? categoryId)
    {
        try
        {
            var takeValue = take ?? 50;
            var skipValue = skip ?? 0;

            string categoryFilter = categoryId is not null ? "and ac.\"ExperienceCategoryId\" = " + categoryId + " " : string.Empty;

            string query = "select ac.\"Id\", ac.\"Title\", ac.\"Handler\", ac.\"ExperienceTypeId\", ac.\"ExperienceCreationTypeId\", " +
                               "ad.\"CityName\", ad.\"RegionName\", ad.\"PinnedLocation\", su.\"ImageBannerSrc\", " +
                               "su.\"Ongoing\", su.\"Completed\", su.\"TotalReviews\", su.\"ReviewAccumulated\", " +
                               "su.\"TotalParticipants\", ac.\"Price\", ac.\"IsNew\" " +
                           "from public.\"Activities\" ac " +
                           "left join public.\"ActivityAddress\" ad " +
                               "on ac.\"Id\" = ad.\"ActivityId\" " +
                           "left join public.\"ActivitySummaries\" su " +
                               "on ac.\"Id\" = su.\"ActivityId\" " +
                           "where ac.\"IsDeactivated\" = false and ac.\"Status\" = 1 " +
                               "and ac.\"IsPublished\" = true and ac.\"ForceDisable\" = false " + categoryFilter +
                           "order by su.\"TotalParticipants\" desc " +
                           "limit " + takeValue + " offset " + skipValue + " ";

            IList<ActivityFeedDTO> listResult = new List<ActivityFeedDTO>();

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

                        listResult = dt.AsEnumerable().Select(item => new ActivityFeedDTO {
                            ActivityId = Convert.ToInt32(item["Id"]),
                            ExperienceCreationTypeId = Convert.ToInt32(item["ExperienceCreationTypeId"]),
                            ExperienceTypeId = Convert.ToInt32(item["ExperienceTypeId"]),
                            Handler = item["Handler"].ToString() ?? string.Empty,
                            Price = item["Price"].ToString() ?? string.Empty,
                            Title = item["Title"].ToString() ?? string.Empty,
                            IsNew = Convert.ToBoolean(item["IsNew"]),
                            Address = new ActivityFeedDTO.Location {
                                City = item["CityName"].ToString() ?? string.Empty,
                                PinnedLocation = item["PinnedLocation"].ToString() ?? string.Empty,
                                Region = item["RegionName"].ToString() ?? string.Empty,
                            },
                            SummaryDetails = new ActivityFeedDTO.Summary {
                                Completed = Convert.ToInt32(item["Completed"]),
                                ImageSrc = item["ImageBannerSrc"].ToString() ?? string.Empty,
                                Ongoing = Convert.ToInt32(item["Ongoing"]),
                                ReviewAccumulated = Convert.ToDecimal(item["ReviewAccumulated"]),
                                TotalParticipants = Convert.ToInt32(item["TotalParticipants"]),
                                TotalReviews = Convert.ToInt32(item["TotalReviews"])
                            }
                        }).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<ActivityFeedDTO>>.CreateSucceeded(listResult, "Successfully get popular activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityFeedDTO>>.CreateFailed(ex, "An error occured when getting popular activities");
        }
    }

    public async Task<AppResult<Activity>> CreateOteActivity(Activity activity, ActivityDescription description, 
        ActivityAddress address, OteSchedule oteSchedule, IList<OteSchedulePricingGroup> schedulePricingGroups,
        IList<OteDate> oteDates, IList<OteDateOverride> dateOverrides, IList<OteOnlineEvent> oteOnlineEvents)
    {
        try
        {
            description.Activity = activity;
            address.Activity = activity;
            oteSchedule.Activity = activity;

            this.applicationContext.Activities.Add(activity);
            this.applicationContext.ActivityAddress.Add(address);
            this.applicationContext.ActivityDescriptions.Add(description);
            this.applicationContext.OteSchedules.Add(oteSchedule);
            this.applicationContext.OteSchedulePricingGroups.AddRange(schedulePricingGroups);
            this.applicationContext.OteDates.AddRange(oteDates);
            if (oteOnlineEvents != null)
            {
                this.applicationContext.OteOnlineEvent.AddRange(oteOnlineEvents);
            }
            this.applicationContext.OteDateOverrides.AddRange(dateOverrides);
            
            
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
            var activityResult = applicationContext.Activities
                    .Where(a => a.Id == activity.Id);
            
            activityResult = activityResult.Include(a => a.ActivityDescription);
            activityResult = activityResult.Include(a => a.Address);
            activityResult = activityResult.Include(a => a.OteSchedule);
            activityResult = activityResult.Include(a => a.OteSchedule).ThenInclude(s => s.OteDates).ThenInclude(d => d.OteSchedulePricing);
            activityResult = activityResult.Include(a => a.OteSchedule).ThenInclude(s => s.OteSchedulePricing);
            activityResult = activityResult.Include(a => a.OteSchedule).ThenInclude(s => s.OteSchedulePricingGroups);
            if (oteSchedule.OteOnlineEvent != null)
            {
                activityResult = activityResult.Include(a => a.OteSchedule).ThenInclude(s => s.OteOnlineEvent);
            }
            var result = await activityResult.FirstOrDefaultAsync();
            
            if(result is not null)
            {
                result.Description = activity.Description;
                result.Title = activity.Title;
                result.ExperienceTypeId = activity.ExperienceTypeId;
                result.Price = activity.Price;
                result.IsPublished = activity.IsPublished;
                result.ExperienceCategoryId = activity.ExperienceCategoryId;
                result.Handler = activity.Handler;
                result.IsPublished = activity.IsPublished;
                result.IsComingSoon = activity.IsComingSoon;

                result.ActivityDescription.Description = description.Description;

                result.Address.Address1 = address.Address1;
                result.Address.City = address.City;
                result.Address.CityName = address.CityName;
                result.Address.Barangay = address.Barangay;
                result.Address.BarangayName = address.BarangayName;
                result.Address.Region = address.Region;
                result.Address.RegionName = address.RegionName;
                result.Address.PinnedLocation = address.PinnedLocation;
                result.Address.PostalCode = address.PostalCode;

                // disable update for ote schedule
                // result.OteSchedule.From = oteSchedule.From;
                // result.OteSchedule.To = oteSchedule.To;
                // result.OteSchedule.Recurrences = oteSchedule.Recurrences;

                var updatedPricingList = oteSchedule.OteSchedulePricing.Where(p => p.Id > 0);
                foreach(var price in updatedPricingList)
                {
                    var priceGroup = result.OteSchedule.OteSchedulePricingGroups.FirstOrDefault(p => p.Id == price.Id);
                    if(priceGroup is not null)
                    {
                        priceGroup.Description = price.Description;
                        priceGroup.IsAbsorbFees = price.IsAbsorbFees;
                        priceGroup.MaxSlots = price.MaxSlots;
                        priceGroup.Price = price.Price;
                        priceGroup.Name = price.Name;

                        var priceList = result.OteSchedule.OteSchedulePricing.Where(p => p.OteSchedulePricingGroupId == priceGroup.Id);
                        if(priceList is not null)
                        {
                            foreach(var ticketPrice in priceList)
                            {
                                ticketPrice.Description = price.Description;
                                ticketPrice.IsAbsorbFees = price.IsAbsorbFees;
                                ticketPrice.MaxSlots = price.MaxSlots;
                                ticketPrice.Price = price.Price;
                                ticketPrice.Name = price.Name;
                            }
                        }
                    }
                }

                var newPricingList = oteSchedule.OteSchedulePricing.Where(p => p.Id == 0);
                var newPricingGroups = newPricingList.Select(p => {
                    return new OteSchedulePricingGroup {
                        Description = p.Description,
                        IsAbsorbFees = p.IsAbsorbFees,
                        MaxSlots = p.MaxSlots,
                        Name = p.Name,
                        Price = p.Price,
                        OteSchedule = result.OteSchedule
                    };
                });
                
                foreach(var item in newPricingGroups)
                {
                    result.OteSchedule.OteSchedulePricingGroups.Add(item);

                    foreach(var oteDate in result.OteSchedule.OteDates)
                    {
                        oteDate.OteSchedulePricing.Add(new OteSchedulePricing {
                            Description = item.Description,
                            IsAbsorbFees = item.IsAbsorbFees,
                            MaxSlots = item.MaxSlots,
                            Name = item.Name,
                            Price = item.Price,
                            TicketSold = item.TicketSold,
                            OteSchedule = result.OteSchedule,
                            OteSchedulePricingGroup = item,
                        });
                    }
                }

                if (result.OteSchedule.OteOnlineEvent != null)
                {
                    // Update Online Events
                    var updatedOnlineEvents = oteSchedule.OteOnlineEvent.Where(p => p.Id > 0);
                    foreach (var onlineEvent in updatedOnlineEvents)
                    {
                        var existingOnlineEvent = result.OteSchedule.OteOnlineEvent.FirstOrDefault(e => e.Id == onlineEvent.Id);
                        if (existingOnlineEvent is not null)
                        {
                            existingOnlineEvent.Title = onlineEvent.Title;
                            existingOnlineEvent.Description = onlineEvent.Description;
                            existingOnlineEvent.Videolink = onlineEvent.Videolink;
                            existingOnlineEvent.TicketRestriction = onlineEvent.TicketRestriction;
                            existingOnlineEvent.OteScheduleId = result.OteSchedule.Id;
                        }
                    }

                    // Add New Online Events
                    var newOnlineEvents = oteSchedule.OteOnlineEvent.Where(p => p.Id == 0);
                    foreach (var newEvent in newOnlineEvents)
                    {
                        var newOnlineEvent = new OteOnlineEvent
                        {
                            Title = newEvent.Title,
                            Description = newEvent.Description,
                            Videolink = newEvent.Videolink,
                            TicketRestriction = newEvent.TicketRestriction,
                            OteScheduleId = result.OteSchedule.Id,
                        };
                        result.OteSchedule.OteOnlineEvent.Add(newOnlineEvent);
                    }
                }


                await applicationContext.SaveChangesAsync();
            }

            return AppResult<Activity>.CreateSucceeded(result, "One time event successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<Activity>.CreateFailed(ex, "An error occured when updating One time event");
        }
    }

    public async Task<AppResult<Activity>> FindOteByHandler(string handler, bool includeDescription = false, bool includeAddress = false,
        bool includeSchedule = false, bool includePricing = false, bool includeProvider = false, bool includeImages = false, bool includeOnlineEvent = false, bool includeTickets = false)
    {
        try
        {
            var query = applicationContext.Activities.Where(a => a.Handler.ToLower() == handler.ToLower() && a.ExperienceCreationTypeId == 3);

            if(includeAddress) query      = query.Include(a => a.Address);
            if(includeDescription) query  = query.Include(a => a.ActivityDescription);
            if(includeProvider) query     = query.Include(a => a.Customer);
            if (includeImages) query      = query.Include(a => a.Images);
            if(includeTickets) query      = query.Include(a => a.Tickets);
            if (includeOnlineEvent)query = query.Include(a => a.OteSchedule).ThenInclude(a => a.OteOnlineEvent);
            if (includeSchedule && includePricing) {
                query = query.Include(a => a.OteSchedule).ThenInclude(a => a.OteDates);
                query = query.Include(a => a.OteSchedule).ThenInclude(a => a.OteSchedulePricing);
                query = query.Include(a => a.OteSchedule).ThenInclude(a => a.OteSchedulePricingGroups);
            }
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

    public async Task<AppResult<IEnumerable<ActivityDTO>>> GetOTEByProvider(int Id)
    {
        try
        {
            string query = "WITH MaxSlotsSum AS (\r\n" +
                "SELECT \"OteScheduleId\", SUM(\"TicketSold\") AS \"TotalOteTickets\", SUM(\"MaxSlots\") AS \"TotalMaxSlots\" \r\n" +
                "FROM public.\"OteSchedulePricings\"\r\n" +
                "GROUP BY \"OteScheduleId\")\r\nSELECT a.\"Id\", a.\"ExperienceTypeId\", a.\"Title\", a.\"Description\", a.\"CreatedOn\",\r\n" +
                "a.\"CreatedBy\", a.\"Handler\", a.\"Status\", a.\"ExperienceCreationTypeId\", d.\"CityName\",\r\n" +
                "d.\"RegionName\", d.\"PinnedLocation\", b.\"From\", b.\"To\", ms.\"OteScheduleId\", ms.\"TotalMaxSlots\",ms.\"TotalOteTickets\",\r\n" +
                "(SELECT \"ImageLocation\" FROM public.\"ActivityImages\" WHERE \"ActivityId\" = a.\"Id\" ORDER BY \"Id\" LIMIT 1) AS \"EventImage\"\r\n" +
                "FROM public.\"Activities\" as a\r\nJOIN public.\"OteSchedules\" as b ON a.\"Id\" = b.\"ActivityId\"\r\n" +
                "JOIN MaxSlotsSum as ms ON ms.\"OteScheduleId\" = b.\"Id\"\r\nJOIN public.\"ActivityAddress\" as d ON d.\"ActivityId\" = a.\"Id\"\r\n" +
                "WHERE a.\"CreatedBy\" = "+ Id + " AND a.\"ExperienceCreationTypeId\" = 3;";
                
            IList<ActivityDTO> listResult = new List<ActivityDTO>();
            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);
                        //Get Activity
                        listResult = dt.AsEnumerable().Select(item => new ActivityDTO
                        {
                            Id               = Convert.ToInt32(item["Id"]),
                            ExperienceTypeId = Convert.ToInt32(item["ExperienceTypeId"]),
                            Title            = item["Title"].ToString() ?? string.Empty,
                            Description      = item["Description"].ToString() ?? string.Empty,
                            CreatedBy        = Convert.ToInt32(item["CreatedBy"]),
                            Handler          = item["Handler"].ToString() ?? string.Empty,
                            CityName         = item["CityName"].ToString() ?? string.Empty,
                            RegionName       = item["RegionName"].ToString() ?? string.Empty,
                            PinnedLocation   = item["PinnedLocation"].ToString() ?? string.Empty,
                            OteSchedule      = new OteActivityDTO()
                            {
                                ScheduleFrom       = item["From"] != DBNull.Value ? Convert.ToDateTime(item["From"]) : DateTime.MinValue,
                                ScheduleTo         = item["To"] != DBNull.Value ? Convert.ToDateTime(item["To"]) : DateTime.MinValue,
                                Slots              = Convert.ToInt32(item["TotalMaxSlots"]),
                                Sold               = Convert.ToInt32(item["TotalOteTickets"]),
                                EventImage         = item["EventImage"].ToString() ?? string.Empty,
                            }
                        }).ToList();
                    }
                }
             }
            return AppResult<IEnumerable<ActivityDTO>>.CreateSucceeded(listResult, "Successfully get ote");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(ex, "An error occured when trying to get ote");
        }
    }

    public async Task<AppResult<IEnumerable<OteOngoingDTO>>> CustomerOte(int customerId)
    {
        try
        {
            string query = "with groupTickets as ( " +
                           "select distinct t.\"ActivityId\", t.\"CustomerId\", t.\"PurchaseOrderId\" " +
                           "from public.\"OteTickets\" t " +
                           "where t.\"CustomerId\" = @customerId " +
                           "), " +
                           "activityTicket as ( " +
                           "select t.*, c.\"Email\", ac.\"Title\", ai.\"ImageLocation\", " +
                               "Row_Number() over ( " +
                                   "partition by t.\"PurchaseOrderId\" " + 
                                   "order by t.\"PurchaseOrderId\", ai.\"Id\", ai.\"Order\" " +
                               ") as \"RowCnt\" " +
                           "from groupTickets t " +
                           "join public.\"Customers\" c " +
                               "on c.\"Id\" = t.\"CustomerId\" " +
                           "join public.\"Activities\" ac " +
                               "on ac.\"Id\" = t.\"ActivityId\" " +
                           "left join public.\"ActivityImages\" ai " +
                               "on ai.\"ActivityId\" = ac.\"Id\" " +
                           ") " +
                           "select * " +
                           "from activityTicket " +
                           "where \"RowCnt\" = 1 ";
            
            IList<OteOngoingDTO> listResult = new List<OteOngoingDTO>();
            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                var customerParameter = new NpgsqlParameter("customerId", customerId);
                command.Parameters.Add(customerParameter);

                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        listResult = dt.AsEnumerable().Select(item => new OteOngoingDTO {
                            ActivityId = Convert.ToInt32(item["ActivityId"]),
                            CustomerEmail = item["Email"].ToString() ?? string.Empty,
                            CustomerId = Convert.ToInt32(item["CustomerId"]),
                            EventTitle = item["Title"].ToString() ?? string.Empty,
                            ImageSrc = item["ImageLocation"].ToString() ?? string.Empty,
                            PurchaseOrderId = Convert.ToInt32(item["PurchaseOrderId"])
                        }).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<OteOngoingDTO>>.CreateSucceeded(listResult, "Successfully get customer ote.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteOngoingDTO>>.CreateFailed(ex, "An error occured when trying to get customer ote");
        }
    }

    public async Task<AppResult<IEnumerable<OteActivityPerDateDTO>>> OtePerDate(int? providerId)
    {
        try
        {
            string customerIdQuery = providerId is not null ? "and ac.\"CreatedBy\" = @providerId " : string.Empty;

            string query = "select ac.\"Id\", ac.\"Title\", ac.\"Description\", ac.\"Handler\", ac.\"ForceDisable\", ad.\"PinnedLocation\", " +
                               "ad.\"CityName\", ad.\"RegionName\", ac.\"ExperienceTypeId\", od.\"Date\", " +
                               "od.\"DateStart\", od.\"DateEnd\", od.\"Id\" \"DateId\", " +
                               "( " +
                                   "SELECT \"ImageLocation\" " +
                                   "FROM public.\"ActivityImages\" " +
                                   "WHERE \"ActivityId\" = ac.\"Id\" " +
                                   "ORDER BY \"Id\" LIMIT 1 " +
                               ") \"EventImage\" " +
                           "from public.\"Activities\" ac " +
                           "join public.\"ActivityAddress\" ad " +
                               "on ac.\"Id\" = ad.\"ActivityId\" " +
                           "join public.\"OteSchedules\" os " +
                               "on ac.\"Id\" = os.\"ActivityId\" " +
                           "join public.\"OteDates\" od " +
                               "on os.\"Id\" = od.\"OteScheduleId\" " +
                           "where ac.\"ExperienceCreationTypeId\" = 3 " + customerIdQuery +
                           "order by od.\"Date\" ";

            IList<OteActivityPerDateDTO> listResult = new List<OteActivityPerDateDTO>();
            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                if(providerId is not null)
                {
                    var customerParameter = new NpgsqlParameter("providerId", providerId);
                    command.Parameters.Add(customerParameter);
                }

                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        listResult = dt.AsEnumerable().Select(item => new OteActivityPerDateDTO {
                            ActivityId = Convert.ToInt32(item["Id"]),
                            CityName = item["CityName"].ToString() ?? string.Empty,
                            Date = Convert.ToDateTime(item["Date"]),
                            DateEnd = Convert.ToDateTime(item["DateEnd"]),
                            DateId = Convert.ToInt32(item["DateId"]),
                            DateStart = Convert.ToDateTime(item["DateStart"]),
                            Description = item["Description"].ToString() ?? string.Empty,
                            EventImage = item["EventImage"].ToString() ?? string.Empty,
                            ExperienceTypeId = Convert.ToInt32(item["ExperienceTypeId"]),
                            Handler = item["Handler"].ToString() ?? string.Empty,
                            PinnedLocation = item["PinnedLocation"].ToString() ?? string.Empty,
                            RegionName = item["RegionName"].ToString() ?? string.Empty,
                            Title = item["Title"].ToString() ?? string.Empty,
                            ForceDisable = Convert.ToBoolean(item["ForceDisable"])
                        }).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<OteActivityPerDateDTO>>.CreateSucceeded(listResult, "Successfully get customer ote per date");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteActivityPerDateDTO>>.CreateFailed(ex, "An error occured when getting ote per date");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityDTO>>> GetActivitiesNeedToDisable()
    {
        try
        {
            var dateString = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            string query = "with activityDates as " +
                           "( " +
                               "select ac.\"Id\", ac.\"Title\", ac.\"IsPublished\", ac.\"ForceDisable\", " +
                                   "os.\"From\", os.\"To\", od.\"Date\", od.\"DateStart\", od.\"DateEnd\", " +
                                   "Row_Number() over (partition by ac.\"Id\", os.\"Id\" order by ac.\"Id\", os.\"Id\", od.\"DateEnd\" desc) as \"RwCnt\" " +
                               "from public.\"Activities\" ac " +
                               "join public.\"OteSchedules\" os " +
                                   "on os.\"ActivityId\" = ac.\"Id\" " +
                               "join public.\"OteDates\" od " +
                                   "on od.\"OteScheduleId\" = os.\"Id\" " +
                               "where ac.\"ExperienceCreationTypeId\" = 3 and ac.\"ForceDisable\" = false " +
                           ") " +
                           "select * " +
                           "from activityDates " +
                           "where \"RwCnt\" = 1 and \"DateEnd\" < '" + dateString + "' ";
            
            IList<ActivityDTO> listResult = new List<ActivityDTO>();
            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        listResult = dt.AsEnumerable().Select(item => new ActivityDTO {
                            Id = Convert.ToInt32(item["Id"]),
                            Title = item["Title"].ToString() ?? string.Empty,
                            IsPublished = Convert.ToBoolean(item["IsPublished"]),
                        }).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<ActivityDTO>>.CreateSucceeded(listResult, "Successfully get activities need to disable.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(ex, "An error occured when getting activities.");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityDTO>>> ForceDisableActivities(IList<int> activityIds)
    {
        try
        {
            var activities = await applicationContext.Activities.Where(a => activityIds.Contains(a.Id)).ToListAsync();
            foreach(var activity in activities)
            {
                activity.ForceDisable = true;
            }

            await applicationContext.SaveChangesAsync();

            return AppResult<IEnumerable<ActivityDTO>>.CreateSucceeded(activities.Select(a => new ActivityDTO {
                Id = a.Id,
                IsPublished = a.IsPublished
            }), "Successfully force disable activities.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(ex, "An error occured when forcing disable activities.");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityFeedDTO>>> ActivityFeed(int take, int skip, string? search = null, 
        int? categoryId = null, int? starReview = null, int? experienceType = null)
    {
        try
        {
            string categoryClause = categoryId.HasValue ? "and ac.\"ExperienceCategoryId\" = " + categoryId + " " : string.Empty;
            string starReviewClause = starReview.HasValue ? "and su.\"ReviewAccumulated\" >= " + starReview + " " : string.Empty;
            string experienceTypeClause = experienceType.HasValue ? "and ac.\"ExperienceTypeId\" = " + experienceType + " " : string.Empty;
            string searchClause = string.Empty;
            if(!string.IsNullOrEmpty(search))
            {
                searchClause = "and (ac.\"Title\" Ilike @search or su.\"Provider\" Ilike @search or su.\"Location\" Ilike @search )";
            }

            string query = "select ac.\"Id\", ac.\"Title\", ac.\"Handler\", ac.\"ExperienceTypeId\", ac.\"ExperienceCreationTypeId\", " +
                                "ad.\"CityName\", ad.\"RegionName\", ad.\"PinnedLocation\", su.\"ImageBannerSrc\", " +
                                "su.\"Ongoing\", su.\"Completed\", su.\"TotalReviews\", su.\"ReviewAccumulated\",  " +
                                "su.\"TotalParticipants\", ac.\"Price\", ac.\"IsNew\" " +
                            "from public.\"Activities\" ac " +
                            "left join public.\"ActivityAddress\" ad " +
                                "on ac.\"Id\" = ad.\"ActivityId\" " +
                            "left join public.\"ActivitySummaries\" su " +
                                "on ac.\"Id\" = su.\"ActivityId\" " +
                            "where ac.\"IsDeactivated\" = false and ac.\"Status\" = 1 " +
                                "and ac.\"IsPublished\" = true and ac.\"ForceDisable\" = false " + 
                                categoryClause + searchClause + starReviewClause + experienceTypeClause +
                            "order by ac.\"Guid\" " +
                            "limit " + take + " offset " + skip + " ";
            
            IList<ActivityFeedDTO> listResult = new List<ActivityFeedDTO>();
            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                if(!string.IsNullOrEmpty(search))
                {
                    var parameterSearch = new NpgsqlParameter("search", $"%{search.Trim()}%");
					command.Parameters.Add(parameterSearch);
                }

                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        listResult = dt.AsEnumerable().Select(item => new ActivityFeedDTO {
                            ActivityId = Convert.ToInt32(item["Id"]),
                            ExperienceCreationTypeId = Convert.ToInt32(item["ExperienceCreationTypeId"]),
                            ExperienceTypeId = Convert.ToInt32(item["ExperienceTypeId"]),
                            Handler = item["Handler"].ToString() ?? string.Empty,
                            Price = item["Price"].ToString() ?? string.Empty,
                            Title = item["Title"].ToString() ?? string.Empty,
                            IsNew = Convert.ToBoolean(item["IsNew"]),
                            Address = new ActivityFeedDTO.Location {
                                City = item["CityName"].ToString() ?? string.Empty,
                                PinnedLocation = item["PinnedLocation"].ToString() ?? string.Empty,
                                Region = item["RegionName"].ToString() ?? string.Empty,
                            },
                            SummaryDetails = new ActivityFeedDTO.Summary {
                                Completed = Convert.ToInt32(item["Completed"]),
                                ImageSrc = item["ImageBannerSrc"].ToString() ?? string.Empty,
                                Ongoing = Convert.ToInt32(item["Ongoing"]),
                                ReviewAccumulated = Convert.ToDecimal(item["ReviewAccumulated"]),
                                TotalParticipants = Convert.ToInt32(item["TotalParticipants"]),
                                TotalReviews = Convert.ToInt32(item["TotalReviews"])
                            }
                        }).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<ActivityFeedDTO>>.CreateSucceeded(listResult, "Successfully get activity feed");
        }
        catch (System.Exception ex)
        {
            return AppResult<IEnumerable<ActivityFeedDTO>>.CreateFailed(ex, "An error occured when getting activity feed.");
        }       
    }

    public async Task<AppResult<bool>> BatchSummaryUpdate()
    {
        try
        {
            string query = "Begin; " +
                               "with activities as ( " +
                                   "select ac.\"Id\", " +
                                       "case when ai.\"ImageLocation\" is null then '' else ai.\"ImageLocation\" end as \"ImageLocation\", " +
                                       "Row_Number() over (partition by ac.\"Id\" order by ac.\"Id\", ai.\"Order\") as \"RowCnt\" " +
                                   "from public.\"Activities\" ac " +
                                   "left join public.\"ActivityImages\" ai " +
                                       "on ac.\"Id\" = ai.\"ActivityId\" " +
                               ") " +
                               "insert into public.\"ActivitySummaries\" " +
                               "(\"ActivityId\", \"ImageBannerSrc\", \"Ongoing\", \"Completed\", \"TotalReviews\", \"ReviewAccumulated\", " +
                                   "\"CreatedOn\", \"CreatedBy\", \"ChangedOn\", \"ChangedBy\", \"TotalParticipants\") " +
                               "select ac.\"Id\" \"ActivityId\", ac.\"ImageLocation\", 0, 0, 0, 0, " +
                                   "current_timestamp, 0, current_timestamp, 0, 0 " +
                               "from activities ac " +
                               "left join public.\"ActivitySummaries\" sm " +
                                   "on sm.\"ActivityId\" = ac.\"Id\" " +
                               "where ac.\"RowCnt\" = 1 and sm.\"Id\" is null; " +
                                
                               "with ongoingStundents as ( " +
                                   "select ac.\"Id\" \"ActivityId\", Count(st.\"Id\") \"Ongoing\" " +
                                   "from public.\"Activities\" ac " +
                                   "join public.\"Students\" st " +
                                       "on ac.\"Id\" = st.\"ActivityId\" " +
                                   "where (st.\"SessionsAttended\" < st.\"NumberOfSessions\" and st.\"ExpirationDateEnd\" = '-infinity') or " +
                                       "(st.\"ExpirationDateEnd\" != '-infinity' and Date(st.\"ExpirationDateEnd\") > Date(current_timestamp) " +
                                                   "and st.\"SessionsAttended\" < st.\"NumberOfSessions\") " +
                                   "group by ac.\"Id\" " +
                               ") " +
                               "update public.\"ActivitySummaries\" sm " +
                               "set \"Ongoing\" = og.\"Ongoing\" " +
                               "from ongoingStundents og " +
                               "where sm.\"ActivityId\" = og.\"ActivityId\" and og.\"Ongoing\" != sm.\"Ongoing\"; " +

                               "with withCompletedStudents as ( " +
                                   "select ac.\"Id\" \"ActivityId\", Count(st.\"Id\") \"Completed\" " +
                                   "from public.\"Activities\" ac " +
                                   "join public.\"Students\" st " +
                                       "on ac.\"Id\" = st.\"ActivityId\" " +
                                   "where (st.\"SessionsAttended\" >= st.\"NumberOfSessions\") or " +
                                       "(st.\"ExpirationDateEnd\" != '-infinity' and Date(st.\"ExpirationDateEnd\") <= Date(current_timestamp) ) " +
                                   "group by ac.\"Id\" " +
                               ") " +
                               "update public.\"ActivitySummaries\" sm " +
                               "set \"Completed\" = cm.\"Completed\" " +
                               "from withCompletedStudents cm " +
                               "where sm.\"ActivityId\" = cm.\"ActivityId\" and sm.\"Completed\" != cm.\"Completed\"; " +

                               "with withTotalStudents as ( " +
                                   "select ac.\"Id\" \"ActivityId\", Count(st.\"Id\") \"TotalParticipants\" " +
                                   "from public.\"Activities\" ac " +
                                   "join public.\"Students\" st " +
                                       "on ac.\"Id\" = st.\"ActivityId\" " +
                                   "group by ac.\"Id\" " +
                               ") " +
                               "update public.\"ActivitySummaries\" sm " +
                               "set \"TotalParticipants\" = st.\"TotalParticipants\" " +
                               "from withTotalStudents st " +
                               "where sm.\"ActivityId\" = st.\"ActivityId\" " +
                                   "and st.\"TotalParticipants\" != sm.\"TotalParticipants\"; " +

                               "with withOngoingOte as ( " +
                                   "select ac.\"Id\" \"ActivityId\", Count(ot.\"Id\") \"Ongoing\" " +
                                   "from public.\"Activities\" ac " +
                                   "join public.\"OteTickets\" ot " +
                                       "on ac.\"Id\" = ot.\"ActivityId\" " +
                                   "where ot.\"Status\" = 'UNVERIFIED' " +
                                   "group by ac.\"Id\" " +
                               ") " +
                               "update public.\"ActivitySummaries\" sm " +
                               "set \"Ongoing\" = ote.\"Ongoing\" " +
                               "from withOngoingOte ote " +
                               "where sm.\"ActivityId\" = ote.\"ActivityId\" " +
                                   "and sm.\"Ongoing\" != ote.\"Ongoing\"; " +

                               "with withCompletedOte as ( " +
                                   "select ac.\"Id\" \"ActivityId\", Count(ot.\"Id\") \"Completed\" " +
                                   "from public.\"Activities\" ac " +
                                   "join public.\"OteTickets\" ot " +
                                       "on ac.\"Id\" = ot.\"ActivityId\" " +
                                   "where ot.\"Status\" = 'VERIFIED' " +
                                   "group by ac.\"Id\" " +
                               ") " +
                               "update public.\"ActivitySummaries\" sm " +
                               "set \"Completed\" = ote.\"Completed\" " +
                               "from withCompletedOte ote " +
                               "where sm.\"ActivityId\" = ote.\"ActivityId\" " +
                                   "and sm.\"Completed\" != ote.\"Completed\"; " +

                               "with withTotalOte as ( " +
                                   "select ac.\"Id\" \"ActivityId\", Count(ot.\"Id\") \"TotalParticipants\" " +
                                   "from public.\"Activities\" ac " +
                                   "join public.\"OteTickets\" ot " +
                                       "on ot.\"ActivityId\" = ac.\"Id\" " +
                                   "group by ac.\"Id\" " +
                               ") " +
                               "update public.\"ActivitySummaries\" sm " +
                               "set \"TotalParticipants\" = ote.\"TotalParticipants\" " +
                               "from withTotalOte ote " +
                               "where sm.\"ActivityId\" = ote.\"ActivityId\" " +
                                   "and sm.\"TotalParticipants\" != ote.\"TotalParticipants\"; " +

                               "with withTotalRatings as ( " +
                                   "select ac.\"Id\" \"ActivityId\", Count(ar.\"Id\") \"ReviewCount\", " +
                                       "Trunc(Coalesce(Sum(ar.\"Rating\"::decimal) / Count(ar.\"Id\"),0),1) \"Rating\" " +
                                   "from public.\"Activities\" ac " +
                                   "left join public.\"Reviews\" ar " +
                                       "on ac.\"Id\" = ar.\"ActivityId\" " +
                                   "group by ac.\"Id\" " +
                               ") " +
                               "update public.\"ActivitySummaries\" sm " +
                               "set \"ReviewAccumulated\" = rt.\"Rating\", \"TotalReviews\" = rt.\"ReviewCount\" " +
                               "from withTotalRatings rt " +
                               "where sm.\"ActivityId\" = rt.\"ActivityId\" and " +
                                   "rt.\"ReviewCount\" != sm.\"TotalReviews\" and rt.\"Rating\" != sm.\"ReviewAccumulated\"; " +
                                
                               "with activities as ( " +
                                   "select ac.\"Id\", " +
                                       "case when ai.\"ImageLocation\" is null then '' else ai.\"ImageLocation\" end as \"ImageLocation\", " +
                                       "Row_Number() over (partition by ac.\"Id\" order by ac.\"Id\", ai.\"Order\") as \"RowCnt\" " +
                                   "from public.\"Activities\" ac " +
                                   "left join public.\"ActivityImages\" ai " +
                                       "on ac.\"Id\" = ai.\"ActivityId\" " +
                                   "where (ai.\"ImageLocation\" != '' or ai.\"ImageLocation\" is not null) " +
                               ") " +
                               "update public.\"ActivitySummaries\" sm " +
                               "set \"ImageBannerSrc\" = ac.\"ImageLocation\" " +
                               "from activities ac " +
                               "where ac.\"RowCnt\" = 1 and sm.\"ActivityId\" = ac.\"Id\" and " +
                                   "( sm.\"ImageBannerSrc\" != ac.\"ImageLocation\" ); " +
                                   
                               "with actLocation as ( " +
                                   "select ac.\"Id\", ac.\"ExperienceTypeId\", " +
                                       "case " +
                                           "when ad.\"Address1\" = '--NOTHING PROVIDED--' then '' " +
                                           "when ac.\"ExperienceTypeId\" = 1 then ad.\"Address1\" " +
                                           "else '' " +
                                       "end \"Address1\", " +
                                       "case " +
                                           "when ad.\"Address2\" = '--NOTHING PROVIDED--' then '' " +
                                           "when ac.\"ExperienceTypeId\" = 1 then ad.\"Address2\" " +
                                           "else '' " +
                                       "end \"Address2\", " +
                                       "case " +
                                           "when ad.\"District\" = '--NOTHING PROVIDED--' then '' " +
                                           "when ac.\"ExperienceTypeId\" = 1 then ad.\"District\" " +
                                           "else '' " +
                                       "end \"District\", " +
                                       "case " +
                                           "when ad.\"Subdivision\" = '--NOTHING PROVIDED--' then '' " +
                                           "when ac.\"ExperienceTypeId\" = 1 then ad.\"Subdivision\" " +
                                           "else '' " +
                                       "end \"Subdivision\", " +
                                       "case " +
                                           "when ad.\"BarangayName\" = '--NOTHING PROVIDED--' then '' " +
                                           "when ac.\"ExperienceTypeId\" = 1 then ad.\"BarangayName\" " +
                                           "else '' " +
                                       "end \"BarangayName\", " +
                                       "case " +
                                           "when ad.\"CityName\" = '--NOTHING PROVIDED--' then '' " +
                                           "when ac.\"ExperienceTypeId\" = 1 then ad.\"CityName\" " +
                                           "else '' " +
                                       "end \"CityName\", " +
                                       "case " +
                                           "when ad.\"RegionName\" = '--NOTHING PROVIDED--' then '' " +
                                           "when ac.\"ExperienceTypeId\" = 1 then ad.\"RegionName\" " +
                                           "else '' " +
                                       "end \"RegionName\", " +
                                       "case  " +
                                           "when ad.\"PinnedLocation\" = '--NOTHING PROVIDED--' then '' " +
                                           "when ac.\"ExperienceTypeId\" = 1 then ad.\"PinnedLocation\" " +
                                           "else '' " +
                                       "end \"PinnedLocation\" " +
                                   "from public.\"Activities\" ac " +
                                   "join public.\"ActivityAddress\" ad " +
                                       "on ad.\"ActivityId\" = ac.\"Id\" " +
                               "), " +
                               "combinedLocation as ( " +
                                   "select ac.\"Id\", Trim(ac.\"Address1\" || ' ' || ac.\"Address2\" || ' ' || " +
                                       "ac.\"District\" || ' ' || ac.\"Subdivision\" || ' ' || " +
                                       "ac.\"BarangayName\" || ' ' || ac.\"CityName\" || ' ' || " +
                                       "ac.\"RegionName\" || ' ' || ac.\"PinnedLocation\") \"Address\" " +
                                   "from actLocation ac	" +
                               ") " +
                               "update public.\"ActivitySummaries\" su " +
                               "set \"Location\" = ac.\"Address\" " +
                               "from combinedLocation ac " +
                               "where su.\"ActivityId\" = ac.\"Id\" and  " +
                                   "(Trim(su.\"Location\") != ac.\"Address\"); " +

                               "with provider as ( " +
                                   "select ac.\"Id\", (cs.\"FirstName\" || ' ' || cs.\"LastName\") \"Provider\" " +
                                   "from public.\"Customers\" cs " +
                                   "join public.\"Activities\" ac " +
                                       "on cs.\"Id\" = ac.\"CreatedBy\"	" +
                               ") " +
                               "update public.\"ActivitySummaries\" su " +
                               "set \"Provider\" = pr.\"Provider\" " +
                               "from provider pr " +
                               "where su.\"ActivityId\" = pr.\"Id\" and su.\"Provider\" != pr.\"Provider\"; " +

                           "commit; ";
            
            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                applicationContext.Database.OpenConnection();

                await command.ExecuteNonQueryAsync();
            }

            return AppResult<bool>.CreateSucceeded(true, "Successfully update activity summary.");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured when updating summary by batch.");
        }
    }
}