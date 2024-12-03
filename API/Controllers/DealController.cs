using API.Data;
using API.DTOs;
using API.Entities;
using API.Migrations;
using API.Services;
using API.Services.Interfaces;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Mysqlx.Crud;
using OfficeOpenXml.Drawing.Chart;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace API.Controllers
{
    [Route("api/[controller]")]
    public class DealController : BaseApiController
    {
        private readonly DataContext _context;
        private TransactionService transactionService;
        private ExcelReader excelReader;
        private FinancialMetrics financialMetrics;

        public DealController(DataContext context)
        {
            _context = context;
            //transactionService = new TransactionService(null, _context);
            excelReader = new ExcelReader();
        }

        // Corrected GetDeal method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deal>>> GetDeal()
        {
            // Assuming _context.Deals is your DbSet<Deal>
            return await _context.Deals.ToListAsync();
        }

        [HttpGet("DealInformation/{dealName}")]
        public async Task<ActionResult<IEnumerable<Deal>>> GetDealDetails(string dealName)
        {
            var dealDetails = await _context.Deals.Where(t => string.Equals(t.Deal_Name, dealName)).ToListAsync();

            return Ok(dealDetails);


        }

        [HttpGet("FacilityInformation/{dealName}")]
        public async Task<ActionResult<FinancialMetrics>> GetFacilityDetails(string dealName)
        {
            //var deal = await _context.Deals
            //    .Where(d => string.Equals(dealName, d.Deal_Name))
            //    .Select(d => new FacilityInformationDTO
            //    {
            //        Facility = d.Facility,
            //        UndrawnAmount = _context.Transactions
            //            .Where(t => t.Deal_Name == dealName)
            //            .OrderByDescending(t => t.Transaction_Date)
            //            .Select(t => t.Undrawn_Amount)
            //            .FirstOrDefault()
            //    }).FirstOrDefaultAsync();

            FinancialMetrics metrics = transactionService.MetricsCalculations(dealName);


            return Ok(metrics);
        }

        //[HttpGet("DealbyFunds/{fundId}")]
        //public async Task<List<Deal>> GetDealsForFund(int FundId)
        //{
        //    List<Deal> dealsFromFund = await _context.Deals.Where(t => t.Related_fund_id == FundId).OrderByDescending(t => t.Deal_Id).ToListAsync();
        //    return dealsFromFund;
        //}

        [HttpPost]
        public async Task<ActionResult<Deal>> NewDeal(Deal deal)
        {
            //if(await dealExist)

            var newDeal = new Deal
            {
                Deal_Name = deal.Deal_Name,
                //Client_Id = deal.Client_Id,
                //Facility = deal.Facility,
                //Asset_Id = deal.Asset_Id,
                ////Country = deal.Country,
                ////Sector = deal.Sector,
                ////Subsector = deal.Subsector,
                //Investment_date = deal.Investment_date,
                //Maturity_date = deal.Maturity_date,
                //Opening_fee = deal.Opening_fee,
                //Minimum_multiple = deal.Minimum_multiple,
                //Underwriting_IRR = deal.Underwriting_IRR,
                //Underwriting_MOIC = deal.Underwriting_MOIC,
                //Underwriting_NAV = deal.Underwriting_NAV,
                //Availability_period = deal.Availability_period,
                //Availability_fee = deal.Availability_fee,
                //Intercompany_loan = deal.Intercompany_loan,
                //Entity_Id = deal.Entity_Id,
                //Interest_Id = deal.Interest_Id,
                //Amortization_type = deal.Amortization_type,
                //Ownership_Id = deal.Ownership_Id,
                //LTV_Entry = deal.LTV_Entry,
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
        public async Task<List<Cash_Rec>> ReadExcelCashRec()
        {
            string filepath = @"I:\Finance\Funds\Banks\Cash rec\Cash rec.xlsx";
            List<Cash_Rec> visualizeValues = new List<Cash_Rec>();

            try
            {
                List<List<string>> result = excelReader.ReadExcel(filepath, "Cash_Rec");

                foreach (List<string> column in result.Skip(3))
                {
                    var cashRec = new Cash_Rec
                    {
                        Fund = string.IsNullOrEmpty(column[1]) ? null : column[1], // Fund, nullable
                        Type = string.IsNullOrEmpty(column[2]) ? null : column[2], // Type, nullable
                        SubType = string.IsNullOrEmpty(column[3]) ? null : column[3], // SubType, nullable
                        Counterparty = string.IsNullOrEmpty(column[4]) ? null : column[4], // Counterparty, nullable
                        Project = string.IsNullOrEmpty(column[5]) ? null : column[5], // Project, nullable
                        IncludedInLoanTemplate = string.IsNullOrEmpty(column[6]) ? null : column[6], // Included in Loan Template
                        TypeIncludedInLoanTemplate = string.IsNullOrEmpty(column[7]) ? null : column[7], // Type included in Loan Template
                        Error = string.IsNullOrEmpty(column[8]) ? null : column[8], // Error, nullable
                        ProjectExits = string.IsNullOrEmpty(column[9]) ? null : column[9], // Project Exits, nullable
                        LoanTemplate = string.IsNullOrEmpty(column[10]) ? null : column[10], // Loan Template, nullable
                        Account = string.IsNullOrEmpty(column[11]) ? null : column[11], // Account, nullable
                        AccountHolder = string.IsNullOrEmpty(column[12]) ? null : column[12], // Account Holder, nullable
                        Bank = string.IsNullOrEmpty(column[13]) ? null : column[13], // Bank, nullable
                        EntryDate = string.IsNullOrEmpty(column[14]) ? null : column[14], // Entry Date as varchar
                        ValueDate = string.IsNullOrEmpty(column[15]) ? null : column[15], // Value Date as varchar
                        TransactionAmount = string.IsNullOrEmpty(column[16]) ? null : column[16], // Transaction Amount as varchar
                        TransactionCurrency = string.IsNullOrEmpty(column[17]) ? null : column[17], // Transaction Currency
                        CounterpartyName = string.IsNullOrEmpty(column[18]) ? null : column[18], // Counterparty Name, nullable
                        TransactionMotivation = string.IsNullOrEmpty(column[19]) ? null : column[19], // Text field
                        Comments = string.IsNullOrEmpty(column[20]) ? null : column[20] // Text field
                    };

                    visualizeValues.Add(cashRec);
                    
                    // Save each cashRec to the database
                    _context.Cash_Rec.Add(cashRec);
                    await _context.SaveChangesAsync();
                }



                return visualizeValues;
            }
            catch (FileNotFoundException ex)
            {
                return null;
            }

        }

        [HttpGet("readDealLoanTemplateExcel")]
        public async Task<List<Deal>> ReadExcelDealLoanTemplate()
        {
            string filepath = @"C:\Users\mkhanna\Documents\Fund IV - Loan Template Database Macro.xlsm";
            List<Deal> dealList = new List<Deal>();

            try
            {
                List<List<string>> result = excelReader.ReadExcel(filepath, "Loan Data Table Fund IV");

                foreach (List<string> column in result.Skip(3))
                {
                    var createdDeal = new Deal
                    {
                        //Deal_Id = Code column 1,
                        //Deal_Name = Loan column 3,
                        //Instrument_LoanTemplateddbb =Instrument 4,
                        //Initial_Date =  initialDate 5,
                        //Maturity_date = matcu 6,
                        //Extension_Period = Extension 7,
                        //Facility = fac 8,
                        //First_Utilization = 9,
                        //Purchase_Discount_Rate = 10,
                        //Opening_fee = 11,
                        //Exit_fee = 12,
                        //Minimum_Multiple = 13,
                        //EURIBOR_Interest_Rate = 14,
                        //CashInterest_Rate_1st   = 15,
                        //CashInterest_End_1st = 16,
                        //CashInterest_Rate_2nd = 17,
                        //CashInterest_End_2nd = 18,
                        //CashInterest_Rate_3rd = 19,
                        //CashInterest_End_3rd = 20,
                        //PIKInterest_Rate_1st =22,
                        //PIKInterest_End_1st = 23,
                        //PIKInterest_Rate_2nd =24,
                        //PIKInterest_End_2nd =25,
                        //PIKInterest_Rate_3rd =26,
                        //PIKInterest_End_3rd =27,
                        //PIYCInterest_Rate_1st =29,
                        //PIYCInterest_End_1st =30,
                        //PIYCInterest_Rate_2nd =31,
                        //PIYCInterest_End_2nd = 32,
                        //PIYCInterest_Rate_3rd = 33,
                        //PIYCInterest_End_3rd= 34,
                        //Year_Base =36,
                        //Interest_Period =37,
                        //Default_interest_rate =38,
                        //Undrawn_fee =39,
                        //Availability_period =40,
                        //Default_Capitalization_Periods =41,
                        //Acceleration_Date =42,
                        //Share_Premium = 43,
                        //Lender = 44,
                        //Borrower =45,

                        Related_Fund = "Fund IV",
                        Deal_Id = column[1] == "-" ? null : column[1], // Deal_Id: Code
                        Deal_Name = column[3] == "-" ? null : column[3], // Deal_Name: Loan
                        Instrument_LoanTemplateddbb = column[4] == "-" ? null : column[4], // Instrument_LoanTemplateddbb: Instrument
                        Investment_date = column[5] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[5], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime initialDate) ? initialDate : (DateTime?)null), // Initial Date
                        Maturity_date = column[6] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[6], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime maturityDate) ? maturityDate : (DateTime?)null), // Maturity Date
                        Extension_Period = column[7] == "-" ? null : column[7], // Extension Period
                        Facility = (column[8] == "-" || string.IsNullOrWhiteSpace(column[8])) ? 0 : (decimal.TryParse(column[8], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal facilityValue) ? facilityValue : 0), // Facility
                        First_Utilization = (column[9] == "-" || string.IsNullOrWhiteSpace(column[9])) ? 0 : (decimal.TryParse(column[9], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal firstUtilizationValue) ? firstUtilizationValue : 0), // First Utilization
                        Purchase_Discount_Rate = (column[10] == "-" || string.IsNullOrWhiteSpace(column[10])) ? 0 : (decimal.TryParse(column[10], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal purchaseDiscountRateValue) ? purchaseDiscountRateValue * 100 : 0), // Purchase Discount Rate (multiply by 100)
                        Opening_fee = (column[11] == "-" || string.IsNullOrWhiteSpace(column[11])) ? 0 : (decimal.TryParse(column[11], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal openingFeeValue) ? openingFeeValue * 100 : 0), // Opening Fee (multiply by 100)
                        Exit_fee = (column[12] == "-" || string.IsNullOrWhiteSpace(column[12])) ? 0 : (decimal.TryParse(column[12], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal exitFeeValue) ? exitFeeValue * 100 : 0), // Exit Fee (multiply by 100)
                        Minimum_Multiple = (column[13] == "-" || string.IsNullOrWhiteSpace(column[13])) ? 0 : (decimal.TryParse(column[13], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal minimumMultipleValue) ? minimumMultipleValue * 100 : 0), // Minimum Multiple (multiply by 100)
                        EURIBOR_Interest_Rate = column[14] == "-" ? null : column[14], // EURIBOR Interest Rate
                        CashInterest_Rate_1st = (column[15] == "-" || string.IsNullOrWhiteSpace(column[15])) ? 0 : (decimal.TryParse(column[15], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal cashInterestRate1stValue) ? cashInterestRate1stValue * 100 : 0), // Cash Interest Rate 1st (multiply by 100)
                        CashInterest_End_1st = column[16] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[16], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime cashInterestEnd1stDate) ? cashInterestEnd1stDate : (DateTime?)null), // Cash Interest End 1st
                        CashInterest_Rate_2nd = (column[17] == "-" || string.IsNullOrWhiteSpace(column[17])) ? 0 : (decimal.TryParse(column[17], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal cashInterestRate2ndValue) ? cashInterestRate2ndValue * 100 : 0), // Cash Interest Rate 2nd (multiply by 100)
                        CashInterest_End_2nd = column[18] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[18], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime cashInterestEnd2ndDate) ? cashInterestEnd2ndDate : (DateTime?)null), // Cash Interest End 2nd
                        CashInterest_Rate_3rd = (column[19] == "-" || string.IsNullOrWhiteSpace(column[19])) ? 0 : (decimal.TryParse(column[19], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal cashInterestRate3rdValue) ? cashInterestRate3rdValue * 100 : 0), // Cash Interest Rate 3rd (multiply by 100)
                        CashInterest_End_3rd = column[20] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[20], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime cashInterestEnd3rdDate) ? cashInterestEnd3rdDate : (DateTime?)null), // Cash Interest End 3rd
                        PIKInterest_Rate_1st = (column[22] == "-" || string.IsNullOrWhiteSpace(column[22])) ? 0 : (decimal.TryParse(column[22], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal pikInterestRate1stValue) ? pikInterestRate1stValue * 100 : 0), // PIK Interest Rate 1st (multiply by 100)
                        PIKInterest_End_1st = column[23] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[23], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime pikInterestEnd1stDate) ? pikInterestEnd1stDate : (DateTime?)null), // PIK Interest End 1st
                        PIKInterest_Rate_2nd = (column[24] == "-" || string.IsNullOrWhiteSpace(column[24])) ? 0 : (decimal.TryParse(column[24], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal pikInterestRate2ndValue) ? pikInterestRate2ndValue * 100 : 0), // PIK Interest Rate 2nd (multiply by 100)
                        PIKInterest_End_2nd = column[25] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[25], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime pikInterestEnd2ndDate) ? pikInterestEnd2ndDate : (DateTime?)null), // PIK Interest End 2nd
                        PIKInterest_Rate_3rd = (column[26] == "-" || string.IsNullOrWhiteSpace(column[26])) ? 0 : (decimal.TryParse(column[26], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal pikInterestRate3rdValue) ? pikInterestRate3rdValue * 100 : 0), // PIK Interest Rate 3rd (multiply by 100)
                        PIKInterest_End_3rd = column[27] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[27], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime pikInterestEnd3rdDate) ? pikInterestEnd3rdDate : (DateTime?)null), // PIK Interest End 3rd
                        PIYCInterest_Rate_1st = (column[29] == "-" || string.IsNullOrWhiteSpace(column[29])) ? 0 : (decimal.TryParse(column[29], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal piycInterestRate1stValue) ? piycInterestRate1stValue * 100 : 0), // PIYC Interest Rate 1st (multiply by 100)
                        PIYCInterest_End_1st = column[30] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[30], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime piycInterestEnd1stDate) ? piycInterestEnd1stDate : (DateTime?)null), // PIYC Interest End 1st
                        PIYCInterest_Rate_2nd = (column[31] == "-" || string.IsNullOrWhiteSpace(column[31])) ? 0 : (decimal.TryParse(column[31], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal piycInterestRate2ndValue) ? piycInterestRate2ndValue * 100 : 0), // PIYC Interest Rate 2nd (multiply by 100)
                        PIYCInterest_End_2nd = column[32] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[32], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime piycInterestEnd2ndDate) ? piycInterestEnd2ndDate : (DateTime?)null), // PIYC Interest End 2nd
                        PIYCInterest_Rate_3rd = (column[33] == "-" || string.IsNullOrWhiteSpace(column[33])) ? 0 : (decimal.TryParse(column[33], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal piycInterestRate3rdValue) ? piycInterestRate3rdValue * 100 : 0), // PIYC Interest Rate 3rd (multiply by 100)
                        PIYCInterest_End_3rd = column[34] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[34], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime piycInterestEnd3rdDate) ? piycInterestEnd3rdDate : (DateTime?)null), // PIYC Interest End 3rd
                        Year_Base = (column[36] == "-" || string.IsNullOrWhiteSpace(column[36])) ? 0 : (decimal.TryParse(column[36], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal yearBaseValue) ? yearBaseValue : 0), // Year Base
                        Interest_Period = column[37] == "-" ? null : column[37], // Interest Period
                        Default_interest_rate = (column[38] == "-" || string.IsNullOrWhiteSpace(column[38])) ? 0 : (decimal.TryParse(column[38], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal defaultInterestRateValue) ? defaultInterestRateValue * 100 : 0), // Default Interest Rate (multiply by 100)
                        Undrawn_fee = (column[39] == "-" || string.IsNullOrWhiteSpace(column[39])) ? 0 : (decimal.TryParse(column[39], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal undrawnFeeValue) ? undrawnFeeValue * 100 : 0), // Undrawn Fee (multiply by 100)
                        Availability_period = (column[40] == "-" || string.IsNullOrWhiteSpace(column[40])) ? 0 : int.Parse(column[40]), // Availability Period
                        Default_Capitalization_Periods = column[42] == "-" ? null : column[42], // Default Capitalization Periods
                        Acceleration_Date = column[43] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[43], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime accelerationDate) ? accelerationDate : (DateTime?)null), // Acceleration Date
                        Share_Premium = (column[44] == "-" || string.IsNullOrWhiteSpace(column[44])) ? 0 : (decimal.TryParse(column[44], NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal sharePremiumValue) ? sharePremiumValue * 100 : 0), // Share Premium (multiply by 100)
                        Lender = column[45] == "-" ? null : column[45], // Lender
                        Borrower = column[46] == "-" ? null : column[46] // Borrower





                        //Fund = column[1] == "-" ? null : column[1], // Fund as a string
                        //Deal_Id = column[2] == "-" ? null : column[2], // Deal_Id: Investment Code
                        //General_Investment_Code = column[3] == "-" ? null : column[3], // General Investment Code
                        //Deal_Name = column[4] == "-" ? null : column[4], // Deal Name
                        //General_Investment_Name = column[5] == "-" ? null : column[5], // General Investment Name
                        //Client = column[6] == "-" ? null : column[6], //
                        //Investment_date = column[7] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[7], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime investment_date) ? investment_date : (DateTime?)null),
                        //Realization_Date = column[8] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[8], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime date) ? date : (DateTime?)null), // Realization Date
                        //Facility = (column[9] == "-" || string.IsNullOrWhiteSpace(column[9])) ? 0 : (decimal.TryParse(column[9].Trim(), NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal facilityValue) ? facilityValue : 0),
                        //Percent_Master_Fund = column[10] == "-" ? (decimal?)null : (decimal.TryParse(column[10], out decimal percentMasterFund) ? percentMasterFund * 100 : (decimal?)null),
                        //Percent_Coinvestors = column[11] == "-" ? (decimal?)null : (decimal.TryParse(column[11], out decimal percentCoinvestors) ? percentCoinvestors * 100 : (decimal?)null), // % Coinvestors
                        //Country_Code = column[12] == "-" ? null : column[12], // Country Code
                        //Country = column[13] == "-" ? null : column[13], // Country
                        //Client_Country_Code = column[14] == "-" ? null : column[14], // Client Country Code
                        //Asset_Class = column[15] == "-" ? null : column[15], // Asset Class
                        //Product = column[16] == "-" ? null : column[16], // Product
                        //Sector = column[17], // Sector
                        //Subsector = column[18] == "-" ? null : column[18], // Subsector
                        //Underwriting_IRR = column[19] == "-" ? (decimal?)null : (decimal.TryParse(column[19], out decimal underwritingIRR) ? underwritingIRR * 100 : (decimal?)null),
                        //Underwriting_MOIC = column[20] == "-" ? (decimal?)null : (decimal.TryParse(column[20], out decimal underwritingMOIC) ? underwritingMOIC : (decimal?)null), // Underwriting MOIC
                        //Strategy = column[21] == "-" ? null : column[21], // Strategy
                        //Deal_Grouping = column[22] == "-" ? null : column[22], // Grouping
                        //Loan_Type = column[23] == "-" ? null : column[23], // Loan Type
                        //Seniority = column[24] == "-" ? null : column[24], // Seniority
                        //Capital_Repayment = column[25] == "-" ? null : column[25], // Capital Repayment
                        //Coupon = column[26] == "-" ? null : column[26], // Coupon
                        //Interest_Rate = column[27] == "-" ? null : column[27], // Interest Rate
                        //Thematic_vs_Opportunistic = column[28] == "-" ? null : column[28], // Thematic vs Opportunistic
                        //Theme = column[29] == "-" ? null : column[29], // Theme
                        //Origination = column[30] == "-" ? null : column[30], // Origination
                        //Sponsorship = column[31] == "-" ? null : column[31], // Sponsorship
                        //Repeat_Counterparty = column[32] == "-" ? null : column[32], // Repeat Counterparty
                        //Deal_Source = column[33] == "-" ? null : column[33], // Deal Source
                        //Instrument = column[34] == "-" ? null : column[34] // Instrument,
                    };

                    dealList.Add(createdDeal);

                    _context.Deals.Add(createdDeal);
                    await _context.SaveChangesAsync();
                }

                return dealList;
            }
            catch (FileNotFoundException ex)
            {
                return null;
            }

        }

        [HttpGet("readDealExcel")]
        public async Task<List<Deal>> ReadExcelDeal()
        {
            string filepath = @"I:\Finance\Funds\Reporting\.Databases\Fund IV\Fund IV - Investments.xlsx";
            List<Deal> dealList = new List<Deal>();

            try
            {
                List<List<string>> result = excelReader.ReadExcel(filepath, "Deals");

                foreach (List<string> column in result.Skip(0))
                {

                   string dealName = column[4] == "-" ? null : column[4];
                   Deal existingDeal = _context.Deals.Where(t => string.Equals(t.Deal_Name, dealName)).FirstOrDefault();

                    if (existingDeal != null)
                    {
                        // Update fields of the existing deal
                        existingDeal.Percent_Master_Fund = column[10] == "-" ? (decimal?)null : (decimal.TryParse(column[10], out decimal percentMasterFund) ? percentMasterFund * 100 : (decimal?)null);
                        existingDeal.Percent_Coinvestor = column[11] == "-" ? (decimal?)null : (decimal.TryParse(column[11], out decimal percentCoinvestors) ? percentCoinvestors * 100 : (decimal?)null); // % Coinvestors
                        existingDeal.Country_Code = column[12] == "-" ? null : column[12]; // Country Code
                        existingDeal.Country = column[13] == "-" ? null : column[13]; // Country
                        existingDeal.Client_Country_Code = column[14] == "-" ? null : column[14]; // Client Country Code
                        existingDeal.Asset_Class = column[15] == "-" ? null : column[15]; // Asset Class
                        existingDeal.Product = column[16] == "-" ? null : column[16]; // Product
                        existingDeal.Sector = column[17]; // Sector
                        existingDeal.Subsector = column[18] == "-" ? null : column[18]; // Subsector
                        existingDeal.Underwriting_IRR = column[19] == "-" ? (decimal?)null : (decimal.TryParse(column[19], out decimal underwritingIRR) ? underwritingIRR * 100 : (decimal?)null);
                        existingDeal.Underwriting_MOIC = column[20] == "-" ? (decimal?)null : (decimal.TryParse(column[20], out decimal underwritingMOIC) ? underwritingMOIC : (decimal?)null); // Underwriting MOIC
                        existingDeal.Instrument_Dealddbbb = column[21] == "-" ? null : column[21];
                        existingDeal.Grouping = column[22] == "-" ? null : column[22]; 
                        existingDeal.Loan_Type = column[23] == "-" ? null : column[23]; // Loan Type
                        existingDeal.Capital_Repayment = column[25] == "-" ? null : column[25]; // Capital Repayment
                        existingDeal.Interest_Rate_Type = column[27] == "-" ? null : column[27];
                        existingDeal.Thematic_Vs_Opportunistic = column[28] == "-" ? null : column[28]; // Thematic vs Opportunistic
                        existingDeal.Theme = column[29] == "-" ? null : column[29]; // Theme
                        existingDeal.Origination = column[30] == "-" ? null : column[30]; // Origination
                        existingDeal.Sponsorship = column[31] == "-" ? null : column[31]; // Sponsorship
                        existingDeal.Repeat_Counterparty = column[32] == "-" ? null : column[32]; // Repeat Counterparty
                        existingDeal.Deal_Source = column[33] == "-" ? null : column[33]; // Deal Source
                        existingDeal.Strategy = column[34] == "-" ? null : column[34]; // Strategy
                    }

                        ////var createdDeal = new Deal
                        //{
                        //    //Fund = column[1] == "-" ? null : column[1], // Fund as a string
                        //    //Deal_Id = column[2] == "-" ? null : column[2], // Deal_Id: Investment Code
                        //    //Deal_Name = column[4] == "-" ? null : column[4], // Deal Name
                        //    //General_Investment_Name = column[5] == "-" ? null : column[5], // General Investment Name
                        //    //Client = column[6] == "-" ? null : column[6], //
                        //    //Investment_date = column[7] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[7], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime investment_date) ? investment_date : (DateTime?)null),
                        //    //Realization_Date = column[8] == "-" ? (DateTime?)null : (DateTime.TryParseExact(column[8], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime date) ? date : (DateTime?)null), // Realization Date
                        //    //Facility = (column[9] == "-" || string.IsNullOrWhiteSpace(column[9])) ? 0 : (decimal.TryParse(column[9].Trim(), NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal facilityValue) ? facilityValue : 0),
                        //    Percent_Master_Fund = column[10] == "-" ? (decimal?)null : (decimal.TryParse(column[10], out decimal percentMasterFund) ? percentMasterFund * 100 : (decimal?)null),
                        //    Percent_Coinvestor = column[11] == "-" ? (decimal?)null : (decimal.TryParse(column[11], out decimal percentCoinvestors) ? percentCoinvestors * 100 : (decimal?)null), // % Coinvestors
                        //    Country_Code = column[12] == "-" ? null : column[12], // Country Code
                        //    Country = column[13] == "-" ? null : column[13], // Country
                        //    Client_Country_Code = column[14] == "-" ? null : column[14], // Client Country Code
                        //    Asset_Class = column[15] == "-" ? null : column[15], // Asset Class
                        //    Product = column[16] == "-" ? null : column[16], // Product
                        //    Sector = column[17], // Sector
                        //    Subsector = column[18] == "-" ? null : column[18], // Subsector
                        //    Underwriting_IRR = column[19] == "-" ? (decimal?)null : (decimal.TryParse(column[19], out decimal underwritingIRR) ? underwritingIRR * 100 : (decimal?)null),
                        //    Underwriting_MOIC = column[20] == "-" ? (decimal?)null : (decimal.TryParse(column[20], out decimal underwritingMOIC) ? underwritingMOIC : (decimal?)null), // Underwriting MOIC
                        //    Instrument_Dealddbbb = column[21] == "-" ? null : column[21],
                        //    Grouping = column[22] == "-" ? null : column[22],
                        //    Loan_Type = column[23] == "-" ? null : column[23], // Loan Type
                        //    Capital_Repayment = column[25] == "-" ? null : column[25], // Capital Repayment
                        //    Interest_Rate_Type = column[27] == "-" ? null : column[27],
                        //    Thematic_Vs_Opportunistic = column[28] == "-" ? null : column[28], // Thematic vs Opportunistic
                        //    Theme = column[29] == "-" ? null : column[29], // Theme
                        //    Origination = column[30] == "-" ? null : column[30], // Origination
                        //    Sponsorship = column[31] == "-" ? null : column[31], // Sponsorship
                        //    Repeat_Counterparty = column[32] == "-" ? null : column[32], // Repeat Counterparty
                        //    Deal_Source = column[33] == "-" ? null : column[33], // Deal Source
                        //    Strategy = column[34] == "-" ? null : column[34], // Strategy
                        //};

                        //dealList.Add(createdDeal);

                    //_context.SaveChanges(;
                    await _context.SaveChangesAsync();
                }

                return dealList;
            }
            catch (FileNotFoundException ex)
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
