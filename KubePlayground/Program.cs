var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();
builder.Services.AddControllersWithViews();
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

// Register endpoints
app.EndpointsConfiguration();

// Map the hub
app.MapHub<KubernetesLogHub>("/kubernetesLogs");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
