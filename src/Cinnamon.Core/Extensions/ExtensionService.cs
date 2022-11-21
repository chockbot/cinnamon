using Microsoft.Extensions.DependencyInjection;
using Cinnamon.Core.Module.EmailService.Handler;
using Cinnamon.Core.Module.EmailService.Handler.MicrosoftGraph;
using Cinnamon.Core.Module.NotificationService.Handler;
using Cinnamon.Core.Module.NotificationService.Handler.VerifyEmail;
using Cinnamon.Core.Module.NotificationService.Handler.WelcomeNotifiy;
using Cinnamon.Core.Module.CinnamonMakerService.Handler;
using Cinnamon.Core.Module.CinnamonMakerService.Handler.Register;
using Cinnamon.Core.Module.CinnamonMakerService.Handler.WaitingList;
using Cinnamon.Core.Module.ActivityService.Handler;
using Cinnamon.Core.Module.ActivityService.Handler.PublishActivity;
using Cinnamon.Core.Module.ActivityService.Handler.UpdateActivityHandler;
using Cinnamon.Core.Module.UploadService.Handler;
using Cinnamon.Core.Module.UploadService.Handler.AzureBlob;
using Cinnamon.Core.Module.CinnamonMakerService.Handler.Activity;
using Cinnamon.Core.Services;
using Cinnamon.Core.Services.DefaultJsonSerialization;
using Cinnamon.Core.Module.CustomerService.Handler;
using Cinnamon.Core.Module.CustomerService.Handler.SignedCustomer;
using Cinnamon.Core.Module.CustomerService.Handler.Profile;

namespace Cinnamon.Core.Extensions;

public static class ExtensionService
{
    public static IServiceCollection ExtendServices(this IServiceCollection services)
    {
        services.AddTransient<IJsonSerializationService, DefaultJsonSerializationService>();
        services.AddTransient<IUploadImages, UploadImagesHandler>();
        services.AddTransient<IDeleteImage, DeleteImageHandler>();
        services.AddTransient<ISendMailHandler, SendMailHandler>();
        services.AddTransient<IEmailVerification, EmailVerificationHandler>();
        services.AddTransient<IWelcomeNotification, WelcomeNotificationHandler>();
        services.AddTransient<IRegisterMaker, RegisterMakerHandler>();
        services.AddTransient<IEmailVerification, EmailVerificationHandler>();
        services.AddTransient<ISubmitWaitngList, SubmitWaitingListHandler>();
        services.AddTransient<IVerifyEmail, VerifyEmailHandler>();
        services.AddTransient<ISubmitUpdatedActivity, SubmitUpdatedActivityHandler>();
        services.AddTransient<IPublishActivityHandler, PublishActivityHandler>();
        services.AddTransient<IUpdateActivityHandler, UpdateActivityHandler>();
        services.AddTransient<ISignedCustomer, SignedCustomerHandler>();
        services.AddTransient<IUpdateCustomerAbout, UpdateCustomerAboutHandler>();

        return services;
    } 
}