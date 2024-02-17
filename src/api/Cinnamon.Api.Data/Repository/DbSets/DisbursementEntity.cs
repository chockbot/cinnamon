using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DisbursementEntity : GenericEntity<Disbursement>, IDisbursement 
{
    private readonly ApplicationContext applicationContext;

    public DisbursementEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<Disbursement>>> CreateDisbursements(IEnumerable<Disbursement> disbursements, 
        IEnumerable<int> studentIds, IEnumerable<int> purchaseOrderIds)
    {
        try
        {
            applicationContext.Disbursements.AddRange(disbursements);
            
            foreach(var id in studentIds)
            {
                var student = await applicationContext.Students.FindAsync(id);
                if(student is not null)
                {
                    student.IsDisbursement = true;
                }
            }

            foreach(var id in purchaseOrderIds)
            {
                var purchaseOrder = await applicationContext.PurchaseOrders.FindAsync(id);
                if(purchaseOrder is not null)
                {
                    purchaseOrder.Status = 5;
                }
            }

            await applicationContext.SaveChangesAsync();

            return AppResult<IEnumerable<Disbursement>>.CreateSucceeded(disbursements, "Disbursement successfully created.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<Disbursement>>.CreateFailed(ex, "An error occured when creating disbursements");
        }
    }
}