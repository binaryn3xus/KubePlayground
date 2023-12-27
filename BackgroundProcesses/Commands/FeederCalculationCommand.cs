namespace BackgroundProcesses.Commands;

public class FeederCalculationCommand : Command
{
    public readonly ILogger<FeederCalculationCommand> _logger;
    public readonly FeederCalculationService _feederCalcService;

    private readonly Option<int> consumerIdOption = new(name: "--consumer-id", description: "Consumer Id to use for the feeder calc") { IsRequired = true };

    public FeederCalculationCommand(ILogger<FeederCalculationCommand> logger, FeederCalculationService feederCalcService) : base("feedercalculation", "Feeder Calulation Process")
    {
        _logger = logger;
        _feederCalcService = feederCalcService;

        AddOption(consumerIdOption);

        this.SetHandler(HandleCommand);
    }

    private async Task HandleCommand(InvocationContext context)
    {
        try
        {
            var msSqlConnection = context.ParseResult.GetValueForOption(GlobalOptions.MicrosoftSqlConnection);
            int? consumerId = context.ParseResult.GetValueForOption(consumerIdOption);
            var token = context.GetCancellationToken();
            await _feederCalcService.Execute(new FeederCalcCommandOptions() { ConsumerId = consumerId, MsSqlConnection = msSqlConnection }, token);
        } catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }
}
