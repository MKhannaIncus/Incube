using API.Data;
using API.Entities;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly TransactionService _transactionService;

        public TransactionsController(TransactionService transactionService, DataContext context)
        {
            _transactionService = transactionService;
            _context = context;
        }


        //All transactionsse
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entities.Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        //Get most recent transactions for the given deal
        [HttpGet("TransactionsInDescending")]
        public async Task<Transaction> GetMostRecentTransactions(string dealId)
        {
            Transaction mostRecentTransaction = await _context.Transactions
                .Where(t => t.Deal_Name == dealId)
                .OrderByDescending(t => t.Transaction_Date)
                .FirstOrDefaultAsync();

            return mostRecentTransaction;
        }

        //Get all transactions based on a certain deal
        [HttpGet("TransactionsFromDeal/{DealId}")]
        public async Task<List<Transaction>> GetTransactionsForDeals(string DealId)
        {
            //List<Transaction> transactionForDeal = await _context.Transactions.Where(t => string.Equals(t.Deal_Name, DealId)).ToListAsync();

            List<Transaction> transactionForDeal = await _context.Transactions
                    .Where(t => string.Equals(t.Deal_Name, DealId))
                    .OrderBy(t => t.Transaction_Date) // Replace TransactionDate with your date column
                    .ToListAsync();


            return transactionForDeal;
        }

        //Repayment is added manually
        [HttpPost("NewTransactionRepayment")]
        public Task<Transaction> Repayment(Transaction transaction)
        {
            try
            {
                //var result = _transactionService.NewTransactionMade(transaction);
                var result = _transactionService.NewTransaction_Repayment(transaction);
                return result;
            }
            catch (System.Exception ex)
            {
                BadRequest(ex.Message);
                return null;
            }
        }

        //Disbursement is added manually
        [HttpPost("NewTransactionDisbursement")]
        public Task<Transaction> Disbursement(Transaction transaction)
        {
            try
            {
                //var result = _transactionService.NewTransactionMade(transaction);
                var result = _transactionService.NewTransaction_Disbursement(transaction);
                return result;
            }
            catch (System.Exception ex)
            {
                BadRequest(ex.Message);
                return null;
            }
        }


        //Removes the list of transactions already stored in the database and recalculates the transactions and accrued and stores it again in the database
        [HttpGet("CashRecTransfer/{dealName}")]
        public async Task<List<Transaction>> TransferCashRecToTransactions(string dealName)
        {
            Deal relatedDeal = _context.Deals.Where(d => d.Deal_Name == dealName).FirstOrDefault();

            // Get all transactions associated with the deal
            var transactionsToDelete = _context.Transactions
                .Where(t => t.Deal_Name == relatedDeal.Deal_Name);

            // Remove the transactions from the context
            _context.Transactions.RemoveRange(transactionsToDelete);

            // Save changes to persist the deletion
            _context.SaveChanges();

            List<Transaction> result = await _transactionService.TransactionsFromCashRec(relatedDeal);

            return result;
        }



        //Removes the list of transactions already stored in the database and recalculates the transactions and accrued and stores it again in the database
        [HttpGet("TransactionsAndAccrued/{dealName}")]
        public async Task<List<Transaction>> TransactionsAndAccrued(string dealName)
        {
            Deal relatedDeal = _context.Deals.Where(d => d.Deal_Name == dealName).FirstOrDefault();

            // Get all transactions associated with the deal
            var transactionsToDelete = _context.Transactions
                .Where(t => t.Deal_Name == relatedDeal.Deal_Name);

            // Remove the transactions from the context
            _context.Transactions.RemoveRange(transactionsToDelete);

            // Save changes to persist the deletion
            _context.SaveChanges();

            List<Transaction> result = await _transactionService.CombineTransactionsandAccruedTest(relatedDeal);

            return result;
        }


        //Calculate the Cash and PIK projections related to the deal and outputs them 
        //Not saved in the database
        [HttpGet("Projections/{dealName}")]
        public async Task<List<Transaction>> Projections(string dealName)
        {
            List<Transaction> ProjectedValuesPIK = new List<Transaction>();
            List<Transaction> ProjectedValuesCash = new List<Transaction>();
            List<Transaction> AllTransactions = new List<Transaction>();

            Deal relatedDeal = _context.Deals.Where(d => d.Deal_Name == dealName).FirstOrDefault();

            ProjectedValuesPIK = _transactionService.ProjectionsPIK(relatedDeal);
            ProjectedValuesCash = _transactionService.ProjectionsCash(relatedDeal);
            AllTransactions.AddRange(ProjectedValuesPIK);
            AllTransactions.AddRange(ProjectedValuesCash);


            return AllTransactions;

        }

        //Calculate the PIK Accrued related to the deal and outputs them 
        //Not saved in the database
        [HttpGet("Accrued/PIKInterest/{dealName}")]
        public async Task<List<Transaction>> AccruedPIKTransactions(string dealName)
        {
            List<Transaction> AccruedValues = new List<Transaction>();

            Transaction lastTransaction = new Transaction();
            lastTransaction = await GetMostRecentTransactions(dealName);

            Deal relatedDeal = _context.Deals.Where(d => d.Deal_Name == dealName).FirstOrDefault();

            AccruedValues = _transactionService.PreviousAccruedPIK(relatedDeal);

            return AccruedValues;

        }

        //Caclulates accrued PIK and Cash interests
        //Not saved in the database
        [HttpGet("Accrued/{dealName}")]
        public async Task<List<Transaction>> AccruedValues(string dealName)
        {
            List<Transaction> AccruedValues = new List<Transaction>();


            //List<Deal> relatedDeal = await _context.Deals.ToListAsync();

            //foreach(Deal deal in relatedDeal)
            //{
            //    AccruedValues = _transactionService.PIKAccrued(deal);
            //    AllTransactions.Add(AccruedValues);
            //}

            List<Transaction> AccruedValuesPIK = new List<Transaction>();
            List<Transaction> AccruedValuesCash = new List<Transaction>();
            List<Transaction> AllTransactions = new List<Transaction>();

            Deal relatedDeal = _context.Deals.Where(d => d.Deal_Name == dealName).FirstOrDefault();


            //List<Deal> relatedDeal = await _context.Deals.ToListAsync();

            //foreach (Deal deal in relatedDeal)
            //{
            AccruedValuesPIK = _transactionService.PIKAccrued(relatedDeal);
            AccruedValuesCash = _transactionService.CashAccrued(relatedDeal);
            AllTransactions.AddRange(AccruedValuesPIK);
            AllTransactions.AddRange(AccruedValuesCash);


            return AllTransactions;

        }




        //[HttpGet("Projections/{dealId}")]
        //public async Task<ActionResult<List<Transaction>>> GetProjections(string dealId)
        //{
        //    List<Transaction> projectionTransactions = new List<Transaction>();
        //    projectionTransactions = await _transactionService.Projections(dealId);
        //    return projectionTransactions;

        //}


        //[HttpPost("PeriodicAccrued")]
        //public Task<Transaction> Periodic_Accrued(Transaction transaction)
        //{


        //    return transaction;
        //}



        //Creation of a new transaction and submitted
        //[HttpPost("NewTransaction")]
        //public IActionResult NewTransactionRecalculation(Transaction transaction)
        //{
        //    //Recieves new transactions of type Transaction from the front end
        //    try
        //    {
        //        //var result = _transactionService.NewTransactionMade(transaction);
        //        var result = 
        //        return Ok(result);                      
        //    }
        //    catch(System.Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
