using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PennyPlanner.Migrations
{
    /// <inheritdoc />
    public partial class RemovePeriodicity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Periodicity",
                table: "Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Periodicity",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
