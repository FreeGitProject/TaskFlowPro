using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlowPro.Shared.Interfaces
{
    public interface ITaskHubClient
    {
        Task TaskUpdated(string message);
        Task NotificationReceived(string message);
    }
}
