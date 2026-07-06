using System;
using System.Collections.Generic;
using System.Text;
using IEC104.Master.Domain.Enums;

namespace IEC104.Master.Domain.Entities
{
    public sealed record ProtocolMessage(
        DateTimeOffset Timestamp,
        MessageKind Kind,
        string Title,
        string Details);
}