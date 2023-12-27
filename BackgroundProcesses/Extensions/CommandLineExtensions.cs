namespace BackgroundProcesses.Extensions;

public static class CommandLineExtensions
{
    public static RootCommand AddProjectCommands(this RootCommand rootCommand, IServiceProvider serviceProvider)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        var commandTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Command)));

        foreach (var type in commandTypes)
        {
            var command = (Command)serviceProvider.GetRequiredService(type);
            rootCommand.AddCommand(command);
        }

        return rootCommand;
    }
}
