using ECOMSYSTEM.Repository.ApplicationUsers;
using ECOMSYSTEM.Repository.Counselors;

using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Models;
using ECOMSYSTEM.Web;
using ECOMSYSTEM.Web.Services;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Repository.StudentBehaviourEntries;
using ESCHOOLING.Repository.StudentHomework;
using ESCHOOLING.Repository.StudentMarks;
using ESCHOOLING.Repository.StudentMarksEntries;
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
builder.Services.AddScoped<IStudentMarksEntryRepository, StudentMarksEntryRepository>();
builder.Services.AddScoped<IStudentMarksEntryService, StudentMarksEntryService>();
builder.Services.AddScoped<IStudentBehaviourEntryRepository, StudentBehaviourEntryRepository>();
builder.Services.AddScoped<IStudentBehaviourEntryService, StudentBehaviourEntryService>();
builder.Services.AddScoped<IHomeworkRepository, HomeworkRepository>();
builder.Services.AddScoped<IHomeworkService, HomeworkService>();
builder.Services.AddSingleton<IOnnxMarkPredictionService, OnnxMarkPredictionService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

var app = builder.Build();

// Block startup until the ONNX model has finished loading, so the app doesn't start
// accepting requests until it's already warm. Without this, the model would still load only
// once (it's a singleton), but the very first PredictMark request after startup would pay the
// full cold-load cost — unacceptable for a live demo where that request needs to be fast.
var onnxMarkPredictionService = app.Services.GetRequiredService<IOnnxMarkPredictionService>();
await onnxMarkPredictionService.WarmupAsync();

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
