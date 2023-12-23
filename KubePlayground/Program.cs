using k8s.Models;
using KubePlayground.Kubernetes.Jobs;
using KubePlayground.Services;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();
builder.Services.AddSingleton<KubernetesService>();
builder.Services.Configure<HostOptions>(option =>
{
    option.ShutdownTimeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/pods", async () =>
{
    var service = app.Services.GetRequiredService<KubernetesService>();
    var list = await service.GetPods("default");
    StringBuilder sbPods = new();
    if (list.Items.Count > 0)
    {
        foreach (var item in list)
        {
            sbPods.AppendLine(item.Name());
        }
    }
    else
    {
        sbPods.AppendLine("No Pods Found");
    }
    return sbPods.ToString();
});
app.MapGet("/jobs", async () =>
{
    var service = app.Services.GetRequiredService<KubernetesService>();
    var list = await service.GetJobs("default");
    StringBuilder sbPods = new();
    if (list.Items.Count > 0)
    {
        foreach (var item in list)
        {
            sbPods.AppendLine(item.Name());
        }
    }
    else
    {
        sbPods.AppendLine("No Jobs Found");
    }
    return sbPods.ToString();
});
app.MapGet("/jobs/start/sample", async () =>
{
    var service = app.Services.GetRequiredService<KubernetesService>();
    var job = SampleJob.CreateJob("sample-job", 120);
    await service.DeployJob(job, "default");
});
app.MapGet("/logs/pod/{podName}", async (string podName, CancellationToken token) =>
{
    var service = app.Services.GetRequiredService<KubernetesService>();
    await service.GetPodLogs(podName, "default", cancellationToken: token);
    return Task.CompletedTask;
});

app.Run();
