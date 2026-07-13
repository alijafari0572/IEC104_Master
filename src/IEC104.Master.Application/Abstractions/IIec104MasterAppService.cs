using System;
using System.Collections.Generic;
using System.Text;
using IEC104.Master.Application.DTOs;

namespace IEC104.Master.Application.Abstractions
{
    public interface IIec104MasterAppService
    {
        event Action<ProtocolMessageDto>? MessagePublished;

        event Action<ConnectionStateDto>? ConnectionStateChanged;

        Task ConnectAsync(ConnectRequestDto request, CancellationToken cancellationToken = default);

        Task DisconnectAsync(CancellationToken cancellationToken = default);

        Task SendGeneralInterrogationAsync(GeneralInterrogationRequestDto request, CancellationToken cancellationToken = default);

        /// <summary>
        /// ارسال درخواست خواندن یک نقطه‌ی خاص
        /// </summary>
        Task SendSinglePointReadAsync(int ioa);
    }
}