using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazorise.RichTextEdit;
using Blazorise.DataGrid;
using Cinnamon.Web.Extensions;
using Cinnamon.Web.Providers;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;

var builder = WebApplication.CreateBuilder(args);

// register flurl
builder.Services.AddSingleton<IFlurlClientFactory,PerBaseUrlFlurlClientFactory>();

builder.Services.AddControllers();
builder.Services.AddRazorPages(opts => {
    opts.Conventions.AddAreaPageRoute("Identity", "/Account/Onboarding", "/Onboarding");
});
builder.Services.AddServerSideBlazor().AddCircuitOptions(opts => {
    opts.DetailedErrors = true;
});

// blazorise
builder.Services.AddBlazorise(options => { options.Immediate = true; })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons()
    .AddBlazoriseRichTextEdit();

builder.Services.AddSignalR(options => { options.MaximumReceiveMessageSize = 10 * 1024 * 1024;});

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
    // o.CallbackPath = builder.Configuration["AppConfig:Authentication:Google:CallbackPath"];
    o.ClaimActions.MapJsonKey("urn:google:profile", "link");
    o.ClaimActions.MapJsonKey("urn:google:image", "picture");
    o.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents {
    };
});

// add config
Cinnamon.Web.Config.Config  config = new Cinnamon.Web.Config.Config();
builder.Configuration.GetSection("AppConfig").Bind(config);
builder.Services.AddSingleton(config);

// logger
var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.WithProperty("ApplicationContext", "Cinnamon.Web")
                        .Enrich.FromLogContext()
                        .WriteTo.File(@"Logs/log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                        .CreateLogger();
builder.Host.UseSerilog(logger);

builder.Services.AppExtendServices();

var app = builder.Build();
app.UseSerilogRequestLogging();

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

