using System.Data.Entity;
using System.Globalization;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.PromServiceDbContext;
using Promrub.Services.API.Repositories;
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
            var result = await _service.GetReceiveList(request.Keyword, request.PosId, request.Payer, request.Page,
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
            var key = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
                throw new ArgumentException("1101");
            var result = await _service.GetReceiveByDate(request.Keyword, request.PosId, request.Payer, request.Page,
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

    public record DateTimeQuery(string DateTime);

    public record GetReceiveQuery(string? Keyword, string? StartDate, string? EndDate, string? PosId, Guid? Payer,
        int Page = 1, int PageSize = 10);
}

public interface IReceiptRepository
{
    public IQueryable<ReceiptScheduleEntity> GetReceiptScheduleQuery();
    public void Add(ReceiptScheduleEntity entity);
    public void AddRange(List<ReceiptScheduleEntity> entity);

    public PromrubDbContext Context();
}

public class ReceiptScheduleRepository : BaseRepository, IReceiptRepository
{
    private readonly PromrubDbContext _context;
    public ReceiptScheduleRepository(PromrubDbContext context)
    {
        this._context = context;
    }

    public IQueryable<ReceiptScheduleEntity> GetReceiptScheduleQuery()
    {
        return _context.ReceiptSchedule.AsQueryable();
    }

    public void Add(ReceiptScheduleEntity entity)
    {
        _context.Add(entity);
    }

    public void AddRange(List<ReceiptScheduleEntity> entity)
    {
        _context.AddRange(entity);
    }

    public PromrubDbContext Context()
    {
        return _context;
    }
}

public interface IReceivePaymentService
{
    public Task<PagedList<ReceivePaymentResponse>> GetReceiveList(string keyword, string? posId, Guid? paymentChannel,
        int page,
        int pageSize);

    public Task<PagedList<ReceivePaymentByDateResponse>> GetReceiveByDate(string keyword, string? posId,
        Guid? paymentChannel, int page,
        int pageSize);

    public Task GenerateSchedule(DateTime dateTime);
}

public class ReceivePaymentService : IReceivePaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IReceiptRepository _receiptRepository;

    public ReceivePaymentService(IPaymentRepository paymentRepository, IReceiptRepository receiptRepository)
    {
        _paymentRepository = paymentRepository;
        _receiptRepository = receiptRepository;
    }

    public async Task<PagedList<ReceivePaymentResponse>> GetReceiveList(string keyword, string posId,
        Guid? paymentChannel, int page,
        int pageSize)
    {
        var query = _paymentRepository.GetQuery()
            .Where(x => x.PaymentStatus == 3
                        && (string.IsNullOrEmpty(posId) || x.PosId == posId)
                        && (string.IsNullOrEmpty(keyword) || x.ReceiptNo.Contains(keyword))
            )
            .OrderByDescending(x => x.CreateAt);

        var sum = query.Sum(x => x.TotalTransactionPrices);

        var list = await query.Select(x => new ReceivePaymentResponse()
        {
            PosId = x.PosId,
            PaymentDateTime = x.CreateAt.Value.ToString("dd/MM/yyyy"),
            ReceiveNo = x.ReceiptNo,
            Amount = x.TotalTransactionPrices,
            PaidBy = "CSH",
            InvoiceNo = "-"
        }).ToListAsync();


        var paged = new PagedList<ReceivePaymentResponse>(list, query.Count(), page, pageSize);
        paged.TotalSummary = sum;
        return paged;
    }


    public async Task<PagedList<ReceivePaymentByDateResponse>> GetReceiveByDate(string keyword, string? posId,
        Guid? paymentChannel, int page, int pageSize)
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

    public async Task GenerateSchedule(DateTime dateTime)
    {
        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        var payment =  _paymentRepository.GetQuery()
            .Where(x => x.PaymentStatus == 3 && x.CreateAt.Value.Date == dateTime.Date)
            .OrderBy(x => x.CreateAt)
            .ToList();

        var posList = payment.Select(x => x.PosId)
            .Distinct();


        var entites = new List<ReceiptScheduleEntity>();

        foreach (var posId in posList)
        {
            var query =  _paymentRepository.GetQuery()
                .Where(x => x.PaymentStatus == 3 && x.CreateAt.Value.Date == dateTime.Date)
                .OrderBy(x => x.CreateAt)
                .ToList();

            var receiptItem = query.Select(x => new TaxReceiptEntity
                {
                    ReceiptNo = x.ReceiptNo,
                    PaymentTransactionId = x.PaymentTransactionId.Value,
                    CreatedDate = x.CreateAt.Value
                })
                .OrderBy(x => x.CreatedDate);


            var receiptHyphen = "-";

            if (receiptItem.Any())
            {
                receiptHyphen = receiptItem.First().ReceiptNo;
            }

            if (receiptItem.Any() && receiptItem.Count() > 1)
            {
                receiptHyphen += $"- {receiptItem.Last().ReceiptNo}";
            }

            var entity = new ReceiptScheduleEntity
            {
                ReceiptDate = dateTime,
                PosId = posId,
                Amount = query.Sum(x => x.TotalTransactionPrices),
                ReceiveNo = receiptHyphen,
                // Item = query.Select(x => new ReceiptPaymentEntity
                // {
                //     ReceiptNo = x.ReceiptNo
                // }).ToList()
            };

            _receiptRepository.Add(entity);
            entity.Item = query.Select(x => new ReceiptPaymentEntity
            {
                ReceiptId = entity.ReceiptId,
                ReceiptNo = x.ReceiptNo
            }).ToList();

        }

        try
        {
            await _receiptRepository.Context().SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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