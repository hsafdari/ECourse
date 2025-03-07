using ECourse.Services.AuthAPI.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using System.Reflection;

namespace ECourse.Services.AuthAPI
{
    public class ApplicationDataContext:DbContext      
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options)
        : base(options)
        {
        }
        public DbSet<SocialLink> Courses { get; init; }            
        public static ApplicationDataContext Create(IMongoDatabase database) => new(new DbContextOptionsBuilder<ApplicationDataContext>().
        UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName).Options);
        //public DataContext(DbContextOptions options) : base(options)
        //{
            
        //}        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SocialLink>().ToCollection(SocialLink.DocumentName);              
            base.OnModelCreating(modelBuilder);
        }
    }
}
