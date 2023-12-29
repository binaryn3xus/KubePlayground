namespace BackgroundProcesses.Models;

public static class GlobalOptions
{
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

    public static Option?[] GetGlobalOptions()
    {
        return typeof(GlobalOptions)
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            .Where(field => typeof(Option).IsAssignableFrom(field.FieldType))
            .Select(field => field.GetValue(null) as Option)
            .ToArray();
    }

    public static RootCommand AddGlobalOptions(this RootCommand command)
    {
        var options = GetGlobalOptions();

        if (options != null)
        {
            foreach (var option in options)
            {
                command.AddGlobalOption(option);
            }
        }

        return command;
    }
}
