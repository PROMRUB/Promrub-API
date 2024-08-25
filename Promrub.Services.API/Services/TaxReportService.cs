using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Controllers.v1;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.ResponseModels.Tax;
using Promrub.Services.API.Utils;

namespace Promrub.Services.API.Services;

public class TaxReportService : ITaxReportService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITaxRepository _taxRepository;

    public TaxReportService(IPaymentRepository paymentRepository, ITaxRepository taxRepository)
    {
        _paymentRepository = paymentRepository;
        _taxRepository = taxRepository;
    }

    public async Task<PagedList<TaxResponse>> GetTax(string keyword, string? posId, string startDate, string endDate,
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

        var query = _paymentRepository.GetQuery()
            .Where(x => x.PaymentStatus == 3
                        && (string.IsNullOrEmpty(posId) || x.PosId == posId)
                        && (string.IsNullOrEmpty(keyword) || x.ReceiptNo.Contains(keyword))
                        && (!start.HasValue || x.CreateAt > start.Value)
                        && (!end.HasValue || x.CreateAt < end.Value)
            )
            .OrderByDescending(x => x.CreateAt);

        var amount = query.Sum(x => x.TotalTransactionPrices);

        var list = query.Select(x => Generate(x));

        var paged = await PagedList<TaxResponse>.Create(list, page, pageSize);
        paged.TotalSummary = amount;

        return paged;
    }

    public async Task<PagedList<TaxResponse>> GetTaxByDate(string keyword, string? posId, string startDate,
        string endDate, Guid? paymentChannel, int page, int pageSize)
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

        var query = _taxRepository.GetTaxScheduleQuery()
            .Include(x => x.Item)
            .Where(x => (string.IsNullOrEmpty(keyword) || x.Item.Any(y => y.ReceiptNo.Contains(keyword)))
                        && (string.IsNullOrEmpty(posId) || x.PosId == posId)
                        && (!start.HasValue || x.TaxDate > start.Value)
                        && (!end.HasValue || x.TaxDate < end.Value)
            )
            .OrderByDescending(x => x.TaxDate);

        var amount = query.Sum(x => x.Amount);
        var list = query.Select(x => new TaxResponse
        {
            PosId = x.PosId,
            TaxReceive = x.TotalReceipt,
            TaxDateTime = x.TaxDate.ToString("dd/MM/yyyy"),
            Price = x.Amount,
            Vat = x.Vat,
            Amount = x.TotalAmount
        });

        var paged = await PagedList<TaxResponse>.Create(list, page, pageSize);
        paged.TotalSummary = amount;
        return paged;
        
    }

    public Task Add(TaxScheduleEntity entity)
    {
        throw new NotImplementedException();
    }

    private static TaxResponse Generate(PaymentTransactionEntity transactionEntity)
    {
        var amount = transactionEntity.TotalTransactionPrices;
        var price = transactionEntity.TotalTransactionPrices / (decimal)1.07;
        price = Math.Round(price, 2, MidpointRounding.ToEven);
        var vat = amount - price;
        return new TaxResponse
        {
            PosId = transactionEntity.PosId,
            TaxReceive = transactionEntity.ReceiptNo,
            TaxDateTime = transactionEntity.ReceiptDate.Value.ToString("dd/MM/yyyy"),
            Price = price,
            Vat = vat,
            Amount = amount
        };
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

            var receiptItem = query.Select(x => new TaxScheduleEntity
                {
                    TotalAmount = x.TotalTransactionPrices,
                    TotalReceipt = x.ReceiptNo
                })
                .OrderBy(x => x.TaxDate);


            var receiptHyphen = "-";

            if (receiptItem.Any())
            {
                receiptHyphen = receiptItem.First().TotalReceipt;
            }

            if (receiptItem.Any() && receiptItem.Count() > 1)
            {
                receiptHyphen += $"- {receiptItem.Last().TotalReceipt}";
            }

            var amount = (decimal)receiptItem.Sum(x => x.TotalAmount);
            var price = amount / (decimal)1.07;
            price = Math.Round(price, 2, MidpointRounding.ToEven);
            var vat = amount - price;

            var entity = new TaxScheduleEntity
            {
                TaxDate = dateTime,
                PosId = posId,
                Amount = price,
                Vat = vat,
                TotalAmount = amount,
                TotalReceipt = receiptHyphen,
            };

            _taxRepository.Add(entity);
            entity.Item = query.Select(x => new TaxReceiptEntity
            {
                TaxId = entity.TaxId,
                ReceiptNo = x.ReceiptNo,
                CreatedDate = x.CreateAt.Value
            }).ToList();
        }

        try
        {
            await _taxRepository.Context().SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}