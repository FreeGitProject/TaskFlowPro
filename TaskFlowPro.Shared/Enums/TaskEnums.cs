using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlowPro.Shared.Enums
{
    public enum TaskStatus
    {
        New,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
