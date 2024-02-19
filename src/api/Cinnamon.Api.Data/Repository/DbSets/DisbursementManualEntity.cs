using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DisbursementManualEntity : GenericEntity<DisbursementManual>, IDisbursementManual 
{
    private readonly ApplicationContext applicationContext;

    public DisbursementManualEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<DisbursementManual>> CreateManualDisbursement(DisbursementManual disbursementManual)
    {
        try
        {
            var disbursement = await applicationContext.Disbursements.FindAsync(disbursementManual.DisbursementId);
            if(disbursement is null)
            {
                return AppResult<DisbursementManual>.CreateFailed(
                    new ApplicationException("Unable to find disbursement need to manual."), "Unable to find disbursement need to manual.");
            }

            disbursement.Status = "disbursed";
            disbursement.Remarks = disbursementManual.Reason;
            applicationContext.DisbursementManuals.Add(disbursementManual);
            await applicationContext.SaveChangesAsync();

            return AppResult<DisbursementManual>.CreateSucceeded(disbursementManual, "Successfully create manual disbursement.");
        }
        catch (Exception ex)
        {
            return AppResult<DisbursementManual>.CreateFailed(ex, "An error occured when creating manual disbursement.");
        }
    }
}