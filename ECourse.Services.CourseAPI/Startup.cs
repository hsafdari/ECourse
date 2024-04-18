//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Diagnostics.HealthChecks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Rewrite;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Diagnostics.HealthChecks;
//using Microsoft.Extensions.Hosting;
//using Microsoft.OpenApi.Models;
//using ECourse.MiddleWares.MiddleWare;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Repository;
using Microsoft.OpenApi.Models;
using Middleware;
using MongoDB.Driver;
using System;

namespace ECourse.Services.CourseAPI;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddMongoDb(Configuration);
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECourse.Services.CourseAPI", Version = "v1" });
        });
        services.AddSingleton<ICourseRepository>(sp =>
        new CourseRepository(sp.GetService<IMongoDatabase>() ?? throw new Exception("IMongoDatabase not found"), Course.DocumentName));
        services.AddSingleton<ICourseLevelRepository>(sp=>new CourseLevelRepository(sp.GetService<IMongoDatabase>() ?? throw new Exception("IMongoDatabase not found"), CourseLevel.DocumentName));
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping.API v1"));
        }

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}