using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class DealValuesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Cash_Interest_Period",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Client_Id",
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
                name: "First_CashInterest_Period_EndPeriods",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "First_PIKInterest_Period_EndPeriods",
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
                name: "Interest_Rate",
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
                name: "NAV",
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
                name: "Percent_Coinvestors",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Related_fund_id",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Second_CashInterest_Period_EndPeriods",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Second_PIKInterest_Period_EndPeriods",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Third_CashInterest_Period_EndPeriods",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Third_PIKInterest_Period_EndPeriods",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.RenameColumn(
                name: "Thematic_vs_Opportunistic",
                schema: "incus_capital",
                table: "deal",
                newName: "Thematic_Vs_Opportunistic");

            migrationBuilder.RenameColumn(
                name: "Minimum_multiple",
                schema: "incus_capital",
                table: "deal",
                newName: "Minimum_Multiple");

            migrationBuilder.RenameColumn(
                name: "Third_PIKInterest_Period_Rate",
                schema: "incus_capital",
                table: "deal",
                newName: "Year_Base");

            migrationBuilder.RenameColumn(
                name: "Third_CashInterest_Period_Rate",
                schema: "incus_capital",
                table: "deal",
                newName: "Undrawn_fee");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "incus_capital",
                table: "deal",
                newName: "Undrawn_fee_periods");

            migrationBuilder.RenameColumn(
                name: "Second_PIKInterest_Period_Rate",
                schema: "incus_capital",
                table: "deal",
                newName: "Share_Premium");

            migrationBuilder.RenameColumn(
                name: "Second_CashInterest_Period_Rate",
                schema: "incus_capital",
                table: "deal",
                newName: "Purchase_Discount_Rate");

            migrationBuilder.RenameColumn(
                name: "Realization_Date",
                schema: "incus_capital",
                table: "deal",
                newName: "PIYCInterest_End_3rd");

            migrationBuilder.RenameColumn(
                name: "Instrument",
                schema: "incus_capital",
                table: "deal",
                newName: "Others");

            migrationBuilder.RenameColumn(
                name: "General_Investment_Name",
                schema: "incus_capital",
                table: "deal",
                newName: "Loan");

            migrationBuilder.RenameColumn(
                name: "General_Investment_Code",
                schema: "incus_capital",
                table: "deal",
                newName: "Lender");

            migrationBuilder.RenameColumn(
                name: "Fund",
                schema: "incus_capital",
                table: "deal",
                newName: "Interest_Rate_Type");

            migrationBuilder.RenameColumn(
                name: "First_PIKInterest_Period_Rate",
                schema: "incus_capital",
                table: "deal",
                newName: "Percent_Coinvestor");

            migrationBuilder.RenameColumn(
                name: "First_CashInterest_Period_Rate",
                schema: "incus_capital",
                table: "deal",
                newName: "PIYCInterest_Rate_3rd");

            migrationBuilder.RenameColumn(
                name: "Deal_Grouping",
                schema: "incus_capital",
                table: "deal",
                newName: "Interest_Period");

            migrationBuilder.AlterColumn<decimal>(
                name: "Percent_Master_Fund",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Minimum_Multiple",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Coupon",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country_Code",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                schema: "incus_capital",
                table: "deal",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Client_Country_Code",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Client",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Acceleration_Date",
                schema: "incus_capital",
                table: "deal",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Borrower",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CashInterest_End_1st",
                schema: "incus_capital",
                table: "deal",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CashInterest_End_2nd",
                schema: "incus_capital",
                table: "deal",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CashInterest_End_3rd",
                schema: "incus_capital",
                table: "deal",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CashInterest_Rate_1st",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CashInterest_Rate_2nd",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CashInterest_Rate_3rd",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Default_Capitalization_Periods",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Default_interest_rate",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EURIBOR_Interest_Rate",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Exit_fee",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension_Period",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "First_Utilization",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grouping",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instrument_Dealddbbb",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instrument_LoanTemplateddbb",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PIKInterest_End_1st",
                schema: "incus_capital",
                table: "deal",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PIKInterest_End_2nd",
                schema: "incus_capital",
                table: "deal",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PIKInterest_End_3rd",
                schema: "incus_capital",
                table: "deal",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PIKInterest_Rate_1st",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PIKInterest_Rate_2nd",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PIKInterest_Rate_3rd",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PIYCInterest_End_1st",
                schema: "incus_capital",
                table: "deal",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PIYCInterest_End_2nd",
                schema: "incus_capital",
                table: "deal",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PIYCInterest_Rate_1st",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PIYCInterest_Rate_2nd",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acceleration_Date",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Borrower",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "CashInterest_End_1st",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "CashInterest_End_2nd",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "CashInterest_End_3rd",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "CashInterest_Rate_1st",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "CashInterest_Rate_2nd",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "CashInterest_Rate_3rd",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Default_Capitalization_Periods",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Default_interest_rate",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "EURIBOR_Interest_Rate",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Exit_fee",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Extension_Period",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "First_Utilization",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Grouping",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Instrument_Dealddbbb",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "Instrument_LoanTemplateddbb",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "PIKInterest_End_1st",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "PIKInterest_End_2nd",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "PIKInterest_End_3rd",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "PIKInterest_Rate_1st",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "PIKInterest_Rate_2nd",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "PIKInterest_Rate_3rd",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "PIYCInterest_End_1st",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "PIYCInterest_End_2nd",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "PIYCInterest_Rate_1st",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.DropColumn(
                name: "PIYCInterest_Rate_2nd",
                schema: "incus_capital",
                table: "deal");

            migrationBuilder.RenameColumn(
                name: "Thematic_Vs_Opportunistic",
                schema: "incus_capital",
                table: "deal",
                newName: "Thematic_vs_Opportunistic");

            migrationBuilder.RenameColumn(
                name: "Minimum_Multiple",
                schema: "incus_capital",
                table: "deal",
                newName: "Minimum_multiple");

            migrationBuilder.RenameColumn(
                name: "Year_Base",
                schema: "incus_capital",
                table: "deal",
                newName: "Third_PIKInterest_Period_Rate");

            migrationBuilder.RenameColumn(
                name: "Undrawn_fee_periods",
                schema: "incus_capital",
                table: "deal",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Undrawn_fee",
                schema: "incus_capital",
                table: "deal",
                newName: "Third_CashInterest_Period_Rate");

            migrationBuilder.RenameColumn(
                name: "Share_Premium",
                schema: "incus_capital",
                table: "deal",
                newName: "Second_PIKInterest_Period_Rate");

            migrationBuilder.RenameColumn(
                name: "Purchase_Discount_Rate",
                schema: "incus_capital",
                table: "deal",
                newName: "Second_CashInterest_Period_Rate");

            migrationBuilder.RenameColumn(
                name: "Percent_Coinvestor",
                schema: "incus_capital",
                table: "deal",
                newName: "First_PIKInterest_Period_Rate");

            migrationBuilder.RenameColumn(
                name: "PIYCInterest_Rate_3rd",
                schema: "incus_capital",
                table: "deal",
                newName: "First_CashInterest_Period_Rate");

            migrationBuilder.RenameColumn(
                name: "PIYCInterest_End_3rd",
                schema: "incus_capital",
                table: "deal",
                newName: "Realization_Date");

            migrationBuilder.RenameColumn(
                name: "Others",
                schema: "incus_capital",
                table: "deal",
                newName: "Instrument");

            migrationBuilder.RenameColumn(
                name: "Loan",
                schema: "incus_capital",
                table: "deal",
                newName: "General_Investment_Name");

            migrationBuilder.RenameColumn(
                name: "Lender",
                schema: "incus_capital",
                table: "deal",
                newName: "General_Investment_Code");

            migrationBuilder.RenameColumn(
                name: "Interest_Rate_Type",
                schema: "incus_capital",
                table: "deal",
                newName: "Fund");

            migrationBuilder.RenameColumn(
                name: "Interest_Period",
                schema: "incus_capital",
                table: "deal",
                newName: "Deal_Grouping");

            migrationBuilder.AlterColumn<decimal>(
                name: "Percent_Master_Fund",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Minimum_multiple",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Coupon",
                schema: "incus_capital",
                table: "deal",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country_Code",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                schema: "incus_capital",
                table: "deal",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Client_Country_Code",
                schema: "incus_capital",
                table: "deal",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Client",
                schema: "incus_capital",
                table: "deal",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Amortization_type",
                schema: "incus_capital",
                table: "deal",
                type: "longtext",
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

            migrationBuilder.AddColumn<DateTime>(
                name: "First_CashInterest_Period_EndPeriods",
                schema: "incus_capital",
                table: "deal",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "First_PIKInterest_Period_EndPeriods",
                schema: "incus_capital",
                table: "deal",
                type: "datetime(6)",
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

            migrationBuilder.AddColumn<string>(
                name: "Interest_Rate",
                schema: "incus_capital",
                table: "deal",
                type: "longtext",
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

            migrationBuilder.AddColumn<decimal>(
                name: "NAV",
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

            migrationBuilder.AddColumn<decimal>(
                name: "Percent_Coinvestors",
                schema: "incus_capital",
                table: "deal",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Related_fund_id",
                schema: "incus_capital",
                table: "deal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Second_CashInterest_Period_EndPeriods",
                schema: "incus_capital",
                table: "deal",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Second_PIKInterest_Period_EndPeriods",
                schema: "incus_capital",
                table: "deal",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Third_CashInterest_Period_EndPeriods",
                schema: "incus_capital",
                table: "deal",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Third_PIKInterest_Period_EndPeriods",
                schema: "incus_capital",
                table: "deal",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
