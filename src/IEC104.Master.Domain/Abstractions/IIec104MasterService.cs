using IEC104.Master.Domain.Entities;
using IEC104.Master.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Domain.Abstractions
{
    public interface IIec104MasterService
    {
        event Action<ProtocolMessage>? MessageReceived;

        event Action<bool>? ConnectionStateChanged;

        Task ConnectAsync(ConnectionSettings settings, CancellationToken cancellationToken = default);

        Task DisconnectAsync(CancellationToken cancellationToken = default);

        Task SendGeneralInterrogationAsync(byte qoi = 20, CancellationToken cancellationToken = default);
    }
}