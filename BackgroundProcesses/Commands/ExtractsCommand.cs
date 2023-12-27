namespace BackgroundProcesses.Commands;

public class ExtractsCommand : Command
{
    private readonly ILogger<ExtractsCommand> _logger;
    private readonly ExtractService _extractService;
    private readonly Option<bool> _isDailyOption = new(name: "--daily", description: "Defines if you want to start a daily extract", getDefaultValue: () => false) { IsRequired = true };
    private readonly Option<bool> _isHourlyOption = new(name: "--hourly", description: "Defines if you want to start a hourly extract", getDefaultValue: () => false) { IsRequired = true };

    public ExtractsCommand(ILogger<ExtractsCommand> logger, ExtractService extractService) : base("extracts", "Interface with Extract Process")
    {
        _logger = logger;
        _extractService = extractService;

        AddOption(_isDailyOption);
        AddOption(_isHourlyOption);
        this.SetHandler(HandleCommand);
    }

    private async Task HandleCommand(InvocationContext context)
    {
        try
        {
            bool isDaily = context.ParseResult.GetValueForOption(_isDailyOption);
            bool isHourly = context.ParseResult.GetValueForOption(_isHourlyOption);
            var token = context.GetCancellationToken();
            var commandOptions = new ExtractCommandOptions { IsDaily = isDaily, IsHourly = isHourly };
            await _extractService.Execute(commandOptions, token);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.ToString());
        }
    }
}
