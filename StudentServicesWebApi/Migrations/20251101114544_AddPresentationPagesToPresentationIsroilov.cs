using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentServicesWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPresentationPagesToPresentationIsroilov : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresentationPages_PresentationIsroilovs_PresentationIsroil~1",
                table: "PresentationPages");

            migrationBuilder.DropIndex(
                name: "IX_PresentationPages_PresentationIsroilovId1",
                table: "PresentationPages");

            migrationBuilder.DropColumn(
                name: "PresentationIsroilovId1",
                table: "PresentationPages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PresentationIsroilovId1",
                table: "PresentationPages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PresentationPages_PresentationIsroilovId1",
                table: "PresentationPages",
                column: "PresentationIsroilovId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PresentationPages_PresentationIsroilovs_PresentationIsroil~1",
                table: "PresentationPages",
                column: "PresentationIsroilovId1",
                principalTable: "PresentationIsroilovs",
                principalColumn: "Id");
        }
    }
}
