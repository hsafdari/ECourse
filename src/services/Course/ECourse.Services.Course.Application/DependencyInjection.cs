using ECourse.Services.Course.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace ECourse.Services.Course.Application
{   
    public static class DependencyInjection
    {
        public static IServiceCollection AddCourseApplication(this IServiceCollection services)
        {            
            services.AddMediatR(configuration => {
                configuration.RegisterServicesFromAssembly(ApplicationAssemblyReference.Assembly);

                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly, includeInternalTypes: true);

            services.AddSingleton(TimeProvider.System);
            return services;
        }
    }
}
