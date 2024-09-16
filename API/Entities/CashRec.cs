using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class CashRec
    {
        [Key]
        [Column("cash_rec_id")]
        public int? CashRec_Id { get; set; }

        [Column("fund")]
        public string? Fund { get; set; }

        [Column("projection_date")]
        public DateTime? ProjectionDate { get; set; }

        [Column("investment_name")]
        public string InvestmentName { get; set; }

        [Column("date")]
        public DateTime? Date { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; } 

        [Column("master_fund")]
        public string MasterFund { get; set; }

        [Column("sc_fund")]
        public string SCFund { get; set; }

        [Column("coinvestors")]
        public string CoInvestors { get; set; }

        [Column("movement_type")]
        public string MovementType { get; set; }

        [Column("subtype_movement")]
        public string SubTypeMovement { get; set; }

        [Column("investment_code")]
        public string InvestmentCode { get; set; }

        [Column("closed")]
        public bool Closed { get; set; }

        [Column("deal_status")]
        public string DealStatus { get; set; }
    }
}
