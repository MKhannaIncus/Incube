using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class financialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "financial_metrics",
                columns: table => new
                {
                    Metrics_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Deal_id = table.Column<int>(type: "int", nullable: false),
                    Deal_Id1 = table.Column<int>(type: "int", nullable: true),
                    Transaction_id = table.Column<int>(type: "int", nullable: false),
                    Transaction_Id1 = table.Column<int>(type: "int", nullable: true),
                    Nav = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Nav_irr = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Nav_moic = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Nav_profit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total_Collections = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total_Invested = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Interest_Generated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Accrued_Cash_Interest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Accrued_Pik_Interest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Accrued_Undrawn_Interest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Interest_Payed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total_Debt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Facility = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Undrawn_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_financial_metrics", x => x.Metrics_id);
                    table.ForeignKey(
                        name: "FK_financial_metrics_deal_Deal_Id1",
                        column: x => x.Deal_Id1,
                        principalSchema: "incus_capital",
                        principalTable: "deal",
                        principalColumn: "Deal_Id");
                    table.ForeignKey(
                        name: "FK_financial_metrics_transactions_Transaction_Id1",
                        column: x => x.Transaction_Id1,
                        principalSchema: "incus_capital",
                        principalTable: "transactions",
                        principalColumn: "Transaction_Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_financial_metrics_Deal_Id1",
                table: "financial_metrics",
                column: "Deal_Id1");

            migrationBuilder.CreateIndex(
                name: "IX_financial_metrics_Transaction_Id1",
                table: "financial_metrics",
                column: "Transaction_Id1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "financial_metrics");
        }
    }
}
