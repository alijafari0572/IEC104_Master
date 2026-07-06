using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.DTOs;
using IEC104.Master.Infrastructure.Adapters;
using IEC104.Master.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Infrastructure.Services
{
    public sealed class Iec104Transport : IIec104Transport, IDisposable
    {
        private readonly ILib60870ConnectionAdapter _adapter;
        private int _commonAddress = 1;

        public event Action<ProtocolMessageDto>? MessageReceived;

        public event Action<bool>? ConnectionStateChanged;

        public Iec104Transport(ILib60870ConnectionAdapter adapter)
        {
            _adapter = adapter;

            _adapter.Connected += () =>
            {
                ConnectionStateChanged?.Invoke(true);
                MessageReceived?.Invoke(new ProtocolMessageDto(
                    DateTimeOffset.Now, "Info", "Connection", "Connected"));
            };

            _adapter.Disconnected += () =>
            {
                ConnectionStateChanged?.Invoke(false);
                MessageReceived?.Invoke(new ProtocolMessageDto(
                    DateTimeOffset.Now, "Info", "Connection", "Disconnected"));
            };

            _adapter.ErrorOccurred += err =>
                MessageReceived?.Invoke(new ProtocolMessageDto(
                    DateTimeOffset.Now, "Error", "Transport Error", err));

            _adapter.AsduReceived += asdu =>
                MessageReceived?.Invoke(new ProtocolMessageDto(
                    DateTimeOffset.Now, "FrameReceived", "ASDU", asdu));
        }

        public Task ConnectAsync(ConnectRequestDto request, CancellationToken cancellationToken = default)
        {
            _commonAddress = request.CommonAddress;

            var ok = _adapter.Connect(request.Host, request.Port, request.CommonAddress, request.TimeoutMs);
            if (!ok)
            {
                MessageReceived?.Invoke(new ProtocolMessageDto(
                    DateTimeOffset.Now, "Error", "Connect", "Connection failed"));
            }

            return Task.CompletedTask;
        }

        public Task DisconnectAsync(CancellationToken cancellationToken = default)
        {
            _adapter.Disconnect();
            return Task.CompletedTask;
        }

        public Task SendGeneralInterrogationAsync(byte qoi, CancellationToken cancellationToken = default)
        {
            var ok = _adapter.SendGeneralInterrogation(qoi, _commonAddress);
            if (!ok)
            {
                MessageReceived?.Invoke(new ProtocolMessageDto(
                    DateTimeOffset.Now, "Error", "GI", "Interrogation failed"));
            }

            return Task.CompletedTask;
        }

        public void Dispose() => _adapter.Dispose();
    }
}