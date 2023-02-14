using Microsoft.EntityFrameworkCore;
using Notiflex.Infrastructure.Data;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
using Notiflex.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

using Notiflex.Core.Services.Contracts;
using Notiflex.Core.Services.AccountServices;
using Notiflex.Core.Services.BotServices;
using Notiflex.Core.Services.APIServices;
using Notiflex.Core.Services.BOtServices;
using Notiflex.MapperProfiles;
using Quartz;
using Notiflex.Core.Quartz.Jobs;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Notiflex.Core.Services.SchedulerServices;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NotiflexDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<NotiflexUser>(options => options.SignIn.RequireConfirmedAccount = false)
     .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NotiflexDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/Account/Login";
    options.AccessDeniedPath = "/Home/Account/Logout";
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<AccountMapperProfile>();
    config.AddProfile<DashboardMapperProfile>();
});

builder.Services.AddQuartz(config =>
{
    config.SchedulerId = "Notiflex-Scheduler";
    config.UseMicrosoftDependencyInjectionJobFactory();
    config.UseSimpleTypeLoader();
    config.UsePersistentStore(a =>
    {
        a.UseSqlServer(connectionString);
        a.UseProperties = true;
        a.UseJsonSerializer();
    });
    config.UseDefaultThreadPool(tp =>
    {
        tp.MaxConcurrency = 20;
    });

    config.AddJob<ReportSenderJob>(j =>
    {
        j.WithIdentity("ReportSenderJob");
        j.StoreDurably(true);
    });


});
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = false;
});

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IMessageSender, MessageSender>();
builder.Services.AddScoped<IMessageConfigurer, MessageConfigurer>();
builder.Services.AddScoped<IWeatherApiService, WeatherAPIService>();
builder.Services.AddScoped<IModelConfigurer, ModelConfigurer>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ITriggerService, TriggerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    app.MapControllerRoute(
    name: "areas",
    pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}"
    );
});

//app.MapRazorPages();
app.Run();

[ExcludeFromCodeCoverage]
public partial class Program { }