namespace ECourse.Services.CourseAPI.Repository
{
    public class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : class
    {
        public int Create(TModel entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(TModel entity)
        {
            throw new NotImplementedException();
        }

        public int DeleteAll(TModel entity)
        {
            throw new NotImplementedException();
        }

        public IList<TModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public TModel GetById(string id)
        {
            throw new NotImplementedException();
        }

        public int Update(TModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
