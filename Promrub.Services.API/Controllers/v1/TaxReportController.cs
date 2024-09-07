using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Interfaces;

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
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                throw new ArgumentException("1101");
            var result = await _service.GetTax(request.Keyword, request.PosId, request.StartDate, request.EndDate,
                request.Payer, request.Page, request.PageSize);
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
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                throw new ArgumentException("1101");
            var result = await _service.GetTaxByDate(request.Keyword, request.PosId, request.StartDate, request.EndDate,
                request.Payer, request.Page, request.PageSize);
            return Ok(ResponseHandler.Response("1000", null, result));
        }
        catch (Exception ex)
        {
            return Ok(ResponseHandler.Response(ex.Message, null));
        }
    }
    
    [HttpPost]
    [Route("org/{id}/generate_datetime")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> Generate(string id,
        [FromBody] ReportDateTimeQuery request)
    {
        try
        {
            var key = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
                throw new ArgumentException("1101");
            var datetime = DateTime.ParseExact(request.DateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            await _service.GenerateSchedule(datetime);
            return Ok(ResponseHandler.Response("1000", null));
        }
        catch (Exception ex)
        {
            return Ok(ResponseHandler.Response(ex.Message, null));
        }
    }

    public record ReportDateTimeQuery(string DateTime);

    public record GetTaxQuery(string? Keyword, string? StartDate, string? EndDate, string? PosId, Guid? Payer,
        int Page = 1, int PageSize = 10);
}