using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();
builder.Services.AddControllersWithViews();
//builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddSingleton<IKubernetes>(KubernetesClientExtensions.BuildConfigFromEnvVariable());
builder.Services.AddSingleton<KubernetesService>();
builder.Services.Configure<HostOptions>(option =>
{
    option.ShutdownTimeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();
app.UseCors("corsapp");

//app.MapGet("/", () => "Hello World!");
app.MapGet("/k8s/pods", async () =>
{
    var service = app.Services.GetRequiredService<KubernetesService>();
    var list = await service.GetPods("default");
    var podList = list.Items.Select(pod => pod.Metadata.Name).ToList();
    return Results.Ok(podList);
});
app.MapGet("/k8s/jobs", async () =>
{
    var service = app.Services.GetRequiredService<KubernetesService>();
    var list = await service.GetJobs("default");
    var jobList = list.Items.Select(pod => pod.Metadata.Name).ToList();
    return Results.Ok(jobList);
});
app.MapGet("/k8s/jobs/start/sample", async () =>
{
    var service = app.Services.GetRequiredService<KubernetesService>();
    string[] args = ["FeederCalculation", "--consumer-id", "17"];
    var job = SampleBackgroundCliProcess.CreateJob("FeederCalc", "default",  args);
    await service.DeployJob(job, "default");
});



app.MapGet("/k8s/jobs/cli/{subcommand}", async ([FromRoute] string subcommand, [FromHeader(Name = "Command-Args")] string commandArgs) =>
{
    if (string.IsNullOrWhiteSpace(commandArgs)) {
        return Results.BadRequest($"Header '{nameof(commandArgs)}' is missing");
    }

    try
    {
        var service = app.Services.GetRequiredService<KubernetesService>();
        List<string> args = [.. commandArgs.Split(' ')];
        args.Insert(0, subcommand);
        var job = SampleBackgroundCliProcess.CreateJob("FeederCalc", "default", [.. args]);
        await service.DeployJob(job, "default");
        return Results.Ok();
    } catch (Exception ex)
    {
        return Results.Problem(ex.ToString());
    }
});


app.MapGet("/k8s/logs/pod/{podName}", async (string podName, CancellationToken token) =>
{
    var service = app.Services.GetRequiredService<KubernetesService>();
    await service.GetPodLogs(podName, "default", cancellationToken: token);
    return Task.CompletedTask;
});

// Map the hub
app.MapHub<KubernetesLogHub>("/kubernetesLogs");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
