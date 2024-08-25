using Promrub.Services.API.Interfaces;

namespace Promrub.Services.API.BackgroundService;

public class Background : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly ITaxReportService _taxReportService;
    private readonly IReceivePaymentService _receivePaymentService;

    public Background(ITaxReportService taxReportService,IReceivePaymentService receivePaymentService)
    {
        _taxReportService = taxReportService;
        _receivePaymentService = receivePaymentService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // calculate seconds till midnight
        var now = DateTime.Now;
        var hours = 23 - now.Hour;
        var minutes = 59 - now.Minute;
        var seconds = 59 - now.Second;
        var secondsTillMidnight = hours * 3600 + minutes * 60 + seconds;

        // wait till midnight
        await Task.Delay(TimeSpan.FromSeconds(secondsTillMidnight), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            // do stuff
            var datetime = DateTime.Now;
            await _taxReportService.GenerateSchedule(datetime);
            await _receivePaymentService.GenerateSchedule(datetime);

            // wait 24 hours
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}