namespace KubePlayground.SignalR.Interfaces;

public interface IKubernetesLogHub
{
    Task ReceiveLog(string? log);
}
