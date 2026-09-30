using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace ECourse.Services.Course.Application.Observability;
public static class CourseTelemetry
{
    public const string ServiceName = "ECourse.Services.Course.API";
    public const string ActivitySourceName = "ECourse.Services.Course";
    public const string MeterName ="ECourse.Services.Course";
    public readonly static ActivitySource ActivitySource = new(ActivitySourceName);

    public readonly static Meter Meter = new(MeterName);
}
