using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Pdf_Acc_Toolset;
using Pdf_Acc_Toolset.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Toast messages
builder.Services.AddBlazoredToast();

// Set the Env
Config.Env = builder.HostEnvironment.Environment;
Console.WriteLine($"Env is {Config.Env}");

await builder.Build().RunAsync();
