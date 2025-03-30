using ThreeChess.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSignalR();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<MoveHub>("/move");

app.UseRouting();
app.MapControllers();


app.MapDefaultControllerRoute();

app.Run();
