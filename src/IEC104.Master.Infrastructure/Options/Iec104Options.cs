using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Infrastructure.Options
{
    public sealed class Iec104Options
    {
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 2404;
        public int CommonAddress { get; set; } = 1;
        public int TimeoutMs { get; set; } = 5000;
        public bool UseTls { get; set; } = false;
    }
}