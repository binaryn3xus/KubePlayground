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
                                    Name = "busy-box",
                                    Image = "busybox:1.28",
                                    Command = new List<string> { "/bin/sh", "-c" },
                                    Args = new List<string>
                                    {
                                        $@"i=0; while [ $i -lt {waitTimeInSeconds} ]; do head /dev/urandom | tr -dc A-Za-z0-9 | head -c 13; echo; i=$(($i+5)); sleep 5; done"
                                    }
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

