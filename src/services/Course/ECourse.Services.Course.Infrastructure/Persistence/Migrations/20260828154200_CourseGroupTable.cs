using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECourse.Services.Course.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CourseGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "course");

            migrationBuilder.CreateTable(
                name: "CourseGroups",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    slug = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_at_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroups", x => x.id);
                    table.ForeignKey(
                        name: "fk_course_groups_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "course",
                        principalTable: "CourseGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "fk_course_groups_parent_id",
                schema: "course",
                table: "CourseGroups",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ux_course_groups_slug",
                schema: "course",
                table: "CourseGroups",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseGroups",
                schema: "course");
        }
    }
}
