using ECourse.Services.Course.Domain.Entities;
using ECourse.Services.Course.Domain.Rules;
using ECourse.Services.Course.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECourse.Services.Course.Infrastructure.Persistence.Configurations
{
    internal sealed class CourseGroupConfiguration : IEntityTypeConfiguration<CourseGroup>
    {
        internal const string SlugUniqueIndexName = "ux_course_groups_slug";
        internal const string ParentIndexName = "fk_course_groups_parent_id";
        public void Configure(EntityTypeBuilder<CourseGroup> builder)
        {
            ConfigureTable(builder);
            ConfigurePrimaryKey(builder);
            ConfigureProperties(builder);
            configureIndexes(builder);
            ConfigureQueryFilters(builder);
            ConfigureParent(builder);

        }

        private void ConfigureParent(EntityTypeBuilder<CourseGroup> builder)
        {
            builder.HasOne(x => x.Parent)
                    .WithMany()
                    .HasForeignKey(x => x.ParentId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName(ParentIndexName);
        }

        private void ConfigureQueryFilters(EntityTypeBuilder<CourseGroup> builder)
        {
            builder.HasQueryFilter(courseGroup => !courseGroup.IsDeleted);
        }

        private void configureIndexes(EntityTypeBuilder<CourseGroup> builder)
        {
            builder.HasIndex(cg => cg.Slug)
                .IsUnique()
                .HasDatabaseName(SlugUniqueIndexName);

            builder.HasIndex(x => x.ParentId)
                .HasDatabaseName(ParentIndexName);
        }

        private void ConfigureProperties(EntityTypeBuilder<CourseGroup> builder)
        {
            builder.Property(cg => cg.Title)
                .HasColumnName("title")
                .HasMaxLength(CourseGroupRules.MaxTitleLength)
                .IsRequired();

            builder.Property(cg => cg.Slug)
                .HasColumnName("slug")
                .HasConversion(
                    slug => slug.Value,
                    value => Slug.Create(value))
                .HasMaxLength(Slug.MaxLength)
                .IsRequired();

            builder.Property(cg => cg.Description)
                .HasColumnName("description")
                .HasMaxLength(CourseGroupRules.MaxDescriptionLength)
                .IsRequired(false);

            builder.Property(cg => cg.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            builder.Property(cg => cg.ParentId)
                .HasColumnName("parent_id")
                .IsRequired(false);

            builder.Property(cg => cg.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .IsRequired();

            builder.Property(cg => cg.ModifiedAtUtc)
                .HasColumnName("modified_at_utc")
                .IsRequired(false);

            builder.Property(cg => cg.DeletedAtUtc)
                .HasColumnName("deleted_at_utc")
                .IsRequired(false);

            builder.Property(cg => cg.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired();
        }

        private void ConfigurePrimaryKey(EntityTypeBuilder<CourseGroup> builder)
        {
            builder.HasKey(cg => cg.Id);
            builder.Property(cg => cg.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();
        }

        private void ConfigureTable(EntityTypeBuilder<CourseGroup> builder)
        {
            builder.ToTable("CourseGroups", "course");
        }
    }
}
