using Promrub.Services.API.Controllers.v1;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.ResponseModels.Tax;
using Promrub.Services.API.Utils;

namespace Promrub.Services.API.Interfaces;

public interface ITaxReportService
{
    public Task<PagedList<TaxResponse>> GetTax(string keyword, string? posId, string startDate, string endDate,
        Guid? paymentChannel, int page,
        int pageSize);

    public Task<PagedList<TaxResponse>> GetTaxByDate(string keyword, string? posId, string startDate, string endDate,
        Guid? paymentChannel, int page,
        int pageSize);
    
    public Task GenerateSchedule(DateTime dateTime);


    public Task Add(TaxScheduleEntity entity);
}