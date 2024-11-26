using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Repositories;
using Infrastructure.Models;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ECourse.Services.CourseAPI.Tests.Repositories
{
    public interface IBaseRepositoryTest<T> where T : BaseEntity
    {
        public T InsertFakeData();
        public T InsertFakeDataList();
        [Fact]
        public Task Should_Return_Created_Model();
        [Fact]
        public Task Should_Update_Model();
        [Fact]
        public Task Should_Return_Row_Model();
        [Fact]
        public Task Should_DeleteRow_Model();                
        public Task Should_Return_Grid(string filter, int take, int skip, string orderby, string select);
        

    }
}
