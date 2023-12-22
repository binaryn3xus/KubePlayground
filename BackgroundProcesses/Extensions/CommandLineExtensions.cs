namespace BackgroundProcesses.Extensions;

public static class CommandLineExtensions
{
    public static void AddAliasVariations(this Option options, string aliasVariations)
    {
        options.AddAlias(aliasVariations.ToLower());
        options.AddAlias(aliasVariations.ToUpper());
    }

    public static void AddFeederCalcCommand(this RootCommand rootCommand, IServiceProvider serviceProvider)
    {
        var fcCommand = new Command("feedercalculation", "Interface with feeder calculations");

        var consumerIdOption = new Option<int>(name: "--consumer-id", description: "Define a delay before the process starts") { IsRequired = true};
        fcCommand.Add(consumerIdOption);

        fcCommand.SetHandler(async (context) =>
        {
            int? consumerId = context.ParseResult.GetValueForOption(consumerIdOption);
            var feederCalcService = serviceProvider.GetRequiredService<FeederCalculationService>();
            var token = context.GetCancellationToken();
            await feederCalcService.Execute(new FeederCalcCommandOptions() { ConsumerId = consumerId }, token); 
        });

        rootCommand.AddCommand(fcCommand);
    }

    public static void AddExtractsCommand(this RootCommand rootCommand, IServiceProvider serviceProvider)
    {
        var extractCommand = new Command("extract", "Interface with Extract Process");

        var isDailyOption = new Option<bool>(name: "--daily", description: "Defines if you want to start a daily extract", getDefaultValue: () => false) { IsRequired = true };
        extractCommand.Add(isDailyOption);

        var isHourlyOption = new Option<bool>(name: "--hourly", description: "Defines if you want to start a hourly extract", getDefaultValue: () => false) { IsRequired = true };
        extractCommand.Add(isHourlyOption);

        var extractsService = serviceProvider.GetRequiredService<ExtractService>();

        rootCommand.Add(extractCommand);

        extractCommand.SetHandler(async (context) =>
        {
            bool isDaily = context.ParseResult.GetValueForOption(isDailyOption);
            bool isHourly = context.ParseResult.GetValueForOption(isHourlyOption);
            var feederCalcService = serviceProvider.GetRequiredService<ExtractService>();
            var token = context.GetCancellationToken();
            var commandOptions = new ExtractCommandOptions() { IsDaily = isDaily, IsHourly = isHourly };
            await extractsService.Execute(commandOptions, token); 
        });
    }
}