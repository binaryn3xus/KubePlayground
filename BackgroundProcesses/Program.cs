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


// Define Services
services.AddSingleton<FeederCalculationService>();
services.AddSingleton<ExtractService>();

// Define Commands
services.AddSingleton<FeederCalculationCommand>();
services.AddSingleton<ExtractsCommand>();

using var ServiceProvider = services.BuildServiceProvider();

// Setup Root/Sub commands
var rootCommand = new RootCommand("Background Process Application")
    .AddGlobalOptions()
    .AddProjectCommands(ServiceProvider);

var invoked = await rootCommand.InvokeAsync(args);
Console.WriteLine($"Invoked Status: {invoked}");