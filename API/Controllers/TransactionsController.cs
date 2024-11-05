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

        //Get most recent transaction
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
            List<Transaction> transactionForDeal = await _context.Transactions.Where(t => string.Equals(t.Deal_Name,DealId)).ToListAsync();


            return transactionForDeal;
        }


        [HttpPost("NewTransactionRepayment")]
        public Task<Transaction> Repayment(Transaction transaction)
        {
            try
            {
                //var result = _transactionService.NewTransactionMade(transaction);
                var result =_transactionService.NewTransaction_Repayment(transaction);
                    return result;
            }
            catch (System.Exception ex)
            {
                BadRequest(ex.Message);
                return null;
            }
        }


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


        //[HttpGet("Projections/{dealId}")]
        //public async Task<ActionResult<List<Transaction>>> GetProjections(string dealId)
        //{
        //    List<Transaction> projectionTransactions = new List<Transaction>();
        //    projectionTransactions = await _transactionService.Projections(dealId);
        //    return projectionTransactions;

        //}

        [HttpGet("Projections/CashRecTransfer")]  
        public async Task<List<Transaction>> TransferCashRecToTransactions()
        {
            List < Transaction > result = await _transactionService.TransactionsFromCashRec();

            return result;
        }

        [HttpGet("Projections/{dealName}")]
        public async Task<List<Transaction>> Projections(string dealName)
        {
            List<Transaction> ProjectedValuesPIK = new List<Transaction>();
            List<Transaction> ProjectedValuesCash = new List<Transaction>();
            List<Transaction> AllTransactions = new List<Transaction>();

            Deal relatedDeal = _context.Deals.Where(d => d.Deal_Name == dealName).FirstOrDefault();


            //List<Deal> relatedDeal = await _context.Deals.ToListAsync();

            //foreach (Deal deal in relatedDeal)
            //{
                ProjectedValuesPIK = _transactionService.ProjectionsPIK(relatedDeal);
                ProjectedValuesCash = _transactionService.ProjectionsCash(relatedDeal);
                AllTransactions.AddRange(ProjectedValuesPIK);
                AllTransactions.AddRange(ProjectedValuesCash);
            

            return AllTransactions;

        }


        [HttpGet("Accrued/PIKInterest/{dealName}")]
        public async Task<List<Transaction>> AccruedPIKTransactions(string dealName)
        {
            List<Transaction> AccruedValues = new List<Transaction>();

            Transaction lastTransaction = new Transaction();
            lastTransaction =await GetMostRecentTransactions(dealName);

            Deal relatedDeal = _context.Deals.Where(d => d.Deal_Name == dealName).FirstOrDefault();

            AccruedValues = _transactionService.AccruedPIK(relatedDeal);

            return AccruedValues;

        }

        [HttpGet("Accrued/AllPIK")]
        public async Task<List<List<Transaction>>> AccruedPIKTransactionsForAllDeals()
        {
            List<Transaction> AccruedValues = new List<Transaction>();
            List<List<Transaction>> AllTransactions = new List<List<Transaction>>();


            List<Deal> relatedDeal = await _context.Deals.ToListAsync();

            foreach(Deal deal in relatedDeal)
            {
                AccruedValues = _transactionService.AccruedPIK(deal);
                AllTransactions.Add(AccruedValues);
            }

            return AllTransactions;

        }


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
