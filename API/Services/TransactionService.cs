using API.Data;
using API.Entities;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using SQLitePCL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _context;

        public TransactionService(DataContext context)
        {
            _context = context;
        }

        //public List<int> Projections()
        //{
        //    var deals = _context.Deals.ToList();

        //    return deals;
        //}


        //REPAYMENT
        //When a new transaction is added and Calculate button is Clicked --> amount divided between - Cash Interest, PIK Interest, Principal, Fees
        //The result is simply returned, not stored in the database  
        public async Task<Transaction> NewTransaction_Repayment(Transaction transaction)
        {
            // Implementation to process and save the transaction
            //_context.Transactions.Add(transaction);
            //await _context.SaveChangesAsync();
            
            if(transaction.Amount_Due_BOP > 0 && transaction.Repayment != 0){

                //If repayment- cashInterest >0 meaning there is sufficient repayment to pay the cash
                decimal? RemainingAfterCash = transaction.Repayment - transaction.Cash_Interest_BOP;

                #region Cash Interest 
                if ((transaction.Repayment - transaction.Cash_Interest_BOP) > 0) 
                {
                    transaction.Repayment_CashInterest = transaction.Cash_Interest_BOP;

                    #region PIK Interest
                    //If the remaining after paying cash subtracted from PIK Interest is greater than 0 meaning that there is sufficient to pay PIK and left over
                    if (RemainingAfterCash - transaction.PIK_Interest_BOP > 0) {
                        transaction.Repayment_PIKInterest = transaction.PIK_Interest_BOP;

                        //If PIK Interest is payed, it does not need to be capitalized
                        transaction.Capitalized = 0;

                        //For now- the rest gets payed to principal amount
                        transaction.Repayment_Principal = transaction.Repayment - transaction.Repayment_PIKInterest;
                        
                        //PENDING --  Payment to undrawn interest 

                    }
                    //Else not sufficient to pay PIK - Capitalized the rest
                    else
                    {
                        transaction.Repayment_PIKInterest = transaction.Repayment;
                        decimal? RemainingPIK = transaction.PIK_Interest_BOP - transaction.Repayment;
                        //Remaining PIK gets capitalized if the period is reached
                        //IF three months or six months passed capitalize
                        transaction.Capitalized = RemainingPIK;
                        //IF three months or six months pass calculate accruals 
                    }
                    #endregion
                }
                #endregion
            }


            return transaction;
        }

        //DRAWDOWNS
        //Money payed to the client
        public async Task<Transaction> NewTransaction_Drawdown (Transaction transaction)
        {
            if(transaction.Drawdown!= null)
            {
                //If more money is payed to the client- it is subtracted from the undrawn amount
                transaction.Undrawn_Amount = transaction.Undrawn_Amount - transaction.Drawdown;
            }

            return transaction;
        }

        //ACCRUED
        //Calculation of all the built up Accruels
        //Cash Interest, PIK Interest, Undrawn Interest Accrued
        public async Task<Transaction> AccruedValues (Transaction transaction)
        {
            //Calculate accrued values according to rate
            //Cash interest is calfculated on the principal payed in the beginning of time or principal in that moment(if the client has already payed?)
            transaction.Cash_Interest_Accrued = transaction.Principal_BOP * (transaction.Cash_Interest_Rate / 100);
            transaction.PIK_Interest_Accrued = transaction.Amount_Due_BOP * (transaction.Cash_Interest_Rate / 100);
            transaction.Undrawn_Interest_Accrued = transaction.Undrawn_Amount * (transaction.Undrawn_Interest_Accrued / 100);

            return transaction;

        }


        //Drawdown
        //calculation of the interests that are generated from the previous date
        //public async Task<Transaction> Accruels(Transaction transaction)
        //{
        //    decimal? amount_eop = _context.Transactions.OrderByDescending(t => t.Transaction_Date).FirstOrDefault().Amount_Due_EOP.Value;

        //    //If there is no prevous entry then the amount eop should be the entered amount
        //    if(amount_eop != null)
        //    {
        //        transaction.Amount_Due_EOP = amount_eop;                
        //    }
        //    //Else the amount entered should be added to the prevous amount eop
        //    else
        //    {
        //        //The final amount is the combination of the new value included the value of the final 
        //        transaction.Amount_Due_EOP = amount_eop.Value+transaction.;

        //    }
        //}




        #region CALCULATIONS
        //Method called from TransactionsController--> Manages the additions of transactions being made
        //public Transaction NewTransactionMade(Transaction transaction)
        //{
        //    //If the PIK interest to be payed at the beginning of period is null, there is no capitalization
        //    //if (transaction.PIK_Interest_BOP == null)
        //    //{
        //    //    transaction.Capitalized = 0;
        //    //    //transaction.PIK_Interest_BOP = _context.Transactions.OrderByDescending(t => t.Transaction_Date).FirstOrDefault().PIK_Interest_BOP;
        //    //}
        //    //else if(transaction.PIK_Interest_BOP!= null && transaction.)
        //    return transaction;
        //}


        //Amount due BoP

        //INTERESTS

        //Cash Interests

        //PIK Interests returned

        //Undrawn Interest

        //CAPITALIZED

        //REPAYMENT

        //EOP
        //PIK Interest EOP 
        //Cash Interest EOP
        //Undrawn Interest EOP
        //Principial EOP

        //Amount due EOP

        #endregion
    }
}
