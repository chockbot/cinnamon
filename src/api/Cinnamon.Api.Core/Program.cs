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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register application config as singleton instance
ApplicationConfig applicationConfig = new ApplicationConfig();
builder.Configuration.GetSection("Applicationconfig").Bind(applicationConfig);
builder.Services.AddSingleton(applicationConfig);

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

    if(applicationConfig.Disbursement.RunDisbursement)
    {
        var payoutJobkey = new JobKey("GeneratePayoutHandler");
        q.AddJob<GeneratePayoutJob>(opts => opts.WithIdentity(payoutJobkey));

        q.AddTrigger(opts => opts
            .ForJob(payoutJobkey)
            .WithIdentity("GeneratePayoutHandler-trigger")
            .WithSimpleSchedule(x => x.WithIntervalInHours(applicationConfig.Disbursement.RunPerHour).RepeatForever())
        );
    }
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();
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

app.Run();
