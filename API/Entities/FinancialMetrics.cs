using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("financial_metrics")] // Specify the table name if different from the class name
    public class FinancialMetrics
    {
        [Key]
        public int Metrics_id { get; set; }

        //public int? Deal_Id { get; set; }
        //public Deal Deal { get; set; }

        public int? Transaction_Id { get; set; }
        //public Transaction Transaction { get; set; }
        public decimal? Nav { get; set; }
        public decimal? Nav_irr { get; set; }
        public decimal? Nav_moic { get; set; }
        public decimal? Nav_profit { get; set; }
        public decimal? Total_Collections { get; set; }
        public decimal? Total_Invested { get; set; }
        public decimal? Interest_Generated { get; set; }
        public decimal? Accrued_Cash_Interest { get; set; }
        public decimal? Accrued_Pik_Interest { get; set; }
        public decimal? Accrued_Piyc_Interest { get; set; }
        public decimal? Accrued_Undrawn_Interest { get; set; }
        public decimal? Repayed_Cash_Interest { get; set; }
        public decimal? Repayed_Pik_Interest { get; set; }
        public decimal? Repayed_Piyc_Interest { get; set; }
        public decimal? Repayed_Undrawn_Interest { get; set; }
        public decimal? Interest_Payed { get; set; }
        public decimal? Total_Debt { get; set; }
        public decimal? Facility { get; set; }
        public decimal? Undrawn_Amount { get; set; }

    }
}
