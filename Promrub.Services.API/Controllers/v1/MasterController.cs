using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Interfaces;

namespace Promrub.Services.API.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/api/[controller]")]
[ApiVersion("1")]
public class MasterController : BaseController
{
    private readonly IMasterService _service;

    public MasterController(IMasterService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("org/{id}/pos")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> GetPos(string id
    )
    {
        try
        {
            var key = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
                throw new ArgumentException("1101");
            var result = await _service.GetPos(id);
            return Ok(ResponseHandler.Response("1000", null, result));
        }
        catch (Exception ex)
        {
            return Ok(ResponseHandler.Response(ex.Message, null));
        }
    }

    [HttpGet]
    [Route("org/{id}/payment_channel")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> GetPaymentChannel(string id
    )
    {
        try
        {
            var key = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
                throw new ArgumentException("1101");
            var result = await _service.GetPaymentChannel(id);
            return Ok(ResponseHandler.Response("1000", null, result));
        }
        catch (Exception ex)
        {
            return Ok(ResponseHandler.Response(ex.Message, null));
        }
    }

    public interface IMasterService
    {
        public Task<List<DropDown>> GetPos(string orgId);
        public Task<List<DropDown>> GetPaymentChannel(string orgId);
    }

    public class MasterService : IMasterService
    {
        private readonly IPosRepository _posRepository;
        private readonly IPaymentChannelRepository _paymentChannelRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public MasterService(IPosRepository posRepository, IPaymentChannelRepository paymentChannelRepository,
            IOrganizationRepository organizationRepository)
        {
            _posRepository = posRepository;
            _paymentChannelRepository = paymentChannelRepository;
            _organizationRepository = organizationRepository;
        }

        private void SetOrgId(string orgId)
        {
            _posRepository.SetCustomOrgId(orgId);
        }

        public async Task<List<DropDown>> GetPos(string orgId)
        {
            SetOrgId(orgId);
            var org = await _organizationRepository.GetOrganizationWithOrgId(orgId);

            var query = _posRepository.GetPosByOrgCustom(org.OrgCustomId);

            return query.Select(x => new DropDown
            {
                Id = x.PosId,
                Name = x.PosKey
            }).ToList();
        }

        public async Task<List<DropDown>> GetPaymentChannel(string orgId)
        {
            return new List<DropDown>()
            {
                new DropDown
                {
                    Id = Guid.Parse("842d3fc7-d6cc-4951-849b-5ca6ad75585d"),
                    Name = "CSH"
                }
            };
        }
    }

    public class DropDown
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}