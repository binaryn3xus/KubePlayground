// See https://aka.ms/new-console-template for more information

Console.WriteLine("App Starting...");
var rootCommand = new RootCommand("Background Process Application");

//Define subcommands
#region Feeder Calc
var fcCommand = new Command("FeederCalculation", "Interface with feeder calculations");

var consumerIdOption = new Option<int>(name: "--ConsumerId", description: "Define a delay before the process starts");
consumerIdOption.IsRequired = true;
fcCommand.Add(consumerIdOption);

fcCommand.SetHandler((consumerId) => { new FeederCalculation().Execute(consumerId); }, consumerIdOption);

rootCommand.Add(fcCommand);

#endregion

#region Extract
var extractCommand = new Command("Extract", "Interface with Extract Process");

var isDailyOption = new Option<bool>(name: "--IsDaily", description: "Defines if you want to start a daily extract", getDefaultValue: () => false) { IsRequired = true };
extractCommand.Add(isDailyOption);

var isHourlyOption = new Option<bool>(name: "--IsHourly", description: "Defines if you want to start a hourly extract", getDefaultValue: () => false) { IsRequired = true };
extractCommand.Add(isHourlyOption);

extractCommand.SetHandler((isDaily, isHourly) => { Extracts.Execute(isDaily, isHourly); }, isDailyOption, isHourlyOption);

rootCommand.Add(extractCommand);

#endregion

var invoked = await rootCommand.InvokeAsync(args);
Console.WriteLine($"Invoked Status: {invoked}");
