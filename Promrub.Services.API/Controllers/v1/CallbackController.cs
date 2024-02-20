using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Models.ResponseModels.ApiKey;

namespace Promrub.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class CallbackController : BaseController
    {
        public CallbackController() { 
        }

        [HttpPost]
        [Route("/action/SCBCallback")]
        [MapToApiVersion("1")]
        public IActionResult SCBCallback()
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new ArgumentException("1101");
                return Ok(ResponseHandler.Response("1000", null));

            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
