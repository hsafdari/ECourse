using System.Collections;

namespace ECourse.Services.CourseAPI.Tests.Data
{
    /// <summary>
    /// Data Generated based on GridQuery class
    /// </summary>
    public class FakeDataGenerator : IEnumerable<object[]>, IFakeDataGenerator
    {
        public FakeDataGenerator(List<object[]> data)
        {
            _data = data;
        }
        //GridQuery class properties
        //string? filter
        //int top
        //int skip
        //string? orderby
        //string? select
        protected List<object[]> _data;
        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public List<object[]> dataList()
        {
            return _data;
        }
    }
}
