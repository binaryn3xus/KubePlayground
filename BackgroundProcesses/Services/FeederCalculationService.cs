namespace BackgroundProcesses.Services;

public class FeederCalculationService(ILogger<FeederCalculationService> logger) : ICommandProcess<FeederCalcCommandOptions>
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger = logger;

    public Task Execute(FeederCalcCommandOptions options, CancellationToken cancellationToken)
    {
        // where do we use options here?
        // is it taken into consideration that options can have everything nullable?
        try
        {
            _logger.LogInformation("Feeder Calculation Parameters:");
            _logger.LogInformation("Consumer Id = {ConsumerId}", options.ConsumerId);
            _logger.LogInformation("Connection String = {ConnectionString}", options.MsSqlConnection);
            _logger.LogInformation("Starting Feeder Calculation with Consumer Id of {ConsumerId}", options.ConsumerId);

            int count = 0;
            while (count <= 20 & !cancellationToken.IsCancellationRequested)
            {
                count++;
                _logger.LogDebug("Feeder Calculation - Step {count}...", count);
                // thread.sleep is evil
                Thread.Sleep(5000);
            }

            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Feeder Calculation Complete...");
        }
        catch (OperationCanceledException oce)
        {
            // log as error
            _logger.LogWarning("{Message}", oce.Message);
        }

        return Task.CompletedTask;
    }
}
