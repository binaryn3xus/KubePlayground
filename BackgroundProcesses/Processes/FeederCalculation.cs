namespace BackgroundProcesses.Processes;

public class FeederCalculation
{
    public Task Execute(int consumerId)
    {
        Console.WriteLine("Starting Feeder Calculation with Consumer Id of {0}", consumerId);
        return Task.CompletedTask;
    }
}
