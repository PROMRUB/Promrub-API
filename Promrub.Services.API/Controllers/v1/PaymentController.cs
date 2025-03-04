﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using System.Net.Http.Headers;
using System.Drawing;

namespace Promrub.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class PaymentController : BaseController
    {
        private readonly IPaymentServices services;

        public PaymentController(IPaymentServices services)
        {
            this.services = services;
        }

        [HttpPost]
        [Authorize(Policy = "GenericRolePolicy")]
        [Route("org/{id}/action/GeneratePaymentTransaction")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GeneratePaymentTransaction(string id, [FromBody] GeneratePaymentTransactionLinkRequestModel request)
        {
            try
            {
                var profile = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(profile);
                if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(profile))
                    throw new ArgumentException("1101");
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
                var key = APIKeyHandler.GetBasicAPIKey(credentialBytes);
                var result = await services.GeneratePaymentTransaction(id, request, key);
                return Ok(ResponseHandler.Response<GeneratePaymentLinkModel>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }


        [HttpGet]
        [Route("org/{id}/action/GetPaymentDetails/{transactionId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetPaymentDetails(string id, string transactionId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GetPaymentTransactionDetails(id, transactionId);
                return Ok(ResponseHandler.Response<PaymentTransactionDetails>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }


        [HttpGet]
        [Route("org/{id}/action/GenerateQr30/{transactionId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GenerateQr30(string id, string transactionId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GetPromtPayQrCode(id, transactionId);
                return Ok(ResponseHandler.Response<Qr30GenerateResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Authorize(Policy = "GenericRolePolicy")]
        [Route("org/{id}/action/InquiryTransaction/{transactionId}")]
        [MapToApiVersion("1")]
        public IActionResult InquiryTransaction(string id, string transactionId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                return Ok(ResponseHandler.Response("1000", null, new InquiryTransactionResponseModel()));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/GetFullReceipt/{transactionId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFullReceipt(string id, string transactionId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GenerateReceipt(id, transactionId, false);
                return File(result.Item1, "application/pdf", $"{result.ReceiptNo}.pdf");
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }


        [HttpGet]
        [Route("org/{id}/action/GetReceipt/{transactionId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetReceipt(string id, string transactionId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GenerateReceipt(id, transactionId, false);
                return File(result.Item1, "application/pdf", $"{result.ReceiptNo}.pdf");
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
        
        [HttpGet]
        [Route("org/{id}/action/GetReceiptImage/{transactionId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetReceiptImage(string id, string transactionId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GenerateReceipt(id, transactionId, true);
                return Ok(Convert.ToBase64String(result.Item1.ToArray()));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
