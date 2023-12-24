namespace KubePlayground.Services;

public class KubernetesService(IKubernetes kubernetesClient)
{
    private readonly IKubernetes _kubernetesClient = kubernetesClient;

    public async Task<V1PodList> GetPods(string kubernetesNamespace = "default", CancellationToken cancellationToken = default)
    {
        return await _kubernetesClient.CoreV1.ListNamespacedPodAsync(kubernetesNamespace, cancellationToken: cancellationToken);
    }

    public async Task<V1JobList> GetJobs(string kubernetesNamespace = "default", CancellationToken cancellationToken = default)
    {
        return await _kubernetesClient.BatchV1.ListNamespacedJobAsync(kubernetesNamespace, cancellationToken: cancellationToken);
    }

    public async Task<V1Job> DeployJob(V1Job jobObject, string kubernetesNamespace = "default", CancellationToken cancellationToken = default)
    {
        return await _kubernetesClient.BatchV1.CreateNamespacedJobAsync(jobObject, kubernetesNamespace, cancellationToken: cancellationToken);
    }

    public async Task GetPodLogs(string podName, string kubernetesNamespace = "default", CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(podName);

        var response = await _kubernetesClient.CoreV1.ReadNamespacedPodLogWithHttpMessagesAsync(podName, kubernetesNamespace, follow: true, cancellationToken: cancellationToken);
        var stream = response.Body;
        stream.CopyTo(Console.OpenStandardOutput());
    }

    //protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    //{
    //    while (!cancellationToken.IsCancellationRequested)
    //    {

    //        // Get the base directory of the application
    //        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

    //        // Construct the path to the template file
    //        string templateFilePath = Path.Combine(baseDirectory, "Templates", "JobSampleBusyBox.yaml");

    //        // Read the template from the file
    //        string template = File.ReadAllText(templateFilePath);

    //        var documentResult = Document.CreateDefault(template); // Create from template string
    //        var document = documentResult.DocumentOrThrow; // Throws ParseException on error

    //        //var template = "Hello {jobName}, stay awhile and listen {sleepTime}!";

    //        //var documentResult = Document.CreateDefault(template); // Create from template string
    //        //var document = documentResult.DocumentOrThrow; // Throws ParseException on error

    //        var context = Cottle.Context.CreateBuiltin(new Dictionary<Cottle.Value, Cottle.Value>
    //        {
    //            ["jobName"] = "poop-test",
    //            ["sleepTime"] = "3600"
    //        });

    //        _logger.LogInformation(document.Render(context));

    //        if (_logger.IsEnabled(LogLevel.Information))
    //        {
    //            _logger.LogInformation(
    //                "Worker running at: {time}",
    //                DateTimeOffset.Now
    //            );
    //        }
    //        await Task.Delay(5000, cancellationToken);
    //    }
    //}
}
