using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class CashRec
    {
        [Key]
        [Column("cash_rec_id")]
        public int? CashRec_Id { get; set; }  // Nullable integer for the primary key

        [Column("fund")]
        public string? Fund { get; set; }  // All varchar columns are string and nullable

        [Column("type")]
        public string? Type { get; set; }

        [Column("subtype")]
        public string? SubType { get; set; }

        [Column("counterparty")]
        public string? Counterparty { get; set; }

        [Column("project")]
        public string? Project { get; set; }

        [Column("included_in_loan_template")]
        public string? IncludedInLoanTemplate { get; set; }

        [Column("type_included_in_loan_template")]
        public string? TypeIncludedInLoanTemplate { get; set; }

        [Column("error")]
        public string? Error { get; set; }

        [Column("project_exits")]
        public string? ProjectExits { get; set; }

        [Column("loan_template")]
        public string? LoanTemplate { get; set; }

        [Column("account")]
        public string? Account { get; set; }

        [Column("account_holder")]
        public string? AccountHolder { get; set; }

        [Column("bank")]
        public string? Bank { get; set; }

        [Column("entry_date")]
        public string? EntryDate { get; set; }  // Dates are also varchar

        [Column("value_date")]
        public string? ValueDate { get; set; }

        [Column("transaction_amount")]
        public string? TransactionAmount { get; set; }  // Transaction amount as string (varchar)

        [Column("transaction_currency")]
        public string? TransactionCurrency { get; set; }

        [Column("counterparty_name")]
        public string? CounterpartyName { get; set; }

        [Column("transaction_motivation")]
        public string? TransactionMotivation { get; set; }  // Text field for longer input

        [Column("comments")]
        public string? Comments { get; set; }  // Text field for longer input
    }
}
