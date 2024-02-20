namespace Promrub.Services.API.Models.ResponseModels.Payment
{
    public class ScbQrGenerateResponse
    {
        public ScbQrGenerateData? Data { get; set; }
    }

    public class ScbQrGenerateData
    {
        public string? QrRawData { get; set; }
        public string? QrImage { get; set; }
    }
}
