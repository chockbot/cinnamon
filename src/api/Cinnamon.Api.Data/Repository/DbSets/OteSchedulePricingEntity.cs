using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteSchedulePricingEntity : GenericEntity<OteSchedulePricing>, IOteSchedulePricing
{
    private readonly ApplicationContext applicationContext;
    public OteSchedulePricingEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<OteSchedulePricing>>> GetPricings(int Id)
    {
        try
        {
            string query = "SELECT \"Id\", \"OteScheduleId\", \"Price\", \"MaxSlots\", \"Description\", \r\n\"IsAbsorbFees\", \"CreatedOn\", \"CreatedBy\", \"ChangedOn\", \"ChangedBy\", \r\n" +
                "\"Name\", \"TicketSold\", \"OteDateId\", \"OteSchedulePricingGroupId\"\r\n" +
                "FROM public.\"OteSchedulePricings\" WHERE \"OteSchedulePricingGroupId\" = "+ Id +";";

            IList<OteSchedulePricing> listResult = new List<OteSchedulePricing>();

            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = System.Data.CommandType.Text;

                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        listResult = dt.AsEnumerable().Select(item => new OteSchedulePricing
                        {
                            Id           = Convert.ToInt32(item["Id"]),
                            IsAbsorbFees = Convert.ToBoolean(item["IsAbsorbFees"]),
                            Description  = item["Description"].ToString() ?? string.Empty,
                            MaxSlots     = Convert.ToInt32(item["MaxSlots"]),
                            Price        = Convert.ToDecimal(item["Price"]),
                            Name         = item["Name"].ToString() ?? string.Empty,
                            TicketSold   = Convert.ToInt32(item["TicketSold"]),
                        }).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<OteSchedulePricing>>.CreateSucceeded(listResult, "Successfully get ote pricings");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteSchedulePricing>>.CreateFailed(ex, "An error occured when trying to get ote pricings");
        }
    }
}