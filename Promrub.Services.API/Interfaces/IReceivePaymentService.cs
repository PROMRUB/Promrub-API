using Promrub.Services.API.Models.ResponseModels.Receipt;
using Promrub.Services.API.Utils;

namespace Promrub.Services.API.Interfaces;

public interface IReceivePaymentService
{
    public Task<PagedList<ReceivePaymentResponse>> GetReceiveList(string keyword, string? posId,
        string startDate,
        string endDate,
        Guid? paymentChannel,
        int page,
        int pageSize);

    public Task<PagedList<ReceivePaymentByDateResponse>> GetReceiveByDate(string keyword, string? posId,
        string startDate, string endDate,
        Guid? paymentChannel, int page,
        int pageSize);

    public Task GenerateSchedule(DateTime dateTime);
}