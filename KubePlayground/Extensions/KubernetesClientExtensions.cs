namespace KubePlayground.Extensions;

public static class KubernetesClientExtensions
{
    public static k8s.Kubernetes BuildConfigFromEnvVariable(string envVarName = "KUBE_CONFIG")
    {
        var kubeConfigBase64 = Environment.GetEnvironmentVariable(envVarName);
        if (string.IsNullOrEmpty(kubeConfigBase64))
        {
            Console.WriteLine($"Error: {envVarName} environment variable not set.");
            throw new ArgumentException($"Error: {envVarName} environment variable not set.");
        }

        // Decode the base64 string into bytes
        byte[] kubeConfigBytes = Convert.FromBase64String(kubeConfigBase64);

        // Convert the bytes to a UTF-8 encoded string (YAML content)
        string kubeConfigYaml = Encoding.UTF8.GetString(kubeConfigBytes);

        // Create an instance of the deserializer
        var deserializer = new DeserializerBuilder().Build();

        // Deserialize the kube config YAML into the K8SConfiguration object
        var k8sConfig = deserializer.Deserialize<K8SConfiguration>(kubeConfigYaml);
        var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(k8sConfig);
        return new k8s.Kubernetes(config);
    }
}
