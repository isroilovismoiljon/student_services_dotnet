using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentServicesWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanToPresentationIsroilov : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "PresentationIsroilovs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_PlanId",
                table: "PresentationIsroilovs",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_PresentationIsroilovs_Plans_PlanId",
                table: "PresentationIsroilovs",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresentationIsroilovs_Plans_PlanId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropIndex(
                name: "IX_PresentationIsroilovs_PlanId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "PresentationIsroilovs");
        }
    }
}
