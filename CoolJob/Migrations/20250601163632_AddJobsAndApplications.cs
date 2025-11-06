using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolJob.Migrations
{

    public partial class AddJobsAndApplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Jobs");
        }
    }
}
