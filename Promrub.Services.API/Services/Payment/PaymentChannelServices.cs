﻿using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Payment;

namespace Promrub.Services.API.Services.Payment
{
    public class PaymentChannelServices : IPaymentChannelServices
    {
        private readonly IMapper mapper;
        private readonly IPaymentChannelRepository paymentChannelRepository;

        public PaymentChannelServices(IMapper mapper,
            IPaymentChannelRepository paymentChannelRepository)
        {
            this.mapper = mapper;
            this.paymentChannelRepository = paymentChannelRepository;
        }
        public async Task<List<PaymentChannelEntity>> GetPaymentChannel(string orgId)
        {
            paymentChannelRepository.SetCustomOrgId(orgId);
            var result = await paymentChannelRepository.GetPaymentChannels();
            return result;
        }

        public void AddPaymentChannel(string orgId, PaymentChannelRequest request)
        {
            paymentChannelRepository.SetCustomOrgId(orgId);
            var query = mapper.Map<PaymentChannelRequest, PaymentChannelEntity>(request);
            paymentChannelRepository.AddPaymentChannel(query);
            paymentChannelRepository.Commit();
        }

        public async Task UpdateBillerId(string orgId, Guid paymentChannelId, PaymentChannelRequest request)
        {
            paymentChannelRepository.SetCustomOrgId(orgId);
            paymentChannelRepository.UpdateBillerId(paymentChannelId, request.BillerId);
        }
    }
}
