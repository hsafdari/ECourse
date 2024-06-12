
using AutoMapper;
using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Repositories;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


namespace ECourse.Services.CourseAPI;

public class Startup(IConfiguration configuration)
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddHealthChecks();
        services.AddDbContextFactory<ApplicationDataContext>(op => 
        op.UseMongoDB(configuration.GetSection("mongo")
        .GetSection("connectionString").Value, 
        configuration.GetSection("mongo")
        .GetSection("database").Value));

        services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECourse.Services.CourseAPI", Version = "v1" });
});
        //load repository dynamically
        services.Scan(scan => scan.FromCallingAssembly()
            .AddClasses()
            .AsImplementedInterfaces()
            .WithScopedLifetime());
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
            endpoints.MapHealthChecks("/healthcheck");
            endpoints.MapControllers();
        });
    }
}