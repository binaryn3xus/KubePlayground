namespace BackgroundProcesses.Models;

public abstract class CommandOptions
{
    public string? MsSqlConnection { get; set; }
    public string? Db2Connection { get; set; }

    public void LogProperties(Microsoft.Extensions.Logging.ILogger logger)
    {
        logger.LogInformation("Options ({Type}):", GetType().Name);
        foreach (var prop in GetType().GetProperties())
        {
            var value = prop.GetValue(this);
            if (value != null)
            {
                logger.LogInformation("{PropertyName} = {PropertyValue}", prop.Name, value);
            }
        }
    }
}
