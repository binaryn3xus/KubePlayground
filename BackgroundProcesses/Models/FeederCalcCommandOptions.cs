namespace BackgroundProcesses.Models;

public class FeederCalcCommandOptions : CommandOptions
{
    // do we need a setter in the interface?
    public int? ConsumerId { get; set; }

    // do we need a setter in the interface?
    public string? MsSqlConnection { get; set; }
}
