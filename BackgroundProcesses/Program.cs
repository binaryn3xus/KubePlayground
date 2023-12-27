using BackgroundProcesses.Commands;

Console.WriteLine("App Starting...");

// Convert all args to lower-case
args = args.Select(arg => arg.ToLower()).ToArray();

var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).Build();
var services = new ServiceCollection();
services.AddLogging(builder =>
{
    var logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();
    builder.AddSerilog(logger);
});
services.AddSingleton<FeederCalculationService>();
services.AddSingleton<ExtractService>();

services.AddSingleton<FeederCalculationCommand>();
services.AddSingleton<ExtractsCommand>();

using var ServiceProvider = services.BuildServiceProvider();

//Define commands / subcommands
var rootCommand = new RootCommand("Background Process Application").AddGlobalOptions();
rootCommand.AddCommand(ServiceProvider.GetRequiredService<FeederCalculationCommand>());
rootCommand.AddCommand(ServiceProvider.GetRequiredService<ExtractsCommand>());
//rootCommand.AddFeederCalcCommand();
//rootCommand.AddExtractsCommand(ServiceProvider);

var invoked = await rootCommand.InvokeAsync(args);
Console.WriteLine($"Invoked Status: {invoked}");