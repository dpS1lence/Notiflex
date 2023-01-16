using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Notiflex.Infrastructure.Data;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
using Notiflex.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

using Notiflex.Core.Services.Contracts;
using Notiflex.Core.Services.AccountServices;
using Notiflex.Core.Services.BotServices;
using Notiflex.Core.Services.Contracts;
using Notiflex.Core.Services.APIServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NotiflexDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<NotiflexUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<NotiflexDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/Home/Index";
});

builder.Services.AddAutoMapper(config =>
{
    //TODO: Add Profiles
});

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IMessageSender, MessageSender>();
builder.Services.AddScoped<IWeatherApiService, WeatherAPIService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

app.Run();
