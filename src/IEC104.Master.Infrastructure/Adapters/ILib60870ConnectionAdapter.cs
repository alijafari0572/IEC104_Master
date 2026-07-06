using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Infrastructure.Adapters
{
    public interface ILib60870ConnectionAdapter : IDisposable
    {
        event Action? Connected;

        event Action? Disconnected;

        event Action<string>? ErrorOccurred;

        event Action<string>? AsduReceived;

        bool Connect(string host, int port, int commonAddress, int timeoutMs);

        void Disconnect();

        bool SendGeneralInterrogation(byte qoi, int commonAddress);
    }
}