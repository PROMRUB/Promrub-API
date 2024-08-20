using Promrub.Services.API.Handlers;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Utils;

namespace Promrub.Services.API.Controllers.v1;

[ApiController]
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
            var key = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
                throw new ArgumentException("1101");
            var result = await _service.GetReceiveList();
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
            var key = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
                throw new ArgumentException("1101");
            var result = await _service.GetReceiveByDate();
            return Ok(ResponseHandler.Response("1000", null, result));
        }
        catch (Exception ex)
        {
            return Ok(ResponseHandler.Response(ex.Message, null));
        }
    }

    public record GetReceiveQuery(string? Keyword, string? StartDate, string? EndDate, string? Payer);
}

public interface IReceivePaymentService
{
    public Task<PagedList<ReceivePaymentResponse>> GetReceiveList();
    public Task<PagedList<ReceivePaymentByDateResponse>> GetReceiveByDate();
}

public class ReceivePaymentService : IReceivePaymentService
{
    public async Task<PagedList<ReceivePaymentResponse>> GetReceiveList()
    {
        var list = new List<ReceivePaymentResponse>()
        {
            new ReceivePaymentResponse
            {
                PosId = "POS_ID_123456789",
                PaymentDateTime = "DD/MM/YYYY",
                ReceiveNo = "RCPYYYMMDD_9999ABBTX",
                Amount = 0,
                PaidBy = "CSH",
                InvoiceNo = "INVYYYYMMDD_99999"
            },
            new ReceivePaymentResponse
            {
                PosId = "POS_ID_123456789",
                PaymentDateTime = "DD/MM/YYYY",
                ReceiveNo = "RCPYYYMMDD_9999ABBTX",
                Amount = 0,
                PaidBy = "CSH",
                InvoiceNo = "INVYYYYMMDD_99999"
            },
        };
        return new PagedList<ReceivePaymentResponse>(list, 2, 1, 10);
    }

    public async Task<PagedList<ReceivePaymentByDateResponse>> GetReceiveByDate()
    {
        var list = new List<ReceivePaymentByDateResponse>()
        {
            new ReceivePaymentByDateResponse
            {
                PosId = "POS_ID_123456789",
                PaymentDateTime = "DD/MM/YYYY",
                ReceiveNo = "RCPYYYMMDD_9999ABBTX",
                Amount = (decimal)107.00,
                PaidBy = "CSH"
            },
            new ReceivePaymentByDateResponse
            {
                PosId = "POS_ID_123456789",
                PaymentDateTime = "DD/MM/YYYY",
                ReceiveNo = "RCPYYYMMDD_9999ABBTX",
                Amount = (decimal)107.00,
                PaidBy = "CSH"
            }
        };
        return new PagedList<ReceivePaymentByDateResponse>(list, 2, 1, 10);
    }
}

public class ReceivePaymentByDateResponse
{
    public string PosId { get; set; }
    public string PaymentDateTime { get; set; }
    public decimal Amount { get; set; }
    public string PaidBy { get; set; }
    public string ReceiveNo { get; set; }
}

public class ReceivePaymentResponse
{
    public Guid Id { get; set; }
    public string PosId { get; set; }
    public string PaymentDateTime { get; set; }
    public string ReceiveNo { get; set; }
    public decimal Amount { get; set; }
    public string PaidBy { get; set; }
    public string InvoiceNo { get; set; }
}