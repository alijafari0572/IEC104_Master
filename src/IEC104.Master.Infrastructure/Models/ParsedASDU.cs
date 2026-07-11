using lib60870.CS101;
using System;

using System;

using System.Collections.Generic;

using System.Collections.Generic;

using System.Text;

namespace IEC104.Master.Infrastructure.Models
{
    public sealed record ParsedASDU(
        TypeID TypeId,
        string TypeName,
        int CauseOfTransmission,
        string CotDescription,
        int CommonAddress,
        DateTimeOffset Timestamp,
        List<ParsedInformationObject> InformationObjects);
}