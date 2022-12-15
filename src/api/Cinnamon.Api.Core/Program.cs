using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Newtonsoft.Json.Serialization;
using Cinnamon.Api.Core.Extensions;
using Cinnamon.Api.Core.Config;
using Flurl.Http.Configuration;
using Flurl.Http;
using Cinnamon.Api.Core.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register application config as singleton instance
ApplicationConfig applicationConfig = new ApplicationConfig();
builder.Configuration.GetSection("Applicationconfig").Bind(applicationConfig);
builder.Services.AddSingleton(applicationConfig);

// register flurl
builder.Services.AddSingleton<IFlurlClientFactory,PerBaseUrlFlurlClientFactory>();

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
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// logger
builder.Services.AddLogging(logBuilder => 
    logBuilder.AddSerilog(dispose: true));

var app = builder.Build();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
