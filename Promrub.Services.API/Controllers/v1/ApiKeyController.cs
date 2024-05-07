using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.ApiKey;
using Promrub.Services.API.Models.ResponseModels.ApiKey;
using Promrub.Services.API.Models.ResponseModels.User;
using System.Diagnostics.CodeAnalysis;

namespace Promrub.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class ApiKeyController : BaseController
    {
        private readonly IApiKeyService services;
        public ApiKeyController(IApiKeyService services)
        {
            this.services = services;
        }

        [HttpPost]
        [Route("org/{id}/action/GetApiKeys")]
        [MapToApiVersion("1")]
        public IActionResult GetApiKeys(string id)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = services.GetApiKeys(id);
                return Ok(ResponseHandler.Response<List<ApiKeyResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/VerifyApiKey/{apiKey}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> VerifyApiKey(string id, string apiKey)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.VerifyApiKey(id, apiKey);
                return Ok(ResponseHandler.Response<ApiKeyResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/AddApiKey")]
        [MapToApiVersion("1")]
        public IActionResult AddApiKey(string id, [FromBody] ApiKeyRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                services.AddApiKey(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/UpdateApiKey")]
        [MapToApiVersion("1")]
        public IActionResult UpdateApiKey(string id, [FromBody] ApiKeyRequest request)
        {
            try
            {
                var key = Request.Headers["Authorization"].ToString();
                if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
                    throw new ArgumentException("1101");
                services.Update(id, request, key);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpDelete]
        [Route("org/{id}/action/DeleteApiKeyById/{keyId}")]
        [MapToApiVersion("1")]
        public IActionResult DeleteApiKeyById(string id, string keyId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                services.DeleteApiKeyById(id, keyId);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
