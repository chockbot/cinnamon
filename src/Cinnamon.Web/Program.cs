using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazorise.RichTextEdit;
using Cinnamon.Web.Extensions;
using Cinnamon.Web.Providers;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using Cinnamon.Web.Middleware;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// register flurl
builder.Services.AddSingleton<IFlurlClientFactory,PerBaseUrlFlurlClientFactory>();

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(opts => {
    opts.DetailedErrors = true;
});
builder.Services.AddHttpContextAccessor();

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

builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = builder.Configuration["AppConfig:Authentication:Facebook:AppId"];
    facebookOptions.AppSecret = builder.Configuration["AppConfig:Authentication:Facebook:AppSecret"];
    facebookOptions.AccessDeniedPath = "/explore";
});

// add config
Cinnamon.Web.Config.Config  config = new Cinnamon.Web.Config.Config();
builder.Configuration.GetSection("AppConfig").Bind(config);
builder.Services.AddSingleton(config);

// logger
var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .Enrich.WithProperty("ApplicationContext", "Cinnamon.Web")
                        .CreateLogger();
builder.Host.UseSerilog(logger);

builder.Services.AddSingleton<IHttpContextAccessor, CustomHttpContextAccessor>();

builder.Services.AddScoped(sp =>
{
    var navMan = sp.GetRequiredService<NavigationManager>();
    return new HubConnectionBuilder()
        .WithUrl(navMan.ToAbsoluteUri(builder.Configuration["AppConfig:ChatHubUrl"]), options =>
        {
            options.AccessTokenProvider = async () =>
            {
                logger.Information("add scoped HubConnectionBuilder was called");

                var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;

                if (httpContext != null)
                {
                    logger.Information("httpContext is not null");

                    var claimsPrincipal = httpContext.User as ClaimsPrincipal;

                    var accessToken = claimsPrincipal.FindFirst(c => c.Type == "Token")?.Value;

                    foreach (var item in claimsPrincipal.Claims)
                    {
                        logger.Information($"claim type: {item.Type} | claim value: {item.Value}");

                    }

                    return accessToken;
                }

                logger.Information("httpContext is null");

                return null;
            };
        })
        .WithAutomaticReconnect(new TimeSpan[] { TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) })
        .Build();
});

builder.Services.AppExtendServices();


var app = builder.Build();
app.UseSerilogRequestLogging();

app.Use((context,next) => {
    if (context.Request.Headers["x-forwarded-proto"] == "https")
    {
        context.Request.Scheme = "https";
    }
    return next();
});

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
app.UseMiddleware<PageAuthMiddleware>();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();

public class CustomHttpContextAccessor : IHttpContextAccessor
{
    private static AsyncLocal<HttpContext> _httpContextCurrent = new AsyncLocal<HttpContext>();

    public HttpContext HttpContext
    {
        get => _httpContextCurrent.Value;
        set => _httpContextCurrent.Value = value;
    }
}