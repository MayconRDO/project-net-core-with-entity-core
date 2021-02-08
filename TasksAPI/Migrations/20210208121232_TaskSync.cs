using Microsoft.EntityFrameworkCore.Migrations;

namespace TasksAPI.Migrations
{
    public partial class TaskSync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Exclude",
                table: "Tasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IdApp",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exclude",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IdApp",
                table: "Tasks");
        }
    }
}
