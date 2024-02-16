using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.FirstMigration
{
    /// <inheritdoc />
    public partial class UserRegisterationDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }
    }
}
