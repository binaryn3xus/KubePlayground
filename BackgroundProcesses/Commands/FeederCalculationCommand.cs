namespace BackgroundProcesses.Commands;

public class FeederCalculationCommand : Command
{
    private readonly ILogger<FeederCalculationCommand> _logger;
    private readonly FeederCalculationService _feederCalcService;

    private readonly Option<int> _consumerIdOption = new(name: "--consumer-id", description: "Consumer Id to use for the feeder calc") { IsRequired = true };

    public FeederCalculationCommand(ILogger<FeederCalculationCommand> logger, FeederCalculationService feederCalcService)
        : base("feedercalculation", "Feeder Calculation Process")
    {
        _logger = logger;
        _feederCalcService = feederCalcService;

        AddOption(_consumerIdOption);

        this.SetHandler(HandleCommand);
    }

    private async Task HandleCommand(InvocationContext context)
    {
        try
        {
            var msSqlConnection = context.ParseResult.GetValueForOption(GlobalOptions.MicrosoftSqlConnection);
            var db2Connection = context.ParseResult.GetValueForOption(GlobalOptions.Db2Connection);
            int? consumerId = context.ParseResult.GetValueForOption(_consumerIdOption);
            var token = context.GetCancellationToken();
            var commandOptions = new FeederCalcCommandOptions() { ConsumerId = consumerId, MsSqlConnection = msSqlConnection };
            await _feederCalcService.Execute(commandOptions, token);
        } catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.ToString());
        }
    }
}
