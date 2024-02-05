using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Models.Request;
using Promrub.Services.API.Models.ResponseModels.Payment;
using System.Reflection.PortableExecutable;
using System.Transactions;

namespace Promrub.Services.API.Controllers
{
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        [MapToApiVersion("1")]
        [Route("org/{orgId}/action/GeneratePaymentTransaction")]
        public IActionResult GeneratePaymentTransaction(string orgId, [FromBody] GeneratePaymentTransactionLinkRequestModel requestModel)
        {
            try
            {
                return Ok( new { Status = ResponseHandler.ResponseMessage("9500", null) });
                if (!ModelState.IsValid || string.IsNullOrEmpty(orgId))
                    return Ok( new { Status = ResponseHandler.ResponseMessage("1101", null) });

                return Ok( new { Status = ResponseHandler.ResponseMessage("1000", null), Data = new GeneratePaymentLinkModel() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [MapToApiVersion("1")]
        [Route("org/{orgId}/action/InquiryTransaction/{transactionId}")]
        public IActionResult InquiryTransaction(string orgId, string transactionId)
        {
            try
            {
                return Ok(new { Status = ResponseHandler.ResponseMessage("9500", null) });
                if (!ModelState.IsValid || string.IsNullOrEmpty(orgId))
                    return Ok(new { Status = ResponseHandler.ResponseMessage("1101", null) });

                return Ok(new { Status = ResponseHandler.ResponseMessage("1000", null), Data = new InquiryTransactionResponseModel() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
