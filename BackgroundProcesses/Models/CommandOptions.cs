namespace BackgroundProcesses.Models;

public abstract class CommandOptions {
    public string? MsSqlConnection { get; set; }
    public string? Db2Connection { get; set; }
}
