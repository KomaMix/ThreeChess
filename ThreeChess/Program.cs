using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThreeChess.Data;
using ThreeChess.Hubs;
using ThreeChess.Interfaces;
using ThreeChess.Services;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseUrls(
    $"http://0.0.0.0:{7288}"
);
  
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }); ;
builder.Services.AddSignalR();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddTransient<IBoardElementsService, BoardElementsService>();
builder.Services.AddTransient<IMoveLogicalElementsService, MoveLogicalElementsService>();
builder.Services.AddSingleton<LobbyManager>();
builder.Services.AddSingleton<IGameRepository, GameRepository>();
builder.Services.AddSingleton<IGameManager, GameManager>();
builder.Services.AddSingleton<ILobbyWaitingService, LobbyWaitingService>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<MoveHub>("/moveHub");
app.MapHub<LobbyHub>("/lobbyHub");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.MapDefaultControllerRoute();

app.Run();
