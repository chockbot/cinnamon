using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dna;
using Cinnamon.Web.Areas.Identity;
using Cinnamon.Data;
using Cinnamon.Core;

var builder = WebApplication.CreateBuilder(args);

// Application Setup
Framework.Construct<DefaultFrameworkConstruction>()
    .AddFileLogger()
    .UseClientDataStore()
    .AddViewModels()
    .AddClientServices()
    .Build();

// Ensure the client data store 
await Framework.Service<IDataStore>().EnsuredataStoreAsync();

// Apply Seed Data
await Framework.Service<ApplicationViewModel>().applySeedDemoData();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("CinnamonDB");
builder.Services.AddDbContext<DataStoreDbContext>(options =>
    options.UseNpgsql(connectionString),ServiceLifetime.Transient);
builder.Services.AddScoped<DataStore>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<DataStoreDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

