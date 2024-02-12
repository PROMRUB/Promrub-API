using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Organization;
using Promrub.Services.API.Models.ResponseModels.Organization;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Promrub.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [Authorize(Policy = "GenericRolePolicy")]
    [ApiVersion("1")]
    public class OrganizationController : BaseController
    {
        private readonly IOrganizationService services;

        public OrganizationController(IOrganizationService services)
        {
            this.services = services;
        }

        [HttpGet]
        [Route("org/{id}/action/GetOrganization")]
        public async Task<IActionResult> GetOrganization(string id)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GetOrganization(id);
                return Ok(ResponseHandler.Response<OrganizationResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [ExcludeFromCodeCoverage]
        [HttpGet]
        [Route("org/{id}/action/AdminGetUserAllowedOrganization/{userName}")]
        public async Task<IActionResult> AdminGetUserAllowedOrganization(string id, string userName)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GetUserAllowedOrganization(userName!);
                return Ok(ResponseHandler.Response<List<OrganizationUserResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [ExcludeFromCodeCoverage]
        [HttpPost]
        [Route("org/{id}/action/AddUserToOrganization")]
        public IActionResult AddUserToOrganization(string id, [FromBody] OrganizationUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                services.AddUserToOrganization(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [ExcludeFromCodeCoverage]
        [HttpPost]
        [Route("org/{id}/action/AdminAddOrganization")]
        public IActionResult AdminAddOrganization(string id, [FromBody] OrganizationRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                services.AddOrganization(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [ExcludeFromCodeCoverage]
        [HttpPost]
        [Route("org/{id}/action/AdminAddUserToOrganization")]
        public IActionResult AdminAddUserToOrganization(string id, [FromBody] OrganizationUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var userOrgId = request.OrgCustomId;
                services.AddUserToOrganization(userOrgId!, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
