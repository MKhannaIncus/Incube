using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("transactions", Schema = "incus_capital")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Transaction_Id { get; set; }

        public int? Related_Deal_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Transaction_Date { get; set; }

        public bool? Occurred { get; set; }

        public decimal? Amount_Due_BOP { get; set; }

        public decimal? Principal_BOP { get; set; }

        public decimal? Cash_Interest_BOP { get; set; }

        public decimal? PIK_Interest_BOP { get; set; }

        public decimal? Undrawn_Interest_BOP { get; set; }

        public decimal? Cash_Interest_Rate { get; set; }

        public decimal? PIK_Interest_Rate { get; set; }

        public decimal? Undrawn_Fee_Interest_Rate { get; set; }

        public decimal? Undrawn_Amount { get; set; }

        public decimal? Cash_Interest_Accrued { get; set; }

        public decimal? PIK_Interest_Accrued { get; set; }

        public decimal? Undrawn_Interest_Accrued { get; set; }

        public decimal? Repayment { get; set; }

        public decimal? Repayment_CashInterest { get; set; }

        public decimal? Repayment_PIKInterest { get; set; }

        public decimal? Repayment_Principal { get; set; }

        public decimal? Repayment_UndrawnFees { get; set; }

        public decimal? Capitalized { get; set; }

        public decimal? Principal_EOP { get; set; }

        public decimal? Cash_Interest_EOP { get; set; }

        public decimal? PIK_Interest_EOP { get; set; }

        public decimal? Undrawn_Interest_EOP { get; set; }

        public decimal? Amount_Due_EOP { get; set; }

        public decimal? Drawdown { get; set; }

        //// Navigation property for the related deal
        //[ForeignKey("Related_Deal_Id")]
        //public virtual Deal RelatedDeal { get; set; }
    }
}
