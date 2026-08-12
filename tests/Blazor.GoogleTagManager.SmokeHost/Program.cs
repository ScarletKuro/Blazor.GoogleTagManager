using Blazor.GoogleTagManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddGoogleTagManager(options =>
{
    options.GtmId = "GTM-SMOKE";
    options.Url = builder.Configuration["GTM_URL"] ?? "http://127.0.0.1:5517";
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapGet("/ready", () => Results.Ok(new { status = "ready" }));
app.MapGet("/gtm.js", () => Results.Text("window.__smokeGtmLoaded = true;", "application/javascript"));
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

public partial class Program;
