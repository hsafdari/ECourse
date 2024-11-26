namespace ECourse.Services.CourseAPI.Tests.Data
{
    public interface IFakeDataGenerator
    {
        IEnumerator<object[]> GetEnumerator();
    }
}