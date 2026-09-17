using System.Reflection;

namespace ECourse.Services.Course.Application
{
    public static class ApplicationAssemblyReference
    {
        public static Assembly Assembly { get; } = typeof(ApplicationAssemblyReference).Assembly;
    }
}
