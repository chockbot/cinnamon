using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using Newtonsoft.Json.Serialization;
using Cinnamon.Api.Core.Extensions;
using Cinnamon.Api.Core.Config;
using Flurl.Http.Configuration;
using Flurl.Http;
using Cinnamon.Api.Core.Providers;
using Microsoft.AspNetCore.Http.Features;
using Quartz;
using Cinnamon.Api.Core.Services.JobService;
using Microsoft.AspNetCore.ResponseCompression;
using Cinnamon.Api.Core.Hubs;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register application config as singleton instance
ApplicationConfig applicationConfig = new ApplicationConfig();
builder.Configuration.GetSection("Applicationconfig").Bind(applicationConfig);
builder.Services.AddSingleton(applicationConfig);
builder.Services.AddSignalR().AddAzureSignalR(builder.Configuration.GetSection("ConnectionStrings:AzureSignalRConnectionString").Value);
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

// register flurl
builder.Services.AddSingleton<IFlurlClientFactory,PerBaseUrlFlurlClientFactory>();

// register http context accessor
builder.Services.AddHttpContextAccessor();

// logger
var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .Enrich.WithProperty("ApplicationContext", "Cinnamon.API.Core")
                        .CreateLogger();
builder.Host.UseSerilog(logger);

// auto mapper
var mapperConfig = new MapperConfiguration(mapper => {
    mapper.AddProfile(new Cinnamon.Api.Core.Models.MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// register application services
builder.Services.ExtendServices();

// register newtonsoft.json
builder.Services.AddControllers()
    .AddNewtonsoftJson(opts =>
    {
        opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });

// add jwt authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts => {
        opts.SaveToken = true;
        opts.TokenValidationParameters = new TokenValidationParameters {
            ValidIssuer = applicationConfig.Jwt.Issuer,
            ValidAudience = applicationConfig.Jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(applicationConfig.Jwt.Key)),
            ValidateIssuer = true,
            ValidateAudience = true
        };
        opts.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/hubs/chat")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<FormOptions>(opts => {
    // 10mb
    opts.MemoryBufferThreshold = 10000000;
});

