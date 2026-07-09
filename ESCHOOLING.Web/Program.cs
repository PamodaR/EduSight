using ECOMSYSTEM.Repository.ApplicationUsers;
using ECOMSYSTEM.Repository.Counselors;

using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Models;
using ECOMSYSTEM.Web;
using ECOMSYSTEM.Web.Services;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Repository.StudentMarks;
using ESCHOOLING.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ECOM_WebContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient();
builder.Services.AddScoped<IMyInterface, MyService>();
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddScoped<IApplicatioUser, ApplicationUserService>();
builder.Services.AddScoped<ICounselorRepository, CounselorRepository>();
builder.Services.AddScoped<ICounselorService, CounselorService>();
builder.Services.AddScoped<IMarksRepository, MarksRepository>();
builder.Services.AddScoped<IMarksService, MarksService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
