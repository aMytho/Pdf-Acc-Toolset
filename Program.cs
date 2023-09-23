using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Pdf_Acc_Toolset;
using Pdf_Acc_Toolset.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<PdfManager>();
builder.Services.AddSingleton<TaskManager>();
builder.Services.AddSingleton<ConfigService>();

// Toast messages
builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();
