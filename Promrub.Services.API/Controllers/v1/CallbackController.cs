using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Payment;

namespace Promrub.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class CallbackController : BaseController
    {
        private readonly IPaymentServices services;
        public CallbackController(IPaymentServices services)
        {
            this.services = services;
        }

        [HttpPost]
        [Route("action/SCBCallback")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SCBCallback(ScbCallbackRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new ArgumentException("1101");
                await services.SCBCallback(request);
                return Ok(ResponseHandler.Response("1000", null));

            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
