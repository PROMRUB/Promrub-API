using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using System.Reflection.PortableExecutable;
using System.Transactions;

namespace Promrub.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [Authorize(Policy = "GenericRolePolicy")]
    [ApiVersion("1")]
    public class PaymentController : BaseController
    {
        [HttpPost]
        [MapToApiVersion("1")]
        [Route("org/{id}/action/GeneratePaymentTransaction")]
        public IActionResult GeneratePaymentTransaction(string id, [FromBody] GeneratePaymentTransactionLinkRequestModel requestModel)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                return Ok(ResponseHandler.Response("1000", null, new GeneratePaymentLinkModel()));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }


        [HttpGet]
        [MapToApiVersion("1")]
        [Route("org/{id}/action/InquiryTransaction/{transactionId}")]
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
    }
}
