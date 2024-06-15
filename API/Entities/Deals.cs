using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("deal", Schema = "incus_capital")]
    public class Deal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Deal_Id { get; set; }

        [MaxLength(255)]
        public string Deal_Name { get; set; }

        public int? Client_Id { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal? Facility { get; set; }

        public int? Asset_Id { get; set; }

        [MaxLength(255)]
        public string? Country { get; set; }

        [MaxLength(255)]
        public string? Sector { get; set; }

        [MaxLength(255)]
        public string? Subsector { get; set; }

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
        public decimal? MOIC { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal? NAV { get; set; }

        public int? Availability_period { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal? Availability_fee { get; set; }

        public bool? Intercompany_loan { get; set; }

        public int? Entity_Id { get; set; }

        public int? Interest_Id { get; set; }

        [MaxLength(255)]
        public string? Amortization_type { get; set; }

        public int? Ownership_Id { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? LTV_Entry { get; set; }

        public int? Related_fund_id { get; set; }

        public int? Cash_Interest_Period { get; set; }
        public int? PIK_Interest_Period { get; set; }

    }
}
