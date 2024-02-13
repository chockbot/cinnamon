using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Interactors;
using Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.Disbursement;

public class GenerateDisbursement : IGenerateDisbursement
{
    private readonly IStudentData studentData;
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly Dictionary<int, Disbursement> disbursements = new();
    private readonly Dictionary<int, int> studentIds = new();
    private readonly Dictionary<int, int> purchaseOrderIds = new();

    public GenerateDisbursement(IStudentData studentData, IPurchaseOrderData purchaseOrderData)
    {
        this.studentData = studentData;
        this.purchaseOrderData = purchaseOrderData;
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

            var inclusiveTransaction = await studentData.GetStudentsToDisburse(new Framework.ApiCommand.ApiData.Student.Request.GetStudentsToDisburseArgs {
                IsInclusive = true
            });
            if(!inclusiveTransaction.Succeeded || inclusiveTransaction.Result is null || !inclusiveTransaction.Result.IsSuccess)
            {
                return AppResult<GenerateDisbursementResult>.CreateFailed(
                    new ApplicationException(inclusiveTransaction.Result?.ErrorInfo?.Message), inclusiveTransaction.Message);
            }
            foreach(var transaction in inclusiveTransaction.Result.Result)
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

            var exclusiveTransaction = await studentData.GetStudentsToDisburse(new Framework.ApiCommand.ApiData.Student.Request.GetStudentsToDisburseArgs {
                IsInclusive = false
            });
            if(!exclusiveTransaction.Succeeded || exclusiveTransaction.Result is null || !exclusiveTransaction.Result.IsSuccess)
            {
                return AppResult<GenerateDisbursementResult>.CreateFailed(
                    new ApplicationException(exclusiveTransaction.Result?.ErrorInfo?.Message), exclusiveTransaction.Message);
            }
            foreach(var transaction in exclusiveTransaction.Result.Result)
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

            var addonTransactions = await purchaseOrderData.AddOnsNeedToDisburse();
            if(!addonTransactions.Succeeded || addonTransactions.Result is null || !addonTransactions.Result.IsSuccess)
            {
                return AppResult<GenerateDisbursementResult>.CreateFailed(
                    new ApplicationException(addonTransactions.Result?.ErrorInfo?.Message), addonTransactions.Message);
            }
            foreach(var transaction in addonTransactions.Result.Result)
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

                // store purchase order id to update status 
                if(!purchaseOrderIds.ContainsKey(transaction.TransactionId))
                {
                    purchaseOrderIds.Add(transaction.TransactionId, transaction.TransactionId);
                }
            }

            var oteTransactions = await purchaseOrderData.GetOteNeedToDisburse();
            if(!oteTransactions.Succeeded || oteTransactions.Result is null || !oteTransactions.Result.IsSuccess)
            {
                return AppResult<GenerateDisbursementResult>.CreateFailed(
                    new ApplicationException(oteTransactions.Result?.ErrorInfo?.Message), oteTransactions.Message);
            }
            foreach(var transaction in oteTransactions.Result.Result)
            {
                if(!disbursements.ContainsKey(transaction.TransactionId))
                {
                    disbursements.Add(transaction.TransactionId, new Disbursement {
                        CustomerId = transaction.MakerId,
                        Amount = transaction.TotalDisburseAmount,
                        InclusivePayment = transaction.IsInclusivePayment,
                        Label = transaction.Title,
                        PurchaseOrderId = transaction.TransactionId,
                        Status = "initiated",
                    });
                }

                var disbursement = disbursements[transaction.TransactionId];
                disbursement.Details.Add(new DisbursementDetail {
                    Amount = transaction.PerUnitDisburseAmount,
                    Label = transaction.Name
                });

                // store purchase order id to update status 
                if(!purchaseOrderIds.ContainsKey(transaction.TransactionId))
                {
                    purchaseOrderIds.Add(transaction.TransactionId, transaction.TransactionId);
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