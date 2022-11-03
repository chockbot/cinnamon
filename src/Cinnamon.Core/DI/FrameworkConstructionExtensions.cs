using Dna;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Cinnamon.Core.Config;
using Cinnamon.Core.Services;
using Cinnamon.Core.Services.DefaultJsonSerialization;
using Cinnamon.Core.Module.EmailService.Handler;
using Cinnamon.Core.Module.EmailService.Handler.MicrosoftGraph;
using Cinnamon.Core.Module.NotificationService.Handler;
using Cinnamon.Core.Module.NotificationService.Handler.VerifyEmail;
using Cinnamon.Core.Module.NotificationService.Handler.WelcomeNotifiy;
using Cinnamon.Core.Module.CinnamonMakerService.Handler;
using Cinnamon.Core.Module.CinnamonMakerService.Handler.EmailConfirm;
using Cinnamon.Core.Module.CinnamonMakerService.Handler.Register;
using Cinnamon.Core.Module.CinnamonMakerService.Handler.WaitingList;
using Cinnamon.Core.Module.CinnamonMakerService;
namespace Cinnamon.Core
{
    /// <summary>
    /// Extension methods for the <see cref="FrameworkConstruction"/>
    /// </summary>
    public static class FrameworkConstructionExtensions
    {
        /// <summary>
        /// Injects the view models needed for Cinnamon application
        /// </summary>
        /// <param name="construction"></param>
        /// <returns></returns>
        public static FrameworkConstruction AddViewModels(this FrameworkConstruction construction)
        {
            // Bind to a single instance of Application view model
            construction.Services.AddSingleton<ApplicationViewModel>();

            // Return the construction for chaining
            return construction;
        }

        /// <summary>
        /// Injects the Cinnamon client application services needed
        /// for the Cinnamon application
        /// </summary>
        /// <param name="construction"></param>
        /// <returns></returns>
        public static FrameworkConstruction AddClientServices(this FrameworkConstruction construction)
        {

            // Add our task manager
            construction.Services.AddTransient<ITaskManager, BaseTaskManager>();

            // Return the construction for chaining
            return construction;
        }

        /// <summary>
        /// Inject Cinnamon core configuration from ICongif
        /// </summary>
        /// <param name="construction"></param>
        /// <returns></returns>
        public static FrameworkConstruction AddCoreConfiguration(this FrameworkConstruction construction)
        {
            CoreConfig coreConfig = new CoreConfig();
            
            // add core configuration
            construction.Configuration.GetSection("AppConfig").Bind(coreConfig);
            construction.Services.AddSingleton(coreConfig);

            // Return the construction for chaining
            return construction;
        }

        /// <summary>
        /// Inject default json serialization
        /// </summary>
        /// <param name="construction"></param>
        /// <returns></returns>
        public static FrameworkConstruction AddDefaultJsonSerialization(this FrameworkConstruction construction)
        {
            construction.Services.AddTransient<IJsonSerializationService, DefaultJsonSerializationService>();

            // Return the construction for chaining
            return construction;
        }

        public static FrameworkConstruction AddApplicationServices(this FrameworkConstruction construction)
        {
            construction.Services.AddTransient<ISendMailHandler, SendMailHandler>();
            construction.Services.AddTransient<IEmailVerification, EmailVerificationHandler>();
            construction.Services.AddTransient<IWelcomeNotification, WelcomeNotificationHandler>();
            construction.Services.AddTransient<IRegisterMaker, RegisterMakerHandler>();
            construction.Services.AddTransient<IEmailVerification, EmailVerificationHandler>();
            construction.Services.AddTransient<ISubmitWaitngList, SubmitWaitingListHandler>();
            construction.Services.AddTransient<IVerifyEmail, VerifyEmailHandler>();
            construction.Services.AddTransient<ICinnamonMakerService, CinnamonMakerServiceHandler>();

            return construction;
        }
    }
}
