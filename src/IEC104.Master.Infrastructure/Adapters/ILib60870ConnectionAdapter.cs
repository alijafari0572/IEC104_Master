using System;
using System.Collections.Generic;
using System.Text;
using IEC104.Master.Infrastructure.Models;

namespace IEC104.Master.Infrastructure.Adapters
{
    public interface ILib60870ConnectionAdapter : IDisposable
    {
        event Action? Connected;

        event Action? Disconnected;

        event Action<string>? ErrorOccurred;

        event Action<ParsedASDU>? AsduReceived;

        bool Connect(string host, int port, int commonAddress, int timeoutMs);

        void Disconnect();

        bool SendGeneralInterrogation(byte qoi, int commonAddress);
    }
}