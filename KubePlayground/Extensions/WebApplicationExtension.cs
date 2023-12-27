using Microsoft.AspNetCore.Mvc;

namespace KubePlayground.Extensions;

public static class WebApplicationExtension
{
    public static void EndpointsConfiguration(this WebApplication app)
    {
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
            var job = SampleBackgroundCliProcess.CreateJob("FeederCalc", "default", args);
            await service.DeployJob(job, "default");
        });

        app.MapGet("/k8s/jobs/cli/{subcommand}", async ([FromRoute] string subcommand, [FromHeader(Name = "Command-Args")] string commandArgs) =>
        {
            if (string.IsNullOrWhiteSpace(commandArgs))
            {
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
            }
            catch (Exception ex)
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
    }
}

