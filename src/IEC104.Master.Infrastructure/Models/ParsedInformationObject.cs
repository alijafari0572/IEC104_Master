using System;
using System.Collections.Generic;
using System.Text;
using lib60870.CS101;

namespace IEC104.Master.Infrastructure.Models
{
    public sealed record ParsedInformationObject(
        int InformationObjectAddress,
        string ValueType,
        object Value,
        QualityDescriptor? Quality = null);
}