using AutoMapper;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using System.Linq;

namespace Promrub.Services.API.Services.Payment
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IMapper mapper;
        private readonly IOrganizationRepository organizationRepository;
        public PaymentServices(IMapper mapper,
            IOrganizationRepository organizationRepository)
        {
            this.mapper = mapper;
            this.organizationRepository = organizationRepository;
        }

        public async Task<GeneratePaymentLinkModel> GeneratePaymentTransaction(string orgId, GeneratePaymentTransactionLinkRequestModel request)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            if(organization is null)
                throw new ArgumentException("1111");
            return new GeneratePaymentLinkModel();
        }
    }
}
