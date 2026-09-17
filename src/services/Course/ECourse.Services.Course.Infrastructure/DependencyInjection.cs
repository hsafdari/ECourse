using ECourse.Services.Course.Application.Abstractions.Persistence;
using ECourse.Services.Course.Application.Features.CourseGroups.GetById;
using ECourse.Services.Course.Application.Features.CourseGroups.GetList;
using ECourse.Services.Course.Infrastructure.Persistence;
using ECourse.Services.Course.Infrastructure.Persistence.Queries.CourseGroups;
using ECourse.Services.Course.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECourse.Services.Course.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("CourseDatabase") ?? throw new InvalidOperationException("Connection string 'Course Database' not found.");
            services.AddDbContext<CourseDbContext>(options =>
            {
                options.UseNpgsql(connectionString, 
                        npgsqlOptions => 
                        { npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorCodesToAdd: null); });
            });

            services.AddScoped<ICourseGroupRepository, CourseGroupRepository>();
            services.AddScoped<IGetCourseGroupByIdReadService,GetCourseGroupByIdReadService>();
            services.AddScoped<IGetCouseGroupsByParentIdReadService,GetCouseGroupsByParentIdReadService>();

            services.AddScoped<IUnitOfWork>(serviceProvider=> serviceProvider.GetRequiredService<CourseDbContext>());
            return services;

        }
    }
}
