using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Controllers.v1;
using Promrub.Services.API.Entities;
using Promrub.Services.API.PromServiceDbContext;
using Promrub.Services.API.Repositories;
using Promrub.Services.API.Services;
using Promrub.Services.API.Utils;

namespace Promrub.Services.API.Interfaces;


public interface ITaxService
{
    public Task<PagedList<TaxResponse>> GetByList(string keyword, Guid? posId, Guid? paymentChannel, int page,
        int pageSize);

    public Task<PagedList<TaxResponse>> GetByDate(string keyword, Guid? posId, Guid? paymentChannel, int page,
        int pageSize);

    public Task Add(TaxScheduleEntity entity);
}

public class TaxService(ITaxRepository repository, IPaymentRepository paymentRepository)
    : BaseService, ITaxService
{
    private readonly ITaxRepository repository = repository;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;

    public Task<PagedList<TaxResponse>> GetByList(string keyword, Guid? posId, Guid? paymentChannel, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<TaxResponse>> GetByDate(string keyword, Guid? posId, Guid? paymentChannel, int page, int pageSize)
    {
        throw new NotImplementedException();
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