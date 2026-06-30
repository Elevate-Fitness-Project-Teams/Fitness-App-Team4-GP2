using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Contracts.Progress
{
    public class SessionCompletedEvent
    {
        public string SessionId { get; set; } = null!;
    }
}
