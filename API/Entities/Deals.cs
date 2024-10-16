using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("deal", Schema = "incus_capital")]
    public class Deal
    {
        [Key]
        public string? Deal_Id { get; set; }

        public string? Related_Fund {  get; set; }
        
        public string Deal_Name { get; set; }

        public decimal? Facility { get; set; }

        public string? Country { get; set; }

        public string? Sector { get; set; }

        public string? Subsector { get; set; }

        public DateTime? Investment_date { get; set; }

        public DateTime? Maturity_date { get; set; }

        public decimal? Opening_fee { get; set; }

        public int? Availability_period { get; set; }

        public decimal? Underwriting_IRR { get; set; }

        public decimal? Underwriting_MOIC { get; set; }

        public decimal? Underwriting_NAV { get; set; }

        public string? Comments { get; set; }

        public string? Loan { get; set; }

        public string? Instrument_Dealddbbb { get; set; }

        public string? Extension_Period { get; set; }

        public decimal? First_Utilization { get; set; }

        public decimal? Purchase_Discount_Rate { get; set; }

        public decimal? Exit_fee { get; set; }

        public decimal? Minimum_Multiple { get; set; }

        public string? EURIBOR_Interest_Rate { get; set; }

        public decimal? CashInterest_Rate_1st { get; set; }

        public DateTime? CashInterest_End_1st { get; set; }

        public decimal? CashInterest_Rate_2nd { get; set; }

        public DateTime? CashInterest_End_2nd { get; set; }

        public decimal? CashInterest_Rate_3rd { get; set; }

        public DateTime? CashInterest_End_3rd { get; set; }

        public decimal? PIKInterest_Rate_1st { get; set; }

        public DateTime? PIKInterest_End_1st { get; set; }

        public decimal? PIKInterest_Rate_2nd { get; set; }

        public DateTime? PIKInterest_End_2nd { get; set; }

        public decimal? PIKInterest_Rate_3rd { get; set; }

        public DateTime? PIKInterest_End_3rd { get; set; }

        public decimal? PIYCInterest_Rate_1st { get; set; }

        public DateTime? PIYCInterest_End_1st { get; set; }

        public decimal? PIYCInterest_Rate_2nd { get; set; }

        public DateTime? PIYCInterest_End_2nd { get; set; }

        public decimal? PIYCInterest_Rate_3rd { get; set; }

        public DateTime? PIYCInterest_End_3rd { get; set; }

        public string? Others { get; set; }

        public decimal? Year_Base { get; set; }

        public string? Interest_Period { get; set; }

        public decimal? Default_interest_rate { get; set; }

        public decimal? Undrawn_fee { get; set; }

        public string? Undrawn_fee_periods { get; set; }

        public string? Default_Capitalization_Periods { get; set; }

        public DateTime? Acceleration_Date { get; set; }

        public decimal? Share_Premium { get; set; }

        public string? Lender { get; set; }

        public string? Borrower { get; set; }

        public string? Client { get; set; }

        public decimal? Percent_Master_Fund { get; set; }

        public decimal? Percent_Coinvestor { get; set; }

        public string? Country_Code { get; set; }

        public string? Client_Country_Code { get; set; }

        public string? Asset_Class { get; set; }

        public string? Product { get; set; }

        public string? Strategy { get; set; }

        public string? Grouping { get; set; }

        public string? Loan_Type { get; set; }

        public string? Seniority { get; set; }

        public string? Capital_Repayment { get; set; }

        public string? Coupon { get; set; }
        
        public string? Interest_Rate_Type { get; set; }

        public string? Thematic_Vs_Opportunistic { get; set; }

        public string? Theme { get; set; }

        public string? Origination { get; set; }

        public string? Sponsorship { get; set; }

        public string? Repeat_Counterparty { get; set; }

        public string? Deal_Source { get; set; }

        public string? Instrument_LoanTemplateddbb { get; set;}
    }
}
