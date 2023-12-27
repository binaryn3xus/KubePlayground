namespace BackgroundProcesses.Commands;

public class ExtractsCommand : Command
{
    public readonly ILogger<ExtractsCommand> _logger;
    public readonly ExtractService _extractService;
    private readonly Option<bool> isDailyOption = new(name: "--daily", description: "Defines if you want to start a daily extract", getDefaultValue: () => false) { IsRequired = true };
    private readonly Option<bool> isHourlyOption = new(name: "--hourly", description: "Defines if you want to start a hourly extract", getDefaultValue: () => false) { IsRequired = true };

public ExtractsCommand(ILogger<ExtractsCommand> logger, ExtractService extractService) : base("extracts", "Interface with Extract Process")
    {
        _logger = logger;
        _extractService = extractService;

        AddOption(isDailyOption);
        AddOption(isHourlyOption);
        this.SetHandler(HandleCommand);
    }

    private async Task HandleCommand(InvocationContext context)
    {
        try
        {
            bool isDaily = context.ParseResult.GetValueForOption(isDailyOption);
            bool isHourly = context.ParseResult.GetValueForOption(isHourlyOption);
            var token = context.GetCancellationToken();
            var commandOptions = new ExtractCommandOptions() { IsDaily = isDaily, IsHourly = isHourly };
            await _extractService.Execute(commandOptions, token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }
}