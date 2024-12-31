using Promrub.Services.API.Handlers;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Interfaces;

namespace Promrub.Services.API.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/api/[controller]")]
[ApiVersion("1")]
public class TaxReceiptController : BaseController
{
    private readonly ITaxReceiptService _service;

    public TaxReceiptController(ITaxReceiptService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{id}")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> GetBusinessByTaxId(string id)
    {
        try
        {
            var result = await _service.GetCustomerByTaxId(id);
            return Ok(ResponseHandler.Response("1000", null, result));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.Response(ex.Message, null));
        }
    }

    [HttpPost]
    [Route("{id}")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> Update(string id,
        [FromBody] BusinessResource request)
    {
        return BadRequest(ResponseHandler.Response("ex.Message", null));

        try
        {
            var result = await _service.Update(id,request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.Response(ex.Message, null));
        }
    }
    
    [HttpPost]
    [MapToApiVersion("1")]
    public async Task<IActionResult> Create([FromBody] BusinessResource request)
    {
        try
        {
            var result = await _service.Create(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.Response(ex.Message, null));
        }
    }

    public record BusinessResource(string TaxId,string Name,string Address,string PostCode,string Email,string Tel,bool IsMemo,string TransactionId);
}