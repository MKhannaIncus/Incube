﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idusers");

                    b.Property<string>("Email")
                        .HasColumnType("longtext")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext")
                        .HasColumnName("firstname");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext")
                        .HasColumnName("lastname");

                    b.Property<string>("Password")
                        .HasColumnType("longtext")
                        .HasColumnName("Password");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("longblob")
                        .HasColumnName("PasswordHash");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("longblob")
                        .HasColumnName("PasswordSalt");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API.Entities.Cash_Rec", b =>
                {
                    b.Property<int?>("CashRec_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("cash_rec_id");

                    b.Property<string>("Account")
                        .HasColumnType("longtext")
                        .HasColumnName("account");

                    b.Property<string>("AccountHolder")
                        .HasColumnType("longtext")
                        .HasColumnName("account_holder");

                    b.Property<string>("Bank")
                        .HasColumnType("longtext")
                        .HasColumnName("bank");

                    b.Property<string>("Comments")
                        .HasColumnType("longtext")
                        .HasColumnName("comments");

                    b.Property<string>("Counterparty")
                        .HasColumnType("longtext")
                        .HasColumnName("counterparty");

                    b.Property<string>("CounterpartyName")
                        .HasColumnType("longtext")
                        .HasColumnName("counterparty_name");

                    b.Property<string>("EntryDate")
                        .HasColumnType("longtext")
                        .HasColumnName("entry_date");

                    b.Property<string>("Error")
                        .HasColumnType("longtext")
                        .HasColumnName("error");

                    b.Property<string>("Fund")
                        .HasColumnType("longtext")
                        .HasColumnName("fund");

                    b.Property<string>("IncludedInLoanTemplate")
                        .HasColumnType("longtext")
                        .HasColumnName("included_in_loan_template");

                    b.Property<string>("LoanTemplate")
                        .HasColumnType("longtext")
                        .HasColumnName("loan_template");

                    b.Property<string>("Project")
                        .HasColumnType("longtext")
                        .HasColumnName("project");

                    b.Property<string>("ProjectExits")
                        .HasColumnType("longtext")
                        .HasColumnName("project_exits");

                    b.Property<string>("SubType")
                        .HasColumnType("longtext")
                        .HasColumnName("subtype");

                    b.Property<string>("TransactionAmount")
                        .HasColumnType("longtext")
                        .HasColumnName("transaction_amount");

                    b.Property<string>("TransactionCurrency")
                        .HasColumnType("longtext")
                        .HasColumnName("transaction_currency");

                    b.Property<string>("TransactionMotivation")
                        .HasColumnType("longtext")
                        .HasColumnName("transaction_motivation");

                    b.Property<string>("Type")
                        .HasColumnType("longtext")
                        .HasColumnName("type");

                    b.Property<string>("TypeIncludedInLoanTemplate")
                        .HasColumnType("longtext")
                        .HasColumnName("type_included_in_loan_template");

                    b.Property<string>("ValueDate")
                        .HasColumnType("longtext")
                        .HasColumnName("value_date");

                    b.HasKey("CashRec_Id");

                    b.ToTable("Cash_Rec");
                });

            modelBuilder.Entity("API.Entities.Deal", b =>
                {
                    b.Property<string>("Deal_Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Amortization_type")
                        .HasColumnType("longtext");

                    b.Property<string>("Asset_Class")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("Asset_Id")
                        .HasColumnType("int");

                    b.Property<decimal?>("Availability_fee")
                        .HasColumnType("decimal(15, 2)");

                    b.Property<int?>("Availability_period")
                        .HasColumnType("int");

                    b.Property<string>("Capital_Repayment")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("Cash_Interest_Period")
                        .HasColumnType("int");

                    b.Property<string>("Client")
                        .HasColumnType("longtext");

                    b.Property<string>("Client_Country_Code")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<int?>("Client_Id")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Country_Code")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<string>("Coupon")
                        .HasColumnType("longtext");

                    b.Property<string>("Deal_Grouping")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Deal_Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Deal_Source")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("Drawdown")
                        .HasColumnType("int");

                    b.Property<int?>("Entity_Id")
                        .HasColumnType("int");

                    b.Property<decimal?>("Facility")
                        .HasColumnType("decimal(15, 2)");

                    b.Property<DateTime?>("First_CashInterest_Period_EndPeriods")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("First_CashInterest_Period_Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("First_PIKInterest_Period_EndPeriods")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("First_PIKInterest_Period_Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Fund")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("General_Investment_Code")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("General_Investment_Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<decimal?>("IRR")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("Instrument")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool?>("Intercompany_loan")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("Interest_Id")
                        .HasColumnType("int");

                    b.Property<string>("Interest_Rate")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Investment_date")
                        .HasColumnType("date");

                    b.Property<decimal?>("LTV_Entry")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("Loan_Type")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<decimal?>("MOIC")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<DateTime?>("Maturity_date")
                        .HasColumnType("date");

                    b.Property<decimal?>("Minimum_multiple")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal?>("NAV")
                        .HasColumnType("decimal(15, 2)");

                    b.Property<decimal?>("Opening_fee")
                        .HasColumnType("decimal(15, 2)");

                    b.Property<string>("Origination")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("Ownership_Id")
                        .HasColumnType("int");

                    b.Property<int?>("PIK_Interest_Period")
                        .HasColumnType("int");

                    b.Property<decimal?>("Percent_Coinvestors")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal?>("Percent_Master_Fund")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("Product")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("Realization_Date")
                        .HasColumnType("date");

                    b.Property<int?>("Related_fund_id")
                        .HasColumnType("int");

                    b.Property<string>("Repeat_Counterparty")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("Second_CashInterest_Period_EndPeriods")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("Second_CashInterest_Period_Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("Second_PIKInterest_Period_EndPeriods")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("Second_PIKInterest_Period_Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Sector")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Seniority")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Sponsorship")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Status")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Strategy")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Subsector")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Thematic_vs_Opportunistic")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Theme")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("Third_CashInterest_Period_EndPeriods")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("Third_CashInterest_Period_Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("Third_PIKInterest_Period_EndPeriods")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("Third_PIKInterest_Period_Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Underwriting_IRR")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal?>("Underwriting_MOIC")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal?>("Underwriting_NAV")
                        .HasColumnType("decimal(15, 2)");

                    b.HasKey("Deal_Id");

                    b.ToTable("deal", "incus_capital");
                });

            modelBuilder.Entity("API.Entities.FinancialMetrics", b =>
                {
                    b.Property<int>("Metrics_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal?>("Accrued_Cash_Interest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Accrued_Pik_Interest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Accrued_Undrawn_Interest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Facility")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Interest_Generated")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Interest_Payed")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Nav")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Nav_irr")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Nav_moic")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Nav_profit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Total_Collections")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Total_Debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Total_Invested")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("Transaction_Id")
                        .HasColumnType("int");

                    b.Property<decimal?>("Undrawn_Amount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Metrics_id");

                    b.ToTable("financial_metrics");
                });

            modelBuilder.Entity("API.Entities.Fund", b =>
                {
                    b.Property<int?>("Fund_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("Administrator")
                        .HasColumnType("longtext");

                    b.Property<int?>("Bank_account_entity")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasColumnType("longtext");

                    b.Property<string>("Custodian")
                        .HasColumnType("longtext");

                    b.Property<string>("Director_one")
                        .HasColumnType("longtext");

                    b.Property<string>("Director_thre")
                        .HasColumnType("longtext");

                    b.Property<string>("Director_two")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("End_Hp")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("End_Ip")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("Entity")
                        .HasColumnType("int");

                    b.Property<string>("Fund_name")
                        .HasColumnType("longtext");

                    b.Property<int?>("NIF")
                        .HasColumnType("int");

                    b.Property<string>("Register")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Start_Hp")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("Start_Ip")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("Total_commitment")
                        .HasColumnType("int");

                    b.Property<int?>("VAT_number")
                        .HasColumnType("int");

                    b.HasKey("Fund_Id");

                    b.ToTable("fund", "incus_capital");
                });

            modelBuilder.Entity("API.Entities.Transaction", b =>
                {
                    b.Property<int?>("Transaction_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool?>("Accrued")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal?>("Amount_Due_BOP")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Amount_Due_EOP")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Capitalized")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Cash_Interest_Accrued")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Cash_Interest_BOP")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Cash_Interest_EOP")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Cash_Interest_Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Deal_Name")
                        .HasColumnType("longtext");

                    b.Property<decimal?>("Drawdown")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool?>("Occurred")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal?>("PIK_Interest_Accrued")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PIK_Interest_BOP")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PIK_Interest_EOP")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PIK_Interest_Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Principal_BOP")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Principal_EOP")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool?>("Projection")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Related_Deal_Id")
                        .HasColumnType("longtext");

                    b.Property<decimal?>("Repayment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Repayment_CashInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Repayment_PIKInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Repayment_Principal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Repayment_UndrawnFees")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Transaction_Date")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("Undrawn_Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Undrawn_Fee_Interest_Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Undrawn_Interest_Accrued")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Undrawn_Interest_BOP")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Undrawn_Interest_EOP")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Transaction_Id");

                    b.ToTable("transactions", "incus_capital");
                });
#pragma warning restore 612, 618
        }
    }
}
