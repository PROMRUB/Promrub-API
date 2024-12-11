namespace Promrub.Services.API.Controllers.v1;

public class CustomerResponse
{
    public Guid CustomerTaxId { get; set; }
    public string TaxId { get; set; }
    public string Address { get; set; }
    public string PostCode { get; set; }
    public string Email { get; set; }
    public string Tel { get; set; }
    public string Name { get; set; }
}