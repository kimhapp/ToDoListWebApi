using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListWebApi.Migrations
{
    /// <inheritdoc />
    public partial class RenamingTimestampToAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedTimeStamp",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedTimeStamp",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedTimeStamp",
                table: "ToDos",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedTimeStamp",
                table: "ToDos",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Users",
                newName: "UpdatedTimeStamp");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "CreatedTimeStamp");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ToDos",
                newName: "UpdatedTimeStamp");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ToDos",
                newName: "CreatedTimeStamp");
        }
    }
}
