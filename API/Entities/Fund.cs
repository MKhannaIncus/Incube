using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("fund", Schema ="incus_capital")]
    public class Fund
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Fund_Id { get; set; }
        public string? Fund_name {  get; set; }
        public int? Total_commitment { get; set; }
        public DateTime? Start_Ip { get; set; }
        public DateTime? End_Ip { get; set; }
        public DateTime? Start_Hp { get; set; }
        public DateTime? End_Hp { get;set; }
        public string? Register {  get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public int? NIF { get; set; }
        public int? VAT_number { get; set; }
        public int? Bank_account_entity {get; set; }
        public int? Entity {  get; set; }
        public string? Administrator { get; set; }
        public string? Custodian { get; set; }
        public string? Director_one { get; set; }
        public string? Director_two { get; set; }
        public string? Director_thre { get; set; }


    }
}
