using ECourse.Services.CourseAPI.Models;
using Microsoft.EntityFrameworkCore;
using Middleware;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using System.Reflection;

namespace ECourse.Services.CourseAPI
{
    public class DataContext:DbContext      
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }
        //public DbSet<Course> Courses { get; init; }
        //public DbSet<CourseItem> CourseItems { get; init; }
        public DbSet<CourseLevel> CourseLevels { get; init; }
        //public DbSet<CoursePrice> CoursePrices { get; init; }
        //public DbSet<CourseSection> CourseSections { get; init; }
        public static DataContext Create(IMongoDatabase database) => new(new DbContextOptionsBuilder<DataContext>().
        UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName).Options);
        //public DataContext(DbContextOptions options) : base(options)
        //{
            
        //}        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Course>().ToCollection(Course.DocumentName);
            //modelBuilder.Entity<CourseItem>().ToCollection(CourseItem.DocumentName);
            modelBuilder.Entity<CourseLevel>().ToCollection(CourseLevel.DocumentName);
            //modelBuilder.Entity<CoursePrice>().ToCollection(CoursePrice.DocumentName);
            //modelBuilder.Entity<CourseSection>().ToCollection(CourseSection.DocumentName);
        }
    }
}
