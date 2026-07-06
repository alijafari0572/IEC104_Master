using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Domain.ValueObjects
{
    public sealed record ConnectionSettings(
        string Host = "172.18.20.186",
        int Port = 2404,
        int CommonAddress = 1,
        int TimeoutMs = 5000,
        bool UseTls = false);
}