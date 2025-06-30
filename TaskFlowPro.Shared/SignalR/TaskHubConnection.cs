using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlowPro.Shared.Interfaces;

namespace TaskFlowPro.Shared.SignalR
{
    public class TaskHubConnection
    {
        private readonly HubConnection _connection;

        public TaskHubConnection(string hubUrl, ITaskHubClient client, string token)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .ConfigureLogging(logging =>
                {
                    //logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .Build();

            _connection.On<string>("TaskUpdated", client.TaskUpdated);
            _connection.On<string>("NotificationReceived", client.NotificationReceived);
        }

        public async Task StartAsync()
        {
            await _connection.StartAsync();
        }

        public async Task JoinTaskGroup(int taskId)
        {
            await _connection.InvokeAsync("JoinTaskGroup", taskId);
        }

        public async Task LeaveTaskGroup(int taskId)
        {
            await _connection.InvokeAsync("LeaveTaskGroup", taskId);
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }
        }
    }
}
