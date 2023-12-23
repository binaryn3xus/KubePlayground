using k8s.Models;

namespace KubePlayground.Kubernetes.Jobs
{
    public static class SampleJob
    {
        public static V1Job CreateJob(string name, int waitTimeInSeconds)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(waitTimeInSeconds);

            return new V1Job()
            {
                ApiVersion = "batch/v1",
                Kind = "Job",
                Metadata = new V1ObjectMeta
                {
                    Name = name.ToLower() ?? nameof(SampleJob).ToLower()
                },
                Spec = new V1JobSpec
                {
                    Template = new V1PodTemplateSpec
                    {
                        Spec = new V1PodSpec
                        {
                            Containers = new List<V1Container>
                            {
                                new() {
                                    Name = "feeder-calc",
                                    Image = "ghcr.io/binaryn3xus/kubeplayground:0.0.3",
                                    ImagePullPolicy = "IfNotPresent",
                                    Args = new List<string>{ "FeederCalculation", "--consumer-id", "17" }
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
}

