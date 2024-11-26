namespace ECourse.Services.CourseAPI.Tests.Data
{
    public interface IFakeData<T1,T2> where T1 : class where T2 : class
    {
        public T1 FakeGetSingleRow();
        public T2 FakeGetSingleRowDTO();
       // public T1 InsertFakeData();
    }
}
