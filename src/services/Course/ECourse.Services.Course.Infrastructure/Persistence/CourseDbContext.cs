using ECourse.Services.Course.Application.Abstractions.Persistence;
using ECourse.Services.Course.Application.Common.Exceptions;
using ECourse.Services.Course.Domain.Entities;
using ECourse.Services.Course.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ECourse.Services.Course.Infrastructure.Persistence;

public class CourseDbContext : DbContext, IUnitOfWork
{
    public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options)
    {
    }
    public DbSet<CourseGroup> CourseGroups => Set<CourseGroup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseDbContext).Assembly);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
            when (IsDuplicateCourseGroupSlug(exception))
        {

            throw new ConflictException("A course group with the same slug already exists.", exception);
        }

    }

    private bool IsDuplicateCourseGroupSlug(DbUpdateException exception)
    {
        return exception.InnerException
         is PostgresException
        {
            SqlState:
                 PostgresErrorCodes.UniqueViolation,
            ConstraintName:
                 CourseGroupConfiguration.SlugUniqueIndexName
        };
    }
}
