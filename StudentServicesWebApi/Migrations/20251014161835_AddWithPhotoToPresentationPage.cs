using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace StudentServicesWebApi.Migrations
{
    public partial class AddWithPhotoToPresentationPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WithPhoto",
                table: "PresentationPages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WithPhoto",
                table: "PresentationPages");
        }
    }
}
