namespace BackgroundProcesses.Models;

public class ExtractCommandOptions : CommandOptions
{
    // do we need a setter in the interface?
    public bool IsDaily { get; set; }

    // do we need a setter in the interface?
    public bool IsHourly { get; set; }

    // do we need a setter in the interface?
    public string? MsSqlConnection { get; set; }
}
