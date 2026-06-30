using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Contracts.Progress
{
    public class SessionStartedEvent
    {
        public string SessionId { get; init; } = null!;
        public string UserId { get; set; } = null!;
    }
}
