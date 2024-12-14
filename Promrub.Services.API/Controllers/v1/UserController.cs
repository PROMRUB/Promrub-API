using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.User;
using Promrub.Services.API.Models.ResponseModels.Organization;
using Promrub.Services.API.Models.ResponseModels.User;
using System.Diagnostics.CodeAnalysis;

namespace Promrub.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class UserController : BaseController
    {
        private readonly IUserService services;
        public UserController(IUserService services)
        {
            this.services = services;
        }

        [HttpGet]
        [Route("org/{id}/action/AdminGetUsers")]
        [MapToApiVersion("1")]
        public IActionResult AdminGetUsers(string id)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = services.GetUsers(id);
                return Ok(ResponseHandler.Response<List<UserResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/AdminAddUser")]
        [MapToApiVersion("1")]
        public IActionResult AdminAddUser(string id, [FromBody] UserRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                services.AddUser(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
