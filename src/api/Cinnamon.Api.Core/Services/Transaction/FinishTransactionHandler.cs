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
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Microsoft.AspNetCore.SignalR;
using Cinnamon.Api.Core.Hubs;

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
    private readonly ICreateChatRoomHandler createChatRoomHandler;
    private readonly IHubContext<ChatHub> chathub;

    public FinishTransactionHandler(ICreateOngoingActivityHandler createOngoingActivityHandler, ICustomerPayedNotificationHandler customerPayedNotificationHandler,
        IMakerEnrolledNotificationHandler makerEnrolledNotificationHandler, IJsonSerializationProvider jsonSerializationProvider,
        IPurchaseOrderData purchaseOrderData, ICustomerData customerData, IGetActivityHandler getActivityHandler,
        IUpdateCreditBalanceHandler updateCreditBalanceHandler, ICreateChatRoomHandler createChatRoomHandler,
        IHubContext<ChatHub> chathub)
    {
        this.createOngoingActivityHandler = createOngoingActivityHandler;
        this.customerPayedNotificationHandler = customerPayedNotificationHandler;
        this.makerEnrolledNotificationHandler = makerEnrolledNotificationHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.purchaseOrderData = purchaseOrderData;
        this.customerData = customerData;
        this.getActivityHandler = getActivityHandler;
        this.updateCreditBalanceHandler = updateCreditBalanceHandler;
        this.createChatRoomHandler = createChatRoomHandler;
        this.chathub = chathub;
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

            //Check if user selected schedule 
            if (purchaseOrder.ScheduleId != 0)
            {
                var createOngoingActivityRes = await createOngoingActivityHandler.ExecuteAsync(new OngoingActivityService.Interactors.CreateOngoingActivityArgs
                {
                    ActivityId = purchaseOrder.ActivityId,
                    CustomerId = purchaseOrder.CustomerId,
                    PurchaseOrderId = purchaseOrder.Id,
                    ScheduleId = purchaseOrder.ScheduleId,
                    Students = deserializedPayload.Students.Select(s => {
                        return new OngoingActivityService.Interactors.CreateOngoingActivityArgs.Student
                        {
                            FamilyMemberId = s.Id,
                            Name = s.Name
                        };
                    }),
                    SelectedPeriod = deserializedPayload.SelectedPeriod,
                });
                if (!createOngoingActivityRes.Succeeded || createOngoingActivityRes.Result == null)
                {
                    return AppResult<FinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
                }
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
                Amount         = purchaseOrder.Total,
                CoachName      = $"{activity.Owner?.FirstName} {activity.Owner?.LastName}",
                CoachNumber    = activity.Owner.PhoneNumber,
                CustomerName   = $"{customer.FirstName} {customer.LastName}",
                Email          = customer.Email,
                ExperienceName = activity.Title,
                PayerName      = $"{customer.FirstName} {customer.LastName}",
                PurchaseDate   = DateTime.Now,
                Members        = deserializedPayload.Students.Select(s => {
                    return new Modules.NotificationDriver.Interactors.CustomerPayedNotificationArgs.IncludedMembers {
                        Name = s.Name,
                    };
                }),
                PaymentMethod      = deserializedPayload.PaymentChannel ?? deserializedPayload.PaymentMethod,
                ReferenceNumber    = referenceId,
                MakerEmail         = $"{activity.Owner?.Email}",
                ServiceFee         = deserializedPayload.Fees.ServiceFee,
                PaymentProviderFee = deserializedPayload.Fees.PaymentProviderFee,
                AppliedCredits     = purchaseOrder.CreditAmount,
                IsInclusivePayment = deserializedPayload.IsInclusivePayment,
                DiscountAmount     = purchaseOrder.CouponAmount ?? 0,
                AddOnsAmount       = purchaseOrder.AddOnsAmount,
                AddOnsDetails      = deserializedPayload.AddOnsDetails.Select(a =>
                {
                    return new Modules.NotificationDriver.Interactors.CustomerPayedNotificationArgs.AddOnDetails {
                        AddOnName = a.AddOnName,
                        AddOnCount = a.AddOnCount

                    };
                })
            });
            if(!emailNotifyRes.Succeeded || emailNotifyRes.Result == null)
            {
                return AppResult<FinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            // send mail notification for maker
            var makerNotification = await makerEnrolledNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.MakerEnrolledNotificationArgs {
                Amount         = purchaseOrder.Total,
                Email          = $"{activity.Owner?.Email}",
                ExperienceName = activity.Title,
                MakerName      = $"{activity.Owner?.FirstName} {activity.Owner?.LastName}",
                PayerName      = $"{customer.FirstName} {customer.LastName}",
                PurchaseDate   = DateTime.Now,
                Students       = deserializedPayload.Students.Select(s => {
                    return new Modules.NotificationDriver.Interactors.MakerEnrolledNotificationArgs.IncludedStudents {
                        Name = s.Name
                    };
                }),
                PaymentMethod      = deserializedPayload.PaymentChannel ?? deserializedPayload.PaymentMethod,
                ReferenceNumber    = referenceId,
                PayerEmail         = customer.Email,
                ServiceFee         = deserializedPayload.Fees.ServiceFee,
                PaymentProviderFee = deserializedPayload.Fees.PaymentProviderFee,
                AppliedCredits     = purchaseOrder.CreditAmount,
                IsInclusivePayment = deserializedPayload.IsInclusivePayment,
                DiscountAmount     = purchaseOrder.CouponAmount ?? 0,
                AddOnsAmount       = purchaseOrder.AddOnsAmount,
                AddOnsDetails      = deserializedPayload.AddOnsDetails.Select(a =>
                {
                    return new Modules.NotificationDriver.Interactors.MakerEnrolledNotificationArgs.AddOnDetails
                    {
                        AddOnName = a.AddOnName,
                        AddOnCount = a.AddOnCount
                    };
                })
            });
            if(!makerNotification.Succeeded || makerNotification.Result == null)
            {
                return AppResult<FinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            // add in group chat
            var groupName = Guid.NewGuid().ToString();
            var createChatRes = await createChatRoomHandler.ExecuteAsync(new ChatService.Interactors.CreateChatRoomArgs {
                ChatName = $"{activity.Owner?.FirstName} {activity.Owner?.LastName}'s Chat Group",
                ChatType = Framework.Enums.Enums.ChatType.GroupChat,
                FromUserId = purchaseOrder.CustomerId,
                GroupName = groupName,
                ToUserId = activity.Owner?.Id ?? 0
            });
            if(createChatRes.Succeeded && createChatRes.Result is not null)
            {
                await chathub.Clients.All.SendAsync("AddToGroupAfterPayment", $"{createChatRes.Result.GroupName}|{createChatRes.Result.ChatRoomId}|{customer.Id}|{customer.FirstName}|{customer.LastName}|{customer.ProfileImg}|{activity.Owner?.FirstName} {activity.Owner?.LastName}'s Chat Group");
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
        public IEnumerable<AddOnDetails> AddOnsDetails { get; set;}
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
    class AddOnDetails
    {
        public int AddOnId { get; set; }
        public string AddOnName { get; set; }
        public int AddOnCount { get; set; }
    }
}