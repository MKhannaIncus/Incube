using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class financialMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_deal",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Borrower_one",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Borrower_two",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Deal_Performance",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Fund",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Guarantee_1",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "NAV_type",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "National_amount",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Ownership_one",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Percent_ownership_one",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Signing_date",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Strategy",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Type_Investment",
                table: "deal");

            migrationBuilder.RenameTable(
                name: "deal",
                newName: "deal",
                newSchema: "incus_capital");

            migrationBuilder.RenameColumn(
                name: "Sub_sector",
                schema: "incus_capital",
                table: "deal",
                newName: "Subsector");

            migrationBuilder.RenameColumn(
                name: "Type_Investor",
                schema: "incus_capital",
                table: "deal",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "Related_investment_code",
                schema: "incus_capital",
                table: "deal",
                newName: "Deal_Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Transaction_Date",
                schema: "incus_capital",
                table: "transactions",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Repayment_UndrawnFees",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Repayment_Principal",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Repayment_PIKInterest",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Repayment_CashInterest",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "Cash_Interest_Accrued",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Cash_Interest_Rate",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Drawdown",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PIK_Interest_Accrued",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PIK_Interest_Rate",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Undrawn_Amount",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Undrawn_Fee_Interest_Rate",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Undrawn_Interest_Accrued",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sector",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subsector",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Deal_Id",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Amortization_type",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Asset_Id",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Availability_fee",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(15,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Availability_period",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cash_Interest_Period",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Client_Id",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Deal_Name",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Drawdown",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Entity_Id",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Facility",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(15,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "IRR",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Intercompany_loan",
                schema: "incus_capital",
                table: "deal",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Interest_Id",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Investment_date",
                schema: "incus_capital",
                table: "deal",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LTV_Entry",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MOIC",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Maturity_date",
                schema: "incus_capital",
                table: "deal",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Minimum_multiple",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NAV",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(15,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Opening_fee",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(15,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ownership_Id",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PIK_Interest_Period",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Related_fund_id",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Underwriting_IRR",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Underwriting_MOIC",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Underwriting_NAV",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_deal",
                schema: "incus_capital",
                table: "deal",
                column: "Deal_Id");

            migrationBuilder.CreateTable(
                name: "fund",
                schema: "incus_capital",
                columns: table => new
                {
                    Fund_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Fund_name = table.Column<string>(type: "longtext", nullable: true),
                    Total_commitment = table.Column<int>(type: "int", nullable: true),
                    Start_Ip = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    End_Ip = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Start_Hp = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    End_Hp = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Register = table.Column<string>(type: "longtext", nullable: true),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    Country = table.Column<string>(type: "longtext", nullable: true),
                    NIF = table.Column<int>(type: "int", nullable: true),
                    VAT_number = table.Column<int>(type: "int", nullable: true),
                    Bank_account_entity = table.Column<int>(type: "int", nullable: true),
                    Entity = table.Column<int>(type: "int", nullable: true),
                    Administrator = table.Column<string>(type: "longtext", nullable: true),
                    Custodian = table.Column<string>(type: "longtext", nullable: true),
                    Director_one = table.Column<string>(type: "longtext", nullable: true),
                    Director_two = table.Column<string>(type: "longtext", nullable: true),
                    Director_thre = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fund", x => x.Fund_Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fund",
                schema: "incus_capital");

            migrationBuilder.DropPrimaryKey(
                name: "PK_deal",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Cash_Interest_Accrued",
                schema: "incus_capital",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Cash_Interest_Rate",
                schema: "incus_capital",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Drawdown",
                schema: "incus_capital",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "PIK_Interest_Accrued",
                schema: "incus_capital",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "PIK_Interest_Rate",
                schema: "incus_capital",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Undrawn_Amount",
                schema: "incus_capital",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Undrawn_Fee_Interest_Rate",
                schema: "incus_capital",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Undrawn_Interest_Accrued",
                schema: "incus_capital",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Amortization_type",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Asset_Id",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Availability_fee",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Availability_period",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Cash_Interest_Period",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Client_Id",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Deal_Name",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Drawdown",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Entity_Id",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Facility",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "IRR",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Intercompany_loan",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Interest_Id",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Investment_date",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "LTV_Entry",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "MOIC",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Maturity_date",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Minimum_multiple",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "NAV",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Opening_fee",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Ownership_Id",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "PIK_Interest_Period",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Related_fund_id",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Underwriting_IRR",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Underwriting_MOIC",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Underwriting_NAV",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.RenameTable(
                name: "deal",
                schema: "incus_capital",
                newName: "deal");

            migrationBuilder.RenameColumn(
                name: "Subsector",
                table: "deal",
                newName: "Sub_sector");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "deal",
                newName: "Type_Investor");

            migrationBuilder.RenameColumn(
                name: "Deal_Id",
                table: "deal",
                newName: "Related_investment_code");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Transaction_Date",
                schema: "incus_capital",
                table: "transactions",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<decimal>(
                name: "Repayment_UndrawnFees",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Repayment_Principal",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Repayment_PIKInterest",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Repayment_CashInterest",
                schema: "incus_capital",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sector",
                table: "deal",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sub_sector",
                table: "deal",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Related_investment_code",
                table: "deal",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "deal",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Borrower_one",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Borrower_two",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Deal_Performance",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fund",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guarantee_1",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NAV_type",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "National_amount",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ownership_one",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Percent_ownership_one",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Signing_date",
                table: "deal",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Strategy",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type_Investment",
                table: "deal",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_deal",
                table: "deal",
                column: "Alias");
        }
    }
}
