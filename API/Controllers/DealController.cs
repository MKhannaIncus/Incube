using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class DealController : BaseApiController
    {
        private readonly DataContext _context;

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

        //[HttpPost]
        //public async Task<ActionResult<IEnumerable<Deal>>> AddDeal()
        //{
        //    return;
        //}

        [HttpPost]
        public async Task<ActionResult<DealDTO>> NewDeal(DealDTO deal)
        {
            //if(await dealExist)

            var newDeal = new Deal
            {
                Deal_Name = deal.Deal_Name
            };

            _context.Deals.Add(newDeal);
            await _context.SaveChangesAsync();
            return new DealDTO
            {
                Deal_Name = deal.Deal_Name,
            };
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
