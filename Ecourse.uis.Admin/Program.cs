using ECourse.Admin.Service;
using ECourse.Admin.Service.CourseAPI;
using ECourse.Admin.Utility;
using ECourse.Admin.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICourseLevelService, CourseLevelService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
SD.CourseAPIBase = builder.Configuration["ServiceUrls:CourseAPI"];
builder.Services.AddScoped<ICourseLevelService, CourseLevelService>();
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
