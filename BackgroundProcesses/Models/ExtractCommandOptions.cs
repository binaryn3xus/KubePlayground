namespace BackgroundProcesses.Models;

public class ExtractCommandOptions : CommandOptions
{
    public bool IsDaily { get; }
    public bool IsHourly { get; }
}
