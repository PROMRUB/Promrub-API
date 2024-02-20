using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Payment;

namespace Promrub.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class PaymentChannelController : BaseController
    {
        private readonly IPaymentChannelServices services;

        public PaymentChannelController(IPaymentChannelServices services)
        {
            this.services = services;
        }

        [HttpPost]
        [Route("org/{id}/action/AddPaymentChannel")]
        [MapToApiVersion("1")]
        public IActionResult AddPaymentChannel(string id, [FromBody] PaymentChannelRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                services.AddPaymentChannel(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
