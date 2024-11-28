using Microsoft.EntityFrameworkCore;
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

        private List<KeyValuePair<string, string>> Header(string token)
        {
            List<KeyValuePair<string, string>>? headers = new List<KeyValuePair<string, string>>();
            
            headers?.Add(new KeyValuePair<string, string>("Authorization", token));

            return headers ?? new List<KeyValuePair<string, string>>();
        }

        public IQueryable<PaymentTransactionEntity> GetQuery()
        {
            return context.PaymentTransactions;
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
        public void AddCouponItem(CouponEntity request)
        {
            request.OrgId = orgId;
            context!.Coupons!.Add(request);
        }
        public IQueryable<PaymentTransactionEntity> GetTransactionDetail(string transactionId)
        {
                return context!.PaymentTransactions!.Where(x => x.OrgId!.Equals(orgId) && x.TransactionId! == transactionId);
        }
        public IQueryable<PaymentTransactionItemEntity> GetTransactionItem(Guid transactionId)
        {
            return context!.PaymentTransactionItems!.Where(x => x.OrgId!.Equals(orgId) && x.PaymentTransactionId! == transactionId);
        }

        public IQueryable<CouponEntity> GetTransactionCoupon(Guid transactionId)
        {
            return context!.Coupons!.Where(x => x.OrgId!.Equals(orgId) && x.PaymentTransactionId! == transactionId);
        }

        public IQueryable<PaymentTransactionEntity> GetTransactionDetailById(string transactionId)
        {
            return context!.PaymentTransactions!.Where(x => x.TransactionId! == transactionId);
        }

        public async Task<ScbQrGenerateResponse> QRGenerate(ScbQr30PaymentRequest request)
        {
            var json = JsonConvert.SerializeObject(request, serializerSettings);
            var response = await HttpUtils.Post<ScbQrGenerateResponse>(httpClient ?? new HttpClient(),
                configuration["SCBServicesUrl"] + configuration["SCBGenerateQRUrl"], headers ?? new List<KeyValuePair<string, string>>(),
                cancelToken, json);
            return response;
        }

        public async Task<ReceiptNumbersEntity> ReceiptNumberAsync(string? orgId,string posId)
        {
            var sub = string.IsNullOrEmpty(posId) ? "" : posId.Substring(posId.Length - 4, 4);
            var currentDate = DateTime.Today.Date.ToUniversalTime().ToString("yyMMdd") + sub;
            var query = await context!.ReceiptNumbers!.Where(x => x.ReceiptDate == currentDate && x.OrgId == orgId).FirstOrDefaultAsync();
            bool HaveReciept = true;
            while (HaveReciept)
            {
                if (query == null)
                {
                    var newRec = new ReceiptNumbersEntity
                    {
                        ReceiptId = Guid.NewGuid(),
                        OrgId = orgId,
                        ReceiptDate = currentDate,
                        Allocated = 0
                    };
                    context.ReceiptNumbers!.Add(newRec);
                    context.SaveChanges();
                    query = await context.ReceiptNumbers!.Where(x => x.ReceiptDate == currentDate && x.OrgId == orgId).FirstOrDefaultAsync();
                }
                else
                {
                    HaveReciept = false;
                }
            }
            query!.Allocated++;
            context.SaveChanges();
            return query;
        }

        public async Task<PaymentTransactionEntity> ReceiptUpdate(PaymentTransactionEntity request)
        {

            var query = await context!.PaymentTransactions!.Where(x => x.TransactionId == request.TransactionId).FirstOrDefaultAsync();
            if (query == null)
                throw new ArgumentException("1102");
            query.ReceiptNo = request.ReceiptNo;
            query.ReceiptDate = request.ReceiptDate;
            query.PaymentStatus = 3;
            context!.SaveChanges();
            return query;
        }

        public async Task<PaymentTransactionEntity> GetPaymentTransaction(string transactionId)
        {
           return await context!.PaymentTransactions!.FirstOrDefaultAsync(x => x.TransactionId == transactionId);
        }

        public async Task<PaymentTransactionEntity> ExpireTransaction(PaymentTransactionEntity request)
        {

            var query = await context!.PaymentTransactions!.Where(x => x.TransactionId == request.TransactionId).FirstOrDefaultAsync();
            if (query == null)
                throw new ArgumentException("1102");
            query.PaymentStatus = request.PaymentStatus;
            context!.SaveChanges();
            return query;
        }

        public async Task<OrganizationCallbackResponse> Callback(string url, OrganizationCallbackRequest request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request, serializerSettings);
            var responseCallback = await HttpUtils.Post<OrganizationCallbackResponse>(httpClient == null ? new HttpClient() : httpClient,
            url, Header(token), cancelToken, jsonRequest);
            return responseCallback;
        }

        public DbContext Context()
        {
            return context;
        }
    }
}
