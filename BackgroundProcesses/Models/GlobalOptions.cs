namespace BackgroundProcesses.Models;

public static class GlobalOptions
{
    // that is field --> we need property
    public static Option<string> MicrosoftSqlConnection { get; } = new("--mssql-connection")
    {
        Description = "Connection string for Microsoft SQL Server",
        IsRequired = false,
    };

    public static Option<string> Db2Connection { get; } = new("--db2-connection")
    {
        Description = "Connection string for Microsoft SQL Server",
        IsRequired = false,
    };

    public static Option[] GetGlobalOptions() => new Option[] { MicrosoftSqlConnection, Db2Connection };

    public static RootCommand AddGlobalOptions(this RootCommand command)
    {
        var options = GetGlobalOptions();
        foreach (var option in options)
        {
            command.AddGlobalOption(option);
        }
        return command;
    }
}
