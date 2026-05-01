using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Load AppConfig from appsettings
var appSettings = builder.Configuration.GetSection("AppSettings");
VoltigeCore.AppConfig.ConnectionString = builder.Configuration.GetConnectionString("VaultingDB")!;
VoltigeCore.AppConfig.BasePath = appSettings["BasePath"] ?? @"C:\episerver\voltige\VoltigeClosedXML\";
VoltigeCore.AppConfig.ContestId = int.TryParse(appSettings["ContestId"], out var cid) ? cid : 2;
VoltigeCore.AppConfig.IsTraHastTavling = bool.TryParse(appSettings["Trahasttavling"], out var tra) && tra;
VoltigeCore.AppConfig.HorsePointTraHastTavling = float.TryParse(appSettings["HorsePointTraHastTavling"],
    NumberStyles.Any, CultureInfo.InvariantCulture, out var hp) ? hp : 6.5f;

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
