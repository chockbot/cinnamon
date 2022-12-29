using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Cinnamon.Core;
using Cinnamon.Core.Config;
using Cinnamon.Core.Extensions;
using Cinnamon.Core.Models;
using Cinnamon.Data;
using Cinnamon.Web.Areas.Identity;
using Cinnamon.Web.Extensions;
using Cinnamon.Web.Providers;
using Dna;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Application Setup
Framework.Construct<DefaultFrameworkConstruction>()
    .AddFileLogger()
    .UseClientDataStore()
    .AddViewModels()
    .AddClientServices()
    .Build();

// Ensure the client data store 
// await Framework.Service<IDataStore>().EnsuredataStoreAsync();

// Apply Seed Data
// await Framework.Service<ApplicationViewModel>().applySeedDemoData();

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("CinnamonDB");
//builder.Services.AddDbContext<DataStoreDbContext>(options =>
//    options.UseNpgsql(connectionString),ServiceLifetime.Transient);

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// register flurl
builder.Services.AddSingleton<IFlurlClientFactory,PerBaseUrlFlurlClientFactory>();

builder.Services.AddControllers();
builder.Services.AddRazorPages(opts => {
    opts.Conventions.AddAreaPageRoute("Identity", "/Account/Onboarding", "/Onboarding");
});
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

builder.Services.AddBlazorise(options => { options.Immediate = true; })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();
builder.Services.AddSignalR(options => { options.MaximumReceiveMessageSize = 10 * 1024 * 1024;});

builder.Services.AddScoped<TokenProvider>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opts => {
        opts.ExpireTimeSpan = TimeSpan.FromDays(1);
        opts.SlidingExpiration = true;
        opts.AccessDeniedPath = "/explore";
        opts.Cookie.Name = "auth";
    });

builder.Services.AddAuthentication().AddGoogle(o =>
{
    o.ClientId = builder.Configuration["AppConfig:Authentication:Google:ClientId"];
    o.ClientSecret = builder.Configuration["AppConfig:Authentication:Google:ClientSecret"];
    o.CallbackPath = builder.Configuration["AppConfig:Authentication:Google:CallbackPath"];
    o.ClaimActions.MapJsonKey("urn:google:profile", "link");
    o.ClaimActions.MapJsonKey("urn:google:image", "picture");
});

CoreConfig coreConfig = new CoreConfig();
// add core configuration
builder.Configuration.GetSection("AppConfig").Bind(coreConfig);
builder.Services.AddSingleton(coreConfig);

// add config
Cinnamon.Web.Config.Config  config = new Cinnamon.Web.Config.Config();
builder.Configuration.GetSection("AppConfig").Bind(config);
builder.Services.AddSingleton(config);

builder.Services.ExtendServices();
builder.Services.AppExtendServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    // for development only to disable flurl untrusted certificates
    FlurlHttp.Configure(settings => {
        settings.HttpClientFactory = new UntrustedCertClientFactory();
    });
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
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();

