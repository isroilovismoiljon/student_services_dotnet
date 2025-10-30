using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentServicesWebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsernameIndexForSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the old unique index on Username
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            // Create a new unique index that only applies to non-deleted users
            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the filtered index
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            // Recreate the old index without filter
            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }
    }
}
