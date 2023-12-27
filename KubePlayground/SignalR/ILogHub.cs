namespace KubePlayground.SignalR;

public interface ILogHub
{
    Task ReceiveLog(string? log);
}
