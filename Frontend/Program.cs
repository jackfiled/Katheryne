using Blazored.LocalStorage;
using Frontend.Components;
using Katheryne;
using Frontend.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAntDesign();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddKatheryne();
builder.Services.AddScoped<GrammarStorageService>();

WebApplication app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();