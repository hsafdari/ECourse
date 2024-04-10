namespace ECourse.Services.CourseAPI.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        int Create(T entity);
        int Update(T entity);
        int Delete(T entity);
        int DeleteAll(T entity);
        IList<T> GetAll();
        T GetById(string id);

    }
}
