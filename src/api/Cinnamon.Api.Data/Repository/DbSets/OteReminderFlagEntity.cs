using System.Data;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteReminderFlag;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteReminderFlagEntity : GenericEntity<OteReminderFlag>, IOteReminderFlag
{
    private readonly ApplicationContext applicationContext;

    public OteReminderFlagEntity(ApplicationContext applicationContext)
        :base(applicationContext)
    {
        this.applicationContext = applicationContext; 
    }

    public async Task<AppResult<IEnumerable<OteForReminderDTO>>> GetEventsForReminder()
    {
        try
        {
            var dateString = DateTime.Now.ToString("yyyy-MM-dd");

            string query = "select od.\"Id\" \"DateId\", ac.\"Id\" \"ActivityId\", od.\"DateStart\", " +
                                "ac.\"Title\", ac.\"Description\", cs.\"FirstName\", cs.\"LastName\", cs.\"Email\" " +
                            "from public.\"OteDates\" od " +
                            "join public.\"OteSchedules\" os " +
                                "on os.\"Id\" = od.\"OteScheduleId\" " +
                            "join public.\"Activities\" ac " +
                                "on ac.\"Id\" = os.\"ActivityId\" " +
                            "join public.\"Customers\" cs " +
                                "on cs.\"Id\" = ac.\"CreatedBy\" " +
                            "left join public.\"OteReminderFlags\" otr " +
                                "on otr.\"ActivityId\" = ac.\"Id\" and otr.\"OteDateId\" = od.\"Id\" " +
                            "where od.\"DateStart\" != '-infinity' and otr.\"Id\" is null and " +
                                "( " +
                                    "Date(od.\"DateStart\") >= Date('" + dateString + "') and  " +
                                    "Date(od.\"DateStart\") <= (Date('" + dateString + "') + Interval '7 DAY') " +
                                ") ";
            
            IList<OteForReminderDTO> listResult = new List<OteForReminderDTO>();
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

						listResult = dt.AsEnumerable().Select(item => new OteForReminderDTO
						{
                            ActivityId = Convert.ToInt32(item["ActivityId"]),
                            DateId = Convert.ToInt32(item["DateId"]),
                            DateStart = Convert.ToDateTime(item["DateStart"]),
                            Description= item["Description"].ToString() ?? string.Empty,
                            ProviderEmail = item["Email"].ToString() ?? string.Empty,
                            ProviderFirstName = item["FirstName"].ToString() ?? string.Empty,
                            ProviderLastName = item["LastName"].ToString() ?? string.Empty,
                            Title = item["Title"].ToString() ?? string.Empty
						}).ToList();
					}
				}
			}

            return AppResult<IEnumerable<OteForReminderDTO>>.CreateSucceeded(listResult, "Successfully get event for reminder.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteForReminderDTO>>.CreateFailed(ex, "An error occured when getting event for reminder.");
        }
    }

    public async Task<AppResult<IEnumerable<CustomersNeedToRemindDTO>>> CustomersToRemind(int activityId, int oteDateId)
    {
        try
        {
            string query = "with rw as ( " +
                                "select ot.\"ActivityId\", ot.\"OteDateId\", ot.\"CustomerId\", " +
                                    "Row_Number() over (partition by ot.\"ActivityId\", ot.\"OteDateId\", ot.\"CustomerId\" order by ot.\"Id\" desc) \"RwCnt\" " +
                                "from public.\"OteTickets\" ot " +
                                "where ot.\"ActivityId\" = " + activityId + " and ot.\"OteDateId\" = " + oteDateId + " " +
                                "order by \"Id\" desc " +
                            ") " +
                            "select rw.\"ActivityId\", rw.\"OteDateId\", rw.\"CustomerId\", " +
                                "cs.\"FirstName\", cs.\"LastName\", cs.\"Email\" " +
                            "from rw " +
                            "join public.\"Customers\" cs " +
                                "on cs.\"Id\" = rw.\"CustomerId\" " +
                            "where rw.\"RwCnt\" = 1 and rw.\"OteDateId\" is not null ";
            
            IList<CustomersNeedToRemindDTO> listResult = new List<CustomersNeedToRemindDTO>();
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

						listResult = dt.AsEnumerable().Select(item => new CustomersNeedToRemindDTO
						{
                            ActivityId = Convert.ToInt32(item["ActivityId"]),
                            CustomerId = Convert.ToInt32(item["CustomerId"]),
                            Email = item["Email"].ToString() ?? string.Empty,
                            FirstName = item["FirstName"].ToString() ?? string.Empty,
                            LastName = item["LastName"].ToString() ?? string.Empty,
                            OteDateId = Convert.ToInt32(item["OteDateId"])
						}).ToList();
					}
				}
			}

            return AppResult<IEnumerable<CustomersNeedToRemindDTO>>.CreateSucceeded(listResult, "Successfully get event for reminder.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<CustomersNeedToRemindDTO>>.CreateFailed(ex, "An error occured when getting customers for reminder.");
        }
    }

    public async Task<AppResult<IEnumerable<OteForReminderDTO>>> GetEventsForThankYou()
    {
        try
        {
            var dateString = DateTime.Now.ToString("yyyy-MM-dd");

            string query = "select od.\"Id\" \"DateId\", ac.\"Id\" \"ActivityId\", od.\"DateStart\", od.\"DateEnd\", " +
                                "ac.\"Title\", ac.\"Description\", cs.\"FirstName\", cs.\"LastName\", cs.\"Email\" " +
                            "from public.\"OteDates\" od " +
                            "join public.\"OteSchedules\" os " +
                                "on os.\"Id\" = od.\"OteScheduleId\" " +
                            "join public.\"Activities\" ac " +
                                "on ac.\"Id\" = os.\"ActivityId\" " +
                            "join public.\"Customers\" cs " +
                                "on cs.\"Id\" = ac.\"CreatedBy\" " +
                            "where od.\"DateEnd\" != '-infinity' and " +
                                "( " +
                                    "Date(od.\"DateEnd\") >= (Date('" + dateString + "') - Interval '7 DAY') and " +
		                            "Date(od.\"DateEnd\") <= (Date('" + dateString + "') - Interval '1 DAY') " +
                                ") ";
            
            IList<OteForReminderDTO> listResult = new List<OteForReminderDTO>();
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

						listResult = dt.AsEnumerable().Select(item => new OteForReminderDTO
						{
                            ActivityId = Convert.ToInt32(item["ActivityId"]),
                            DateId = Convert.ToInt32(item["DateId"]),
                            DateStart = Convert.ToDateTime(item["DateStart"]),
                            Description= item["Description"].ToString() ?? string.Empty,
                            ProviderEmail = item["Email"].ToString() ?? string.Empty,
                            ProviderFirstName = item["FirstName"].ToString() ?? string.Empty,
                            ProviderLastName = item["LastName"].ToString() ?? string.Empty,
                            Title = item["Title"].ToString() ?? string.Empty,
                            DateEnd = Convert.ToDateTime(item["DateEnd"])
						}).ToList();
					}
				}
			}

            return AppResult<IEnumerable<OteForReminderDTO>>.CreateSucceeded(listResult, "Successfully get event for reminder.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteForReminderDTO>>.CreateFailed(ex, "An error occured when getting event for reminder.");
        }
    }
}
