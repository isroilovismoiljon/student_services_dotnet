using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentServicesWebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresentationIsroilovs_Designs_DesignId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropForeignKey(
                name: "FK_PresentationIsroilovs_Plans_PlanId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropIndex(
                name: "IX_PresentationIsroilovs_DesignId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropIndex(
                name: "IX_PresentationIsroilovs_PlanId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropColumn(
                name: "DesignId",
                table: "PresentationIsroilovs");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "PresentationIsroilovs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DesignId",
                table: "PresentationIsroilovs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "PresentationIsroilovs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_DesignId",
                table: "PresentationIsroilovs",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_PlanId",
                table: "PresentationIsroilovs",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_PresentationIsroilovs_Designs_DesignId",
                table: "PresentationIsroilovs",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PresentationIsroilovs_Plans_PlanId",
                table: "PresentationIsroilovs",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
