using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Domain.Enums
{
    public enum ConnectionState
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2,
        Disconnecting = 3,
        Faulted = 4
    }
}