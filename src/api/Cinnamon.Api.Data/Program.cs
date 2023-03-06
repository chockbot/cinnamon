using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using Cinnamon.Api.Data.Repository;
using Cinnamon.Api.Data.Extensions;
using Newtonsoft.Json.Serialization;
using Cinnamon.Api.Data.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// entity framework
var dbConnectionString = builder.Configuration.GetConnectionString("CinnamonDB");
builder.Services.AddDbContext<ApplicationContext>(opts => opts.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);

// indentity framework
builder.Services.AddDefaultIdentity<IdentityUser>(opts => opts.SignIn.RequireConfirmedEmail = false)
    .AddEntityFrameworkStores<ApplicationContext>();

// logger
var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.WithProperty("ApplicationContext", "Cinnamon.API.Data")
                        .Enrich.FromLogContext()
                        .WriteTo.File(@"Logs/log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// logger
builder.Services.AddLogging(logBuilder => 
    logBuilder.AddSerilog(dispose: true));

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

    options.User.RequireUniqueEmail = true;
});

var app = builder.Build();
app.UseSerilogRequestLogging();

// seed database data and ensure table are created
using (var scope = app.Services.CreateScope())
{
    var store = scope.ServiceProvider.GetRequiredService<IDataStore>();
    await store.EnsureMigrate();
    await store.SeedData();
}

// for postgres options to enable timestamp legacy behaviour
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
