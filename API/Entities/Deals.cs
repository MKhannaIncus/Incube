using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace API.Entities
{
    [Table("deal")]
    public class Deal
    {
        [Key]
        public string Alias { get; set; }

        [Column("Fund")]
        public string Fund { get; set; }

        [Column("Signing_date")]
        public DateTime SigningDate { get; set; }

        [Column("Borrower_one")]
        public string BorrowerOne { get; set; }

        [Column("Borrower_two")]
        public string BorrowerTwo { get; set; }

        [Column("Ownership_one")]
        public string OwnershipOne { get; set; }

        [Column("Percent_ownership_one")]
        public string PercentOwnershipOne { get; set; }

        [Column("Type_Investment")]
        public string TypeInvestment { get; set; }

        [Column("Type_Investor")]
        public string TypeInvestor { get; set; }

        [Column("Strategy")]
        public string Strategy { get; set; }

        [Column("Sector")]
        public string Sector { get; set; }

        [Column("Sub_sector")]
        public string SubSector { get; set; }

        [Column("NAV_type")]
        public string NAVType { get; set; }

        [Column("National_amount")]
        public string NationalAmount { get; set; }

        [Column("Guarantee_1")]
        public string Guarantee { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Status")]
        public string Status { get; set; }

        [Column("Deal_Performance")]
        public string DealPerformance { get; set; }

        [Column("Related_investment_code")]
        public int InvestorCode { get; set; }

        //[ForeignKey("deal_ibfk_1")]
        //public Fund Fund { get; set;}
    }
}
