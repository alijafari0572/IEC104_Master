using lib60870.CS101;
using lib60870.CS104;
using IEC104.Master.Infrastructure.Models;
using IEC104.Master.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Infrastructure.Adapters
{
    public sealed class Lib60870ConnectionAdapter : ILib60870ConnectionAdapter
    {
        private Connection? _connection;
        private readonly ASDUParser _parser = new ASDUParser();

        public event Action? Connected;

        public event Action? Disconnected;

        public event Action<string>? ErrorOccurred;

        public event Action<ParsedASDU>? AsduReceived;

        public bool Connect(string host, int port, int commonAddress, int timeoutMs)
        {
            try
            {
                _connection = new Connection(host, port);
                _connection.SetConnectTimeout(timeoutMs);
                _connection.SetConnectionHandler(OnConnectionHandler, null);
                _connection.SetASDUReceivedHandler(OnAsduReceived, null);
                _connection.Connect();
                return true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                if (_connection is null) return;
                _connection.Close();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        public bool SendGeneralInterrogation(byte qoi, int commonAddress)
        {
            try
            {
                if (_connection is null) return false;
                _connection.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, commonAddress, qoi);
                return true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
                return false;
            }
        }

        public bool SendReadCommand(int ioa, int commonAddress)
        {
            try
            {
                if (_connection is null) return false;

                // ارسال فرمان خواندن (Read Command) برای IOA مشخص
                _connection.SendReadCommand(commonAddress, ioa);
                return true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"SendReadCommand failed: {ex.Message}");
                return false;
            }
        }

        private void OnConnectionHandler(object parameter, ConnectionEvent connectionEvent)
        {
            switch (connectionEvent)
            {
                case ConnectionEvent.OPENED:
                    Connected?.Invoke();
                    break;

                case ConnectionEvent.CLOSED:
                case ConnectionEvent.CONNECT_FAILED:
                    Disconnected?.Invoke();
                    break;
            }
        }

        private bool OnAsduReceived(object parameter, ASDU asdu)
        {
            var parsedAsdu = _parser.Parse(asdu);
            AsduReceived?.Invoke(parsedAsdu);
            return true;
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection = null;
        }
    }
}