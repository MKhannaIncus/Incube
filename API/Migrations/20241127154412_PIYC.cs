using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class PIYC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PIYC_Interest_Accrued",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PIYC_Interest_BOP",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PIYC_Interest_EOP",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PIYC_Interest_Accrued",
                schema: "incus_capital",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "PIYC_Interest_BOP",
                schema: "incus_capital",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "PIYC_Interest_EOP",
                schema: "incus_capital",
                table: "transactions");
        }
    }
}
