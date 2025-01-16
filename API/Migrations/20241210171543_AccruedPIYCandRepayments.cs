using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AccruedPIYCandRepayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Accrued_Piyc_Interest",
                table: "financial_metrics",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Repayed_Cash_Interest",
                table: "financial_metrics",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Repayed_Pik_Interest",
                table: "financial_metrics",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Repayed_Piyc_Interest",
                table: "financial_metrics",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Repayed_Undrawn_Interest",
                table: "financial_metrics",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accrued_Piyc_Interest",
                table: "financial_metrics");

            migrationBuilder.DropColumn(
                name: "Repayed_Cash_Interest",
                table: "financial_metrics");

            migrationBuilder.DropColumn(
                name: "Repayed_Pik_Interest",
                table: "financial_metrics");

            migrationBuilder.DropColumn(
                name: "Repayed_Piyc_Interest",
                table: "financial_metrics");

            migrationBuilder.DropColumn(
                name: "Repayed_Undrawn_Interest",
                table: "financial_metrics");
        }
    }
}
