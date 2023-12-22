namespace BackgroundProcesses.Services.Interfaces;
public interface ICommandProcess<T> where T : CommandOptions
{
    Task Execute(T options, CancellationToken cancellationToken);
}