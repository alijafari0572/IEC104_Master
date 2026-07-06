using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Domain.Enums
{
    public enum MessageKind
    {
        Info = 0,
        Success = 1,
        Warning = 2,
        Error = 3,
        FrameReceived = 4,
        FrameSent = 5
    }
}