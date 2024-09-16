using System;

namespace API.DTOs
{
    public class DealDTO
    {
        public int Deal_Id { get; set; }
        public string Deal_Name { get; set; }
        public int? Client_Id { get; set; }
        public decimal? Facility { get; set; }
        public int? Asset_Id { get; set; }
        public string? Country { get; set; }
        public string? Sector { get; set; }
        public string? Subsector { get; set; }
        public DateTime? Investment_date { get; set; }
        public DateTime? Maturity_date { get; set; }
        public decimal? Opening_fee { get; set; }
        public decimal? Minimum_multiple { get; set; }
        public decimal? IRR { get; set; }
        public decimal? MOIC { get; set; }
        public decimal? NAV { get; set; }
        public decimal? Underwriting_IRR { get; set; }
        public decimal? Underwriting_MOIC { get; set; }
        public decimal? Underwriting_NAV { get; set; }

        public int? Availability_period { get; set; }
        public decimal? Availability_fee { get; set; }
        public bool? Intercompany_loan { get; set; }
        public int? Entity_Id { get; set; }
        public int? Interest_Id { get; set; }
        public string Amortization_type { get; set; }
        public int? Ownership_Id { get; set; }
        public decimal? LTV_Entry { get; set; }
        public string? Comments { get; set; }
        public int? Drawdown {  get; set; }

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

    }
}
