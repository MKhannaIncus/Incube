using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddRepaymentFieldsToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "incus_capital");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "lastname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "firstname");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "idusers");

            migrationBuilder.CreateTable(
                name: "deal",
                columns: table => new
                {
                    Alias = table.Column<string>(type: "varchar(255)", nullable: false),
                    Fund = table.Column<string>(type: "longtext", nullable: true),
                    Signing_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Borrower_one = table.Column<string>(type: "longtext", nullable: true),
                    Borrower_two = table.Column<string>(type: "longtext", nullable: true),
                    Ownership_one = table.Column<string>(type: "longtext", nullable: true),
                    Percent_ownership_one = table.Column<string>(type: "longtext", nullable: true),
                    Type_Investment = table.Column<string>(type: "longtext", nullable: true),
                    Type_Investor = table.Column<string>(type: "longtext", nullable: true),
                    Strategy = table.Column<string>(type: "longtext", nullable: true),
                    Sector = table.Column<string>(type: "longtext", nullable: true),
                    Sub_sector = table.Column<string>(type: "longtext", nullable: true),
                    NAV_type = table.Column<string>(type: "longtext", nullable: true),
                    National_amount = table.Column<string>(type: "longtext", nullable: true),
                    Guarantee_1 = table.Column<string>(type: "longtext", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: true),
                    Deal_Performance = table.Column<string>(type: "longtext", nullable: true),
                    Related_investment_code = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deal", x => x.Alias);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "transactions",
                schema: "incus_capital",
                columns: table => new
                {
                    Transaction_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Related_Deal_Id = table.Column<int>(type: "int", nullable: true),
                    Transaction_Date = table.Column<DateTime>(type: "date", nullable: true),
                    Amount_Due_BOP = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Principal_BOP = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Cash_Interest_BOP = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PIK_Interest_BOP = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Undrawn_Interest_BOP = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Capitalized = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Repayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PIK_Interest_EOP = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Cash_Interest_EOP = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Undrawn_Interest_EOP = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Principal_EOP = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount_Due_EOP = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Occurred = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Repayment_CashInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Repayment_PIKInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Repayment_Principal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Repayment_UndrawnFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Transaction_Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deal");

            migrationBuilder.DropTable(
                name: "transactions",
                schema: "incus_capital");

            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "firstname",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "idusers",
                table: "Users",
                newName: "Id");
        }
    }
}
