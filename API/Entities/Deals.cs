using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("deal", Schema = "incus_capital")]
    public class Deal
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Deal_Id { get; set; }

        [MaxLength(255)]
        public string Deal_Name { get; set; }

        public int? Client_Id { get; set; }
        public string? Client { get; set; }


        [Column(TypeName = "decimal(15, 2)")]
        public decimal? Facility { get; set; }

        public int? Asset_Id { get; set; }

        [MaxLength(255)]
        public string? Country { get; set; }

        [MaxLength(255)]
        public string? Sector { get; set; }

        [MaxLength(255)]
        public string? Subsector { get; set; }

        [MaxLength(255)]
        public string? Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Investment_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Maturity_date { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal? Opening_fee { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Minimum_multiple { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? IRR { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Underwriting_IRR { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? MOIC { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Underwriting_MOIC { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal? NAV { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal? Underwriting_NAV { get; set; }

        public int? Availability_period { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal? Availability_fee { get; set; }

        public bool? Intercompany_loan { get; set; }

        public int? Entity_Id { get; set; }

        public int? Interest_Id { get; set; }

        public string? Amortization_type { get; set; }

        public int? Ownership_Id { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? LTV_Entry { get; set; }

        public int? Related_fund_id { get; set; }

        public string? Comments { get; set; }

        public int? Drawdown { get; set; }

        public int? Cash_Interest_Period { get; set; }

        public int? PIK_Interest_Period { get; set; }

        public decimal? First_CashInterest_Period_Rate { get; set; }

        public DateTime? First_CashInterest_Period_EndPeriods { get; set; }

        public decimal? Second_CashInterest_Period_Rate { get; set; }

        public DateTime? Second_CashInterest_Period_EndPeriods { get; set; }

        public decimal? Third_CashInterest_Period_Rate { get; set; }

        public DateTime? Third_CashInterest_Period_EndPeriods { get; set; }

        public decimal? First_PIKInterest_Period_Rate { get; set; }

        public DateTime? First_PIKInterest_Period_EndPeriods { get; set; }

        public decimal? Second_PIKInterest_Period_Rate { get; set; }

        public DateTime? Second_PIKInterest_Period_EndPeriods { get; set; }

        public decimal? Third_PIKInterest_Period_Rate { get; set; }

        public DateTime? Third_PIKInterest_Period_EndPeriods { get; set; }

        // New Fields
        [MaxLength(255)]
        public string? Fund { get; set; }

        [MaxLength(255)]
        public string? General_Investment_Code { get; set; }

        [MaxLength(255)]
        public string? General_Investment_Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Realization_Date { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Percent_Master_Fund { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Percent_Coinvestors { get; set; }

        [MaxLength(3)]
        public string? Country_Code { get; set; }

        [MaxLength(3)]
        public string? Client_Country_Code { get; set; }

        [MaxLength(255)]
        public string? Asset_Class { get; set; }

        [MaxLength(255)]
        public string? Product { get; set; }

        [MaxLength(255)]
        public string? Strategy { get; set; }

        [MaxLength(255)]
        public string? Deal_Grouping { get; set; }

        [MaxLength(255)]
        public string? Loan_Type { get; set; }

        [MaxLength(255)]
        public string? Seniority { get; set; }

        [MaxLength(255)]
        public string? Capital_Repayment { get; set; }


        public string? Coupon { get; set; }                              

        public string? Interest_Rate { get; set; }

        [MaxLength(255)]
        public string? Thematic_vs_Opportunistic { get; set; }

        [MaxLength(255)]
        public string? Theme { get; set; }

        [MaxLength(255)]
        public string? Origination { get; set; }

        [MaxLength(255)]
        public string? Sponsorship { get; set; }

        [MaxLength(255)]
        public string? Repeat_Counterparty { get; set; }

        [MaxLength(255)]
        public string? Deal_Source { get; set; }

        [MaxLength(255)]
        public string? Instrument { get; set; }
    }
}
