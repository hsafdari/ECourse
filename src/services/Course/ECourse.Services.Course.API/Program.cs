using ECourse.Services.Course.API.ExceptionHandling;
using ECourse.Services.Course.Application;
using ECourse.Services.Course.Infrastructure;
using Scalar.AspNetCore;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
    };
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services
    .AddCourseApplication()
    .AddInfrastructure(builder.Configuration);



WebApplication app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseExceptionHandler();
app.UseHttpsRedirection();
//authourization

app.MapControllers();
app.Run();
var environment = builder.Environment.EnvironmentName;

var connectionString =
    builder.Configuration.GetConnectionString("CourseDatabase");
Console.WriteLine($"Environment: {environment}");
Console.WriteLine($"CourseDatabase: {connectionString}");
public partial class Program
{
}
