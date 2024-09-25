using API.Controllers;
using API.Data;
using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
    [Route("api/[controller]")]
    public class FinancialMetricsController: BaseApiController
    {

    private readonly DataContext _context;
    private readonly ILogger<FinancialMetricsController> _logger;
    private FinancialMetricsController financialMetricsController;
    private TransactionService _transactionService;

    public FinancialMetricsController(DataContext context,ILogger<FinancialMetricsController> logger, TransactionService transactionService)
    {
        _context = context;
        _logger = logger;
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FinancialMetrics>>> GetMetrics()
    {
        return await _context.FinancialMetrics.ToListAsync();
    }

    /**DIFFERNT GET METHODS -- to be able to access the information more easily when making tables in the deal screen**/
        //UNDERWRITING NAV VALUES & ACTUAL NAV Values
    [HttpGet("NAV/{dealId}")]
    public async Task<ActionResult<FinancialMetrics>> GetNAVValues(string dealID)
    {
        FinancialMetrics metrics = await _transactionService.MetricsCalculations(dealID);

        return metrics;

    }




    //RETURNS - TOTAL DEBT, PRINCIPAL, ACCRUED CASH INTEREST, ACCRUED PIK INTEREST, ACCRUED UNDRAWN INTEREST


    //INTEREST GENERATED, INTEREST PAYED


    //FACILITY, UNDRAWN AMOUNT




}

