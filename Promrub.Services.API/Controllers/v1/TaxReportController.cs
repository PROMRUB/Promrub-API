using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Models.ResponseModels.Payment;
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
            var result = await _service.GetTax();
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
            var result = await _service.GetTaxByDate();
            return Ok(ResponseHandler.Response("1000", null, result));
        }
        catch (Exception ex)
        {
            return Ok(ResponseHandler.Response(ex.Message, null));
        }
    }
    
    public record GetTaxQuery(string? Keyword,string? StartDate,string? EndDate,string? Payer);

}

public interface ITaxReportService
{
    public Task<PagedList<TaxResponse>> GetTax();
    public Task<PagedList<TaxResponse>> GetTaxByDate();
}

public class TaxReportService : ITaxReportService
{
    public async Task<PagedList<TaxResponse>> GetTax()
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

    public async Task<PagedList<TaxResponse>> GetTaxByDate()
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

