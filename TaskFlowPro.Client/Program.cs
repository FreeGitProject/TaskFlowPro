using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TaskFlowPro.Client.Extensions;
using TaskFlowPro.Client.Services;
using TaskFlowPro.Shared.Interfaces;
using TaskFlowPro.Shared.SignalR;

namespace TaskFlowPro.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        //var builder = WebAssemblyHostBuilder.CreateDefault(args);
        //builder.RootComponents.Add<App>("#app");
        //builder.RootComponents.Add<HeadOutlet>("head::after");

        //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        //builder.Services.AddMudServices();
        //await builder.Build().RunAsync();
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        // Configure HttpClient
        builder.Services.AddScoped(sp =>
            new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        // Add MudBlazor services
        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
        });

        // Register services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITaskService, TaskService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();

        // Register SignalR hub connection factory
        builder.Services.AddSingleton(sp =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            var baseUrl = navigationManager.BaseUri;
            var authStateProvider = sp.GetRequiredService<AuthStateProvider>();

            return new Func<ITaskHubClient, Task<TaskHubConnection>>(client =>
            {
                var connection = new TaskHubConnection(
                    $"{baseUrl}taskhub",
                    client,
                    authStateProvider.Token);

                return Task.FromResult(connection);
            });
        });

        await builder.Build().RunAsync();
    }
}
