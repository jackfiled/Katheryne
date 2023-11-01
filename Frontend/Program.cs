using Blazored.LocalStorage;
using Katheryne;
using Frontend.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddAntDesign();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddKatheryne();
builder.Services.AddScoped<GrammarStorageService>();

WebApplication app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.RunAsync();