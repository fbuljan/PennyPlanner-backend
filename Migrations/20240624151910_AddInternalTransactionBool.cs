using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PennyPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddInternalTransactionBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInternalTransaction",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInternalTransaction",
                table: "Transactions");
        }
    }
}
