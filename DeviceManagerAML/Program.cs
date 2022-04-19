using DeviceManagerAML;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
builder.RootComponents.Add<App>("app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions);
    //options.ProviderOptions.ResponseType = "code";
});

await builder.Build().RunAsync();
