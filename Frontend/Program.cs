using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Katheryne;
using Frontend;
using Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAntDesign();
builder.Services.AddBlazoredLocalStorage();

builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddKatheryne();
builder.Services.AddScoped<GrammarStorageService>();

WebAssemblyHost app = builder.Build();

await app.RunAsync();