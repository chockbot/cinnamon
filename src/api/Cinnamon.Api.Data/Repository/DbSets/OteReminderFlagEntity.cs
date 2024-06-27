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
}
