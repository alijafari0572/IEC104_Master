using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Application.DTOs
{
    public sealed record ConnectionStateDto(
        bool IsConnected,
        string StatusText);
}