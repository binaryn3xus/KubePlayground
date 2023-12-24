namespace KubePlayground.Kubernetes.Jobs;

public static class SampleBackgroundCliProcess
{
    public static V1Job CreateJob(string name, string @namespace, string[] args)
    {
        ArgumentNullException.ThrowIfNull(name);

        return new V1Job()
        {
            ApiVersion = "batch/v1",
            Kind = "Job",
            Metadata = new V1ObjectMeta
            {
                Name = name.ToLower() ?? nameof(SampleBackgroundCliProcess).ToLower(),
                NamespaceProperty = @namespace,
            },
            Spec = new V1JobSpec
            {
                TtlSecondsAfterFinished = 60,
                Template = new V1PodTemplateSpec
                {
                    Spec = new V1PodSpec
                    {
                        Containers = new List<V1Container>
                            {
                                new() {
                                    Name = "background-process-cli",
                                    Image = "ghcr.io/binaryn3xus/kubeplayground:0.0.3",
                                    ImagePullPolicy = "IfNotPresent",
                                    Args = args
                                }
                            },
                        TerminationGracePeriodSeconds = 60,
                        RestartPolicy = "Never"
                    }
                }
            }
        };
    }
}