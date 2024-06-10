using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PennyPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherAccountToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OtherAccountId",
                table: "Transactions",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherAccountId",
                table: "Transactions");
        }
    }
}
