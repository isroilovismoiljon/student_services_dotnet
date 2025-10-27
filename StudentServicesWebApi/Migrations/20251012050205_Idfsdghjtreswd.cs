using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentServicesWebApi.Migrations
{
    /// <inheritdoc />
    public partial class Idfsdghjtreswd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_TextSlides_Plan_1Id",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_TextSlides_Plan_2Id",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_TextSlides_Plan_3Id",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_TextSlides_Plan_4Id",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_TextSlides_Plan_5Id",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_TextSlides_TitleId",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_Plan_1Id",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_Plan_2Id",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_Plan_4Id",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_Plan_5Id",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Plan_1Id",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Plan_2Id",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Plan_4Id",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Plan_5Id",
                table: "Plans");

            migrationBuilder.RenameColumn(
                name: "TitleId",
                table: "Plans",
                newName: "PlansId");

            migrationBuilder.RenameColumn(
                name: "Plan_3Id",
                table: "Plans",
                newName: "PlanTextId");

            migrationBuilder.RenameIndex(
                name: "IX_Plans_TitleId",
                table: "Plans",
                newName: "IX_Plans_PlansId");

            migrationBuilder.RenameIndex(
                name: "IX_Plans_Plan_3Id",
                table: "Plans",
                newName: "IX_Plans_PlanTextId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_TextSlides_PlanTextId",
                table: "Plans",
                column: "PlanTextId",
                principalTable: "TextSlides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_TextSlides_PlansId",
                table: "Plans",
                column: "PlansId",
                principalTable: "TextSlides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_TextSlides_PlanTextId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_TextSlides_PlansId",
                table: "Plans");

            migrationBuilder.RenameColumn(
                name: "PlansId",
                table: "Plans",
                newName: "TitleId");

            migrationBuilder.RenameColumn(
                name: "PlanTextId",
                table: "Plans",
                newName: "Plan_3Id");

            migrationBuilder.RenameIndex(
                name: "IX_Plans_PlanTextId",
                table: "Plans",
                newName: "IX_Plans_Plan_3Id");

            migrationBuilder.RenameIndex(
                name: "IX_Plans_PlansId",
                table: "Plans",
                newName: "IX_Plans_TitleId");

            migrationBuilder.AddColumn<int>(
                name: "Plan_1Id",
                table: "Plans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Plan_2Id",
                table: "Plans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Plan_4Id",
                table: "Plans",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Plan_5Id",
                table: "Plans",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Plan_1Id",
                table: "Plans",
                column: "Plan_1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Plan_2Id",
                table: "Plans",
                column: "Plan_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Plan_4Id",
                table: "Plans",
                column: "Plan_4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Plan_5Id",
                table: "Plans",
                column: "Plan_5Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_TextSlides_Plan_1Id",
                table: "Plans",
                column: "Plan_1Id",
                principalTable: "TextSlides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_TextSlides_Plan_2Id",
                table: "Plans",
                column: "Plan_2Id",
                principalTable: "TextSlides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_TextSlides_Plan_3Id",
                table: "Plans",
                column: "Plan_3Id",
                principalTable: "TextSlides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_TextSlides_Plan_4Id",
                table: "Plans",
                column: "Plan_4Id",
                principalTable: "TextSlides",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_TextSlides_Plan_5Id",
                table: "Plans",
                column: "Plan_5Id",
                principalTable: "TextSlides",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_TextSlides_TitleId",
                table: "Plans",
                column: "TitleId",
                principalTable: "TextSlides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
