namespace BackgroundProcesses.Models;

public abstract class CommandOptions
{
    public string? MsSqlConnection { get; }
    public string? Db2Connection { get; }
}
