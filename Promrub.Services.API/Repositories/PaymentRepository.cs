using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PasswordGenerator;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using Promrub.Services.API.PromServiceDbContext;
using Promrub.Services.API.Utils;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Web;

namespace Promrub.Services.API.Repositories
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        private readonly HttpClient? httpClient;
        private readonly JsonSerializerSettings serializerSettings;
        private CancellationToken cancelToken = new();
        private readonly List<KeyValuePair<string, string>>? headers;
        private readonly IConfiguration configuration;

        public PaymentRepository(PromrubDbContext context,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            this.context = context;
            this.httpClient = httpClient;
            this.httpClient.Timeout = new TimeSpan(0, 1, 0);
            this.headers = new List<KeyValuePair<string, string>>()
            {
            };
            this.serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            this.configuration = configuration;
        }

        public PaymentTransactionEntity AddTransaction(string transactionId, PaymentTransactionEntity request)
        {
            request.TransactionId = transactionId;
            request.OrgId = orgId;
            request.CreateAt = DateTime.UtcNow;
            context!.PaymentTransactions!.Add(request);
            return request!;
        }

        public void AddTransactionItem(PaymentTransactionItemEntity request)
        {
            request.OrgId = orgId;
            context!.PaymentTransactionItems!.Add(request);
        }
        public IQueryable<PaymentTransactionEntity> GetTransactionDetail(string transactionId)
        {
                return context!.PaymentTransactions!.Where(x => x.OrgId!.Equals(orgId) && x.TransactionId! == transactionId);
        }

        public async Task<ScbQrGenerateResponse> QRGenerate(ScbQr30PaymentRequest request)
        {
            var json = JsonConvert.SerializeObject(request, serializerSettings);
            var response = await HttpUtils.Post<ScbQrGenerateResponse>(httpClient ?? new HttpClient(),
                configuration["SCBServicesUrl"] + configuration["SCBGenerateQRUrl"], headers ?? new List<KeyValuePair<string, string>>(),
                cancelToken, json);
            return response;
        }

        public void Commit()
        {
            context!.SaveChanges();
        }
    }
}
