using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Promrub.Services.API.Controllers.v1;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using Task = System.Threading.Tasks.Task;

namespace Promrub.Services.API.Services.CustomerTax;

public class TaxReceiptService : ITaxReceiptService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICustomerTaxRepository _customerTaxRepository;
    private readonly HttpClient _httpClient;

    public TaxReceiptService(IPaymentRepository paymentRepository, ICustomerTaxRepository customerTaxRepository)
    {
        _paymentRepository = paymentRepository;
        _customerTaxRepository = customerTaxRepository;
        _httpClient = new HttpClient();
    }

    public async Task<CustomerResponse> GetCustomerByTaxId(string id)
    {
        var entity = await _customerTaxRepository.GetCustomerTaxQuery()
            .FirstOrDefaultAsync(x => x.TaxId == id && x.IsMemo);

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
            Tel = entity.Tel,
            Name = entity.Name,
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

        var transaction = await _paymentRepository.GetPaymentTransaction(request.TransactionId);
        await SendEmail(transaction, entity);

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

    private async Task SendEmail(PaymentTransactionEntity transaction, CustomerTaxEntity entity)
    {
        var link = GenerateLink("", transaction.OrgId, transaction.TransactionId);
        var subject = GenerateSubject(transaction.RefTransactionId);
        var content = GenerateContent(link,entity.Name,transaction.RefTransactionId);

        var data = new
        {
            To = "kkunayothin@gmail.com",
            Name = "korn",
            Subject = subject,
            Content = content
        };
        
        var json = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var url = "https://sales-api-dev.prom.co.th/v1/api/email";
        var result = await _httpClient.PostAsync(url, json);
        if (result.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Error sending email");
        }
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

        await SendEmail(transaction, entity);

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

    private static string GenerateLink(string baseUrl, string org, string transactionId)
    {
        return $"https://dev-payment-channel.promrub.com/?orgId={org}&transactionId={transactionId}";
    }

    private static string GenerateContent(string content, string businessName, string transactionId)
    {
        var link =
            $"<a href = '{content}'>คล\u0e34\u0e4aกเพ\u0e37\u0e48อดาวน\u0e4cโหลด\n</a>";

        return
            $"เร\u0e35ยน: ล\u0e39กค\u0e49าท\u0e35\u0e48เคารพ<br/>" +
            $"เร\u0e37\u0e48อง: นำส\u0e48งใบกำก\u0e31บภาษ\u0e35เต\u0e47มร\u0e39ปแบบ เลขท\u0e35\u0e48 {transactionId} ว\u0e31นท\u0e35\u0e48 {DateTime.Now:dd/MM/yyyy}\n<br/>" +
            $"ขอนำส\u0e48งใบกำก\u0e31บภาษ\u0e35เต\u0e47มร\u0e39ปแบบเพ\u0e37\u0e48อใช\u0e49แทนใบกำก\u0e31บภาษ\u0e35อย\u0e48างย\u0e48อเลขท\u0e35\u0e48 {transactionId} ลงว\u0e31นท\u0e35\u0e48 {DateTime.Now:dd/MM/yyyy} ตามท\u0e35\u0e48ท\u0e48านได\u0e49ดำเน\u0e34นกาแจ\u0e49ง</br>" +
            $"ขอเข\u0e49าไปในระบบก\u0e48อนหน\u0e49าน\u0e35\u0e49" +
            $"{link}<br/><br/><br/>" +
            $"<dt>จ\u0e36งเร\u0e35ยนมาเพ\u0e37\u0e48อทราบ<br/>\n" +
            $"ชื่อร้านค้า : {businessName}<br/>" +
            $"<hr>" +
            $"เอกสารน\u0e35\u0e49ถ\u0e39กสร\u0e49างและนำส\u0e48งโดย<br/>\n" +
            $"PROM: Cloud Based Business Management Platform (www.prom.co.th)<br/>\n" +
            $"Copyright \u00a9 2024 PROM Digital Co., Ltd. Co., Ltd., All rights reserved.--";
    }

    private static string GenerateSubject(string transactionId)
    {
        return @$"PROM ERP: นำส่งใบกํากับภาษีเต็มรูปแบบ เลขที่: {transactionId}";
    }

    // public static async Task SendEmail(string email, string name, string content, string businessName,
    //     string transactionId)
    // {
    //     var apiInstance = new TransactionalEmailsApi();
    //     string SenderName = "PROM ERP";
    //     string SenderEmail = "e-service@prom.co.th";
    //     SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
    //
    //     List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
    //
    //
    //     string toEmail = email;
    //     string toName = name;
    //     SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(toEmail, toName);
    //     To.Add(smtpEmailTo);
    //
    //     var link =
    //         $"<a href = '{content}'>คล\u0e34\u0e4aกเพ\u0e37\u0e48อดาวน\u0e4cโหลด\n</a>";
    //
    //
    //     string HtmlContent =
    //         $"เร\u0e35ยน: ล\u0e39กค\u0e49าท\u0e35\u0e48เคารพ<br/>" +
    //         $"เร\u0e37\u0e48อง: นำส\u0e48งใบกำก\u0e31บภาษ\u0e35เต\u0e47มร\u0e39ปแบบ เลขท\u0e35\u0e48 {transactionId} ว\u0e31นท\u0e35\u0e48 {DateTime.Now:dd/MM/yyyy}\n<br/>" +
    //         $"ขอนำส\u0e48งใบกำก\u0e31บภาษ\u0e35เต\u0e47มร\u0e39ปแบบเพ\u0e37\u0e48อใช\u0e49แทนใบกำก\u0e31บภาษ\u0e35อย\u0e48างย\u0e48อเลขท\u0e35\u0e48 {transactionId} ลงว\u0e31นท\u0e35\u0e48 {DateTime.Now:dd/MM/yyyy} ตามท\u0e35\u0e48ท\u0e48านได\u0e49ดำเน\u0e34นกาแจ\u0e49ง</br>" +
    //         $"ขอเข\u0e49าไปในระบบก\u0e48อนหน\u0e49าน\u0e35\u0e49" +
    //         $"{link}<br/><br/><br/>" +
    //         $"<dt>จ\u0e36งเร\u0e35ยนมาเพ\u0e37\u0e48อทราบ<br/>\n" +
    //         $"ชื่อร้านค้า : {businessName}<br/>" +
    //         $"<hr>" +
    //         $"เอกสารน\u0e35\u0e49ถ\u0e39กสร\u0e49างและนำส\u0e48งโดย<br/>\n" +
    //         $"PROM: Cloud Based Business Management Platform (www.prom.co.th)<br/>\n" +
    //         $"Copyright \u00a9 2024 PROM Digital Co., Ltd. Co., Ltd., All rights reserved.--";
    //     string Subject = @$"PROM ERP: นำส่งใบกํากับภาษีเต็มรูปแบบ เลขที่: {transactionId}";
    //
    //     try
    //     {
    //         var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, null, Subject);
    //         CreateSmtpEmail result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e.Message);
    //         throw;
    //     }
    // }
}