using System.Data;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ChatUnreadNotificationEntity : GenericEntity<ChatUnreadNotification>, IChatUnreadNotification
{
    private readonly ApplicationContext applicationContext;

    public ChatUnreadNotificationEntity(ApplicationContext applicationContext) : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<ChatUnreadNotification>>> GetUnreadMessages()
    {
        try
        {
            string query = "with withRow as " +
                           "( " +
                               "select ch.\"Id\", ch.\"FromUserId\", ch.\"ToUserId\", ch.\"CreatedOn\" \"ChatDate\", cc.\"Email\", " +
                                   "Row_Number() over(partition by ch.\"ToUserId\", ch.\"FromUserId\" order by ch.\"ToUserId\", ch.\"FromUserId\", ch.\"CreatedOn\" desc) as \"RowCnt\" " +
                               "from public.\"ChatHistories\" ch " +
                               "join public.\"Customers\" cc " +
                                   "on cc.\"Id\" = ch.\"ToUserId\" " +
                               "where \"IsViewed\" = false " +
                           "), " +
                           "withRepeated as " +
                           "( " +
                               "select wr.\"Id\", wr.\"FromUserId\", wr.\"ToUserId\", wr.\"ChatDate\", " +
                                   "cn.\"Repeated\", wr.\"Email\", " +
                                   "Row_Number() over(partition by wr.\"Id\" order by wr.\"Id\", cn.\"Id\" desc) as \"RowCnt\" " +
                               "from withRow wr " +
                               "join public.\"ChatUnreadNotifications\" cn " +
                                   "on wr.\"Id\" = cn.\"ChatHistoryId\" " +
                               "where wr.\"RowCnt\" = 1 " +
                           ") " +
                           "select * " +
                           "from withRepeated " +
                           "where \"RowCnt\" = 1 ";

            List<ChatUnreadNotification> result = new();

            using(var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
				command.CommandType = System.Data.CommandType.Text;

                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if(dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        result = dt.AsEnumerable().Select(item => new ChatUnreadNotification {
                            ChatHistoryId = Convert.ToInt32(item["Id"]),
                            CustomerEmail = item["Email"].ToString() ?? string.Empty,
                            FromUserId = Convert.ToInt32(item["FromUserId"]),
                            ToUserId = Convert.ToInt32(item["ToUserId"]),
                            ChatDate = Convert.ToDateTime(item["ChatDate"]),
                            Repeated = item["Repeated"].ToString() ?? string.Empty
                        }).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<ChatUnreadNotification>>.CreateSucceeded(result, "Successfully get unread messages");

        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ChatUnreadNotification>>.CreateFailed(ex, "An error occured when getting unread messages");
        }
    }
}