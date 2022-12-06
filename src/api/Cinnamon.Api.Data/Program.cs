using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Cinnamon.Api.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// entity framework
var dbConnectionString = builder.Configuration.GetConnectionString("CinnamonDB");
builder.Services.AddDbContext<ApplicationContext>(opts => opts.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);

// indentity framework
builder.Services.AddDefaultIdentity<IdentityUser>(opts => opts.SignIn.RequireConfirmedEmail = false)
    .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// logger
builder.Services.AddLogging(logBuilder => 
    logBuilder.AddSerilog(dispose: true));

var app = builder.Build();

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
