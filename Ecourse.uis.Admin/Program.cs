using ECourse.Admin.Service;
using ECourse.Admin.Service.CourseAPI;
using ECourse.Admin.Utility;
using ECourse.Admin.Components;
using ECourse.Admin.Service.FilesManager;
using Radzen;
using ECourse.Admin.Components.Layout;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICourseLevelService, CourseLevelService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IFTPService, FTPService>();
builder.Services.AddScoped<IFileManagerService, FileManagerService>();
builder.Services.AddRadzenComponents();

SD.CourseAPIBase = builder.Configuration["ServiceUrls:CourseAPI"] ?? throw new Exception("Service Url not configured correctly");
SD.UploadMode =(FileUploadMode) Enum.Parse(typeof(FileUploadMode),builder.Configuration["FileUploadMode"] ?? throw new Exception("FileUploadMode not configured correctly"));
builder.Services.AddScoped<ICourseLevelService, CourseLevelService>();
builder.Services.AddBlazoredLocalStorage();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
