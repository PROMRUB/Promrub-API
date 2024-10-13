using System.Data.Entity;
using System.Globalization;
using Promrub.Services.API.Controllers.v1;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.ResponseModels.Receipt;
using Promrub.Services.API.Utils;

namespace Promrub.Services.API.Services;

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
        string startDate,
        string endDate,
        Guid? paymentChannel, int page,
        int pageSize)
    {
        DateTime? start = null;
        if (!string.IsNullOrEmpty(startDate))
        {
            start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        DateTime? end = null;
        if (!string.IsNullOrEmpty(endDate))
        {
            end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        var query = _paymentRepository.GetQuery()
            .Where(x => x.PaymentStatus == 3
                        && (string.IsNullOrEmpty(posId) || x.PosId == posId)
                        && (string.IsNullOrEmpty(keyword) || x.ReceiptNo.Contains(keyword))
                        && (!start.HasValue || x.CreateAt > start.Value)
                        && (!end.HasValue || x.CreateAt < end.Value)
            )
            .OrderByDescending(x => x.CreateAt);

        var sum = query.Sum(x => x.TotalTransactionPrices);

        var list = query.Select(x => new ReceivePaymentResponse()
        {
            Id = x.PaymentTransactionId.Value,
            PosId = x.PosId,
            PaymentDateTime = x.CreateAt.Value.ToString("dd/MM/yyyy"),
            ReceiveNo = x.ReceiptNo,
            Amount = x.TotalTransactionPrices,
            PaidBy = "CSH",
            InvoiceNo = "-"
        });


        var paged = await PagedList<ReceivePaymentResponse>.Create(list, page, pageSize);
        paged.TotalSummary = sum;
        return paged;
    }


    public async Task<PagedList<ReceivePaymentByDateResponse>> GetReceiveByDate(string keyword, string? posId,
        string startDate,
        string endDate,
        Guid? paymentChannel, int page, int pageSize)
    {
        DateTime? start = null;
        if (!string.IsNullOrEmpty(startDate))
        {
            start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        DateTime? end = null;
        if (!string.IsNullOrEmpty(endDate))
        {
            end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        var query = _receiptRepository.GetReceiptScheduleQuery()
            .Include(x => x.Item)
            .Where(x => 
                (string.IsNullOrEmpty(keyword) || x.Item.Any(y => y.ReceiptNo.Contains(keyword)))
                &&
                (string.IsNullOrEmpty(posId) || x.PosId == posId)
                && 
                (!start.HasValue || x.ReceiptDate > start.Value)
                && (!end.HasValue || x.ReceiptDate < end.Value)
            )
            .OrderByDescending(x => x.ReceiptDate);

        var amount = query.Sum(x => x.Amount);
        var list = query.Select(x => new ReceivePaymentByDateResponse
        {
            PosId = x.PosId,
            PaymentDateTime = x.ReceiptDate.ToString("dd/MM/yyyy"),
            Amount = x.Amount,
            PaidBy = "CSH",
            ReceiveNo = x.ReceiveNo
        });

        var paged = await PagedList<ReceivePaymentByDateResponse>.Create(list, page, pageSize);
        paged.TotalSummary = amount;
        return paged;
    }

    public async Task GenerateSchedule(DateTime dateTime)
    {
        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        var payment = _paymentRepository.GetQuery()
            .Where(x => x.PaymentStatus == 3 && x.CreateAt.Value.Date == dateTime.Date)
            .OrderBy(x => x.CreateAt)
            .ToList();

        var posList = payment.Select(x => x.PosId)
            .Distinct();


        foreach (var posId in posList)
        {
            var query = _paymentRepository.GetQuery()
                .Where(x => x.PaymentStatus == 3 && x.CreateAt.Value.Date == dateTime.Date)
                .OrderBy(x => x.CreateAt)
                .ToList();

            var receiptItem = query.Select(x => new TaxReceiptEntity
                {
                    ReceiptNo = x.ReceiptNo,
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