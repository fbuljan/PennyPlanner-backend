using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PennyPlanner.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGetNotificationsFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GetNotifications",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GetNotifications",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
