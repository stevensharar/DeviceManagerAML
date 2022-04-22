using DeviceManagerAML;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Graph;
using DeviceManagerAML.Graph;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
builder.RootComponents.Add<App>("app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Adds the Microsoft graph client (Graph SDK) support for this app. 
builder.Services.AddMicrosoftGraphClient("openid", "offline_access", "https://graph.microsoft.com/User.Read");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress) });

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
    options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/User.Read");

});


await builder.Build().RunAsync();
