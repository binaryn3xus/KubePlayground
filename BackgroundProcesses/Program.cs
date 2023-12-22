Console.WriteLine("App Starting...");

// Convert all args to lower-case
args = args.Select(arg => arg.ToLower()).ToArray();

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

Console.WriteLine(environmentName);

var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).Build();
var services = new ServiceCollection();
services.AddLogging(builder =>
{
    var logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();
    builder.AddSerilog(logger);
});
services.AddSingleton<FeederCalculationService>();
services.AddSingleton<ExtractService>();

using var ServiceProvider = services.BuildServiceProvider();

//Define commands / subcommands
var rootCommand = new RootCommand("Background Process Application");
rootCommand.AddFeederCalcCommand(ServiceProvider);
rootCommand.AddExtractsCommand(ServiceProvider);

var invoked = await rootCommand.InvokeAsync(args);
Console.WriteLine($"Invoked Status: {invoked}");