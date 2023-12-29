namespace BackgroundProcesses.Services;

public class FeederCalculationService(ILogger<FeederCalculationService> logger) : ICommandProcess<FeederCalcCommandOptions>
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger = logger;

    public async Task Execute(FeederCalcCommandOptions options, CancellationToken cancellationToken)
    {
        try
        {
            options.LogProperties(_logger);
            await CountToNumberAsync(20, cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Feeder Calculation Complete...");
        }
        catch (OperationCanceledException oce)
        {
            _logger.LogWarning("{Message}", oce.Message);
        }
    }

    public async Task CountToNumberAsync(int number, CancellationToken cancellationToken)
    {
        int count = 0;
        while (count < number & !cancellationToken.IsCancellationRequested)
        {
            count++;
            _logger.LogDebug("Step {count}...", count);
            await Task.Delay(2000, cancellationToken).ConfigureAwait(false);
        }
    }
}
