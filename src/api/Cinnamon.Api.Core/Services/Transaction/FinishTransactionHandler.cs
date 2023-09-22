using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.OngoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Core.Services.AccountService.Handlers;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class FinishTransactionHandler : IFinishTransactionHandler
{
    private readonly ICreateOngoingActivityHandler createOngoingActivityHandler;
    private readonly ICustomerPayedNotificationHandler customerPayedNotificationHandler;
    private readonly IMakerEnrolledNotificationHandler makerEnrolledNotificationHandler;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly ICustomerData customerData;
    private readonly IUpdateCreditBalanceHandler updateCreditBalanceHandler;

    public FinishTransactionHandler(ICreateOngoingActivityHandler createOngoingActivityHandler, ICustomerPayedNotificationHandler customerPayedNotificationHandler,
        IMakerEnrolledNotificationHandler makerEnrolledNotificationHandler, IJsonSerializationProvider jsonSerializationProvider,
        IPurchaseOrderData purchaseOrderData, ICustomerData customerData, IGetActivityHandler getActivityHandler,
        IUpdateCreditBalanceHandler updateCreditBalanceHandler)
    {
        this.createOngoingActivityHandler = createOngoingActivityHandler;
        this.customerPayedNotificationHandler = customerPayedNotificationHandler;
        this.makerEnrolledNotificationHandler = makerEnrolledNotificationHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.purchaseOrderData = purchaseOrderData;
        this.customerData = customerData;
        this.getActivityHandler = getActivityHandler;
        this.updateCreditBalanceHandler = updateCreditBalanceHandler;
    }

    public AppResult<FinishTransactionResult> Execute(FinishTransactionArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<FinishTransactionResult>.CreateFailed(ex, "An error occured in FinishTransactionHandler");
        }
    }

    public async Task<AppResult<FinishTransactionResult>> ExecuteAsync(FinishTransactionArgs args)
    {
        try
        {
            var getPurchaseOrder = await purchaseOrderData.GetPurchaseOrderById(args.TransactionId);
            if(!getPurchaseOrder.Succeeded || getPurchaseOrder.Result == null || !getPurchaseOrder.Result.IsSuccess)
            {
                return AppResult<FinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            var purchaseOrder = getPurchaseOrder.Result.Result;

            var customerDetail = await customerData.GetCustomerById(getPurchaseOrder.Result.Result.CustomerId);
            if(!customerDetail.Succeeded || customerDetail.Result == null || !customerDetail.Result.IsSuccess)
            {
                return AppResult<FinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            var customer = customerDetail.Result.Result;

            // get associated activity
            var activityRes = await getActivityHandler.ExecuteAsync(
                    new ActivityService.Interactors.GetActivityArgs {
                        ActivityId = purchaseOrder.ActivityId, 
                        IncludeAtivitySchedules = true,
                        IncludeCustomer = true});

            if(!activityRes.Succeeded || activityRes.Result == null)
            {
                return AppResult<FinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            var activity = activityRes.Result;

            var deserializedPayload = jsonSerializationProvider.Deserialize<PayloadData>(purchaseOrder.Payload);
            if(deserializedPayload == null)
            {
                return AppResult<FinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            var createOngoingActivityRes = await createOngoingActivityHandler.ExecuteAsync(new OngoingActivityService.Interactors.CreateOngoingActivityArgs 
            {
                ActivityId = purchaseOrder.ActivityId,
                CustomerId = purchaseOrder.CustomerId,
                PurchaseOrderId = purchaseOrder.Id,
                ScheduleId = purchaseOrder.ScheduleId,
                Students = deserializedPayload.Students.Select(s => {
                    return new OngoingActivityService.Interactors.CreateOngoingActivityArgs.Student {
                        FamilyMemberId = s.Id,
                        Name = s.Name
                    };
                }),
                SelectedPeriod = deserializedPayload.SelectedPeriod
            });
            if(!createOngoingActivityRes.Succeeded || createOngoingActivityRes.Result == null)
            {
                return AppResult<FinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            // if there is credit applied in purchase order then subract in balance credit
            if(purchaseOrder.CreditAmount > 0)
            {
                var updateCredit = await updateCreditBalanceHandler.ExecuteAsync(new AccountService.Interactors.UpdateCreditBalanceArgs {
                    ActionFlag = 1,
                    Amount = purchaseOrder.CreditAmount,
                    CustomerId = purchaseOrder.CustomerId
                });
                if(!updateCredit.Succeeded || updateCredit.Result == null)
                {
                    return AppResult<FinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
                }
            }


            var referenceId = "000000000000000".Substring(purchaseOrder.Id.ToString().Length) + purchaseOrder.Id;

            // send email notification for customer
            var emailNotifyRes = await customerPayedNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.CustomerPayedNotificationArgs {
                Amount = purchaseOrder.Total,
                CoachName = $"{activity.Owner?.FirstName} {activity.Owner?.LastName}",
                CoachNumber = activity.Owner.PhoneNumber,
                CustomerName = $"{customer.FirstName}",
                Email = customer.Email,
                ExperienceName = activity.Title,
                PayerName = $"{customer.FirstName} {customer.LastName}",
                PurchaseDate = DateTime.Now,
                Members = deserializedPayload.Students.Select(s => {
                    return new Modules.NotificationDriver.Interactors.CustomerPayedNotificationArgs.IncludedMembers {
                        Name = s.Name,
                    };
                }),
                PaymentMethod = deserializedPayload.PaymentChannel ?? deserializedPayload.PaymentMethod,
                ReferenceNumber = referenceId,
                MakerEmail = $"{activity.Owner?.Email}",
                ServiceFee = deserializedPayload.Fees.ServiceFee,
                PaymentProviderFee = deserializedPayload.Fees.PaymentProviderFee,
                AppliedCredits = purchaseOrder.CreditAmount,
                IsInclusivePayment = deserializedPayload.IsInclusivePayment,
                DiscountAmount = purchaseOrder.CouponAmount ?? 0
            });
            if(!emailNotifyRes.Succeeded || emailNotifyRes.Result == null)
            {
                return AppResult<FinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            // send mail notification for maker
            var makerNotification = await makerEnrolledNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.MakerEnrolledNotificationArgs {
                Amount = purchaseOrder.Total,
                Email = $"{activity.Owner?.Email}",
                ExperienceName = activity.Title,
                MakerName = $"{activity.Owner?.FirstName}",
                PayerName = $"{customer.FirstName} {customer.LastName}",
                PurchaseDate = DateTime.Now,
                Students = deserializedPayload.Students.Select(s => {
                    return new Modules.NotificationDriver.Interactors.MakerEnrolledNotificationArgs.IncludedStudents {
                        Name = s.Name
                    };
                }),
                PaymentMethod = deserializedPayload.PaymentChannel ?? deserializedPayload.PaymentMethod,
                ReferenceNumber = referenceId,
                PayerEmail = customer.Email,
                ServiceFee = deserializedPayload.Fees.ServiceFee,
                PaymentProviderFee = deserializedPayload.Fees.PaymentProviderFee,
                AppliedCredits = purchaseOrder.CreditAmount,
                IsInclusivePayment = deserializedPayload.IsInclusivePayment,
                DiscountAmount = purchaseOrder.CouponAmount ?? 0
            });
            if(!makerNotification.Succeeded || makerNotification.Result == null)
            {
                return AppResult<FinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            return AppResult<FinishTransactionResult>.CreateSucceeded(new FinishTransactionResult {}, "Successfully finish transaction");
        }
        catch (Exception ex)
        {
            return AppResult<FinishTransactionResult>.CreateFailed(ex, "An error occured in FinishTransactionHandler");
        }
    }

    class PayloadData 
    {
        public IEnumerable<Student> Students {get; set;}
        public Fees Fees {get; set;}
        public string PaymentMethod {get; set;}
        public string PaymentChannel {get; set;}
        public bool IsInclusivePayment {get; set;}
        public string SelectedPeriod { get; set; }
    }

    class Student 
    {
        public int Id {get; set;}
        public string Name {get; set;}
    }

    class Fees {
        public decimal PaymentProviderFee {get; set;}
        public decimal ServiceFee {get; set;}
    }
}