using Microsoft.AspNetCore.SignalR;
using System.Reactive.Linq;

namespace KubePlayground.SignalR;

public class KubernetesLogHub : Hub<ILogHub>
{
    private readonly IKubernetes _kubernetesClient;

    public KubernetesLogHub(IKubernetes kubernetesClient)
    {
        _kubernetesClient = kubernetesClient;
    }

    public async Task StreamLogs(string podName, string @namespace)
    {
        var response = await _kubernetesClient.CoreV1.ReadNamespacedPodLogWithHttpMessagesAsync(podName, @namespace, follow: true);
        var stream = response.Body;
        var reader = new StreamReader(stream);

        // Create an observable from the stream reader
        var lines = Observable.FromAsync(() => reader.ReadLineAsync())
            .Repeat()
            .TakeWhile(line => line != null);

        // Subscribe to the observable and send each line to the client
        await lines.ForEachAsync(line =>
        {
            _ = Clients.Caller.ReceiveLog(line);
        });
    }
}
