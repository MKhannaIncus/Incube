using API.Controllers;
using API.Data;
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

        #region MANUAL INPUTS- Repayments and Drawdowns

        //REPAYMENT
        //When a new transaction is added and Calculate button is Clicked --> amount divided between - Cash Interest, PIK Interest, Principal, Fees
        //The result is simply returned, not stored in the database  
        public async Task<Transaction> NewTransaction_Repayment(Transaction incomingTransaction)
        {
            decimal? RemainingAfterCash;
            //Get the most recent transaction from the table
            //Transaction newTransaction = await _context.Transactions.Where(t => t.Related_Deal_Id == incomingTransaction.Related_Deal_Id).OrderByDescending(t => t.Transaction_Date).ThenByDescending(t => t.Transaction_Id).FirstOrDefaultAsync();
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
                    newTransaction.Cash_Interest_EOP = 0;

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

        //public async Task<Transaction> AccruedValues(Transaction transaction)
        //{
        //    //Calculate accrued values according to rate
        //    //Cash interest is calfculated on the principal payed in the beginning of time or principal in that moment(if the client has already payed?)

        //    //if last recorded entry is greater than amount of time --  calculate values
        //    //var mostRecentTransactionDate = await _context.Transactions.Where(t => t.Related_Deal_Id == transaction.Related_Deal_Id).OrderByDescending(t => t.Transaction_Date).ThenByDescending(t => t.Transaction_Id).Select(t => t.Transaction_Date).FirstOrDefaultAsync();
        //    //var cashInterestPeriod = await _context.Transactions.Where


        //    var relatedDealId = transaction.Related_Deal_Id;
        //    int? cashInterestPeriod = _context.Deals.Where(d => string.Equals(d.Deal_Id, relatedDealId)).Select(d => d.Cash_Interest_Period).FirstOrDefault();

        //    _logger.LogInformation("Testing");

        //    if (cashInterestPeriod != null)
        //    {
        //        var timePassed = DateTime.Now.AddMonths(-cashInterestPeriod.Value);

        //        //calculating most recent transaction
        //        var mostRecentTransactionDate = await _context.Transactions.Where(t => string.Equals(t.Related_Deal_Id, relatedDealId))
        //                                                                    .OrderByDescending(t => t.Transaction_Date)
        //                                                                    .ThenByDescending(t => t.Transaction_Id)
        //                                                                    .Select(t => t.Transaction_Date)
        //                                                                    .FirstOrDefaultAsync();

        //        if (mostRecentTransactionDate > timePassed)
        //        {
        //            // The most recent transaction date is within the interest period
        //            Console.WriteLine("The most recent transaction date is within the calculated interest period.");
        //        }
        //        else
        //        {
        //            // The most recent transaction date is not within the interest period
        //            //if it is out of the interest period, accrued values need to be calculated
        //            Console.WriteLine("The most recent transaction date is not within the calculated interest period.");
        //        }

        //    }
        //    //Getting how long it has passed



        //    transaction.Cash_Interest_Accrued = transaction.Principal_BOP * (transaction.Cash_Interest_Rate / 100);
        //    transaction.PIK_Interest_Accrued = transaction.Amount_Due_BOP * (transaction.Cash_Interest_Rate / 100);
        //    transaction.Undrawn_Interest_Accrued = transaction.Undrawn_Amount * (transaction.Undrawn_Interest_Accrued / 100);

        //    return transaction;

        //}

        //

        /* Accrued values need to be calculted on the basis of:
         * 1. 
         */
        public Transaction AccruedCash(Transaction mostRecentTransaction, Deal deal)
        {

            List<Transaction> AccruedCashList = new List<Transaction>();
            Transaction transactionCreatedCash = new Transaction();

            int monthsPassed = mostRecentTransaction.Transaction_Date.Month - DateTime.Now.Month;

            while (monthsPassed > int.Parse(deal.Interest_Period))
            {
                transactionCreatedCash.Occurred = false;
                transactionCreatedCash.Transaction_Date = mostRecentTransaction.Transaction_Date.AddDays(deal.Interest_Period.Length);
                transactionCreatedCash.Amount_Due_BOP = mostRecentTransaction.Amount_Due_EOP;
                transactionCreatedCash.Principal_BOP = mostRecentTransaction.Amount_Due_EOP;
                transactionCreatedCash.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
                transactionCreatedCash.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;
                transactionCreatedCash.Undrawn_Interest_BOP = mostRecentTransaction.Undrawn_Interest_EOP;

                //transactionCreatedCash.Cash_Interest_Accrued = (transactionCreatedCash.Amount_Due_BOP * deal.First_CashInterest_Period_Rate) + transactionCreatedCash.Amount_Due_BOP;
                transactionCreatedCash.Principal_EOP = transactionCreatedCash.Principal_BOP;
                transactionCreatedCash.Cash_Interest_EOP = transactionCreatedCash.Cash_Interest_BOP + transactionCreatedCash.Cash_Interest_Accrued;
                transactionCreatedCash.PIK_Interest_EOP = transactionCreatedCash.PIK_Interest_BOP;
                transactionCreatedCash.Undrawn_Interest_EOP = transactionCreatedCash.Undrawn_Interest_BOP;
                transactionCreatedCash.Amount_Due_EOP = transactionCreatedCash.Amount_Due_BOP + transactionCreatedCash.Cash_Interest_Accrued;

                AccruedCashList.Add(transactionCreatedCash);

                //Recalculate months passed
                monthsPassed = transactionCreatedCash.Transaction_Date.Month - DateTime.Now.Month;
            }


            //needs to BE PERSISTED INTO THE DATABASE

            return transactionCreatedCash;

        }

        public List<Transaction> AccruedPIK(Deal deal)
        {

            List<Transaction> AccruedPIKList = new List<Transaction>();
            Transaction transactionCreatedPIK = new Transaction();
            Transaction lastTransaction = new Transaction();

            lastTransaction = _context.Transactions.Where(t => t.Deal_Name == deal.Deal_Name)
                .OrderByDescending(t => t.Transaction_Date)
                .FirstOrDefault();

            if (lastTransaction != null)
            {
                //Make sure that the time frame between the last transaction made and present date is equal to investment period
                //If it is more than the present date, its not accrued values, its projected
                int monthsPassed = (DateTime.Now.Year - lastTransaction.Transaction_Date.Year) * 12
                                 + DateTime.Now.Month - lastTransaction.Transaction_Date.Month;

                // A variable to keep track of the rolling transaction date
                DateTime rollingTransactionDate = lastTransaction.Transaction_Date;

                while (monthsPassed >= int.Parse(deal.Interest_Period) && int.Parse(deal.Interest_Period) > 0)
                {
                    transactionCreatedPIK.Occurred = false;

                    // Set transactionCreatedPIK.Transaction_Date to the new rolling date
                    rollingTransactionDate = rollingTransactionDate.AddMonths(int.Parse(deal.Interest_Period));
                    transactionCreatedPIK.Transaction_Date = rollingTransactionDate;

                    transactionCreatedPIK.Deal_Name = deal.Deal_Name;
                    transactionCreatedPIK.Amount_Due_BOP = (lastTransaction.Amount_Due_EOP ?? 0);
                    transactionCreatedPIK.Principal_BOP = (lastTransaction.Principal_EOP ?? 0);
                    transactionCreatedPIK.Cash_Interest_BOP = lastTransaction.Cash_Interest_EOP;
                    transactionCreatedPIK.PIK_Interest_BOP = lastTransaction.PIK_Interest_EOP;
                    transactionCreatedPIK.Undrawn_Interest_BOP = lastTransaction.Undrawn_Interest_EOP;

                    transactionCreatedPIK.Cash_Interest_EOP = transactionCreatedPIK.Cash_Interest_BOP;
                    // Calculating PIK interest generated
                    transactionCreatedPIK.PIK_Interest_Accrued = (transactionCreatedPIK.Amount_Due_BOP * deal.PIKInterest_Rate_1st) + transactionCreatedPIK.Amount_Due_BOP;

                    //If PIK interest BOP is null it will not add the PIK value
                    transactionCreatedPIK.PIK_Interest_EOP = (transactionCreatedPIK.PIK_Interest_BOP ?? 0) + transactionCreatedPIK.PIK_Interest_Accrued;

                    //The principal stays the same becuase nothing is capitalized
                    transactionCreatedPIK.Principal_EOP = transactionCreatedPIK.Principal_BOP;
                    transactionCreatedPIK.Undrawn_Interest_EOP = transactionCreatedPIK.Undrawn_Interest_BOP;
                    transactionCreatedPIK.Amount_Due_EOP = (transactionCreatedPIK.Amount_Due_BOP ?? 0) + transactionCreatedPIK.PIK_Interest_Accrued;
                    transactionCreatedPIK.Accrued = true;

                    AccruedPIKList.Add(transactionCreatedPIK);

                    // Update lastTransaction to the current transactionCreatedPIK for next iteration
                    lastTransaction = transactionCreatedPIK;

                    // Recalculate monthsPassed based on the updated rollingTransactionDate
                    monthsPassed = (DateTime.Now.Year - rollingTransactionDate.Year) * 12
                                 + DateTime.Now.Month - rollingTransactionDate.Month;
                }
            }
            return AccruedPIKList;
        }



        #endregion

        //public async Task<Transaction> Accrued(Deal deal)
        //{
        //    Transaction transactionCreatedCash = new Transaction();
        //    Transaction transactionCreatedPIK = new Transaction();
        //    //Get most recent transaction -- first by date and then by ID
        //    var mostRecentTransaction = await _context.Transactions.Where(t => t.Related_Deal_Id == deal.Deal_Id)
        //                                                            .OrderByDescending(t => t.Transaction_Date)
        //                                                            .ThenByDescending(t => t.Transaction_Id)
        //                                                            .FirstOrDefaultAsync();
        //    if (mostRecentTransaction != null)
        //    {
        //        DateTime lastTransaction = mostRecentTransaction.Transaction_Date;

        //        if (lastTransaction != null && deal.Cash_Interest_Period != null)
        //        {
        //            //CASH INTEREST
        //            //If more time than the cash interest period has passed, cash interest is accrued
        //            if (lastTransaction.Month > deal.Cash_Interest_Period)
        //            {
        //                //Seperate function for creating the given transaction CASH
        //                transactionCreatedCash = AccruedCash(mostRecentTransaction, deal);
        //                transactionCreatedCash.Occurred = false;
        //                transactionCreatedCash.Accrued = true;
        //                //needs to be created in the database

        //            }
        //            //PIK INTEREST
        //            else if (lastTransaction.Month > deal.PIK_Interest_Period)
        //            {
        //                //Create capitalized input
        //                Transaction lastTransaction = Capitalized(deal, mostRecentTransaction);
        //                //Seperate function for creating the given transaction for PIK
        //                transactionCreatedPIK = AccruedPIK(lastTransaction, deal);
        //                transactionCreatedCash.Occurred = false;
        //                transactionCreatedPIK.Accrued = true;
        //                //needs to be created in the database
        //            }
        //        }
        //    }

        //    return transactionCreatedCash;
        //}

        #region Projections

        //public async Task<List<Transaction>> Projections(string dealId)
        //{
        //    List<Transaction> transactionProjected = new List<Transaction>();
        //    Transaction transactionCreatedCash = new Transaction();
        //    Transaction transactionCreatedPIK = new Transaction();
        //    Deal deal = _context.Deals.Where(t => string.Equals(t.Deal_Id , dealId)).FirstOrDefaultAsync().Result;


        //    //Get most recent transaction -- first by date and then by ID
        //    var mostRecentTransaction = await _context.Transactions.Where(t => t.Related_Deal_Id == deal.Deal_Id)
        //                                                            .OrderByDescending(t => t.Transaction_Date)
        //                                                            .ThenByDescending(t => t.Transaction_Id)
        //                                                            .FirstOrDefaultAsync();

        //    if (mostRecentTransaction != null)
        //    {
        //        DateTime lastTransaction = mostRecentTransaction.Transaction_Date;

        //        DateTime presentCalculatedTime = lastTransaction;

        //        while (presentCalculatedTime < deal.Maturity_date)
        //        {
        //            //transactionCreatedCash.Amount_Due_BOP = mostRecentTransaction.Amount_Due_EOP;
        //            //transactionCreatedCash.Principal_BOP = mostRecentTransaction.Principal_EOP;
        //            //transactionCreatedCash.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
        //            //transactionCreatedCash.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;
        //            //transactionCreatedCash.Undrawn_Interest_BOP = mostRecentTransaction.Undrawn_Interest_EOP;

        //            transactionCreatedCash = AccruedCash(mostRecentTransaction, deal);
        //            transactionCreatedCash.Occurred = false;
        //            transactionCreatedCash.Projection = true;
        //            transactionCreatedCash.Transaction_Date = presentCalculatedTime;

        //            mostRecentTransaction = transactionCreatedCash;

        //            transactionCreatedPIK = AccruedPIK(mostRecentTransaction, deal);
        //            transactionCreatedPIK.Occurred = false;
        //            transactionCreatedPIK.Projection = true;
        //            transactionCreatedPIK.Transaction_Date = presentCalculatedTime;

        //            mostRecentTransaction = transactionCreatedPIK;


        //            transactionProjected.Add(transactionCreatedCash);
        //            transactionProjected.Add(transactionCreatedPIK);

        //            presentCalculatedTime = presentCalculatedTime.AddMonths(3);
        //            //presentCalculatedTime = presentCalculatedTime + deal.Cash_Interest_Period.Value;
        //        }

        //    }
        //    return transactionProjected;

        //}

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

                    if (int.TryParse(deal.Interest_Period, out int interestPeriod) && interestPeriod != 0 )
                    {

                        //while it doesnt reach the maturity date-
                        while (rollingDate < maxDate)
                        {
                            Transaction newTransaction = new Transaction();

                            newTransaction.Occurred = false;

                            // Set transactionCreatedPIK.Transaction_Date to the new rolling date
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
                            newTransaction.Accrued = true;

                            projectionsCreated.Add(newTransaction);

                            // Update lastTransaction to the current transactionCreatedPIK for next iteration
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

                            // Set transactionCreatedPIK.Transaction_Date to the new rolling date
                            rollingDate = rollingDate.AddMonths(int.Parse(deal.Interest_Period));
                            newTransaction.Transaction_Date = rollingDate;

                            newTransaction.Deal_Name = deal.Deal_Name;
                            newTransaction.Related_Deal_Id = deal.Deal_Id;
                            newTransaction.Amount_Due_BOP = (lastTransaction.Amount_Due_EOP ?? 0);

                            newTransaction.Principal_BOP = (lastTransaction.Principal_EOP ?? 0);
                            newTransaction.Cash_Interest_BOP = (lastTransaction.Cash_Interest_EOP ?? 0);
                            newTransaction.PIK_Interest_BOP = (lastTransaction.PIK_Interest_EOP ?? 0);
                            newTransaction.Undrawn_Interest_BOP = lastTransaction.Undrawn_Interest_EOP;

                            //newTransaction.Cash_Interest_BOP = (newTransaction.Cash_Interest_EOP ?? 0);
                            // Calculating PIK interest generated
                            newTransaction.Cash_Interest_Accrued = (newTransaction.Amount_Due_BOP * deal.CashInterest_Rate_1st) + newTransaction.Amount_Due_BOP;

                            //If PIK interest BOP is null it will not add the PIK value
                            newTransaction.Cash_Interest_EOP = (newTransaction.Cash_Interest_BOP ?? 0) + newTransaction.Cash_Interest_Accrued;

                            //The principal stays the same becuase nothing is capitalized
                            newTransaction.Principal_EOP = newTransaction.Principal_BOP;
                            newTransaction.Undrawn_Interest_EOP = newTransaction.Undrawn_Interest_BOP;
                            newTransaction.Amount_Due_EOP = (newTransaction.Amount_Due_BOP ?? 0) + newTransaction.Cash_Interest_Accrued;
                            newTransaction.Projection = true;

                            projectionsCreated.Add(newTransaction);

                            // Update lastTransaction to the current transactionCreatedPIK for next iteration
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

        //public async Task<List<Transaction>> Projections(Deal deal)
        //{
        //    List<Transaction> createdProjections = new List<Transaction>();

        //    //Get most recent transaction -- first by date and then by ID
        //    var mostRecentTransaction = await _context.Transactions.Where(t => t.Related_Deal_Id == deal.Deal_Id)
        //                                                            .OrderByDescending(t => t.Transaction_Date)
        //                                                            .ThenByDescending(t => t.Transaction_Id)
        //                                                            .FirstOrDefaultAsync();
        //    if (mostRecentTransaction != null)
        //    {




        //    }

        //}


        //Recieves transaction as it is called from a transaction method

        public async Task<FinancialMetrics> MetricsCalculations(string dealId)
        {
            FinancialMetrics newMetrics = new FinancialMetrics();

            //Calculate all the different values in the metrics that will have the value changed. 
            //newMetrics.Transaction_Id = transaction.Transaction_Id;
            newMetrics.Total_Collections = 0;
            newMetrics.Total_Invested = 0;
            newMetrics.Accrued_Cash_Interest = 0;
            newMetrics.Accrued_Pik_Interest = 0;
            newMetrics.Accrued_Undrawn_Interest = 0;
            newMetrics.Facility = 0;
            newMetrics.Interest_Payed = 0;

            //In an list retrieve all the values in transactions
            List<Transaction> transactionForDeal = await _context.Transactions.Where(t => string.Equals(t.Related_Deal_Id, dealId))
                .OrderByDescending(t => t.Transaction_Id).ToListAsync();

            Deal dealRelated = await _context.Deals.Where(t => string.Equals(t.Deal_Id, dealId)).FirstAsync();

            foreach (Transaction transaction in transactionForDeal)
            {
                newMetrics.Total_Collections += transaction.Repayment;
                newMetrics.Total_Invested += transaction.Drawdown;
                newMetrics.Accrued_Cash_Interest += transaction.Cash_Interest_Accrued;
                newMetrics.Accrued_Pik_Interest += transaction.PIK_Interest_Accrued;
                newMetrics.Accrued_Undrawn_Interest += transaction.Undrawn_Interest_Accrued;
                newMetrics.Interest_Payed += transaction.Repayment_CashInterest;
                newMetrics.Interest_Payed += transaction.Repayment_PIKInterest;
                newMetrics.Interest_Payed += transaction.Repayment_UndrawnFees;


            }

            newMetrics.Interest_Generated = newMetrics.Accrued_Cash_Interest + newMetrics.Accrued_Pik_Interest + newMetrics.Accrued_Undrawn_Interest;

            Transaction mostRecentTransaction = transactionForDeal.FirstOrDefault();
            newMetrics.Nav = mostRecentTransaction.Amount_Due_EOP;
            newMetrics.Nav_profit = newMetrics.Nav + newMetrics.Total_Collections - newMetrics.Total_Invested;
            newMetrics.Nav_moic = newMetrics.Nav_profit / newMetrics.Total_Invested;
            newMetrics.Total_Debt = newMetrics.Nav;
            newMetrics.Facility = dealRelated.Facility;

            //newMetrics.Total_Debt = mostRecentTransaction.Principal_EOP + mostRecentTransaction.eop


            return newMetrics;
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


        #region INJECT VALUES- First Inputs
        public async Task<Transaction> FirstTransaction(Deal deal)
        {
            Transaction transaction = new Transaction();
            transaction.Transaction_Id = 140;
            transaction.Related_Deal_Id = deal.Deal_Id;
            transaction.Amount_Due_BOP = 0;
            transaction.Principal_BOP = 0;
            transaction.Cash_Interest_BOP = 0;
            transaction.Undrawn_Amount = deal.Facility;
            //transaction.Drawdown = deal.Drawdown;
            //transaction.Principal_EOP = deal.Drawdown;
            //transaction.Amount_Due_EOP = deal.Drawdown;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        #endregion

        #region CashRec Transfer

        //Further Processing Cash rec transactions passed
        public async Task<Transaction> ProcessTransaction(Deal deal, Transaction mostRecentTransaction, Transaction newTransaction, Cash_Rec movement)
        {
            // Set the BOP values from the most recent transaction
            //newTransaction.Amount_Due_BOP = mostRecentTransaction.Amount_Due_EOP;
            //newTransaction.Principal_BOP = mostRecentTransaction.Principal_EOP;
            //newTransaction.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
            //newTransaction.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;

            if (!decimal.TryParse(movement.TransactionAmount, out decimal transactionAmount))
            {
                throw new Exception("Invalid transaction amount in CashRec");
            }

            // Handle Collection
            if (movement.Type == "Collection")
            {
                newTransaction.Repayment = transactionAmount;

                if (movement.SubType == "Interest")
                {
                    newTransaction.Repayment_CashInterest = transactionAmount;
                    newTransaction.Amount_Due_EOP = newTransaction.Amount_Due_BOP - transactionAmount;
                }
                else if (movement.SubType == "Principal")
                {
                    newTransaction.Repayment_Principal = transactionAmount;
                    newTransaction.Principal_EOP = newTransaction.Principal_BOP - transactionAmount;
                    newTransaction.Amount_Due_EOP = newTransaction.Amount_Due_BOP - transactionAmount;
                }
            }
            // Handle Investment
            else if (movement.Type == "Investment")
            {
                newTransaction.Drawdown = transactionAmount;
                newTransaction.Undrawn_Amount = deal.Facility - transactionAmount;
                newTransaction.Principal_EOP = newTransaction.Principal_BOP + transactionAmount;
                newTransaction.Amount_Due_EOP = newTransaction.Amount_Due_BOP + transactionAmount;
            }

            return newTransaction;
        }

        //Passing the movements found in Cash Rec to the transactions table  by Filtering values
        public async Task<List<Transaction>> TransactionsFromCashRec()
        {
            List<Transaction> transactionsCreated = new List<Transaction>();

            //Retrieve all the information from CASH_REC database 
            //all data where -> deal_name is present in the DEAL database
            var dealProjects = await _context.Deals.Select(d => d.Deal_Name).ToListAsync();

            List<Cash_Rec> dealsCashRec = await _context.Cash_Rec
                .Where(t => dealProjects.Contains(t.Project))
                .ToListAsync();

            foreach (Cash_Rec movement in dealsCashRec)
            {
                Transaction newTransaction = new Transaction();

                if (!decimal.TryParse(movement.TransactionAmount, out decimal movementDecimal))
                {
                    // Handle invalid transaction amount gracefully
                    Console.WriteLine("Invalid transaction amount for CashRec with ID: " + movement.Project);
                    continue;
                }

                // Check if there is a related deal
                var dealRelated = await _context.Deals
                    .Where(t => t.Deal_Name == movement.Project)
                    .FirstOrDefaultAsync();

                if (dealRelated == null)
                {
                    Console.WriteLine("No deal found for project: " + movement.Project);
                    continue; // Skip if no deal is found
                }

                // Retrieve most recent transaction for this deal
                //var mostRecentTransaction = await _context.Transactions
                //    .Where(t => t.Deal_Name == movement.Project)
                //    .OrderByDescending(t => t.Transaction_Date)
                //    .ThenByDescending(t => t.Transaction_Id)
                //    .FirstOrDefaultAsync();

                var mostRecentTransaction = transactionsCreated
                    .Where(t => t.Deal_Name == movement.Project)
                    .OrderByDescending(t => t.Transaction_Date)
                    .ThenByDescending(t => t.Transaction_Id)
                    .FirstOrDefault();

                newTransaction.Deal_Name = movement.Project;
                DateTime.TryParse(movement.ValueDate, out DateTime value_date);
                newTransaction.Transaction_Date = value_date;

                if (mostRecentTransaction == null)
                {
                    // First transaction setup
                    newTransaction.Amount_Due_BOP = 0;
                    newTransaction.Principal_BOP = 0;
                    newTransaction.Cash_Interest_BOP = 0;
                    newTransaction.PIK_Interest_BOP = 0;
                    newTransaction.Undrawn_Interest_BOP = 0;
                    newTransaction.Principal_EOP = movementDecimal;
                    newTransaction.Undrawn_Amount = dealRelated.Facility - newTransaction.Principal_EOP;
                    newTransaction.Drawdown = movementDecimal;
                    newTransaction.Amount_Due_EOP = newTransaction.Principal_EOP;

                }
                else
                {
                    newTransaction.Amount_Due_BOP = mostRecentTransaction.Amount_Due_EOP;
                    newTransaction.Principal_BOP = mostRecentTransaction.Principal_EOP;
                    newTransaction.Cash_Interest_BOP = mostRecentTransaction.Cash_Interest_EOP;
                    newTransaction.PIK_Interest_BOP = mostRecentTransaction.PIK_Interest_EOP;
                    // Process posterior transaction
                    newTransaction = await ProcessTransaction(dealRelated, mostRecentTransaction, newTransaction, movement);
                }
                _context.Transactions.Add(newTransaction);
                await _context.SaveChangesAsync();

                transactionsCreated.Add(newTransaction);

            }


            return transactionsCreated;
        }

        #endregion
    }
}
