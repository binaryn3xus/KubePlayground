using BackgroundProcesses.Commands;

// just an experiment, don't take it into consideration
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("App Starting...");

        // Convert all args to lower-case
        args = args.Select(arg => arg.ToLower()).ToArray();

        var services = new ServiceCollection();
        ConfigureServices(services);

        await using var serviceProvider = services.BuildServiceProvider();

        // Define commands / sub-commands
        var rootCommand = new RootCommand("Background Process Application")
            .AddGlobalOptions();

        ConfigureCommands(rootCommand, serviceProvider);

        var invoked = await rootCommand.InvokeAsync(args);
        Console.WriteLine($"Invoked Status: {invoked}");
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel
            .Debug()
            .WriteTo
            .Console()
            .CreateLogger();

        services.AddLogging(builder => builder.AddSerilog(logger));

        services.AddSingleton<FeederCalculationService>();
        services.AddSingleton<ExtractService>();

        services.AddSingleton<FeederCalculationCommand>();
        services.AddSingleton<ExtractsCommand>();
    }

    private static void ConfigureCommands(RootCommand rootCommand, ServiceProvider serviceProvider)
    {
        rootCommand.AddCommand(serviceProvider.GetRequiredService<FeederCalculationCommand>());
        rootCommand.AddCommand(serviceProvider.GetRequiredService<ExtractsCommand>());
    }
}
