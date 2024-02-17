using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DisbursementBulkEntity : GenericEntity<DisbursementBulk>, IDisbursementBulk 
{
    private readonly ApplicationContext applicationContext;

    public DisbursementBulkEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<DisbursementBulk>> CreateDisbursementBulk(DisbursementBulk disbursementBulk)
    {
        try
        {
            applicationContext.DisbursementBulks.AddRange(disbursementBulk);
            foreach(var detail in disbursementBulk.DisbursementDetailBulks)
            {
                var disbursement = await applicationContext.Disbursements.FindAsync(detail.DisbursementId);
                if(disbursement is not null)
                {
                    disbursement.Status = "pending";
                }
            }
            await applicationContext.SaveChangesAsync();

            return AppResult<DisbursementBulk>.CreateSucceeded(disbursementBulk, "Disbursement Bulk successfully created.");
        }
        catch (Exception ex)
        {
            return AppResult<DisbursementBulk>.CreateFailed(ex, "An error occured when creating disbursement bulks.");
        }
    }
}