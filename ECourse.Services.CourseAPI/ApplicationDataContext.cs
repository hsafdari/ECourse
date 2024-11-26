using ECourse.Services.CourseAPI.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using System.Reflection;

namespace ECourse.Services.CourseAPI
{
    public class ApplicationDataContext:DbContext      
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options)
        : base(options)
        {
        }
        public DbSet<Course> Courses { get; init; }
        public DbSet<CourseItem> CourseItems { get; init; }
        public DbSet<CourseLevel> CourseLevels { get; init; }
        public DbSet<CoursePrice> CoursePrices { get; init; }
        public DbSet<CourseSection> CourseSections { get; init; }
        public DbSet<SocialLink> SocialLinks { get; init; }
        public DbSet<Teacher> Teachers { get; init; }
        public DbSet<TeacherSocialLink> teacherSocialLinks { get; init; }
        public static ApplicationDataContext Create(IMongoDatabase database) => new(new DbContextOptionsBuilder<ApplicationDataContext>().
        UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName).Options);
        //public DataContext(DbContextOptions options) : base(options)
        //{
            
        //}        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Course>().ToCollection(Course.DocumentName);
            modelBuilder.Entity<CourseItem>().ToCollection(CourseItem.DocumentName);
            modelBuilder.Entity<CourseLevel>().ToCollection(CourseLevel.DocumentName);
            modelBuilder.Entity<CoursePrice>().ToCollection(CoursePrice.DocumentName);
            modelBuilder.Entity<CourseSection>().ToCollection(CourseSection.DocumentName);
            modelBuilder.Entity<SocialLink>().ToCollection(SocialLink.DocumentName);
            modelBuilder.Entity<Teacher>().ToCollection(Teacher.DocumentName);
            modelBuilder.Entity<TeacherSocialLink>().ToCollection(TeacherSocialLink.DocumentName);
            base.OnModelCreating(modelBuilder);
        }
    }
}
