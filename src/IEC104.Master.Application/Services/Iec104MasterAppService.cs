using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Application.Services
{
    public sealed class Iec104MasterAppService : IIec104MasterAppService
    {
        private readonly IIec104Transport _transport;

        public event Action<ProtocolMessageDto>? MessagePublished;

        public event Action<ConnectionStateDto>? ConnectionStateChanged;

        private int _commonAddress = 1;

        public Iec104MasterAppService(IIec104Transport transport)
        {
            _transport = transport;
            _transport.MessageReceived += m => MessagePublished?.Invoke(m);
            _transport.ConnectionStateChanged += s => ConnectionStateChanged?.Invoke(
                new ConnectionStateDto(s, s ? "Connected" : "Disconnected"));
        }

        public Task ConnectAsync(ConnectRequestDto request, CancellationToken cancellationToken = default)
            => _transport.ConnectAsync(request, cancellationToken);

        public Task DisconnectAsync(CancellationToken cancellationToken = default)
            => _transport.DisconnectAsync(cancellationToken);

        public Task SendGeneralInterrogationAsync(GeneralInterrogationRequestDto request, CancellationToken cancellationToken = default)
            => _transport.SendGeneralInterrogationAsync(request.Qoi, cancellationToken);

        public async Task SendSinglePointReadAsync(int ioa)
        {
            await _transport.SendSinglePointReadAsync(ioa, _commonAddress);
        }
    }
}