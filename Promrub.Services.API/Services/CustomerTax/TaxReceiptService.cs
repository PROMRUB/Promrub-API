using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Controllers.v1;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;

namespace Promrub.Services.API.Services.CustomerTax;

public class TaxReceiptService : ITaxReceiptService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICustomerTaxRepository _customerTaxRepository;

    public TaxReceiptService(IPaymentRepository paymentRepository,ICustomerTaxRepository customerTaxRepository)
    {
        _paymentRepository = paymentRepository;
        _customerTaxRepository = customerTaxRepository;
    }
    public async Task<CustomerResponse> GetCustomerByTaxId(string id)
    {
        var entity = await _customerTaxRepository.GetCustomerTaxQuery().FirstOrDefaultAsync(x => x.TaxId == id && x.IsMemo);

        if (entity == null)
        {
            return null;
        }

        return new CustomerResponse
        {
            CustomerTaxId = entity.Id,
            TaxId = entity.TaxId,
            Address = entity.FullAddress,
            PostCode = entity.PostCode,
            Email = entity.Email,
            Tel = entity.Tel
        };
    }

    public async Task<CustomerResponse> Update(string id, TaxReceiptController.BusinessResource request)
    {
        var entity = await _customerTaxRepository.GetCustomerTaxQuery().FirstOrDefaultAsync(x => x.TaxId == id);

        if (entity == null)
        {
            return null;
        }

        entity.Email = request.Email;
        entity.FullAddress = request.Address;
        entity.PostCode = request.PostCode;
        entity.Name = request.Name;
        entity.IsMemo = request.IsMemo;
        entity.Tel = request.Tel;


        _customerTaxRepository.Context().Update(entity);
        await _customerTaxRepository.Context().SaveChangesAsync();

        return new CustomerResponse
        {
            CustomerTaxId = entity.Id,
            TaxId = entity.TaxId,
            Address = entity.FullAddress,
            PostCode = entity.PostCode,
            Email = entity.Email,
            Tel = entity.Tel
        };
    }

    public async Task<CustomerResponse> Create(TaxReceiptController.BusinessResource request)
    {
        var entity = new CustomerTaxEntity();
        entity.TaxId = request.TaxId;
        entity.Email = request.Email;
        entity.FullAddress = request.Address;
        entity.PostCode = request.PostCode;
        entity.Name = request.Name;
        entity.IsMemo = request.IsMemo;
        entity.Tel = request.Tel;

        var transaction = await _paymentRepository.GetPaymentTransaction(request.TransactionId);
        
        
        _customerTaxRepository.Add(entity);
        await _customerTaxRepository.Context().SaveChangesAsync();

        transaction.CustomerTaxId = entity.Id;
         _paymentRepository.Context().Update(transaction);
         await _paymentRepository.Context().SaveChangesAsync();
         
         
        return new CustomerResponse
        {
            CustomerTaxId = entity.Id,
            TaxId = entity.TaxId,
            Address = entity.FullAddress,
            PostCode = entity.PostCode,
            Email = entity.Email,
            Tel = entity.Tel
        };
    }
}