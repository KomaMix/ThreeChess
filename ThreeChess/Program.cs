using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThreeChess.Data;
using ThreeChess.Hubs;
using ThreeChess.Services;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseUrls(
    $"http://0.0.0.0:{7288}"
);

builder.Services.AddControllers();
builder.Services.AddSignalR();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddTransient<BoardCreateService>();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<MoveHub>("/move");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.MapDefaultControllerRoute();

app.Run();
