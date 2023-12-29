namespace BackgroundProcesses.Services;

public class ExtractService(ILogger<ExtractService> logger) : ICommandProcess<ExtractCommandOptions>
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger = logger;

    public async Task Execute(ExtractCommandOptions options, CancellationToken cancellationToken)
    {
        try
        {
            options.LogProperties(_logger);
            ThrowIfOptionsInvalid(options);

            _logger.LogInformation("Starting Extracts...");
            await CountToNumberAsync(20, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Extracts Complete...");
        }
        catch (OperationCanceledException oce)
        {
            _logger.LogWarning("{Message}", oce.Message);
        }
    }

    public static void ThrowIfOptionsInvalid(ExtractCommandOptions options)
    {
        if (!options.IsDaily && !options.IsHourly)
        {
            throw new ArgumentException("Neither Daily or Hourly was defined for the Extract run");
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
