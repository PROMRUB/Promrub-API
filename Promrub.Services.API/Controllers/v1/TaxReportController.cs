using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Models.ResponseModels.Payment;
using Promrub.Services.API.PromServiceDbContext;
using Promrub.Services.API.Repositories;
using Promrub.Services.API.Utils;

namespace Promrub.Services.API.Controllers.v1;


[ApiController]
[Route("v{version:apiVersion}/api/[controller]")]
[ApiVersion("1")]
public class TaxReportController : BaseController
{
    private readonly ITaxReportService _service;

    public TaxReportController(ITaxReportService service)
    {
        _service = service;
    }
    
    [HttpGet]
    [Route("org/{id}")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> GetTax(string id,
        [FromQuery] GetTaxQuery request)
    {
        try
        {
            var key = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
                throw new ArgumentException("1101");
            var result = await _service.GetTax(request.Keyword,request.PosId,request.Payer,request.Page,request.PageSize);
            return Ok(ResponseHandler.Response("1000", null, result));
        }
        catch (Exception ex)
        {
            return Ok(ResponseHandler.Response(ex.Message, null));
        }
    }
    
    [HttpGet]
    [Route("org/{id}/by_date")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> GetTaxByDate(string id,
        [FromQuery] GetTaxQuery request)
    {
        try
        {
            var key = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
                throw new ArgumentException("1101");
            var result = await _service.GetTaxByDate(request.Keyword,request.PosId,request.Payer,request.Page,request.PageSize);
            return Ok(ResponseHandler.Response("1000", null, result));
        }
        catch (Exception ex)
        {
            return Ok(ResponseHandler.Response(ex.Message, null));
        }
    }
    
    public record GetTaxQuery(string? Keyword,string? StartDate,string? EndDate,Guid? PosId,Guid? Payer,int Page = 1,int PageSize = 10);

}


public interface ITaxRepository
{
    public IQueryable<TaxScheduleEntity> GetTaxScheduleQuery();
    public void Add(TaxScheduleEntity entity);
    public DbContext? Context();
}

public class TaxRepository(PromrubDbContext context) : BaseRepository, ITaxRepository
{
    public IQueryable<TaxScheduleEntity> GetTaxScheduleQuery()
    {
        return context.Tax.AsQueryable();
    }

    public void Add(TaxScheduleEntity entity)
    {
        context.Add(entity);
    }

    public DbContext? Context()
    {
        return this.context;
    }
}

public interface ITaxReportService
{
    public Task<PagedList<TaxResponse>> GetTax(string keyword, Guid? posId, Guid? paymentChannel, int page,
        int pageSize);
    public Task<PagedList<TaxResponse>> GetTaxByDate(string keyword, Guid? posId, Guid? paymentChannel, int page,
        int pageSize);
    public Task Add(TaxScheduleEntity entity);
}

public class TaxReportService : ITaxReportService
{
    public async Task<PagedList<TaxResponse>> GetTax(string keyword, Guid? posId, Guid? paymentChannel, int page, int pageSize)
    {
        var list = new List<TaxResponse>()
        {
            new TaxResponse
            {
                PosId = "POS_ID_123456789",
                TaxReceive = "RCPYYYMMDD_9999ABBTX",
                TaxDateTime = "DD/MM/YYYY",
                Price = (decimal)100.00,
                Vat = (decimal)7.00,
                Amount = (decimal)107.00
            },
            new TaxResponse
            {
                PosId = "POS_ID_123456789",
                TaxReceive = "RCPYYYMMDD_9999ABBTX",
                TaxDateTime = "DD/MM/YYYY",
                Price = (decimal)100.00,
                Vat = (decimal)7.00,
                Amount = (decimal)107.00
            },
        };
        return new PagedList<TaxResponse>(list, 2, 1, 10);
    }

    public async Task<PagedList<TaxResponse>> GetTaxByDate(string keyword, Guid? posId, Guid? paymentChannel, int page, int pageSize)
    {
        var list = new List<TaxResponse>()
        {
            new TaxResponse
            {
                PosId = "POS_ID_123456789",
                TaxReceive = "RCPYYYMMDD_9999ABBTX",
                TaxDateTime = "DD/MM/YYYY",
                Price = (decimal)100.00,
                Vat = (decimal)7.00,
                Amount = (decimal)107.00
            },
            new TaxResponse
            {
                PosId = "POS_ID_123456789",
                TaxReceive = "RCPYYYMMDD_9999ABBTX",
                TaxDateTime = "DD/MM/YYYY",
                Price = (decimal)100.00,
                Vat = (decimal)7.00,
                Amount = (decimal)107.00
            },
        };
        return new PagedList<TaxResponse>(list, 2, 1, 10);
    }
    
    public Task Add(TaxScheduleEntity entity)
    {
        throw new NotImplementedException();
    }
    
    private TaxResponse Generate(PaymentTransactionEntity transactionEntity)
    {
        var beforeVat = transactionEntity.TotalTransactionPrices;
        var amount = 0;
        var tax = 0;
        var total = 0;
        return new TaxResponse();
    }
}

public class TaxResponse
{
    public string PosId { get; set; }
    public string TaxReceive { get; set; }
    public string TaxDateTime { get; set; }
    public decimal Price { get; set; }
    public decimal Vat { get; set; }
    public decimal Amount { get; set; }
    
}

