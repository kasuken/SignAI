using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignAI.Func.Settings;
using System.IO;

var builder = FunctionsApplication.CreateBuilder(args);

// Configure app configuration sources
builder.Configuration.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

// Configure ASP.NET Core integration
builder.ConfigureFunctionsWebApplication();

// Add services
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// Configure strongly typed settings with explicit value retrieval
builder.Services.Configure<EmailSettings>(options => 
{
    // First try to bind from the hierarchical section
    builder.Configuration.GetSection("Email").Bind(options);
    
    // Also try to bind from flattened keys in Values section
    options.DefaultSender = builder.Configuration["Values:Email:DefaultSender"] ?? 
                           builder.Configuration["Email:DefaultSender"] ?? 
                           options.DefaultSender;
    
    options.DefaultSubject = builder.Configuration["Values:Email:DefaultSubject"] ?? 
                            builder.Configuration["Email:DefaultSubject"] ?? 
                            options.DefaultSubject;
    
    if (bool.TryParse(builder.Configuration["Values:Email:TrackOpens"] ?? 
                     builder.Configuration["Email:TrackOpens"], out bool trackOpens))
    {
        options.TrackOpens = trackOpens;
    }
    
    options.DefaultTextBody = builder.Configuration["Values:Email:DefaultTextBody"] ?? 
                             builder.Configuration["Email:DefaultTextBody"] ?? 
                             options.DefaultTextBody;
    
    options.DefaultHtmlTemplate = builder.Configuration["Values:Email:DefaultHtmlTemplate"] ?? 
                                 builder.Configuration["Email:DefaultHtmlTemplate"] ?? 
                                 options.DefaultHtmlTemplate;
    
    options.DefaultTag = builder.Configuration["Values:Email:DefaultTag"] ?? 
                        builder.Configuration["Email:DefaultTag"] ?? 
                        options.DefaultTag;
    
    options.DefaultMessageStream = builder.Configuration["Values:Email:DefaultMessageStream"] ?? 
                                  builder.Configuration["Email:DefaultMessageStream"] ?? 
                                  options.DefaultMessageStream;
    
    if (bool.TryParse(builder.Configuration["Values:Email:EnableReplyTracking"] ?? 
                      builder.Configuration["Email:EnableReplyTracking"], out bool enableReplyTracking))
    {
        options.EnableReplyTracking = enableReplyTracking;
    }
});

builder.Services.Configure<PostmarkSettings>(options => 
{
    // First try to bind from the hierarchical section
    builder.Configuration.GetSection("Postmark").Bind(options);
    
    // Fallback to the individual configuration values
    options.ServerToken = builder.Configuration["Values:Postmark:ServerToken"] ?? 
                          builder.Configuration["Postmark:ServerToken"] ?? 
                          builder.Configuration["PostmarkServerToken"] ?? 
                          options.ServerToken;
    
    options.AccountToken = builder.Configuration["Values:Postmark:AccountToken"] ?? 
                          builder.Configuration["Postmark:AccountToken"] ?? 
                          options.AccountToken;
    
    if (int.TryParse(builder.Configuration["Values:Postmark:RetryCount"] ?? 
                     builder.Configuration["Postmark:RetryCount"], out int retryCount))
    {
        options.RetryCount = retryCount;
    }
    
    if (TimeSpan.TryParse(builder.Configuration["Values:Postmark:RetryDelay"] ?? 
                          builder.Configuration["Postmark:RetryDelay"], out TimeSpan retryDelay))
    {
        options.RetryDelay = retryDelay;
    }
    
    if (int.TryParse(builder.Configuration["Values:Postmark:TimeoutSeconds"] ?? 
                     builder.Configuration["Postmark:TimeoutSeconds"], out int timeoutSeconds))
    {
        options.TimeoutSeconds = timeoutSeconds;
    }
    
    if (bool.TryParse(builder.Configuration["Values:Postmark:UseHttps"] ?? 
                      builder.Configuration["Postmark:UseHttps"], out bool useHttps))
    {
        options.UseHttps = useHttps;
    }
    
    options.BaseUrl = builder.Configuration["Values:Postmark:BaseUrl"] ?? 
                      builder.Configuration["Postmark:BaseUrl"] ?? 
                      options.BaseUrl;
    
    options.WebhookUrl = builder.Configuration["Values:Postmark:WebhookUrl"] ?? 
                         builder.Configuration["Postmark:WebhookUrl"] ?? 
                         options.WebhookUrl;
});

var app = builder.Build();
app.Run();
