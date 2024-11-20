using Promrub.Services.API.Controllers.v1;

namespace Promrub.Services.API.Interfaces;

public interface ITaxReceiptService
{
    Task<CustomerResponse> GetCustomerByTaxId(string id);
    Task<CustomerResponse> Update(string id, TaxReceiptController.BusinessResource request);
    Task<CustomerResponse> Create(TaxReceiptController.BusinessResource request);
}