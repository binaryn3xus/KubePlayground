namespace BackgroundProcesses.Services;

public class ExtractService(ILogger<ExtractService> logger) : ICommandProcess<ExtractCommandOptions>
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger = logger;

    public Task Execute(ExtractCommandOptions options, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Extracts Parameters:");
            _logger.LogInformation("Daily = {IsDaily}", options.IsDaily);
            _logger.LogInformation("Hourly = {IsHourly}", options.IsHourly);

            if (!options.IsDaily && !options.IsHourly)
            {
                _logger.LogWarning("You must define either Daily or Hourly extract. Aborting Run...");
                throw new ArgumentException("Neither Daily or Hourly was defined for the Extract run");
            }

            _logger.LogInformation("Starting Extracts...");

            int count = 0;
            while (count <= 20 & !cancellationToken.IsCancellationRequested)
            {
                count++;
                _logger.LogDebug("Extracts - Step {count}...", count);
                Thread.Sleep(2000);
            }

            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Extracts Complete...");
        }
        catch (OperationCanceledException oce)
        {
            _logger.LogInformation("{Message}", oce.Message);
        }

        return Task.CompletedTask;
    }
}
