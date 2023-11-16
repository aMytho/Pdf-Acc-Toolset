// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Pdf_Acc_Toolset;
using Pdf_Acc_Toolset.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureServices(builder.Services, builder.HostEnvironment);

// Set the Env
Config.Env = builder.HostEnvironment.Environment;
Console.WriteLine($"Env is {Config.Env}");

await builder.Build().RunAsync();

// All Services must be added in this method for pre-rendering
static void ConfigureServices(IServiceCollection services, IWebAssemblyHostEnvironment hostEnv) {
    // Toast messages
    services.AddBlazoredToast();
}