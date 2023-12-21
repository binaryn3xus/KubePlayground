namespace BackgroundProcesses.Processes;

public class Extracts
{
    public static Task Execute(bool isDaily, bool isHourly)
    {
        Console.WriteLine("Extracts Parameters:");
        Console.WriteLine($"Daily = {isDaily}");
        Console.WriteLine($"Hourly = {isHourly}");

        if (!isDaily && !isHourly)
        {
            Console.WriteLine("You must define either Daily or Hourly extract. Aborting Run...");
            return Task.FromException(new ArgumentException("Neither Daily or Hourly was defined for the Extract run"));
        }

        Console.WriteLine("Starting Extracts...");

        return Task.CompletedTask;
    }
}
