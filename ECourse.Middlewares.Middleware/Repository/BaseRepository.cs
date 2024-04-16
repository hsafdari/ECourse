using MongoDB.Driver;
using System.Linq.Expressions;

namespace Middleware.Repository
{
    public class BaseRepository<TModel>(IMongoDatabase db, string documentname) : IBaseRepository<TModel> where TModel : class
    {
        private readonly IMongoCollection<TModel> model = db.GetCollection<TModel>(documentname);
        public async Task<int> Create(TModel entity)
        {
            try
            {            
                await model.InsertOneAsync(entity);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public async Task<int> Delete(Expression<Func<TModel, bool>> where)
        {
            try
            {
                await model.DeleteOneAsync<TModel>(where);
                return 1;
            }
            catch (Exception)
            {

                return -1;
            }
        }

        public async Task<int> DeleteMany(Expression<Func<TModel, bool>> where)
        {
            try
            {
                await model.DeleteManyAsync<TModel>(where);
                return 1;
            }
            catch (Exception)
            {

                return -1;
            }
        }

        public async Task<List<TModel>> GetMany(Expression<Func<TModel, bool>> where) =>
             await model.Find<TModel>(where).ToListAsync();

        public async Task<TModel> GetById(Expression<Func<TModel, bool>> where) =>
            await model.Find<TModel>(where).FirstOrDefaultAsync();

        public async Task<UpdateResult> Update(Expression<Func<TModel, bool>> where, UpdateDefinition<TModel> update)=>
        await model.UpdateOneAsync<TModel>(where, update);
    }
}
