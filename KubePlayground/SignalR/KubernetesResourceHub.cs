namespace KubePlayground.SignalR;

public class KubernetesResourceHub : Hub<IKubernetesResourceHub>
{
    private readonly IKubernetes _kubernetesClient;

    public KubernetesResourceHub(IKubernetes kubernetesClient)
    {
        _kubernetesClient = kubernetesClient;
    }

}
