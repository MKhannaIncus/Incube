using API.Data;
using API.DTOs;
using API.Entities;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace API.Controllers
{
    [Route("api/[controller]")]
    public class DealController : BaseApiController
    {
        private readonly DataContext _context;
        private TransactionService transactionService;
        private ExcelReader excelReader;

        public DealController(DataContext context)
        {
            _context = context;
            transactionService = new TransactionService(null, _context);
            excelReader = new ExcelReader();
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
                Select(t => new DealDTO
                {
                    Comments = t.Comments,
                    Investment_date = t.Investment_date,
                    Maturity_date = t.Maturity_date,
                    Facility = t.Facility,
                    Opening_fee = t.Opening_fee,
                    Availability_fee = t.Availability_fee,
                    Minimum_multiple = t.Minimum_multiple,
                    MOIC = t.MOIC,
                    IRR = t.IRR,
                    NAV = t.NAV,
                    Underwriting_MOIC = t.Underwriting_MOIC,
                    Underwriting_IRR = t.Underwriting_IRR,
                    Underwriting_NAV = t.Underwriting_NAV,
                    First_CashInterest_Period_Rate = t.First_CashInterest_Period_Rate,
                    First_CashInterest_Period_EndPeriods = t.First_CashInterest_Period_EndPeriods,
                    Second_CashInterest_Period_Rate = t.Second_CashInterest_Period_Rate,
                    Second_CashInterest_Period_EndPeriods = t.Second_CashInterest_Period_EndPeriods,
                    Third_CashInterest_Period_Rate = t.Third_CashInterest_Period_Rate,
                    Third_CashInterest_Period_EndPeriods = t.Third_CashInterest_Period_EndPeriods,
                    First_PIKInterest_Period_Rate = t.First_PIKInterest_Period_Rate,
                    First_PIKInterest_Period_EndPeriods = t.First_PIKInterest_Period_EndPeriods,
                    Second_PIKInterest_Period_Rate = t.Second_PIKInterest_Period_Rate,
                    Second_PIKInterest_Period_EndPeriods = t.Second_PIKInterest_Period_EndPeriods,
                    Third_PIKInterest_Period_Rate = t.Third_PIKInterest_Period_Rate,
                    Third_PIKInterest_Period_EndPeriods = t.Third_PIKInterest_Period_EndPeriods

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
                LTV_Entry = deal.LTV_Entry,
                Comments = deal.Comments
            };

            //transactionService.FirstTransaction(newDeal);
            _context.Deals.Add(newDeal);
            await _context.SaveChangesAsync();

            Entities.Transaction transaction = new Entities.Transaction();
            transaction.Drawdown = 0;
            transaction.Related_Deal_Id = newDeal.Deal_Id;
            transactionService.NewTransaction_Disbursement(transaction);

            return newDeal;
        }

        [HttpGet("readCashRecExcel")]
        public async Task<List<CashRec>> ReadExcelCashRec()
        {
            string filepath = @"I:\Finance\Funds\Reporting\.Databases\Fund III\Fund III - Investments.xlsx";
            List<CashRec> visualizeValues = new List<CashRec>();

            try
            {
                List<List<string>> result = excelReader.ReadExcel(filepath, "Projections");
                foreach (List<string> column in result.Skip(3))
                {
                    if (!(string.IsNullOrEmpty(column[1]) || string.IsNullOrEmpty(column[2]) || string.IsNullOrEmpty(column[3])))
                    {
                            var cashRec = new CashRec
                            {
                                Fund = column[1], // Fund III
                                ProjectionDate = DateTime.ParseExact(column[2].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture), // 31/12/2019
                                InvestmentName = column[3], // Alcantara
                                Date = DateTime.ParseExact(column[4].ToString(), "dd MMM yy", CultureInfo.InvariantCulture),
                                Amount = (column[5] =="-" || column[5] == "") ? 0 : decimal.Parse(column[5], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture), // (19,000,001)
                                MasterFund = column[6] == "-" ? null : column[6], // "-"
                                SCFund = column[7] == "-" ? null : column[7], // "-"
                                CoInvestors = column[8] == "-" ? null : column[8], // "-"
                                MovementType = column[9], // Investment
                                SubTypeMovement = column[10],
                                InvestmentCode = column[11], // INV3-009
                                Closed = column[12] == "Cost" ? true : false, // Closed status
                                DealStatus = column[13] // Deal Status, "On track"

                            };
                        visualizeValues.Add(cashRec);

                        _context.CashRecs.Add(cashRec);
                        await _context.SaveChangesAsync();
                    }

                }
                return visualizeValues;
            }
            catch (FileNotFoundException ex)
            {
                return null;
            }

        }

        [HttpGet("readDealExcel")]
        public async Task<List<Deal>> ReadExcelDeal()
        {
            string filepath = @"I:\Finance\Funds\Reporting\.Databases\Fund III\Fund III - Investments.xlsx";
            List<Deal> dealList = new List<Deal>();

            try
            {
                List<List<string>> result = excelReader.ReadExcel(filepath, "Deals");

                foreach(List<string> column in result)
                {
                    var Deal = new Deal
                    {
                        Fund = column[1] == "-" ? null : column[1], // Fund as a string
                        Deal_Id = column[2] == "-" ? 0 : (int.TryParse(column[2], out int dealId) ? dealId : 0), // Deal_Id: Investment Code
                        General_Investment_Code = column[3] == "-" ? null : column[3], // General Investment Code
                        Deal_Name = column[4], // Deal Name
                        General_Investment_Name = column[5] == "-" ? null : column[5], // General Investment Name
                        Client_Id = column[6] == "-" ? (int?)null : (int.TryParse(column[6], out int clientId) ? clientId : (int?)null), // Client
                        Investment_date = column[7] == "-" ? (DateTime?)null : (DateTime.TryParse(column[7], out DateTime investmentDate) ? investmentDate : (DateTime?)null), // Investment Date
                        Realization_Date = column[8] == "-" ? (DateTime?)null : (DateTime.TryParse(column[8], out DateTime realizationDate) ? realizationDate : (DateTime?)null), // Realization Date
                        Facility = column[9] == "-" ? (decimal?)null : (decimal.TryParse(column[9], out decimal facility) ? facility : (decimal?)null), // Facility
                        Percent_Master_Fund = column[10] == "-" ? (decimal?)null : (decimal.TryParse(column[10], out decimal percentMasterFund) ? percentMasterFund : (decimal?)null), // % Master Fund
                        Percent_Coinvestors = column[11] == "-" ? (decimal?)null : (decimal.TryParse(column[11], out decimal percentCoinvestors) ? percentCoinvestors : (decimal?)null), // % Coinvestors
                        Country_Code = column[12] == "-" ? null : column[12], // Country Code
                        Country = column[13] == "-" ? null : column[13], // Country
                        Client_Country_Code = column[14] == "-" ? null : column[14], // Client Country Code
                        Asset_Class = column[15] == "-" ? null : column[15], // Asset Class
                        Product = column[16] == "-" ? null : column[16], // Product
                        Sector = column[17], // Sector
                        Subsector = column[18] == "-" ? null : column[18], // Subsector
                        Underwriting_IRR = column[19] == "-" ? (decimal?)null : (decimal.TryParse(column[19], out decimal underwritingIRR) ? underwritingIRR : (decimal?)null), // Underwriting IRR
                        Underwriting_MOIC = column[20] == "-" ? (decimal?)null : (decimal.TryParse(column[20], out decimal underwritingMOIC) ? underwritingMOIC : (decimal?)null), // Underwriting MOIC
                        Strategy = column[21] == "-" ? null : column[21], // Strategy
                        Grouping = column[22] == "-" ? null : column[22], // Grouping
                        Loan_Type = column[23] == "-" ? null : column[23], // Loan Type
                        Seniority = column[24] == "-" ? null : column[24], // Seniority
                        Capital_Repayment = column[25] == "-" ? null : column[25], // Capital Repayment
                        Coupon = column[26] == "-" ? null : column[26], // Coupon
                        Interest_Rate = column[27] == "-" ? (decimal?)null : (decimal.TryParse(column[27], out decimal interestRate) ? interestRate : (decimal?)null), // Interest Rate
                        Thematic_vs_Opportunistic = column[28] == "-" ? null : column[28], // Thematic vs Opportunistic
                        Theme = column[29] == "-" ? null : column[29], // Theme
                        Origination = column[30] == "-" ? null : column[30], // Origination
                        Sponsorship = column[31] == "-" ? null : column[31], // Sponsorship
                        Repeat_Counterparty = column[32] == "-" ? null : column[32], // Repeat Counterparty
                        Deal_Source = column[33] == "-" ? null : column[33], // Deal Source
                        Instrument = column[34] == "-" ? null : column[34] // Instrument
                    };

                    dealList.Add(Deal);

                }

                return dealList;
            } catch (FileNotFoundException ex)
            {
                return null;
            }

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
