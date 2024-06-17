using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PennyPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Goals");

            migrationBuilder.RenameColumn(
                name: "SetUpDate",
                table: "Goals",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "DesiredAmount",
                table: "Goals",
                newName: "IsAchieved");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Goals",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "CurrentValue",
                table: "Goals",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "GoalType",
                table: "Goals",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "TargetValue",
                table: "Goals",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_Goals_AccountId",
                table: "Goals",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Accounts_AccountId",
                table: "Goals",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Accounts_AccountId",
                table: "Goals");

            migrationBuilder.DropIndex(
                name: "IX_Goals_AccountId",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "CurrentValue",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "GoalType",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "TargetValue",
                table: "Goals");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Goals",
                newName: "SetUpDate");

            migrationBuilder.RenameColumn(
                name: "IsAchieved",
                table: "Goals",
                newName: "DesiredAmount");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Goals",
                type: "TEXT",
                nullable: true);
        }
    }
}
