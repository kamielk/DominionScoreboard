using System.Text.Json;
using Dominion.Services;
using DominionScoreboard.Data;
using DominionScoreboard.Games;
using DominionScoreboard.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(opt =>
     {
         opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
         opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
     });;

builder.Services.AddRazorPages();

// register dominion scoreboard services
builder.Services.RegisterDominionScoreboardService(builder.Configuration);

// register IValidator implementations
builder.Services.AddValidatorsFromAssemblyContaining<CreateGameRequestValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();

app.MapFallbackToFile("index.html");
app.Run();