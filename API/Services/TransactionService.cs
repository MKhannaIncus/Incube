using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Migrations;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;


namespace API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _context;
        private readonly DataContext _DBcontext;
        private readonly ILogger<BackgroundTaskService> _logger;
        public readonly Deal deal;

        public TransactionService(ILogger<BackgroundTaskService> logger, DataContext context)
        {
            _context = context;
            _logger = logger;
        }

        #region MANUAL INPUTS- Repayments and Drawdowns

        //REPAYMENT
        //When a new transaction is added and Calculate button is Clicked --> amount divided between - Cash Interest, PIK Interest, Principal, Fees
        //The result is simply returned, not stored in the database  
        public async Task<Transaction> NewTransaction_Repayment(Transaction incomingTransaction)
        {
            decimal? RemainingAfterCash;
            //Get the most recent transaction from the table
            //Transaction newTransactionInvestment = await _context.Transactions.Where(t => t.Related_Deal_Id == incomingTransaction.Related_Deal_Id).OrderByDescending(t => t.Transaction_Date).ThenByDescending(t => t.Transaction_Id).FirstOrDefaultAsync();
            Transaction newTransaction = await _context.Transactions.Where(t => t.Related_Deal_Id == incomingTransaction.Related_Deal_Id).OrderByDescending(t => t.Transaction_Id).FirstOrDefaultAsync();


            newTransaction.Related_Deal_Id = incomingTransaction.Related_Deal_Id;
            newTransaction.Transaction_Date = incomingTransaction.Transaction_Date;

            //Move the values that were EOP to BOP
            newTransaction.Amount_Due_BOP = newTransaction.Amount_Due_EOP;
            newTransaction.Cash_Interest_BOP = newTransaction.Cash_Interest_EOP;
            newTransaction.PIK_Interest_BOP = newTransaction.PIK_Interest_EOP;
            newTransaction.Principal_BOP = newTransaction.Principal_EOP;
            newTransaction.Undrawn_Interest_BOP = newTransaction.Undrawn_Interest_EOP;

            //Move the repayment from incoming transaction to the newtransaction- repayments are the same
            newTransaction.Repayment = incomingTransaction.Repayment;

            //If previous transancion Amount due EOP >0 and repayment made this time >0 
            if (newTransaction.Amount_Due_BOP > 0 && incomingTransaction.Repayment != 0)
            {

                //If repayment- cashInterest >0 meaning there is sufficient repayment to pay the cash
                //Subtracted from the cash interest due BOP
                RemainingAfterCash = newTransaction.Repayment - newTransaction.Cash_Interest_BOP;

                #region Cash Interest 
                if (RemainingAfterCash > 0)
                {
                    //The cash interest is paid off
                    newTransaction.Repayment_CashInterest = newTransaction.Cash_Interest_BOP;
                    //newTransactionInvestment.Cash_Interest_EOP = 0;

                    #region PIK Interest
                    decimal? RemainingAfterPIK = RemainingAfterCash - newTransaction.PIK_Interest_BOP;

                    if (RemainingAfterPIK > 0)
                    {
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

                        newTransaction.Repayment_PIKInterest = RemainingAfterCash;
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
        public async Task<Transaction> NewTransaction_Disbursement(Transaction transaction)
        {
            Transaction transactionNext = null;

            if (transaction.Drawdown != null)
            {

                if (transaction.Drawdown > 0 && _context.Transactions.Where(t => t.Related_Deal_Id == transaction.Related_Deal_Id).Count() > 0)
                {
                    transactionNext = await _context.Transactions.Where(t => t.Related_Deal_Id == transaction.Related_Deal_Id).OrderByDescending(t => t.Transaction_Date).ThenByDescending(t => t.Transaction_Id).FirstOrDefaultAsync();
                    transactionNext.Drawdown = transaction.Drawdown;
                    //transactionNext.Related_Deal_Id = transaction.Related_Deal_Id;
                    //If more money is payed to the client- it is subtracted from the undrawn amount
                    transactionNext.Undrawn_Amount = transactionNext.Undrawn_Amount - transaction.Drawdown;
                    //Added to the amount due

                    transactionNext.Principal_EOP = transaction.Principal_BOP + transaction.Drawdown;
                    transactionNext.Amount_Due_EOP = transactionNext.Amount_Due_BOP + transaction.Drawdown;
                    transaction.Transaction_Date = DateTime.Today;


                }
                else
                {
                    decimal? facility = _context.Deals.Where(t => t.Deal_Id == transaction.Related_Deal_Id).Select(t => t.Facility).FirstOrDefault();
                    transactionNext = new Transaction();
                    transactionNext.Undrawn_Amount = facility - transaction.Drawdown;
                    transactionNext.Related_Deal_Id = transaction.Related_Deal_Id;
                    transactionNext.Principal_EOP = transaction.Drawdown;
                    transactionNext.Amount_Due_EOP = transaction.Drawdown;
                    transaction.Transaction_Date = DateTime.Today;
                }

                // Get the most recent Transaction_Id and add one to it
                int? maxTransactionId = await _context.Transactions.MaxAsync(t => t.Transaction_Id);

                if (maxTransactionId != null)
                {
                    transactionNext.Transaction_Id = maxTransactionId + 1;
                }
                else
                {
                    transaction.Transaction_Id = 1;
                }


                _context.Transactions.Add(transactionNext);
                await _context.SaveChangesAsync();
            }

            return transactionNext;
        }

        #endregion

        #region ACCRUED VALUES -> Automatic deployment

        public List<Transaction> PIKAccrued(Deal deal)
        {
            List<Transaction> AccruedPIKList = new List<Transaction>();

            //Starting off with the first transaction
            //rollingTransaction = _context.Transactions.Where(t => t.Deal_Name == deal.Deal_Name)
            //    .Order(t => t.Transaction_Date)
            //    .FirstOrDefault();

            Transaction firstTransaction = _context.Transactions
                .Where(t => t.Deal_Name == deal.Deal_Name)
                .OrderBy(t => t.Transaction_Date)
                .FirstOrDefault();

            if (firstTransaction != null)
            {
                //Make sure that the time frame between the last transaction made and present date is equal to investment period
                //If it is more than the present date, its not accrued values, its projected
                //int monthsPassed = (DateTime.Now.Year - rollingTransaction.Transaction_Date.Year) * 12
                //                 + DateTime.Now.Month - rollingTransaction.Transaction_Date.Month;

                DateTime rollingTransactionDate = firstTransaction.Transaction_Date;

                if (!int.TryParse(deal.Interest_Period, out int interestPeriod) || interestPeriod == 0)
                {
                    interestPeriod = 3; // Default to 3 if Interest_Period is invalid or zero
                }

                while (rollingTransactionDate.AddMonths(interestPeriod) <= DateTime.Now)
                {

                    Transaction newAccruedTransaction = new Transaction();
                    // Create a new transaction for the accrued period
                    newAccruedTransaction.Occurred = false;

                    // Set rollingTransaction.Transaction_Date to the new rolling date
                    newAccruedTransaction.Transaction_Date = rollingTransactionDate;

                    newAccruedTransaction.Deal_Name = deal.Deal_Name;
                    newAccruedTransaction.Amount_Due_BOP = (firstTransaction.Amount_Due_EOP ?? 0);
                    newAccruedTransaction.Principal_BOP = (firstTransaction.Principal_EOP ?? 0);
                    newAccruedTransaction.Cash_Interest_BOP = firstTransaction.Cash_Interest_EOP;
                    newAccruedTransaction.PIK_Interest_BOP = firstTransaction.PIK_Interest_EOP;
                    newAccruedTransaction.Undrawn_Interest_BOP = firstTransaction.Undrawn_Interest_EOP;

                    newAccruedTransaction.Cash_Interest_EOP = newAccruedTransaction.Cash_Interest_BOP;
                    // Calculating PIK interest generated
                    newAccruedTransaction.PIK_Interest_Accrued = (newAccruedTransaction.Amount_Due_BOP * deal.PIKInterest_Rate_1st) + newAccruedTransaction.Amount_Due_BOP;

                    //If PIK interest BOP is null it will not add the PIK value
                    newAccruedTransaction.PIK_Interest_EOP = (newAccruedTransaction.PIK_Interest_BOP ?? 0) + newAccruedTransaction.PIK_Interest_Accrued;

                    //The principal stays the same becuase nothing is capitalized
                    newAccruedTransaction.Principal_EOP = newAccruedTransaction.Principal_BOP;
                    newAccruedTransaction.Undrawn_Interest_EOP = newAccruedTransaction.Undrawn_Interest_BOP;
                    newAccruedTransaction.Amount_Due_EOP = (newAccruedTransaction.Amount_Due_BOP ?? 0) + newAccruedTransaction.PIK_Interest_Accrued;
                    newAccruedTransaction.Accrued = 1;

                    AccruedPIKList.Add(newAccruedTransaction);

                    // Update rollingTransaction to the current rollingTransaction for next iteration
                    firstTransaction = newAccruedTransaction;

                    rollingTransactionDate = rollingTransactionDate.AddMonths(interestPeriod);

                    //// Recalculate monthsPassed based on the updated rollingTransactionDate
                    //   monthsPassed = (DateTime.Now.Year - rollingTransactionDate.Year) * 12
                    //             + DateTime.Now.Month - rollingTransactionDate.Month;
                }


            }

            return AccruedPIKList;
        }


        public List<Transaction> CashAccrued(Deal deal)
        {
            List<Transaction> AccruedCashList = new List<Transaction>();

            //Starting off with the first transaction
            //rollingTransaction = _context.Transactions.Where(t => t.Deal_Name == deal.Deal_Name)
            //    .Order(t => t.Transaction_Date)
            //    .FirstOrDefault();

            Transaction firstTransaction = _context.Transactions
                .Where(t => t.Deal_Name == deal.Deal_Name)
                .OrderBy(t => t.Transaction_Date)
                .FirstOrDefault();

            if (firstTransaction != null)
            {
                //Make sure that the time frame between the last transaction made and present date is equal to investment period
                //If it is more than the present date, its not accrued values, its projected
                //int monthsPassed = (DateTime.Now.Year - rollingTransaction.Transaction_Date.Year) * 12
                //                 + DateTime.Now.Month - rollingTransaction.Transaction_Date.Month;

                DateTime rollingTransactionDate = firstTransaction.Transaction_Date;

                if (!int.TryParse(deal.Interest_Period, out int interestPeriod) || interestPeriod == 0)
                {
                    interestPeriod = 3; // Default to 3 if Interest_Period is invalid or zero
                }

                while (rollingTransactionDate.AddMonths(interestPeriod) <= DateTime.Now)
                {

                    Transaction newAccruedTransaction = new Transaction();
                    // Create a new transaction for the accrued period
                    newAccruedTransaction.Occurred = false;

                    // Set rollingTransaction.Transaction_Date to the new rolling date
                    newAccruedTransaction.Transaction_Date = rollingTransactionDate;

                    newAccruedTransaction.Deal_Name = deal.Deal_Name;
                    newAccruedTransaction.Amount_Due_BOP = (firstTransaction.Amount_Due_EOP ?? 0);
                    newAccruedTransaction.Principal_BOP = (firstTransaction.Principal_EOP ?? 0);
                    newAccruedTransaction.Cash_Interest_BOP = firstTransaction.Cash_Interest_EOP;
                    newAccruedTransaction.PIK_Interest_BOP = firstTransaction.PIK_Interest_EOP;
                    newAccruedTransaction.Undrawn_Interest_BOP = firstTransaction.Undrawn_Interest_EOP;

                    newAccruedTransaction.PIK_Interest_EOP = newAccruedTransaction.PIK_Interest_BOP;
                    // Calculating PIK interest generated
                    newAccruedTransaction.PIK_Interest_Accrued = 0;
                    newAccruedTransaction.Cash_Interest_Accrued = (newAccruedTransaction.Amount_Due_BOP * deal.CashInterest_Rate_1st) + newAccruedTransaction.Amount_Due_BOP;

                    //If PIK interest BOP is null it will not add the PIK value
                    newAccruedTransaction.Cash_Interest_EOP = (newAccruedTransaction.Cash_Interest_BOP ?? 0) + newAccruedTransaction.Cash_Interest_Accrued;

                    //The principal stays the same becuase nothing is capitalized
                    newAccruedTransaction.Principal_EOP = newAccruedTransaction.Principal_BOP;
                    newAccruedTransaction.Undrawn_Interest_EOP = newAccruedTransaction.Undrawn_Interest_BOP;
                    newAccruedTransaction.Amount_Due_EOP = (newAccruedTransaction.Amount_Due_BOP ?? 0) + newAccruedTransaction.PIK_Interest_Accrued;
                    newAccruedTransaction.Accrued = 1;

                    AccruedCashList.Add(newAccruedTransaction);

                    // Update rollingTransaction to the current rollingTransaction for next iteration
                    firstTransaction = newAccruedTransaction;

                    rollingTransactionDate = rollingTransactionDate.AddMonths(interestPeriod);

                    //// Recalculate monthsPassed based on the updated rollingTransactionDate
                    //   monthsPassed = (DateTime.Now.Year - rollingTransactionDate.Year) * 12
                    //             + DateTime.Now.Month - rollingTransactionDate.Month;
                }


            }

            return AccruedCashList;
        }

        #endregion

        #region Projections

        public List<Transaction> ProjectionsPIK(Deal deal)
        {
            List<Transaction> projectionsCreated = new List<Transaction>();
            Transaction lastTransaction = new Transaction();
            lastTransaction = _context.Transactions.Where(t => t.Deal_Name == deal.Deal_Name)
                            .OrderByDescending(t => t.Transaction_Date)
                            .FirstOrDefault();



            if (lastTransaction != null)
            {
                DateTime rollingDate = lastTransaction.Transaction_Date;
                DateTime? maxDate = deal.Maturity_date;


                if (maxDate != null && lastTransaction != null)
                {

                    if (int.TryParse(deal.Interest_Period, out int interestPeriod) && interestPeriod != 0)
                    {

                        //while it doesnt reach the maturity date-
                        while (rollingDate < maxDate)
                        {
                            Transaction newTransaction = new Transaction();

                            newTransaction.Occurred = false;

                            // Set rollingTransaction.Transaction_Date to the new rolling date
                            rollingDate = rollingDate.AddMonths(int.Parse(deal.Interest_Period));
                            newTransaction.Transaction_Date = rollingDate;

                            newTransaction.Deal_Name = deal.Deal_Name;
                            newTransaction.Related_Deal_Id = deal.Deal_Id;
                            newTransaction.Amount_Due_BOP = (lastTransaction.Amount_Due_EOP ?? 0);

                            newTransaction.Principal_BOP = (lastTransaction.Principal_EOP ?? 0);
                            newTransaction.Cash_Interest_BOP = lastTransaction.Cash_Interest_EOP;
                            newTransaction.PIK_Interest_BOP = (lastTransaction.PIK_Interest_EOP ?? 0);
                            newTransaction.Undrawn_Interest_BOP = lastTransaction.Undrawn_Interest_EOP;

                            newTransaction.Cash_Interest_EOP = newTransaction.Cash_Interest_BOP;
                            // Calculating PIK interest generated
                            newTransaction.PIK_Interest_Accrued = (newTransaction.Amount_Due_BOP * deal.PIKInterest_Rate_1st) + newTransaction.Amount_Due_BOP;

                            //If PIK interest BOP is null it will not add the PIK value
                            newTransaction.PIK_Interest_EOP = (newTransaction.PIK_Interest_BOP ?? 0) + newTransaction.PIK_Interest_Accrued;

                            //The principal stays the same becuase nothing is capitalized
                            newTransaction.Principal_EOP = newTransaction.Principal_BOP;
                            newTransaction.Undrawn_Interest_EOP = newTransaction.Undrawn_Interest_BOP;
                            newTransaction.Amount_Due_EOP = (newTransaction.Amount_Due_BOP ?? 0) + newTransaction.PIK_Interest_Accrued;
                            newTransaction.Accrued = 1;

                            projectionsCreated.Add(newTransaction);

                            // Update rollingTransaction to the current rollingTransaction for next iteration
                            lastTransaction = newTransaction;
                        }


                    }

                }

            }


            return projectionsCreated;

        }

        public List<Transaction> ProjectionsCash(Deal deal)
        {
            List<Transaction> projectionsCreated = new List<Transaction>();
            Transaction lastTransaction = new Transaction();
            lastTransaction = _context.Transactions.Where(t => t.Deal_Name == deal.Deal_Name)
                            .OrderByDescending(t => t.Transaction_Date)
                            .FirstOrDefault();

            if (lastTransaction != null)
            {
                DateTime rollingDate = lastTransaction.Transaction_Date;
                DateTime? maxDate = deal.Maturity_date;


                if (maxDate != null && lastTransaction != null)
                {

                    if (int.TryParse(deal.Interest_Period, out int interestPeriod) && interestPeriod != 0)
                    {

                        deal.CashInterest_Rate_1st = (deal.CashInterest_Rate_1st != 0) ? deal.CashInterest_Rate_1st : 3;

                        //while it doesnt reach the maturity date-
                        while (rollingDate < maxDate)
                        {
                            Transaction newTransaction = new Transaction();

                            newTransaction.Occurred = false;

                            // Set rollingTransaction.Transaction_Date to the new rolling date
                            rollingDate = rollingDate.AddMonths(int.Parse(deal.Interest_Period));
                            newTransaction.Transaction_Date = rollingDate;

                            newTransaction.Deal_Name = deal.Deal_Name;
                            newTransaction.Related_Deal_Id = deal.Deal_Id;
                            newTransaction.Amount_Due_BOP = (lastTransaction.Amount_Due_EOP ?? 0);

                            newTransaction.Principal_BOP = (lastTransaction.Principal_EOP ?? 0);
                            newTransaction.Cash_Interest_BOP = (lastTransaction.Cash_Interest_EOP ?? 0);
                            newTransaction.PIK_Interest_BOP = (lastTransaction.PIK_Interest_EOP ?? 0);
                            newTransaction.Undrawn_Interest_BOP = lastTransaction.Undrawn_Interest_EOP;

                            //newTransactionInvestment.Cash_Interest_BOP = (newTransactionInvestment.Cash_Interest_EOP ?? 0);
                            // Calculating PIK interest generated
                            newTransaction.Cash_Interest_Accrued = (newTransaction.Amount_Due_BOP * deal.CashInterest_Rate_1st) + newTransaction.Amount_Due_BOP;

                            //If PIK interest BOP is null it will not add the PIK value
                            newTransaction.Cash_Interest_EOP = (newTransaction.Cash_Interest_BOP ?? 0) + newTransaction.Cash_Interest_Accrued;

                            //The principal stays the same becuase nothing is capitalized
                            newTransaction.Principal_EOP = newTransaction.Principal_BOP;
                            newTransaction.Undrawn_Interest_EOP = newTransaction.Undrawn_Interest_BOP;
                            newTransaction.Amount_Due_EOP = (newTransaction.Amount_Due_BOP ?? 0) + newTransaction.Cash_Interest_Accrued;
                            newTransaction.Projection = 1;

                            projectionsCreated.Add(newTransaction);

                            // Update rollingTransaction to the current rollingTransaction for next iteration
                            lastTransaction = newTransaction;
                        }


                    }

                }

            }


            return projectionsCreated;

        }

        public Transaction Capitalized(Deal deal, Transaction latestTransaction)
        {
            Transaction transactionCapitalized = new Transaction();

            transactionCapitalized = latestTransaction;
            transactionCapitalized.Amount_Due_BOP = latestTransaction.Amount_Due_EOP;
            transactionCapitalized.Principal_BOP = latestTransaction.Amount_Due_EOP;
            transactionCapitalized.Cash_Interest_BOP = latestTransaction.Cash_Interest_EOP;
            transactionCapitalized.PIK_Interest_BOP = latestTransaction.PIK_Interest_EOP;
            transactionCapitalized.Undrawn_Interest_BOP = latestTransaction.Undrawn_Interest_EOP;
            transactionCapitalized.PIK_Interest_Accrued = 0;
            transactionCapitalized.Capitalized = transactionCapitalized.PIK_Interest_BOP;
            transactionCapitalized.PIK_Interest_EOP = 0;
            transactionCapitalized.Amount_Due_EOP = transactionCapitalized.Amount_Due_BOP + transactionCapitalized.Capitalized;

            //Needs to be created in the database
            return transactionCapitalized;
        }

        #endregion

        //Recieves transaction as it is called from a transaction method

        #region METRICS
        public FinancialMetrics MetricsCalculations(string dealName)
        {
            FinancialMetrics newMetrics = new FinancialMetrics();
            Deal dealRelated = _context.Deals.Where(t => string.Equals(t.Deal_Name, dealName)).First();

            List<Transaction> accruedValuesPIK = new List<Transaction>();
            accruedValuesPIK = PIKAccrued(dealRelated);

            List<Transaction> accruedValuesCash = new List<Transaction>();
            accruedValuesCash = CashAccrued(dealRelated);

            //Calculate all the different values in the metrics that will have the value changed. 
            //newMetrics.Transaction_Id = transaction.Transaction_Id;
            newMetrics.Total_Collections = 0;
            newMetrics.Total_Invested = 0;
            newMetrics.Accrued_Cash_Interest = 0;
            newMetrics.Accrued_Pik_Interest = 0;
            newMetrics.Accrued_Piyc_Interest = 0;
            newMetrics.Accrued_Undrawn_Interest = 0;

            newMetrics.Repayed_Cash_Interest = 0;
            newMetrics.Repayed_Pik_Interest = 0;
            newMetrics.Repayed_Piyc_Interest = 0;
            newMetrics.Repayed_Undrawn_Interest = 0;

            newMetrics.Facility = 0;
            newMetrics.Interest_Payed = 0;
            newMetrics.Interest_Generated = 0;
            newMetrics.Total_Invested = 0;

            //In an list retrieve all the values in transactions
            List<Transaction> transactionForDeal = _context.Transactions.Where(t => string.Equals(t.Deal_Name, dealName))
                .OrderByDescending(t => t.Transaction_Id).ToList();

            if (transactionForDeal.Count > 0 && transactionForDeal != null)
            {
                foreach (Transaction transaction in transactionForDeal)
                {
                    //Facility Details
                    newMetrics.Total_Invested += (transaction.Withrawn_Principal ?? 0);
                    
                    //Accrued
                    newMetrics.Accrued_Cash_Interest += (transaction.Cash_Interest_Accrued ?? 0);
                    newMetrics.Accrued_Pik_Interest += (transaction.PIK_Interest_Accrued ?? 0);
                    newMetrics.Accrued_Piyc_Interest += (transaction.PIYC_Interest_Accrued ?? 0);
                    newMetrics.Accrued_Undrawn_Interest += (transaction.Undrawn_Interest_Accrued ?? 0);

                    newMetrics.Total_Collections += (transaction.Repayment ?? 0);

                    newMetrics.Repayed_Cash_Interest += (transaction.Repayment_CashInterest ?? 0);
                    newMetrics.Repayed_Pik_Interest += (transaction.Repayment_PIKInterest ?? 0);
                    //newMetrics.Repayed_Piyc_Interest += (transaction.repay?? 0);
                    newMetrics.Repayed_Undrawn_Interest += (transaction.Repayment_UndrawnFees ?? 0);

                    newMetrics.Interest_Payed += (transaction.Repayment_CashInterest ?? 0);
                    newMetrics.Interest_Payed += (transaction.Repayment_PIKInterest ?? 0);
                    newMetrics.Interest_Payed += (transaction.Repayment_UndrawnFees ?? 0);
                    newMetrics.Total_Invested += (transaction.Withrawn_Principal ?? 0);
                    
                }

                //newMetrics.Interest_Generated = newMetrics.Accrued_Cash_Interest + newMetrics.Accrued_Pik_Interest + newMetrics.Accrued_Undrawn_Interest;
                Transaction mostRecentTransaction = transactionForDeal.FirstOrDefault();
                newMetrics.Total_Debt = mostRecentTransaction.Amount_Due_EOP;
                newMetrics.Interest_Generated = newMetrics.Accrued_Cash_Interest + newMetrics.Accrued_Pik_Interest + newMetrics.Accrued_Undrawn_Interest;

                newMetrics.Nav = mostRecentTransaction.Amount_Due_EOP;
                newMetrics.Nav_profit = newMetrics.Nav + newMetrics.Total_Collections - newMetrics.Total_Invested;
                newMetrics.Nav_moic = newMetrics.Nav_profit / newMetrics.Total_Invested;
                newMetrics.Facility = dealRelated.Facility;
            }

            //newMetrics.Total_Debt = mostRecentTransaction.Principal_EOP + mostRecentTransaction.eop


            return newMetrics;
        }

        #endregion

        #region CashRec Transfer

        //Passing the movements found in Cash Rec to the transactions table  by Filtering values
        public async Task<List<Transaction>> TransactionsFromCashRec(Deal deal)
        {
            List<Transaction> transactionsCreated = new List<Transaction>();

            //Retrieve all the information from CASH_REC database 
            //all data where -> deal_name is present in the DEAL database

            ////List of deals
            //var dealProjects = await _context.Deals.Select(d => d.Deal_Name).ToListAsync();

            ////filter all values that are contained in cashrec that contain the correct dealnames
            //List<Cash_Rec> dealsCashRec = await _context.Cash_Rec
            //    .Where(t => dealProjects.Contains(t.Project))
            //    .ToListAsync();


            //List of deals
            //var dealProjects = await _context.Deals.Select(d => d.Deal_Name).ToListAsync();

            //filter all values that are contained in cashrec that contain the correct dealnames
            List<Cash_Rec> dealsCashRec = await _context.Cash_Rec
                .Where(t => t.Project == deal.Deal_Name)
                .ToListAsync();

            //Pass through all values in movements in dealscashrec
            foreach (Cash_Rec movement in dealsCashRec)
            {
                //Create new transaction each time
                Transaction newTransaction = new Transaction();

                if (!decimal.TryParse(movement.TransactionAmount, out decimal movementDecimal))
                {
                    // Handle invalid transaction amount gracefully
                    Console.WriteLine("Invalid transaction amount for CashRec with ID: " + movement.Project);
                    continue;
                }

                // Check and retrieve if there is a related deal
                Deal dealRelated = await _context.Deals
                    .Where(t => t.Deal_Name == movement.Project)
                    .FirstOrDefaultAsync();

                if (dealRelated == null)
                {
                    Console.WriteLine("No deal found for project: " + movement.Project);
                    continue; // Skip if no deal is found
                }

                //Retrieve most recent transaction for this deal

                var mostRecentTransaction = await _context.Transactions
                    .Where(t => t.Deal_Name == movement.Project)
                    .OrderByDescending(t => t.Transaction_Date)
                    .ThenByDescending(t => t.Transaction_Id)
                    .FirstOrDefaultAsync();

                ////
                //Transaction mostRecentTransaction = transactionsCreated
                //     .Where(t => t.Deal_Name == movement.Project)
                //     .OrderByDescending(t => t.Transaction_Date)
                //     .ThenByDescending(t => t.Transaction_Id)
                //     .FirstOrDefault();


                //Add the basic information into the transaction, before checking for further information9
                newTransaction.Deal_Name = movement.Project;
                DateTime.TryParse(movement.ValueDate, out DateTime value_date);
                newTransaction.Transaction_Date = value_date;

                //In case this is the first transaction
                if (mostRecentTransaction == null)
                {
                    if (movement.Type == "Investment")
                    {
                        // First transaction setup
                        newTransaction.Undrawn_Amount = dealRelated.Facility;

                        newTransaction.Amount_Due_BOP = 0;
                        newTransaction.Principal_BOP = 0;
                        newTransaction.Cash_Interest_BOP = 0;
                        newTransaction.PIK_Interest_BOP = 0;
                        newTransaction.Undrawn_Interest_BOP = 0;
                        newTransaction.PIK_Interest_BOP = 0;
                        newTransaction.Cash_Interest_BOP = 0;

                        newTransaction.PIK_Interest_Accrued = 0;
                        newTransaction.Cash_Interest_Accrued = 0;
                        newTransaction.Undrawn_Interest_Accrued = 0;

                        newTransaction.Repayment_CashInterest = 0;
                        newTransaction.Repayment_PIKInterest = 0;
                        newTransaction.Repayment_Principal = 0;
                        newTransaction.Repayment_UndrawnFees = 0;

                        newTransaction.Capitalized = 0;
                        //movement.TransactionAmount
                        //If it is a quantity invested, it is first withdrawn amount
                        newTransaction.Withrawn_Principal = movementDecimal;
                        newTransaction.Principal_EOP = movementDecimal;

                        newTransaction.Cash_Interest_EOP = 0;
                        newTransaction.PIK_Interest_EOP = 0;

                        newTransaction.Drawdown = movementDecimal;
                        newTransaction.Amount_Due_EOP = newTransaction.Principal_EOP;


                        _context.Transactions.Add(newTransaction);
                        await _context.SaveChangesAsync();

                        transactionsCreated.Add(newTransaction);

                    }

                }
                //All other cases
                else
                {
                    if (movement.Type == "Collection" || movement.Type == "Investment")
                    {

                        if (mostRecentTransaction.Withrawn_Principal != 0)
                        {
                            //Remove what was was withdrawn in the previous movement
                            newTransaction.Undrawn_Amount = mostRecentTransaction.Undrawn_Amount - mostRecentTransaction.Withrawn_Principal;
                        }

                        newTransaction.Amount_Due_BOP = mostRecentTransaction.Amount_Due_EOP;
                        newTransaction.Principal_BOP = mostRecentTransaction.Principal_EOP;
                        newTransaction.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
                        newTransaction.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;

                        newTransaction.Cash_Interest_Accrued = 0;
                        newTransaction.PIK_Interest_Accrued = 0;
                        newTransaction.Undrawn_Interest_Accrued = 0;
                        newTransaction.Undrawn_Amount = (mostRecentTransaction.Undrawn_Amount ?? 0);

                        // Process Repayments
                        newTransaction = await ProcessTransactionSubType(dealRelated, mostRecentTransaction, newTransaction, movement);

                        _context.Transactions.Add(newTransaction);
                        await _context.SaveChangesAsync();

                        transactionsCreated.Add(newTransaction);
                    }
                }
                //_context.Transactions.Add(newTransactionInvestment);
                //await _context.SaveChangesAsync();

                //transactionsCreated.Add(newTransactionInvestment);
            }


            return transactionsCreated;
        }


        #endregion

        //public async Task<List<Transaction>> CombineTransactionsandAccrued(Deal deal)
        //{
        //    List<Transaction> transactionsCreated = new List<Transaction>();
        //    List<Transaction> AccruedCashList = new List<Transaction>();

        //    //filter all values that are contained in cashrec that contain the correct dealnames
        //    List<Cash_Rec> dealsCashRec = await _context.Cash_Rec
        //        .Where(t => t.Project == deal.Deal_Name)
        //        .ToListAsync();

        //    //Pass through all values in movements in dealscashrec
        //    foreach (Cash_Rec movement in dealsCashRec)
        //    {
        //        //Create new transaction each time
        //        Transaction newTransactionInvestment = new Transaction();


        //        // Check and retrieve if there is a related deal
        //        Deal dealRelated = await _context.Deals
        //            .Where(t => t.Deal_Name == movement.Project)
        //            .FirstOrDefaultAsync();

        //        if (!decimal.TryParse(movement.TransactionAmount, out decimal movementDecimal))
        //        {
        //            // Handle invalid transaction amount gracefully
        //            Console.WriteLine("Invalid transaction amount for CashRec with ID: " + movement.Project);
        //            continue;
        //        }

        //        if (dealRelated == null)
        //        {
        //            Console.WriteLine("No deal found for project: " + movement.Project);
        //            continue; // Skip if no deal is found
        //        }

        //        //Retrieve most recent transaction for this deal
        //        Transaction mostRecentTransaction = await _context.Transactions
        //            .Where(t => t.Deal_Name == movement.Project)
        //            .OrderByDescending(t => t.Transaction_Date)
        //            .ThenByDescending(t => t.Transaction_Id)
        //            .FirstOrDefaultAsync();


        //        /*Accrued values calculations:*/
        //        if (mostRecentTransaction != null)
        //        {
        //            double totalMonths = 0;

        //            //most recent accrued values
        //            Transaction mostRecentTransaction = await _context.Transactions
        //                                .Where(t => t.Deal_Name == movement.Project && t.Accrued == 1)
        //                                .OrderByDescending(t => t.Transaction_Date)
        //                                .ThenByDescending(t => t.Transaction_Id)
        //                                .FirstOrDefaultAsync();

        //            if (!int.TryParse(deal.Interest_Period, out int interestPeriod) || interestPeriod == 0)
        //            {
        //                interestPeriod = 3; // Default to 3 if Interest_Period is invalid or zero
        //            }



        //            // Parse movement.ValueDate to DateTime
        //            if (DateTime.TryParse(movement.ValueDate, out DateTime valueDate))
        //            {
        //                DateTime transactionDate = dealRelated.Investment_date.Value;


        //                // Calculate the difference in days from most recent transaction and present transaction being working with
        //                totalMonths = Math.Abs((transactionDate.Year - valueDate.Year) * 12 + (transactionDate.Month - valueDate.Month));
        //            }

        //            double division = totalMonths / 3;
        //            DateTime rollingTransactionDate = DateTime.MinValue;
        //            //most recent accrued
        //            if (int.TryParse(dealRelated.Interest_Period, out int months))
        //            {
        //                if (mostRecentTransaction != null)
        //                {
        //                    rollingTransactionDate = mostRecentTransaction.Transaction_Date.AddMonths(months);
        //                }
        //                else
        //                {
        //                    //if no previouse date meaning its first accrued
        //                    rollingTransactionDate = (DateTime)dealRelated.Investment_date;
        //                }
        //            }
        //            while (division >= 1 && rollingTransactionDate < valueDate)
        //            {
        //                Transaction newAccruedTransactionCash = new Transaction();
        //                Transaction newAccruedTransactionPIK = new Transaction();


        //                if (mostRecentTransaction != null)
        //                {
        //                    newAccruedTransactionCash = AccruedCash(newAccruedTransactionCash, mostRecentTransaction, rollingTransactionDate, deal);
        //                    AccruedCashList.Add(newAccruedTransactionCash);

        //                    _context.Transactions.Add(newAccruedTransactionCash);
        //                    await _context.SaveChangesAsync();

        //                    mostRecentTransaction = newAccruedTransactionCash;

        //                    newAccruedTransactionPIK = AccruedPIK(newAccruedTransactionPIK, mostRecentTransaction, rollingTransactionDate, deal);
        //                    AccruedCashList.Add(newAccruedTransactionPIK);


        //                    _context.Transactions.Add(newAccruedTransactionPIK);
        //                    await _context.SaveChangesAsync();


        //                    if (newAccruedTransactionPIK != null)
        //                    {
        //                        mostRecentTransaction = newAccruedTransactionPIK;
        //                    }


        //                }
        //                else
        //                {
        //                    newAccruedTransactionCash = AccruedCash(newAccruedTransactionCash, mostRecentTransaction, rollingTransactionDate, deal);
        //                    AccruedCashList.Add(newAccruedTransactionCash);

        //                    _context.Transactions.Add(newAccruedTransactionCash);
        //                    await _context.SaveChangesAsync();

        //                    mostRecentTransaction = newAccruedTransactionCash;

        //                    newAccruedTransactionPIK = AccruedPIK(newAccruedTransactionPIK, mostRecentTransaction, rollingTransactionDate, deal);
        //                    AccruedCashList.Add(newAccruedTransactionPIK);

        //                    _context.Transactions.Add(newAccruedTransactionPIK);
        //                    await _context.SaveChangesAsync();

        //                    if (newAccruedTransactionPIK != null)
        //                    {
        //                        mostRecentTransaction = newAccruedTransactionPIK;
        //                    }
        //                }



        //                // Update rollingTransaction to the current rollingTransaction for next iteration
        //                //firstTransaction = newAccruedTransactionCash;

        //                rollingTransactionDate = rollingTransactionDate.AddMonths(interestPeriod);

        //                //// Recalculate monthsPassed based on the updated rollingTransactionDate
        //                //   monthsPassed = (DateTime.Now.Year - rollingTransactionDate.Year) * 12
        //                //             + DateTime.Now.Month - rollingTransactionDate.Month;


        //                // Parse movement.ValueDate to DateTime
        //                if (DateTime.TryParse(movement.ValueDate, out valueDate))
        //                {
        //                    DateTime transactionDate = rollingTransactionDate;


        //                    // Calculate the difference in days from most recent transaction and present transaction being working with
        //                    totalMonths = Math.Abs((transactionDate.Year - valueDate.Year) * 12 + (transactionDate.Month - valueDate.Month));
        //                }

        //                division = totalMonths / 3;
        //            }
        //        }

        //        /*Transactions values calculations*/

        //        //Add the basic information into the transaction, before checking for further information9
        //        newTransactionInvestment.Deal_Name = movement.Project;
        //        DateTime.TryParse(movement.ValueDate, out DateTime value_date);
        //        newTransactionInvestment.Transaction_Date = value_date;

        //        //In case this is the first transaction
        //        if (mostRecentTransaction == null)
        //        {
        //            if (movement.Type == "Investment")
        //            {
        //                // First transaction setup
        //                newTransactionInvestment.Undrawn_Amount = dealRelated.Facility;

        //                newTransactionInvestment.Amount_Due_BOP = 0;
        //                newTransactionInvestment.Principal_BOP = 0;
        //                newTransactionInvestment.Cash_Interest_BOP = 0;
        //                newTransactionInvestment.PIK_Interest_BOP = 0;
        //                newTransactionInvestment.Undrawn_Interest_BOP = 0;
        //                newTransactionInvestment.PIK_Interest_BOP = 0;
        //                newTransactionInvestment.Cash_Interest_BOP = 0;

        //                newTransactionInvestment.PIK_Interest_Accrued = 0;
        //                newTransactionInvestment.Cash_Interest_Accrued = 0;
        //                newTransactionInvestment.Undrawn_Interest_Accrued = 0;

        //                newTransactionInvestment.Repayment_CashInterest = 0;
        //                newTransactionInvestment.Repayment_PIKInterest = 0;
        //                newTransactionInvestment.Repayment_Principal = 0;
        //                newTransactionInvestment.Repayment_UndrawnFees = 0;

        //                newTransactionInvestment.Capitalized = 0;
        //                //movement.TransactionAmount
        //                //If it is a quantity invested, it is first withdrawn amount
        //                newTransactionInvestment.Withrawn_Principal = movementDecimal;
        //                newTransactionInvestment.Principal_EOP = movementDecimal;

        //                //newTransactionInvestment.Cash_Interest_EOP = 0;
        //                //newTransactionInvestment.PIK_Interest_EOP = 0;

        //                newTransactionInvestment.Drawdown = movementDecimal;
        //                newTransactionInvestment.Amount_Due_EOP = newTransactionInvestment.Principal_EOP;


        //                _context.Transactions.Add(newTransactionInvestment);
        //                await _context.SaveChangesAsync();

        //                transactionsCreated.Add(newTransactionInvestment);

        //            }

        //        }
        //        //All other cases
        //        else
        //        {
        //            if (movement.Type == "Collection" || movement.Type == "Investment")
        //            {

        //                if (mostRecentTransaction.Withrawn_Principal != 0)
        //                {
        //                    //Remove what was was withdrawn in the previous movement
        //                    newTransactionInvestment.Undrawn_Amount = mostRecentTransaction.Undrawn_Amount - mostRecentTransaction.Withrawn_Principal;
        //                }

        //                newTransactionInvestment.Amount_Due_BOP = mostRecentTransaction.Amount_Due_EOP;
        //                newTransactionInvestment.Principal_BOP = mostRecentTransaction.Principal_EOP;
        //                newTransactionInvestment.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
        //                newTransactionInvestment.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;

        //                //newTransactionInvestment.Cash_Interest_Accrued = 0;
        //                //newTransactionInvestment.PIK_Interest_Accrued = 0;
        //                //newTransactionInvestment.Undrawn_Interest_Accrued = 0;
        //                newTransactionInvestment.Undrawn_Amount = (mostRecentTransaction.Undrawn_Amount ?? 0);

        //                // Process Repayments
        //                newTransactionInvestment = await ProcessTransactionSubType(dealRelated, mostRecentTransaction, newTransactionInvestment, movement);

        //                _context.Transactions.Add(newTransactionInvestment);
        //                await _context.SaveChangesAsync();

        //                transactionsCreated.Add(newTransactionInvestment);
        //            }
        //        }
        //        //_context.Transactions.Add(newTransactionInvestment);
        //        //await _context.SaveChangesAsync();

        //        //transactionsCreated.Add(newTransactionInvestment);
        //    }


        //    return transactionsCreated;
        //}


        //Further Processing Cash rec transactions passed

        public async Task<Transaction> ProcessTransactionSubType(Deal deal, Transaction mostRecentTransaction, Transaction newTransaction, Cash_Rec movement)
        {
            // Set the BOP values from the most recent transaction
            //newTransactionInvestment.Amount_Due_BOP = mostRecentTransaction.Amount_Due_EOP;
            //newTransactionInvestment.Principal_BOP = mostRecentTransaction.Principal_EOP;
            //newTransactionInvestment.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
            //newTransactionInvestment.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;

            if (!decimal.TryParse(movement.TransactionAmount, out decimal transactionAmount))
            {
                throw new Exception("Invalid transaction amount in CashRec");
            }

            // Handle Collection -- What is payed back by the borrower
                newTransaction.Repayment = transactionAmount;

                if (movement.SubType == "Interest")
                {
                    if (transactionAmount != null)
                    {
                        //Process PIK first
                        if (mostRecentTransaction.PIK_Interest_EOP != null)
                        {
                            newTransaction.Repayment_PIKInterest = transactionAmount;
                            newTransaction.PIK_Interest_EOP = (newTransaction.PIK_Interest_BOP ?? 0) - newTransaction.Repayment_PIKInterest;
                            newTransaction.Amount_Due_EOP = (newTransaction.Amount_Due_BOP ?? 0) + (newTransaction.Repayment_PIKInterest ?? 0);
                            //newTransactionInvestment.Cash_Interest_EOP = (newTransactionInvestment.Cash_Interest_BOP ?? 0);

                            //Check if there is remaining after paying PIK
                            decimal? remainingAmountAfterPIK = (mostRecentTransaction.PIK_Interest_EOP ?? 0) - transactionAmount;

                            //Incase there is remaining, pay Cash Interest
                            if (remainingAmountAfterPIK > 0)
                            {
                                //What if there is more repayment than cash interest
                                newTransaction.Repayment_CashInterest = newTransaction.Cash_Interest_BOP - remainingAmountAfterPIK;
                                newTransaction.Cash_Interest_EOP = (newTransaction.Cash_Interest_BOP ?? 0);
                                newTransaction.Amount_Due_EOP = (newTransaction.Amount_Due_BOP ?? 0) - (newTransaction.Repayment_CashInterest ?? 0);

                            }

                        }
                        //If PIK accrued till this point is null, process cash
                        else if(mostRecentTransaction.Cash_Interest_EOP != null)
                        {
                            //Need to put a condition if the cashInterest is less than transactionAmount
                            newTransaction.Repayment_CashInterest = transactionAmount;
                            newTransaction.Cash_Interest_EOP = (newTransaction.Cash_Interest_BOP ?? 0) + newTransaction.Repayment_CashInterest;
                            newTransaction.Amount_Due_EOP = (newTransaction.Amount_Due_BOP ?? 0) - (newTransaction.Repayment_CashInterest ?? 0);
                        }
                    
                    }

                    newTransaction.Principal_EOP = (newTransaction.Principal_BOP ?? 0);


                }
                else if (movement.SubType == "Principal")
                {
                    //Assuming that the entire transactonAmount is not in excess when paying the principal
                    newTransaction.Repayment_Principal = transactionAmount;
                    newTransaction.Principal_EOP = (newTransaction.Principal_BOP ?? 0) + transactionAmount;
                    newTransaction.Amount_Due_EOP = (newTransaction.Amount_Due_BOP ?? 0) + transactionAmount;
                    newTransaction.Cash_Interest_EOP = newTransaction.Cash_Interest_BOP;
                    newTransaction.PIK_Interest_EOP = newTransaction.PIK_Interest_BOP;
                }
            
            // Handle Investment -- what is given to the borrower

            return newTransaction;
        }



        public async Task<List<Transaction>> CombineTransactionsandAccruedTest(Deal deal)
        {
            List<Transaction> transactionsCreated = new List<Transaction>();
            List<Transaction> AccruedList = new List<Transaction>();

            //filter all values that are contained in cashrec that contain the correct dealnames
            List<Cash_Rec> dealsCashRec = await _context.Cash_Rec
                .Where(t => t.Project == deal.Deal_Name)
                .ToListAsync();

            //Retrieve most recent transaction for this deal
            Transaction firstTransacion = await _context.Transactions
                .Where(t => t.Deal_Name == deal.Deal_Name)
                .OrderByDescending(t => t.Transaction_Date) // Sort by date descending
                .ThenByDescending(t => t.Transaction_Id)   // Secondary sort by ID
                .FirstOrDefaultAsync();

            if (firstTransacion == null)
            {
                Transaction firstTransaction = new Transaction();

                /*Transactions values calculations*/
                //Add the basic information into the transaction, before checking for further information9
                firstTransaction.Deal_Name = deal.Deal_Name;

                DateTime investmentDate = deal.Investment_date.Value;
                firstTransaction.Transaction_Date = investmentDate;

                // First transaction setup
                firstTransaction.Undrawn_Amount = deal.Facility;

                await _context.Transactions.AddAsync(firstTransaction);
                await _context.SaveChangesAsync();
            }

            //Pass through all values in movements in dealscashrec
            foreach (Cash_Rec movement in dealsCashRec)
            {
                // Check and retrieve if there is a related deal
                Deal dealRelated = await _context.Deals
                    .Where(t => t.Deal_Name == movement.Project)
                    .FirstOrDefaultAsync();

                if (!decimal.TryParse(movement.TransactionAmount, out decimal movementDecimal))
                {
                    // Handle invalid transaction amount gracefully
                    Console.WriteLine("Invalid transaction amount for CashRec with ID: " + movement.Project);
                    continue;
                }

                //FIRST Transaction
                //Iff the most recent transaction DOES NOT exist
                if (dealRelated == null)
                {
                    Console.WriteLine("No deal found for project: " + movement.Project);
                    continue; // Skip if no deal is found
                }
                DateTime.TryParse(movement.ValueDate, out DateTime valueDate);

                Transaction mostRecentTransaction = await _context.Transactions
                    .Where(t => t.Deal_Name == movement.Project && t.Transaction_Date <= valueDate)
                    .OrderByDescending(t => t.Transaction_Date) // Sort by most recent date
                    .ThenByDescending(t => t.Transaction_Id)
                    .FirstOrDefaultAsync();


                Transaction mostRecentAccrued = await _context.Transactions
                    .Where(t => t.Deal_Name == movement.Project && t.Accrued == 1)
                    .OrderByDescending(t => t.Transaction_Date) // Sort by the latest date
                    .FirstOrDefaultAsync();

                //If the most recent transaction exists
                if (mostRecentTransaction != null)
                {
                    /*Transactions values calculations*/
                    if (movement.Type == "Collection")
                    {
                        //Create new transaction each time
                        Transaction newTransaction = new Transaction();

                        newTransaction.Deal_Name = dealRelated.Deal_Name;
                        newTransaction.Transaction_Date = valueDate;
                        newTransaction.Amount_Due_BOP = mostRecentTransaction.Amount_Due_EOP;
                        newTransaction.Principal_BOP = mostRecentTransaction.Principal_EOP;

                        newTransaction.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
                        newTransaction.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;
                        newTransaction.PIYC_Interest_BOP = mostRecentTransaction.PIYC_Interest_EOP;
                        newTransaction.Undrawn_Interest_BOP = mostRecentTransaction.Undrawn_Interest_EOP;

                        newTransaction.Cash_Interest_EOP = newTransaction.Cash_Interest_BOP;
                        newTransaction.PIK_Interest_EOP = newTransaction.PIK_Interest_BOP;
                        newTransaction.PIYC_Interest_EOP = newTransaction.PIYC_Interest_BOP;
                        newTransaction.Undrawn_Interest_EOP = newTransaction.Undrawn_Interest_BOP;

                        newTransaction.Undrawn_Amount = (mostRecentTransaction.Undrawn_Amount ?? 0);

                        //newTransactionInvestment = AccruedForInvestementAndCollection(newTransactionInvestment, valueDate, mostRecentAccrued.Transaction_Date, deal);
                        newTransaction.Principal_EOP = (newTransaction.Principal_BOP ?? 0);

                        // Process Repayments
                        newTransaction = await ProcessTransactionSubType(dealRelated, mostRecentTransaction, newTransaction, movement);


                        await _context.Transactions.AddAsync(newTransaction);
                        await _context.SaveChangesAsync();


                        //Deletes all the accrued values that exist from the date of the transaction forward
                        //These accrued values were created previously from differnt transactions
                        //await DeleteAccruedTransactionsFromDateAsync(deal.Deal_Name, valueDate);

                        //await _context.Transactions
                        //    .Where(t => t.Accrued == 1 && t.Deal_Name == deal.Deal_Name && t.Transaction_Date >= valueDate)
                        //    .ExecuteDeleteAsync();

                        //New Accrued values from the date of the transaction forward
                        await AccruedValuesModifyLoopOne(deal, newTransaction, valueDate);


                        transactionsCreated.Add(newTransaction);
                    }
                    else if (movement.Type == "Investment")
                    {
                        //Create new transaction each time
                        Transaction newTransactionInvestment = new Transaction();

                        newTransactionInvestment.Deal_Name = dealRelated.Deal_Name;
                        newTransactionInvestment.Transaction_Date = valueDate;

                        newTransactionInvestment.Amount_Due_BOP = (mostRecentTransaction.Amount_Due_EOP ?? 0);
                        newTransactionInvestment.Principal_BOP = (mostRecentTransaction.Principal_EOP ?? 0);

                        newTransactionInvestment.Cash_Interest_BOP = (mostRecentTransaction.Cash_Interest_EOP ?? 0)  + (mostRecentTransaction.Cash_Interest_Accrued ?? 0);
                        newTransactionInvestment.PIK_Interest_BOP = (mostRecentTransaction.PIK_Interest_EOP ?? 0) + (mostRecentTransaction.PIK_Interest_Accrued ?? 0);
                        newTransactionInvestment.PIYC_Interest_BOP = (mostRecentTransaction.PIYC_Interest_EOP ?? 0) + (mostRecentTransaction.PIYC_Interest_Accrued ?? 0);
                        newTransactionInvestment.Undrawn_Interest_BOP = (mostRecentTransaction.Undrawn_Interest_EOP ?? 0) + (mostRecentTransaction.Undrawn_Interest_Accrued ?? 0);

                        newTransactionInvestment.Withrawn_Principal = movementDecimal;
                        newTransactionInvestment.Undrawn_Amount = (mostRecentTransaction.Undrawn_Amount ?? 0) + newTransactionInvestment.Withrawn_Principal;
                        newTransactionInvestment.Principal_EOP = newTransactionInvestment.Principal_BOP + movementDecimal;

                        newTransactionInvestment.Drawdown = movementDecimal;
                        newTransactionInvestment.Amount_Due_EOP = newTransactionInvestment.Amount_Due_BOP + movementDecimal ;

                        //newTransactionInvestment.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
                        //newTransactionInvestment.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;
                        //newTransactionInvestment.PIYC_Interest_BOP = mostRecentTransaction.PIYC_Interest_EOP;

                        newTransactionInvestment.Cash_Interest_EOP = newTransactionInvestment.Cash_Interest_BOP;
                        newTransactionInvestment.PIK_Interest_EOP = newTransactionInvestment.PIK_Interest_BOP;
                        newTransactionInvestment.PIYC_Interest_EOP = newTransactionInvestment.PIYC_Interest_BOP;
                        newTransactionInvestment.Undrawn_Interest_EOP = newTransactionInvestment.Undrawn_Interest_BOP;


                        await _context.Transactions.AddAsync(newTransactionInvestment);
                        await _context.SaveChangesAsync();
                    
                        await AccruedValuesModifyLoopOne(deal, newTransactionInvestment, valueDate);

                        transactionsCreated.Add(newTransactionInvestment);

                    }
                }
            }

            return transactionsCreated;
        }


        //rollingTransactionDate is the date of the transaction
        public async Task<List<Transaction>> AccruedValuesModifyLoopOne(Deal deal, Transaction recentTransaction, DateTime rollingTransactionDate)
        {

            List<Transaction> AccruedList = new List<Transaction>();
            decimal applicableCashRate;
            decimal applicablePIKRate;
            decimal applicablePIYCRate;

            AccruedList = await _context.Transactions
                    .Where(t => t.Deal_Name == deal.Deal_Name)
                    .Where(t => t.Accrued == 1 && t.Transaction_Date > rollingTransactionDate) // Filter by Accrued and Date
                    .OrderBy(t => t.Transaction_Date) // Optional: Sort by date ascending
                    .ToListAsync();

            int interestPeriod = int.TryParse(deal.Interest_Period, out int parsedInterestPeriod)
                                    ? parsedInterestPeriod
                                    : 3; // Default to 1 if parsing fails

            Transaction mostRecentAccrued = await _context.Transactions
                .Where(t => t.Deal_Name == deal.Deal_Name)
                .OrderByDescending(t => t.Transaction_Date)
                .ThenByDescending(t => t.Transaction_Id)   // In case of ties, sort by biggest ID
                .FirstOrDefaultAsync();


            if (AccruedList.Count > 0)
            {
                // Temporary list to store new accrued transactions
                List<Transaction> newAccruedList = new List<Transaction>();

                foreach (Transaction accrued in AccruedList.ToList()) // Use ToList() to avoid modifying the collection being iterated
                {
                    applicableCashRate = GetApplicableRate(rollingTransactionDate, "Cash", deal);
                    applicablePIKRate = GetApplicableRate(rollingTransactionDate, "PIK", deal);
                    applicablePIYCRate = GetApplicableRate(rollingTransactionDate, "PIYC", deal);

                    // Get the most recent transaction

                    // Modify accrued transaction or create a new one
                    Transaction AccruedTransaction = AccruedAllInterestsModifications(
                        recentTransaction, accrued, rollingTransactionDate, deal,
                        applicableCashRate, applicablePIKRate, applicablePIYCRate);

                    // Mark the transaction as modified
                    _context.Entry(AccruedTransaction).State = EntityState.Modified;
                }

                // Save all changes to the database
                await _context.SaveChangesAsync();

                // Add new transactions to the original AccruedList (if needed for further processing)
                AccruedList.AddRange(newAccruedList);
            }
            else
            {
                await AccruedValuesLoopOne(deal, rollingTransactionDate);
            }

            return AccruedList;

        }

        public async Task<List<Transaction>> AccruedValuesLoopOne(Deal deal, DateTime rollingTransactionDate)
        {

            List<Transaction> AccruedList = new List<Transaction>();
            //DateTime rollingTransactionDate = deal.Investment_date ?? DateTime.MinValue;

            Transaction newAccruedTransactionCash = new Transaction();
            Transaction newAccruedTransactionPIK = new Transaction();
            Transaction newAccruedTransactionPIYC = new Transaction();
            Transaction newAccruedTransactionUndrawn = new Transaction();

            int interestPeriod = int.TryParse(deal.Interest_Period, out int parsedInterestPeriod)
                                    ? parsedInterestPeriod
                                    : 3; // Default to 1 if parsing fails

            decimal applicableCashRate;
            decimal applicablePIKRate;
            decimal applicablePIYCRate;


            Transaction mostRecentAccrued = await _context.Transactions
                .Where(t => t.Deal_Name == deal.Deal_Name)
                .OrderByDescending(t => t.Transaction_Date)
                .FirstOrDefaultAsync();



            while (rollingTransactionDate < DateTime.Today)
            {                
                
                rollingTransactionDate = rollingTransactionDate.AddMonths(parsedInterestPeriod);

                applicableCashRate = GetApplicableRate(rollingTransactionDate, "Cash", deal);
                applicablePIKRate = GetApplicableRate(rollingTransactionDate, "PIK", deal);
                applicablePIYCRate = GetApplicableRate(rollingTransactionDate, "PIYC", deal);
                
                ////most recent accrued values
                Transaction mostRecentTransaction = await _context.Transactions
                    .Where(t => t.Deal_Name == deal.Deal_Name)
                    .OrderByDescending(t => t.Transaction_Date) // Sort by most recent date
                    .ThenByDescending(t => t.Transaction_Id)   // In case of ties, sort by biggest ID
                    .FirstOrDefaultAsync();

                Transaction AccruedTransaction = new Transaction();

                AccruedTransaction = AccruedAllInterests(mostRecentTransaction, rollingTransactionDate, deal, applicableCashRate, applicablePIKRate, applicablePIYCRate);
                AccruedList.Add(newAccruedTransactionCash);

                await _context.Transactions.AddAsync(AccruedTransaction);
                await _context.SaveChangesAsync();
            }

            return AccruedList;
        }

        public Transaction AccruedForInvestementAndCollection(Transaction preseTransaction, DateTime presTransDate, DateTime prevAcruedDate, Deal deal)
        {
            //Difference in days from previous accrued to present transaciton date
            int elapsedDays = (presTransDate - prevAcruedDate).Days;

            int.TryParse(deal.Interest_Period, out int interestLength);
            //Days passed between previous accured date and next accrued date (days in interest period)
            int daysInPeriod = (prevAcruedDate.AddMonths(interestLength) - prevAcruedDate).Days;

            //Corresponding interest for days passed
            decimal interestAccruedCash = (elapsedDays / daysInPeriod) * GetApplicableRate(presTransDate, "Cash", deal);
            decimal interestAccruedPIK = (elapsedDays / daysInPeriod) * GetApplicableRate(presTransDate, "PIK", deal);
            decimal interestAccruedPIYC = (elapsedDays / daysInPeriod) * GetApplicableRate(presTransDate, "PIYC", deal);

            //Set values for Interst BOP
            preseTransaction.Cash_Interest_Accrued = interestAccruedCash;
            preseTransaction.PIK_Interest_Accrued = interestAccruedPIK;
            preseTransaction.PIYC_Interest_Accrued = interestAccruedPIYC;

            return preseTransaction;
        }

        public decimal GetApplicableRate(DateTime currentDate, string interestType, Deal deal)
        {
            // Determine the interest type columns based on the provided type
            List<(DateTime EndDate, decimal Rate)> interestRates = new();

            if (interestType == "Cash")
            {
                interestRates = new List<(DateTime EndDate, decimal Rate)>
        {
            (deal.CashInterest_End_1st ?? DateTime.MaxValue, deal.CashInterest_Rate_1st ?? 0),
            (deal.CashInterest_End_2nd ?? DateTime.MaxValue, deal.CashInterest_Rate_2nd ?? 0),
            (deal.CashInterest_End_3rd ?? DateTime.MaxValue, deal.CashInterest_Rate_3rd ?? 0)
        };
            }
            else if (interestType == "PIK")
            {
                interestRates = new List<(DateTime EndDate, decimal Rate)>
        {
            (deal.PIKInterest_End_1st ?? DateTime.MaxValue, deal.PIKInterest_Rate_1st ?? 0),
            (deal.PIKInterest_End_2nd ?? DateTime.MaxValue, deal.PIKInterest_Rate_2nd ?? 0),
            (deal.PIKInterest_End_3rd ?? DateTime.MaxValue, deal.PIKInterest_Rate_3rd ?? 0)
        };
            }
            else if (interestType == "PIYC")
            {
                interestRates = new List<(DateTime EndDate, decimal Rate)>
        {
            (deal.PIYCInterest_End_1st ?? DateTime.MaxValue, deal.PIYCInterest_Rate_1st ?? 0),
            (deal.PIYCInterest_End_2nd ?? DateTime.MaxValue, deal.PIYCInterest_Rate_2nd ?? 0),
            (deal.PIYCInterest_End_3rd ?? DateTime.MaxValue, deal.PIYCInterest_Rate_3rd ?? 0)
        };
            }

            // Match the correct rate for the current date
            var applicableRate = interestRates
                .Where(r => currentDate <= r.EndDate) // Ensure the date falls within the range
                .OrderBy(r => r.EndDate)             // Sort by end date
                .FirstOrDefault();

            return applicableRate.Rate;
        }

        public Transaction AccruedCash(Transaction mostRecentTransaction, DateTime rollingTransactionDate, Deal deal, decimal interestRate)
        {
            Transaction newAccruedTransaction = new Transaction();
            // Create a new transaction for the accrued period
            newAccruedTransaction.Occurred = false;

            // Set rollingTransaction.Transaction_Date to the new rolling date
            newAccruedTransaction.Transaction_Date = rollingTransactionDate;

            newAccruedTransaction.Deal_Name = deal.Deal_Name;

            newAccruedTransaction.Amount_Due_BOP = (mostRecentTransaction.Amount_Due_EOP ?? 0);
            newAccruedTransaction.Principal_BOP = (mostRecentTransaction.Principal_EOP ?? 0);
            newAccruedTransaction.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
            newAccruedTransaction.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;
            newAccruedTransaction.Undrawn_Amount = mostRecentTransaction.Undrawn_Amount;

            newAccruedTransaction.Cash_Interest_Accrued = (newAccruedTransaction.Amount_Due_BOP * (interestRate / 100)) + newAccruedTransaction.Amount_Due_BOP;

            //If PIK interest BOP is null it will not add the PIK value
            newAccruedTransaction.Cash_Interest_EOP = (newAccruedTransaction.Cash_Interest_BOP ?? 0) + newAccruedTransaction.Cash_Interest_Accrued;

            //The principal stays the same becuase nothing is capitalized
            newAccruedTransaction.Principal_EOP = newAccruedTransaction.Principal_BOP;
            newAccruedTransaction.Amount_Due_EOP = (newAccruedTransaction.Amount_Due_BOP ?? 0) + newAccruedTransaction.Cash_Interest_Accrued;
            newAccruedTransaction.Accrued = 1;
            //AccruedList.Add(newAccruedTransactionCash);
            return newAccruedTransaction;

        }

        public Transaction AccruedPIK(Transaction mostRecentTransaction, DateTime rollingTransactionDate, Deal deal, decimal interestRate)
        {
            Transaction newAccruedTransaction = new Transaction();

            // Create a new transaction for the accrued period
            newAccruedTransaction.Occurred = false;

            // Set rollingTransaction.Transaction_Date to the new rolling date
            newAccruedTransaction.Transaction_Date = rollingTransactionDate;

            newAccruedTransaction.Deal_Name = deal.Deal_Name;
            newAccruedTransaction.Amount_Due_BOP = (mostRecentTransaction.Amount_Due_EOP ?? 0);
            newAccruedTransaction.Principal_BOP = (mostRecentTransaction.Principal_EOP ?? 0);
            newAccruedTransaction.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;
            newAccruedTransaction.Undrawn_Amount = mostRecentTransaction.Undrawn_Amount;

            // Calculating PIK interest generated
            newAccruedTransaction.PIK_Interest_Accrued = (newAccruedTransaction.Amount_Due_BOP * (interestRate / 100)) + newAccruedTransaction.Amount_Due_BOP;

            //If PIK interest BOP is null it will not add the PIK value
            newAccruedTransaction.PIK_Interest_EOP = (newAccruedTransaction.PIK_Interest_BOP ?? 0) + newAccruedTransaction.PIK_Interest_Accrued;

            //The principal stays the same becuase nothing is capitalized
            newAccruedTransaction.Principal_EOP = newAccruedTransaction.Principal_BOP;
            newAccruedTransaction.Amount_Due_EOP = (newAccruedTransaction.Amount_Due_BOP ?? 0) + newAccruedTransaction.PIK_Interest_Accrued;
            newAccruedTransaction.Accrued = 1;
            //AccruedList.Add(newAccruedTransactionCash);
            return newAccruedTransaction;

        }


        public Transaction AccruedPIYC(Transaction mostRecentTransaction, DateTime rollingTransactionDate, Deal deal, decimal interestRate)
        {
            Transaction newAccruedTransaction = new Transaction();

            // Create a new transaction for the accrued period
            newAccruedTransaction.Occurred = false;

            // Set rollingTransaction.Transaction_Date to the new rolling date
            newAccruedTransaction.Transaction_Date = rollingTransactionDate;

            newAccruedTransaction.Deal_Name = deal.Deal_Name;
            newAccruedTransaction.Amount_Due_BOP = (mostRecentTransaction.Amount_Due_EOP ?? 0);
            newAccruedTransaction.Principal_BOP = (mostRecentTransaction.Principal_EOP ?? 0);
            newAccruedTransaction.PIYC_Interest_BOP = mostRecentTransaction.PIYC_Interest_EOP;

            newAccruedTransaction.Undrawn_Amount = mostRecentTransaction.Undrawn_Amount;
            // Calculating PIK interest generated
            newAccruedTransaction.PIYC_Interest_Accrued = (newAccruedTransaction.Amount_Due_BOP * (interestRate / 100)) + newAccruedTransaction.Amount_Due_BOP;

            //If PIK interest BOP is null it will not add the PIK value
            newAccruedTransaction.PIYC_Interest_EOP = (newAccruedTransaction.PIYC_Interest_BOP ?? 0) + newAccruedTransaction.PIYC_Interest_Accrued;

            //The principal stays the same becuase nothing is capitalized
            newAccruedTransaction.Principal_EOP = newAccruedTransaction.Principal_BOP;
            newAccruedTransaction.Amount_Due_EOP = (newAccruedTransaction.Amount_Due_BOP ?? 0) + newAccruedTransaction.PIYC_Interest_Accrued;
            newAccruedTransaction.Accrued = 1;
            //AccruedList.Add(newAccruedTransactionCash);
            return newAccruedTransaction;

        }


        public Transaction AccruedUndrawn(Transaction mostRecentTransaction, DateTime rollingTransactionDate, Deal deal)
        {
            Transaction newAccruedTransaction = new Transaction();

            // Create a new transaction for the accrued period
            newAccruedTransaction.Occurred = false;

            // Set rollingTransaction.Transaction_Date to the new rolling date
            newAccruedTransaction.Transaction_Date = rollingTransactionDate;

            newAccruedTransaction.Deal_Name = deal.Deal_Name;
            newAccruedTransaction.Amount_Due_BOP = (mostRecentTransaction.Amount_Due_EOP ?? 0);
            newAccruedTransaction.Principal_BOP = (mostRecentTransaction.Principal_EOP ?? 0);
            newAccruedTransaction.Undrawn_Amount = mostRecentTransaction.Undrawn_Amount;

            newAccruedTransaction.Undrawn_Interest_BOP = mostRecentTransaction.Undrawn_Interest_EOP;
            newAccruedTransaction.Undrawn_Interest_Accrued = (newAccruedTransaction.Amount_Due_BOP * (deal.Undrawn_fee / 100)) + newAccruedTransaction.Amount_Due_BOP;
            newAccruedTransaction.Undrawn_Interest_EOP = mostRecentTransaction.Undrawn_Interest_BOP + newAccruedTransaction.Undrawn_Interest_Accrued;

            //The principal stays the same becuase nothing is capitalized
            newAccruedTransaction.Principal_EOP = newAccruedTransaction.Principal_BOP;
            newAccruedTransaction.Amount_Due_EOP = (newAccruedTransaction.Amount_Due_BOP ?? 0) + newAccruedTransaction.Undrawn_Interest_Accrued;
            newAccruedTransaction.Accrued = 1;
            //AccruedList.Add(newAccruedTransactionCash);
            return newAccruedTransaction;

        }


        public Transaction AccruedAllInterests(Transaction mostRecentTransaction, DateTime rollingTransactionDate,Deal deal,decimal? cashInterestRate,decimal? pikInterestRate,decimal? piycInterestRate)
        {
            Transaction newAccruedTransaction = new Transaction
            {

                // General properties
                Occurred = false,
                Transaction_Date = rollingTransactionDate,
                Deal_Name = deal.Deal_Name,
                Amount_Due_BOP = (mostRecentTransaction.Amount_Due_EOP ?? 0),
                Principal_BOP = (mostRecentTransaction.Principal_EOP ?? 0),
                Undrawn_Amount = (mostRecentTransaction.Undrawn_Amount ?? 0)
            };

            // Calculate Cash Interest
            if (cashInterestRate.HasValue && cashInterestRate.Value > 0)
            {
                newAccruedTransaction.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
                newAccruedTransaction.Cash_Interest_Accrued =
                    (newAccruedTransaction.Amount_Due_BOP * (cashInterestRate.Value / 100));
                newAccruedTransaction.Cash_Interest_EOP =
                    (newAccruedTransaction.Cash_Interest_BOP ?? 0) + newAccruedTransaction.Cash_Interest_Accrued;
            }

            // Calculate PIK Interest
            if (pikInterestRate.HasValue && pikInterestRate.Value > 0)
            {
                newAccruedTransaction.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;
                newAccruedTransaction.PIK_Interest_Accrued =
                    (newAccruedTransaction.Amount_Due_BOP * (pikInterestRate.Value / 100));
                newAccruedTransaction.PIK_Interest_EOP =
                    (newAccruedTransaction.PIK_Interest_BOP ?? 0) + newAccruedTransaction.PIK_Interest_Accrued;
            }

            // Calculate PIYC Interest
            if (piycInterestRate.HasValue && piycInterestRate.Value > 0)
            {
                newAccruedTransaction.PIYC_Interest_BOP = mostRecentTransaction.PIYC_Interest_EOP;
                newAccruedTransaction.PIYC_Interest_Accrued =
                    (newAccruedTransaction.Amount_Due_BOP * (piycInterestRate.Value / 100));
                newAccruedTransaction.PIYC_Interest_EOP =
                    (newAccruedTransaction.PIYC_Interest_BOP ?? 0) + newAccruedTransaction.PIYC_Interest_Accrued;
            }

            // Calculate Undrawn Interest

            if (deal.Undrawn_fee.HasValue && deal.Undrawn_fee.Value > 0)
            {

                newAccruedTransaction.Undrawn_Interest_BOP = mostRecentTransaction.Undrawn_Interest_EOP;
                newAccruedTransaction.Undrawn_Interest_Accrued = 
                    (newAccruedTransaction.Amount_Due_BOP * (deal.Undrawn_fee / 100)) + newAccruedTransaction.Amount_Due_BOP;
                newAccruedTransaction.Undrawn_Interest_EOP = newAccruedTransaction.Undrawn_Interest_BOP + newAccruedTransaction.Undrawn_Interest_Accrued;
            }

            // Common updates
            newAccruedTransaction.Principal_EOP = newAccruedTransaction.Principal_BOP; // Principal remains unchanged
            newAccruedTransaction.Cash_Interest_EOP = (newAccruedTransaction.Cash_Interest_BOP ?? 0) + newAccruedTransaction.Cash_Interest_Accrued;
            newAccruedTransaction.PIK_Interest_EOP = (newAccruedTransaction.PIK_Interest_BOP ?? 0) + newAccruedTransaction.PIK_Interest_Accrued;
            newAccruedTransaction.PIYC_Interest_EOP = (newAccruedTransaction.PIYC_Interest_BOP ?? 0) + newAccruedTransaction.PIYC_Interest_Accrued;
            
            newAccruedTransaction.Amount_Due_EOP =
                (newAccruedTransaction.Amount_Due_BOP ?? 0) +
                (newAccruedTransaction.Cash_Interest_Accrued ?? 0) +
                (newAccruedTransaction.PIK_Interest_Accrued ?? 0) +
                (newAccruedTransaction.PIYC_Interest_Accrued ?? 0);

            newAccruedTransaction.Accrued = 1; // Mark as accrued

            return newAccruedTransaction;
        }


        public Transaction AccruedAllInterestsModifications(Transaction mostRecentTransaction, Transaction accruedMod, DateTime rollingTransactionDate, Deal deal, decimal? cashInterestRate, decimal? pikInterestRate, decimal? piycInterestRate)
        {
            accruedMod.Amount_Due_BOP = mostRecentTransaction.Amount_Due_EOP;
            accruedMod.Principal_BOP = mostRecentTransaction.Principal_EOP;
            accruedMod.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
            accruedMod.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;
            accruedMod.PIYC_Interest_BOP = mostRecentTransaction.PIYC_Interest_EOP;
            accruedMod.Undrawn_Interest_BOP = mostRecentTransaction.Undrawn_Interest_EOP;
            accruedMod.Undrawn_Amount = mostRecentTransaction.Undrawn_Amount;

            // Calculate Cash Interest
            if (cashInterestRate.HasValue && cashInterestRate.Value > 0)
            {
                accruedMod.Cash_Interest_Accrued = (accruedMod.Amount_Due_BOP * (cashInterestRate.Value / 100));
                accruedMod.Cash_Interest_EOP = (accruedMod.Cash_Interest_BOP ?? 0) + accruedMod.Cash_Interest_Accrued;
            }

            // Calculate PIK Interest
            if (pikInterestRate.HasValue && pikInterestRate.Value > 0)
            {
                accruedMod.PIK_Interest_Accrued = (accruedMod.Amount_Due_BOP * (pikInterestRate.Value / 100));
                accruedMod.PIK_Interest_EOP = (accruedMod.PIK_Interest_BOP ?? 0) + accruedMod.PIK_Interest_Accrued;
            }

            // Calculate PIYC Interest
            if (piycInterestRate.HasValue && piycInterestRate.Value > 0)
            {
                accruedMod.PIYC_Interest_Accrued = (accruedMod.Amount_Due_BOP * (piycInterestRate.Value / 100));
                accruedMod.PIYC_Interest_EOP = (accruedMod.PIYC_Interest_BOP ?? 0) + accruedMod.PIYC_Interest_Accrued;
            }

            // Calculate Undrawn Interest
            if (deal.Undrawn_fee.HasValue && deal.Undrawn_fee.Value > 0)
            {
                accruedMod.Undrawn_Interest_Accrued =
                    (accruedMod.Amount_Due_BOP * (deal.Undrawn_fee / 100));
                accruedMod.Undrawn_Interest_EOP = (accruedMod.Undrawn_Interest_BOP ?? 0) + accruedMod.Undrawn_Interest_Accrued;
            }

            // Common updates
            accruedMod.Principal_EOP = accruedMod.Principal_BOP; // Principal remains unchanged

            accruedMod.Amount_Due_EOP =
                (accruedMod.Amount_Due_BOP ?? 0) +
                (accruedMod.Cash_Interest_Accrued ?? 0) +
                (accruedMod.PIK_Interest_Accrued ?? 0) +
                (accruedMod.PIYC_Interest_Accrued ?? 0);

            accruedMod.Accrued = 1; // Mark as accrued

            return accruedMod;
        }



    }
}
