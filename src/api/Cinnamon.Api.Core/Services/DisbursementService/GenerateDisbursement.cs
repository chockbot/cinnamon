using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Interactors;
using Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.Disbursement;

public class GenerateDisbursement : IGenerateDisbursement
{
    private readonly IStudentData studentData;
    private readonly Dictionary<int, Disbursement> disbursements = new();
    private readonly Dictionary<int, int> studentIds = new();

    public GenerateDisbursement(IStudentData studentData)
    {
        this.studentData = studentData;
    }

    public AppResult<GenerateDisbursementResult> Execute(GenerateDisbursementArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<GenerateDisbursementResult>> ExecuteAsync(GenerateDisbursementArgs args)
    {
        try
        {
            var expiredStudentsTransaction = await studentData.GetStudentsToDisburse(new Framework.ApiCommand.ApiData.Student.Request.GetStudentsToDisburseArgs {
                IsExpired = true,
                IsInclusive = false
            });
            if(!expiredStudentsTransaction.Succeeded || expiredStudentsTransaction.Result is null || !expiredStudentsTransaction.Result.IsSuccess)
            {
                return AppResult<GenerateDisbursementResult>.CreateFailed(
                    new ApplicationException(expiredStudentsTransaction.Result?.ErrorInfo?.Message), expiredStudentsTransaction.Message);
            }
            foreach (var transaction in expiredStudentsTransaction.Result.Result)
            {
                if(!disbursements.ContainsKey(transaction.TransactionId))
                {
                    disbursements.Add(transaction.TransactionId, new Disbursement {
                        CustomerId = transaction.MakerId,
                        InclusivePayment = transaction.IsInclusivePayment,
                        Label = transaction.Title,
                        PurchaseOrderId = transaction.TransactionId,
                        Status = "initiated",
                    });
                }

                var disbursement = disbursements[transaction.TransactionId];
                disbursement.Amount += transaction.PerUnitDisburseAmount;
                disbursement.Details.Add(new DisbursementDetail {
                    Amount = transaction.PerUnitDisburseAmount,
                    Label = transaction.Name
                });

                // store student ids to update status in db
                if(!studentIds.ContainsKey(transaction.StudentId) && transaction.StudentId > 0)
                {
                    studentIds.Add(transaction.StudentId, transaction.StudentId);
                }
            }

            return AppResult<GenerateDisbursementResult>.CreateSucceeded(new(), "Successfully generate disbursement.");
        }
        catch (Exception ex)
        {
            return AppResult<GenerateDisbursementResult>.CreateFailed(ex, "An error occured when generating disbursement.");
        }
    }

    record Disbursement 
    {
        public int PurchaseOrderId {get; set;}
        public int CustomerId {get; set;}
        public string Label {get; set;}
        public decimal Amount {get; set;}
        public string Status {get; set;} // initiated|pending|disbursed
        public string Remarks {get; set;}
        public bool InclusivePayment {get; set;}

        public List<DisbursementDetail> Details {get; set;} = new();
    }

    record DisbursementDetail 
    {
        public string Label {get; set;}
        public decimal Amount {get; set;}
    }
}