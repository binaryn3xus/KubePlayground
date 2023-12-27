namespace BackgroundProcesses.Models;

public class ExtractCommandOptions : CommandOptions
{
    public bool IsDaily { get; set; }
    public bool IsHourly { get; set; }
    public string? MsSqlConnection { get; set; }
}
