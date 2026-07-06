using IEC104.Master.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Application.Abstractions
{
    public interface IIec104Transport
    {
        event Action<ProtocolMessageDto>? MessageReceived;

        event Action<bool>? ConnectionStateChanged;

        Task ConnectAsync(ConnectRequestDto request, CancellationToken cancellationToken = default);

        Task DisconnectAsync(CancellationToken cancellationToken = default);

        Task SendGeneralInterrogationAsync(byte qoi, CancellationToken cancellationToken = default);
    }
}