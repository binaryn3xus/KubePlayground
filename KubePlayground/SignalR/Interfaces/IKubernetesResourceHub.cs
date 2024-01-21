namespace KubePlayground.SignalR.Interfaces;

public interface IKubernetesResourceHub
{
    Task ReceivePods(string? log);
}
