using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.DTOs;
using IEC104.Master.Infrastructure.Adapters;
using IEC104.Master.Infrastructure.Models;
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

            // در Iec104Transport.cs

            _adapter.AsduReceived += parsedAsdu =>
            {
                // تبدیل ParsedASDU به ProtocolMessageDto
                var dto = new ProtocolMessageDto
                {
                    Timestamp = parsedAsdu.Timestamp,
                    Kind = "ASDU",
                    Title = parsedAsdu.TypeName,
                    Details = $"Type={parsedAsdu.TypeId}, Points={parsedAsdu.InformationObjects.Count}",
                    TypeId = (int)parsedAsdu.TypeId,
                    TypeName = parsedAsdu.TypeName,
                    CauseOfTransmission = parsedAsdu.CauseOfTransmission,
                    CotDescription = parsedAsdu.CotDescription,
                    CommonAddress = parsedAsdu.CommonAddress,
                    Points = parsedAsdu.InformationObjects.Select(io => new InformationPointDto
                    {
                        ObjectAddress = io.InformationObjectAddress,
                        ValueType = io.ValueType,
                        Value = io.Value,
                        Quality = io.Quality?.ToString() ?? "N/A"
                    }).ToList()
                };

                MessageReceived?.Invoke(dto);
            };
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

        public async Task DisconnectAsync(CancellationToken cancellationToken = default)
        {
            // اجرای قطع اتصال در یک ترد جداگانه
            await Task.Run(() =>
            {
                _adapter.Disconnect();
                // ★ به‌روزرسانی دستی وضعیت پس از قطع اتصال
                ConnectionStateChanged?.Invoke(false);
                MessageReceived?.Invoke(new ProtocolMessageDto(
                    DateTimeOffset.Now, "Info", "Connection", "Disconnected"));
            });
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

        private string FormatParsedASDU(ParsedASDU parsedAsdu)
        {
            var result = new StringBuilder();
            result.AppendLine($"Type: {parsedAsdu.TypeName} ({parsedAsdu.TypeId})");
            result.AppendLine($"COT: {parsedAsdu.CotDescription} ({parsedAsdu.CauseOfTransmission})");
            result.AppendLine($"Common Address: {parsedAsdu.CommonAddress}");
            result.AppendLine($"Timestamp: {parsedAsdu.Timestamp:yyyy-MM-dd HH:mm:ss.fff}");
            result.AppendLine($"Information Objects: {parsedAsdu.InformationObjects.Count}");

            foreach (var io in parsedAsdu.InformationObjects)
            {
                result.AppendLine($"  IOA: {io.InformationObjectAddress}, Type: {io.ValueType}, Value: {io.Value}");
                if (io.Quality != null)
                {
                    result.AppendLine($"    Quality: Overflow={io.Quality.Overflow}, Blocked={io.Quality.Blocked}, Substituted={io.Quality.Substituted}, NonTopical={io.Quality.NonTopical}, Invalid={io.Quality.Invalid}");
                }
            }

            return result.ToString();
        }
    }
}