using System.Globalization;
using Promrub.Services.API.Handlers;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Promrub.Services.API.Controllers.v1;

[ApiController]
[Authorize(Policy = "GenericRolePolicy")]
[Route("v{version:apiVersion}/api/[controller]")]
[ApiVersion("1")]
public class ReceivePaymentController : BaseController
{
    private readonly IReceivePaymentService _service;

    public ReceivePaymentController(IReceivePaymentService service)
    {
        _service = service;
    }


    [HttpGet]
    [Route("org/{id}")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> GetReceive(string id,
        [FromQuery] GetReceiveQuery request)
    {
        try
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                throw new ArgumentException("1101");
            var result = await _service.GetReceiveList(request.Keyword, request.PosId, request.StartDate,
                request.EndDate, request.Payer, request.Page,
                request.PageSize);
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
    public async Task<IActionResult> GetReceiveByDate(string id,
        [FromQuery] GetReceiveQuery request)
    {
        try
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                throw new ArgumentException("1101");
            var result = await _service.GetReceiveByDate(request.Keyword, request.PosId, request.StartDate,
                request.EndDate, request.Payer, request.Page,
                request.PageSize);
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
    public async Task<IActionResult> GenerateDateTime(string id,
        [FromBody] DateTimeQuery request)
    {
        try
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id) )
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

    public record DateTimeQuery(string DateTime);

    public record GetReceiveQuery(string? Keyword, string? StartDate, string? EndDate, string? PosId, Guid? Payer,
        int Page = 1, int PageSize = 10);
}