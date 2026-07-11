using IEC104.Master.Infrastructure.Models;
using lib60870.CS101;
using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using lib60870.CS101;
using IEC104.Master.Infrastructure.Models;

namespace IEC104.Master.Infrastructure.Mappers
{
    public static class ASDUMapper
    {
        public static ParsedASDU MapToParsedASDU(ASDU asdu)
        {
            var parser = new ASDUParser();
            return parser.Parse(asdu);
        }

        public static string GetASDUSummary(ParsedASDU parsedAsdu)
        {
            return $"{parsedAsdu.TypeName} (CA: {parsedAsdu.CommonAddress}, COT: {parsedAsdu.CotDescription}) - {parsedAsdu.InformationObjects.Count} objects";
        }

        public static Dictionary<string, object> GetASDUDetails(ParsedASDU parsedAsdu)
        {
            return new Dictionary<string, object>
            {
                ["TypeId"] = parsedAsdu.TypeId,
                ["TypeName"] = parsedAsdu.TypeName,
                ["CauseOfTransmission"] = parsedAsdu.CauseOfTransmission,
                ["CotDescription"] = parsedAsdu.CotDescription,
                ["CommonAddress"] = parsedAsdu.CommonAddress,
                ["Timestamp"] = parsedAsdu.Timestamp,
                ["InformationObjectsCount"] = parsedAsdu.InformationObjects.Count
            };
        }
    }
}