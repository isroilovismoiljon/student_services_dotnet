using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentServicesWebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePresentationIsroilovTitleAuthorToTextSlides : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PresentationIsroilovs_Author",
                table: "PresentationIsroilovs");

            migrationBuilder.DropIndex(
                name: "IX_PresentationIsroilovs_Title",
                table: "PresentationIsroilovs");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "PresentationIsroilovs");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PresentationIsroilovs");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "PresentationIsroilovs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TitleId",
                table: "PresentationIsroilovs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_AuthorId",
                table: "PresentationIsroilovs",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_TitleId",
                table: "PresentationIsroilovs",
                column: "TitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PresentationIsroilovs_TextSlides_AuthorId",
                table: "PresentationIsroilovs",
                column: "AuthorId",
                principalTable: "TextSlides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PresentationIsroilovs_TextSlides_TitleId",
                table: "PresentationIsroilovs",
                column: "TitleId",
                principalTable: "TextSlides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresentationIsroilovs_TextSlides_AuthorId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropForeignKey(
                name: "FK_PresentationIsroilovs_TextSlides_TitleId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropIndex(
                name: "IX_PresentationIsroilovs_AuthorId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropIndex(
                name: "IX_PresentationIsroilovs_TitleId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "PresentationIsroilovs");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "PresentationIsroilovs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PresentationIsroilovs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_Author",
                table: "PresentationIsroilovs",
                column: "Author");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_Title",
                table: "PresentationIsroilovs",
                column: "Title");
        }
    }
}
