
using AutoMapper;
using AutoMapper.Internal;
using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto;
using ECourse.Services.CourseAPI.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Repository;
using Microsoft.OpenApi.Models;
using System.Reflection;
using static Infrastructure.BaseDbContext;

namespace ECourse.Services.CourseAPI;
public class Startup(IConfiguration configuration)
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        // Register MongoDB Context
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        // Register the database context
        services.AddSingleton<ApplicationDbContext>();
        // Register Repository
        //it should be dynamic for all repositories
        //services.AddScoped<ICourseLevelRepository, CourseLevelRepository>();
        // Register Repository
        services.AddLogging();
        services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECourse.Services.CourseAPI", Version = "v1" });
});
        //load repository dynamically
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.Scan(scan => scan
       .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies()) // Scan all loaded assemblies
       .AddClasses(classes => classes.AssignableTo(typeof(IBaseRepository<>))) // Find classes that implement IBaseRepository<T>
       .AsImplementedInterfaces()
       .WithScopedLifetime()
   );

        IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
        services.AddSingleton(mapper);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddCors(option =>
        {
            option.AddPolicy("AllowAll",
                b => b.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin());

        });
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
        services.AddFluentValidationAutoValidation();
        //it should be dynamic for all validations       
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CourseAPI v1"));
        }

        app.UseRouting();
        app.UseAuthorization();
        app.UseCors("AllowAll");
        app.UseEndpoints(endpoints =>
        {
            //endpoints.MapHealthChecks("/healthcheck");
            endpoints.MapControllers();
        });
    }
}