builder.Services.AddQuartz(q => {
    q.UseMicrosoftDependencyInjectionJobFactory();

    // if(applicationConfig.Disbursement.RunDisbursement)
    // {
    //     var payoutJobkey = new JobKey("GeneratePayoutHandler");
    //     q.AddJob<GeneratePayoutJob>(opts => opts.WithIdentity(payoutJobkey));

    //     q.AddTrigger(opts => opts
    //         .ForJob(payoutJobkey)
    //         .WithIdentity("GeneratePayoutHandler-trigger")
    //         .WithCronSchedule(applicationConfig.Disbursement.CronString)
    //     );
    // }

    var activityGuidJobKey = new JobKey("UpdateActivityGuidHandler");
    q.AddJob<UpdateActivityGuidJob>(opts => opts.WithIdentity(activityGuidJobKey));

    q.AddTrigger(opts => opts
        .ForJob(activityGuidJobKey)
        .WithIdentity("UpdateActivityGuidHandler-trigger")
        .WithSimpleSchedule(x => x.WithIntervalInHours(applicationConfig.Activity.RunPerHour).RepeatForever())
    );

    if(applicationConfig.Sitemap.RunSitemap)
    {
        var generateSitemapJobKey = new JobKey("GenerateSitemapHandler");
        q.AddJob<GenerateSitemapJob>(opts => opts.WithIdentity(generateSitemapJobKey));
        q.AddTrigger(opts => opts
            .ForJob(generateSitemapJobKey)
            .WithIdentity("GenerateSitemapHandler-trigger")
            .WithSimpleSchedule(x => x.WithIntervalInHours(applicationConfig.Sitemap.RunPerHour).RepeatForever())
        );
    }

    if(applicationConfig.ExpiringActivityNotification.IsRunNotification)
    {
        var expiredNotificationKey = new JobKey("NotifyExpiringStudentJob");
        q.AddJob<NotifyExpiringStudentJob>(opts => opts.WithIdentity(expiredNotificationKey));
        q.AddTrigger(opts => opts
            .ForJob(expiredNotificationKey)
            .WithIdentity("NotifyExpiringStudentJob-trigger")
            // .WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever())
            .WithCronSchedule(applicationConfig.ExpiringActivityNotification.CronString)
        );
    }

    var generateDisbursementKey = new JobKey("GenerateDisbursementJob");
    q.AddJob<GenerateDisbursementJob>(opts => opts.WithIdentity(generateDisbursementKey));
    q.AddTrigger(opts => opts
        .ForJob(generateDisbursementKey)
        .WithIdentity("GenerateDisbursementJob-trigger")
        .WithSimpleSchedule(x => x.WithIntervalInHours(2).RepeatForever())
    );

    if(applicationConfig.Disbursement.RunDisbursement)
    {
        var generateDisbursementPayoutKey = new JobKey("GenerateDisbursementPayoutJob");
        q.AddJob<GenerateDisbursementPayoutJob>(opts => opts.WithIdentity(generateDisbursementPayoutKey));
        q.AddTrigger(opts => opts
            .ForJob(generateDisbursementPayoutKey)
            .WithIdentity("GenerateDisbursementPayoutJob-trigger")
            .WithCronSchedule(applicationConfig.Disbursement.CronString)
            // .WithSimpleSchedule(x => x.WithIntervalInHours(3).RepeatForever())
        );
    }

    var unreadMessagesNotificationKey = new JobKey("GenerateUnreadChatsNotificationJob");
    q.AddJob<GenerateUnreadChatsNotificationJob>(opts => opts.WithIdentity(unreadMessagesNotificationKey));
    q.AddTrigger(opts => opts
        .ForJob(unreadMessagesNotificationKey)
        .WithIdentity("GenerateUnreadChatsNotificationJob-trigger")
        .WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever())
    );

    var forceDisabledExpiredEventKey = new JobKey("ForceDisableExpiredEventJob");
    q.AddJob<ForceDisableExpiredEventJob>(otps => otps.WithIdentity(forceDisabledExpiredEventKey));
    q.AddTrigger(opts => opts
        .ForJob(forceDisabledExpiredEventKey)
        .WithIdentity("ForceDisableExpiredEventJob-trigger")
        .WithSimpleSchedule(x => x.WithIntervalInMinutes(5).RepeatForever())
    );

    if(applicationConfig.ActivitySummary.RunJob)
    {
        var updateActivitySummaryJobKey = new JobKey("UpdateActivitySummaryJob");
        q.AddJob<UpdateActivitySummaryJob>(opts => opts.WithIdentity(updateActivitySummaryJobKey));
        q.AddTrigger(opts => opts
            .ForJob(updateActivitySummaryJobKey)
            .WithIdentity("UpdateActivitySummaryJob-trigger")
            .WithSimpleSchedule(x => x.WithIntervalInMinutes(applicationConfig.ActivitySummary.RunPerMinute).RepeatForever())
        );
    }

    if(applicationConfig.EventReminder.RunJob)
    {
        var eventReminderJobKey = new JobKey("EventReminderJob");
        q.AddJob<EventReminderJob>(opts => opts.WithIdentity(eventReminderJobKey));
        q.AddTrigger(opts => opts
            .ForJob(eventReminderJobKey)
            .WithIdentity("EventReminderJob-trigger")
            .WithSimpleSchedule(x => x.WithIntervalInHours(applicationConfig.EventReminder.RunPerHour).RepeatForever())
        );
    }

    if(applicationConfig.EventThankYou.RunJob)
    {
        var eventThankYouJobKey = new JobKey("EventThankYouJob");
        q.AddJob<EventThankYouJob>(opts => opts.WithIdentity(eventThankYouJobKey));
        q.AddTrigger(opts => opts
            .ForJob(eventThankYouJobKey)
            .WithIdentity("EventThankYouJob-trigger")
            .WithSimpleSchedule(x => x.WithIntervalInHours(applicationConfig.EventThankYou.RunPerHour).RepeatForever())
        );
    }
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();
app.UseResponseCompression();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // for development only to disable flurl untrusted certificates
    FlurlHttp.Configure(settings => {
        settings.HttpClientFactory = new UntrustedCertClientFactory();
    });
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub", options =>
{
    options.TransportMaxBufferSize = 256000;
    options.ApplicationMaxBufferSize = 256000;
    options.Transports = (Microsoft.AspNetCore.Http.Connections.HttpTransportType)TransportType.All;
});

app.Run();
