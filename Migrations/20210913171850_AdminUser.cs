using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorStore.Migrations
{
    public partial class AdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminUser_FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminUser_LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminUser_FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdminUser_LastName",
                table: "AspNetUsers");
        }
    }
}
