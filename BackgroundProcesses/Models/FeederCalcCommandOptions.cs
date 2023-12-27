namespace BackgroundProcesses.Models;

public class FeederCalcCommandOptions : CommandOptions
{
    public int? ConsumerId { get; set; }
    public string? MsSqlConnection { get; set; }
}
