using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

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

    public async Task<AppResult<DisbursementBulk>> UpdateDisbursementBulkStatus(int disbursementBulkId, 
        string disbursementBulkStatus, string disbursementStatus, string remarks)
    {
        try
        {
            var disbursementBulk = await applicationContext.DisbursementBulks
                                    .Where(d => d.Id == disbursementBulkId)
                                    .Include(d => d.DisbursementDetailBulks)
                                    .FirstOrDefaultAsync();
            if(disbursementBulk is null)
            {
                return AppResult<DisbursementBulk>.CreateFailed(new ApplicationException("Unable to find disbursement bulk."), "Unable to find disbursement bulk.");
            }

            disbursementBulk.Status = disbursementBulkStatus;
            disbursementBulk.Remarks = remarks ?? string.Empty;
            foreach(var detail in disbursementBulk.DisbursementDetailBulks)
            {
                var disbursement = await applicationContext.Disbursements.FindAsync(detail.DisbursementId);
                if(disbursement is not null)
                {
                    disbursement.Status = disbursementStatus;
                }
            }

            await applicationContext.SaveChangesAsync();

            return AppResult<DisbursementBulk>.CreateSucceeded(disbursementBulk, "Successfully update disbursement bulk status.");
        }
        catch (Exception ex)
        {
            return AppResult<DisbursementBulk>.CreateFailed(ex, "An error occured when updating disbursement bulk status.");
        }
    }
}