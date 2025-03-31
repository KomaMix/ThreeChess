using ThreeChess.Hubs;
using ThreeChess.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddTransient<BoardCreateService>();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<MoveHub>("/move");

app.UseRouting();
app.MapControllers();


app.MapDefaultControllerRoute();

app.Run();
