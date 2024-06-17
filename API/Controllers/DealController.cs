using API.Data;
using API.DTOs;
using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class DealController : BaseApiController
    {
        private readonly DataContext _context;
        private TransactionService transactionService;
        public DealController(DataContext context)
        {
            _context = context;
        }

        // Corrected GetDeal method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deal>>> GetDeal()
        {
            // Assuming _context.Deals is your DbSet<Deal>
            return await _context.Deals.ToListAsync();
        }

        [HttpGet("DealInformation/{dealId}")]
        public async Task<ActionResult<IEnumerable<DealDTO>>> GetDealDetails(int DealId)
        {
            var dealDetails = await _context.Deals.Where(t => t.Deal_Id == DealId).
                Select( t=> new DealDTO
                {
                    Comments = t.Comments,
                    Investment_date = t.Investment_date,
                    Maturity_date = t.Maturity_date,
                    Facility = t.Facility,
                    Opening_fee = t.Opening_fee,
                    Availability_fee = t.Availability_fee,
                    Minimum_multiple =t.Minimum_multiple,
                    MOIC = t.MOIC,
                    IRR = t.IRR,
                    NAV = t.NAV,
                    Underwriting_MOIC = t.Underwriting_MOIC,
                    Underwriting_IRR = t.Underwriting_IRR,
                    Underwriting_NAV = t.Underwriting_NAV
                }).ToListAsync();

            return Ok(dealDetails);


        }
        [HttpGet("FacilityInformation/{dealId}")]
        public async Task<ActionResult<FacilityInformationDTO>> GetFacilityDetails(int DealId)
        {
            var deal = await _context.Deals
                .Where(d => d.Deal_Id == DealId)
                .Select(d => new FacilityInformationDTO
                {
                    Facility = d.Facility,
                    UndrawnAmount = _context.Transactions
                        .Where(t => t.Related_Deal_Id == DealId)
                        .OrderByDescending(t => t.Transaction_Date)
                        .Select(t => t.Undrawn_Amount)
                        .FirstOrDefault()
                }).FirstOrDefaultAsync();

            if (deal == null)
            {
                return NotFound();
            }

            return Ok(deal);
        }

        [HttpGet("DealbyFunds/{fundId}")]
        public async Task<List<Deal>> GetDealsForFund(int FundId)
        {
            List<Deal> dealsFromFund = await _context.Deals.Where(t => t.Related_fund_id == FundId).OrderByDescending(t => t.Deal_Id).ToListAsync();
            return dealsFromFund;
        }

        [HttpPost]
        public async Task<ActionResult<Deal>> NewDeal(Deal deal)
        {
            //if(await dealExist)

            var newDeal = new Deal
            {
                Deal_Name = deal.Deal_Name,
                Client_Id = deal.Client_Id,
                Facility = deal.Facility,
                Asset_Id = deal.Asset_Id,
                //Country = deal.Country,
                //Sector = deal.Sector,
                //Subsector = deal.Subsector,
                Investment_date = deal.Investment_date,
                Maturity_date = deal.Maturity_date,
                Opening_fee = deal.Opening_fee,
                Minimum_multiple = deal.Minimum_multiple,
                Underwriting_IRR = deal.Underwriting_IRR,
                Underwriting_MOIC = deal.Underwriting_MOIC,
                Underwriting_NAV = deal.Underwriting_NAV,
                Availability_period = deal.Availability_period,
                Availability_fee = deal.Availability_fee,
                Intercompany_loan = deal.Intercompany_loan,
                Entity_Id = deal.Entity_Id,
                Interest_Id = deal.Interest_Id,
                Amortization_type = deal.Amortization_type,
                Ownership_Id = deal.Ownership_Id,
                LTV_Entry = deal.LTV_Entry
            };

            transactionService.FirstTransaction(newDeal);
            _context.Deals.Add(newDeal);
            await _context.SaveChangesAsync();

            return newDeal;
        }


        /*Delete deal method-*/
        //[HttpDelete]
        //public void DeleteDeal(DealDTO deal)
        //{          
        //    foreach(Deal dealpres in _context.Deals)
        //    {
        //        if(dealpres.Alias == deal.Alias)
        //        {
        //            _context.Deals.Remove(dealpres);
        //            _context.SaveChanges();
        //            return;
        //        }
        //    }
        //}
    }
}
