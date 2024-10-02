using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class DealName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashRecs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Transaction_Date",
                schema: "incus_capital",
                table: "transactions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Deal_Name",
                schema: "incus_capital",
                table: "transactions",
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cash_Rec",
                columns: table => new
                {
                    cash_rec_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    fund = table.Column<string>(type: "longtext", nullable: true),
                    type = table.Column<string>(type: "longtext", nullable: true),
                    subtype = table.Column<string>(type: "longtext", nullable: true),
                    counterparty = table.Column<string>(type: "longtext", nullable: true),
                    project = table.Column<string>(type: "longtext", nullable: true),
                    included_in_loan_template = table.Column<string>(type: "longtext", nullable: true),
                    type_included_in_loan_template = table.Column<string>(type: "longtext", nullable: true),
                    error = table.Column<string>(type: "longtext", nullable: true),
                    project_exits = table.Column<string>(type: "longtext", nullable: true),
                    loan_template = table.Column<string>(type: "longtext", nullable: true),
                    account = table.Column<string>(type: "longtext", nullable: true),
                    account_holder = table.Column<string>(type: "longtext", nullable: true),
                    bank = table.Column<string>(type: "longtext", nullable: true),
                    entry_date = table.Column<string>(type: "longtext", nullable: true),
                    value_date = table.Column<string>(type: "longtext", nullable: true),
                    transaction_amount = table.Column<string>(type: "longtext", nullable: true),
                    transaction_currency = table.Column<string>(type: "longtext", nullable: true),
                    counterparty_name = table.Column<string>(type: "longtext", nullable: true),
                    transaction_motivation = table.Column<string>(type: "longtext", nullable: true),
                    comments = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cash_Rec", x => x.cash_rec_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cash_Rec");

            migrationBuilder.DropColumn(
                name: "Deal_Name",
                schema: "incus_capital",
                table: "transactions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Transaction_Date",
                schema: "incus_capital",
                table: "transactions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.CreateTable(
                name: "CashRecs",
                columns: table => new
                {
                    cash_rec_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    account = table.Column<string>(type: "longtext", nullable: true),
                    account_holder = table.Column<string>(type: "longtext", nullable: true),
                    bank = table.Column<string>(type: "longtext", nullable: true),
                    comments = table.Column<string>(type: "longtext", nullable: true),
                    counterparty = table.Column<string>(type: "longtext", nullable: true),
                    counterparty_name = table.Column<string>(type: "longtext", nullable: true),
                    entry_date = table.Column<string>(type: "longtext", nullable: true),
                    error = table.Column<string>(type: "longtext", nullable: true),
                    fund = table.Column<string>(type: "longtext", nullable: true),
                    included_in_loan_template = table.Column<string>(type: "longtext", nullable: true),
                    loan_template = table.Column<string>(type: "longtext", nullable: true),
                    project = table.Column<string>(type: "longtext", nullable: true),
                    project_exits = table.Column<string>(type: "longtext", nullable: true),
                    subtype = table.Column<string>(type: "longtext", nullable: true),
                    transaction_amount = table.Column<string>(type: "longtext", nullable: true),
                    transaction_currency = table.Column<string>(type: "longtext", nullable: true),
                    transaction_motivation = table.Column<string>(type: "longtext", nullable: true),
                    type = table.Column<string>(type: "longtext", nullable: true),
                    type_included_in_loan_template = table.Column<string>(type: "longtext", nullable: true),
                    value_date = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashRecs", x => x.cash_rec_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }
    }
}
