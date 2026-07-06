using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Application.DTOs
{
    public sealed record ConnectRequestDto(
        string Host,
        int Port,
        int CommonAddress,
        int TimeoutMs,
        bool UseTls);
}