using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentManagementSystemAPI.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "courseId",
                table: "Grades",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "studentId",
                table: "Grades",
                nullable: false,
                defaultValue: 0); 

            migrationBuilder.AddColumn<string>(
                name: "TeacherName",
                table: "Courses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "courseId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "studentId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "TeacherName",
                table: "Courses");
        }
    }
}
