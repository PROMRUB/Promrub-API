namespace Promrub.Services.API.Models.ResponseModels.Tax;

public class TaxResponse
{
    public string PosId { get; set; }
    public string TaxReceive { get; set; }
    public string TaxDateTime { get; set; }
    public decimal Price { get; set; }
    public decimal Vat { get; set; }
    public decimal Amount { get; set; }
}