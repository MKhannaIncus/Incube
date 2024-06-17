using API.Data;
using API.Entities;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _context;
        private readonly ILogger<BackgroundTaskService> _logger;
        public readonly Deal deal;

        public TransactionService(ILogger<BackgroundTaskService> logger, DataContext context)
        {
            _context = context;
            _logger = logger;

        }

        //public List<int> Projections()
        //{
        //    var deals = _context.Deals.ToList();

        //    return deals;
        //}


        //REPAYMENT
        //When a new transaction is added and Calculate button is Clicked --> amount divided between - Cash Interest, PIK Interest, Principal, Fees
        //The result is simply returned, not stored in the database  
        public async Task<Transaction> NewTransaction_Repayment(Transaction incomingTransaction)
        {
            decimal? RemainingAfterCash;
            //Get the most recent transaction from the table
            //Transaction newTransaction = await _context.Transactions.Where(t => t.Related_Deal_Id == incomingTransaction.Related_Deal_Id).OrderByDescending(t => t.Transaction_Date).ThenByDescending(t => t.Transaction_Id).FirstOrDefaultAsync();
            Transaction newTransaction = await _context.Transactions.Where(t => t.Related_Deal_Id == incomingTransaction.Related_Deal_Id).OrderByDescending(t => t.Transaction_Id).FirstOrDefaultAsync();


            newTransaction.Related_Deal_Id=incomingTransaction.Related_Deal_Id;
            newTransaction.Transaction_Date=incomingTransaction.Transaction_Date;

            //Move the values that were EOP to BOP
            newTransaction.Amount_Due_BOP = newTransaction.Amount_Due_EOP;
            newTransaction.Cash_Interest_BOP = newTransaction.Cash_Interest_EOP;
            newTransaction.PIK_Interest_BOP = newTransaction.PIK_Interest_EOP;
            newTransaction.Principal_BOP = newTransaction.Principal_EOP;
            newTransaction.Undrawn_Interest_BOP = newTransaction.Undrawn_Interest_EOP;

            //Move the repayment from incoming transaction to the newtransaction- repayments are the same
            newTransaction.Repayment = incomingTransaction.Repayment;
            
            //If previous transancion Amount due EOP >0 and repayment made this time >0 
            if (newTransaction.Amount_Due_BOP > 0 && incomingTransaction.Repayment != 0){

                //If repayment- cashInterest >0 meaning there is sufficient repayment to pay the cash
                //Subtracted from the cash interest due BOP
                RemainingAfterCash = newTransaction.Repayment - newTransaction.Cash_Interest_BOP;

                #region Cash Interest 
                if (RemainingAfterCash > 0) 
                {
                    //The cash interest is paid off
                    newTransaction.Repayment_CashInterest = newTransaction.Cash_Interest_BOP;
                    newTransaction.Cash_Interest_EOP = 0;

                    #region PIK Interest
                    decimal? RemainingAfterPIK = RemainingAfterCash - newTransaction.PIK_Interest_BOP;

                    if (RemainingAfterPIK > 0) {
                        newTransaction.Repayment_PIKInterest = newTransaction.PIK_Interest_BOP;
                        newTransaction.PIK_Interest_EOP = 0;
                        //If PIK Interest is payed, it does not need to be capitalized
                        newTransaction.Capitalized = 0;

                        //For now- the rest gets payed to principal amount
                        newTransaction.Repayment_Principal = RemainingAfterPIK;
                        newTransaction.Principal_EOP = newTransaction.Principal_BOP - newTransaction.Repayment_Principal;
                        
                        //PENDING --  Payment to undrawn interest 
                        //PENDING -- If the amount paid is more 
                   

                    }
                    //Else not sufficient to pay PIK - Capitalized the rest
                    else
                    { 

                        newTransaction.Repayment_PIKInterest =  RemainingAfterCash;
                        newTransaction.Capitalized = newTransaction.PIK_Interest_BOP - newTransaction.Repayment_PIKInterest;
                        //Remaining PIK gets capitalized if the period is reached
                        //IF three months or six months passed capitalize


                        //IF three months or six months pass calculate accruals 
                    }
                    #endregion
                }
                else
                {
                    //can only pay part of the cash interest
                    newTransaction.Repayment_CashInterest = newTransaction.Repayment;
                    newTransaction.Cash_Interest_EOP = newTransaction.Cash_Interest_BOP - newTransaction.Repayment_CashInterest;
                    newTransaction.Cash_Interest_EOP = newTransaction.Cash_Interest_BOP - newTransaction.Repayment;

                }
                #endregion
            }

            int? maxTransactionId = await _context.Transactions.MaxAsync(t => t.Transaction_Id);
            newTransaction.Transaction_Id = maxTransactionId + 1;
            newTransaction.Amount_Due_EOP = newTransaction.Cash_Interest_EOP + newTransaction.PIK_Interest_EOP + newTransaction.Principal_EOP;

            _context.Transactions.Add(newTransaction);
            _context.SaveChanges();
            return newTransaction;
        }

        //DRAWDOWNS
        //Money payed to the client
        public async Task<Transaction> NewTransaction_Disbursement (Transaction transaction)
        {
            Transaction transactionNext=null;

            if (transaction.Drawdown!= null)
            {

                if(_context.Transactions.Where(t => t.Related_Deal_Id == transaction.Related_Deal_Id).Count()  > 0)
                {
                    transactionNext = await _context.Transactions.Where(t => t.Related_Deal_Id == transaction.Related_Deal_Id).OrderByDescending(t => t.Transaction_Date).ThenByDescending(t => t.Transaction_Id).FirstOrDefaultAsync();
                }
                else
                {
                    transactionNext = new Transaction();
                    transactionNext.Principal_EOP = transaction.Drawdown;
                    transactionNext.Amount_Due_EOP = transaction.Drawdown;
                }

                // Get the most recent Transaction_Id and add one to it
                int? maxTransactionId = await _context.Transactions.MaxAsync(t => t.Transaction_Id);

                if(transactionNext.Transaction_Id != null)
                {
                    transactionNext.Transaction_Id = maxTransactionId + 1;
                }
                else
                {
                    transaction.Transaction_Id = 1;
                }


                transactionNext.Drawdown = transaction.Drawdown;
                transactionNext.Related_Deal_Id = transaction.Related_Deal_Id;
                //If more money is payed to the client- it is subtracted from the undrawn amount
                transactionNext.Undrawn_Amount = transactionNext.Undrawn_Amount - transaction.Drawdown;
                //Added to the amount due
                
                transactionNext.Principal_EOP = transaction.Principal_BOP + transaction.Drawdown;
                transactionNext.Amount_Due_EOP = transactionNext.Amount_Due_BOP + transaction.Drawdown;


                _context.Transactions.Add(transactionNext);
                await _context.SaveChangesAsync();
            }

            return transactionNext;
        }

        public async Task<Transaction> FirstTransaction (Deal deal)
        {
            Transaction transaction = new Transaction();
            transaction.Related_Deal_Id = deal.Deal_Id;
            transaction.Amount_Due_BOP = 0;
            transaction.Principal_BOP = 0;
            transaction.Cash_Interest_BOP = 0;
            transaction.Undrawn_Amount = deal.Facility;
            transaction.Drawdown = deal.Drawdown;
            transaction.Principal_EOP = deal.Drawdown;
            transaction.Amount_Due_EOP = deal.Drawdown;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;

        }

        public async Task<Transaction> AccruedValues (Transaction transaction)
        {
            //Calculate accrued values according to rate
            //Cash interest is calfculated on the principal payed in the beginning of time or principal in that moment(if the client has already payed?)

            //if last recorded entry is greater than amount of time --  calculate values
            //var mostRecentTransactionDate = await _context.Transactions.Where(t => t.Related_Deal_Id == transaction.Related_Deal_Id).OrderByDescending(t => t.Transaction_Date).ThenByDescending(t => t.Transaction_Id).Select(t => t.Transaction_Date).FirstOrDefaultAsync();
            //var cashInterestPeriod = await _context.Transactions.Where


            var relatedDealId = transaction.Related_Deal_Id;
            int? cashInterestPeriod =  _context.Deals.Where(d => d.Deal_Id == relatedDealId).Select(d => d.Cash_Interest_Period).FirstOrDefault();

            _logger.LogInformation("Testing");

            if (cashInterestPeriod != null)
            {
                var timePassed = DateTime.Now.AddMonths(-cashInterestPeriod.Value);

                //calculating most recent transaction
                var mostRecentTransactionDate = await _context.Transactions.Where(t => t.Related_Deal_Id == relatedDealId)
                                                                            .OrderByDescending(t => t.Transaction_Date)
                                                                            .ThenByDescending(t => t.Transaction_Id)
                                                                            .Select(t => t.Transaction_Date)
                                                                            .FirstOrDefaultAsync();

                if (mostRecentTransactionDate > timePassed)
                {
                    // The most recent transaction date is within the interest period
                    Console.WriteLine("The most recent transaction date is within the calculated interest period.");
                }
                else
                {
                    // The most recent transaction date is not within the interest period
                    //if it is out of the interest period, accrued values need to be calculated
                    Console.WriteLine("The most recent transaction date is not within the calculated interest period.");
                }

            }
            //Getting how long it has passed



            transaction.Cash_Interest_Accrued = transaction.Principal_BOP * (transaction.Cash_Interest_Rate / 100);
            transaction.PIK_Interest_Accrued = transaction.Amount_Due_BOP * (transaction.Cash_Interest_Rate / 100);
            transaction.Undrawn_Interest_Accrued = transaction.Undrawn_Amount * (transaction.Undrawn_Interest_Accrued / 100);

            return transaction;

        }


        //public async Task<int> NAVCalculations (Transaction transaction)
        //{
        //    //Current value of deal= (amount due BOP + cash interest generated) - (cash interest payed)
        //    //Amount of due BOP+ 

        //}

        //ACCRUED
        //Calculation of all the built up Accruels
        //Cash Interest, PIK Interest, Undrawn Interest Accrued

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
