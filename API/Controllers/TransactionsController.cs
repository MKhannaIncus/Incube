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

        //Get all transactions in descending order
        [HttpGet("TransactionsInDescending")]
        public async Task<List<Transaction>> GetMostRecentTransactions()
        {
            List<Transaction> mostRecentTransaction = await _context.Transactions.OrderByDescending(t => t.Transaction_Date).ToListAsync();

            return mostRecentTransaction;
        }

        //Get all transactions based on a certain deal
        [HttpGet("TransactionsFromDeal/{DealId}")]
        public async Task<List<Transaction>> GetTransactionsForDeals(int DealId)
        {
            List<Transaction> transactionForDeal = await _context.Transactions.Where(t => t.Related_Deal_Id == DealId).ToListAsync();

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


        [HttpPost("NewTransactionDrawdown")]
        public Task<Transaction> Drawdown(Transaction transaction)
        {
            try
            {
                //var result = _transactionService.NewTransactionMade(transaction);
                var result = _transactionService.NewTransaction_Drawdown(transaction);
                return result;
            }
            catch (System.Exception ex)
            {
                BadRequest(ex.Message);
                return null;
            }
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
