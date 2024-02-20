using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.PromServiceDbContext;

namespace Promrub.Services.API.Repositories
{
    public class PaymentChannelRepository : BaseRepository, IPaymentChannelRepository
    {
        public PaymentChannelRepository(PromrubDbContext context)
        {
            this.context = context;
        }

        public void AddPaymentChannel(PaymentChannelEntity request)
        {
            request.OrgId = orgId;
            context!.paymentChannels.Add(request);
        }

        public async Task<List<PaymentChannelEntity>> GetPaymentChannels()
        {
            return await context!.paymentChannels.Where(x => x.OrgId!.Equals(orgId) && x.IsActive == true).ToListAsync();
        }
    }
}
