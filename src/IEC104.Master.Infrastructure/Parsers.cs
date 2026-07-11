using IEC104.Master.Infrastructure.Models;
using lib60870.CS101;
using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using lib60870.CS101;
using IEC104.Master.Infrastructure.Models;

namespace IEC104.Master.Infrastructure
{
    public class ASDUParser
    {
        public ParsedASDU Parse(ASDU asdu)
        {
            var typeId = asdu.TypeId;
            var cot = asdu.Cot;
            var ca = asdu.Ca;

            var informationObjects = ParseInformationObjects(asdu);

            return new ParsedASDU(
                typeId,
                GetTypeName(typeId),
                (int)cot,
                GetCotDescription(cot),
                ca,
                DateTimeOffset.Now,
                informationObjects);
        }

        private List<ParsedInformationObject> ParseInformationObjects(ASDU asdu)
        {
            var result = new List<ParsedInformationObject>();

            for (int i = 0; i < asdu.NumberOfElements; i++)
            {
                var informationObject = asdu.GetElement(i);
                var parsedObject = ParseInformationObject(informationObject);
                result.Add(parsedObject);
            }

            return result;
        }

        private ParsedInformationObject ParseInformationObject(InformationObject informationObject)
        {
            var objectAddress = informationObject.ObjectAddress;
            var value = ExtractValue(informationObject);
            var quality = ExtractQuality(informationObject);

            return new ParsedInformationObject(
                objectAddress,
                GetValueType(informationObject),
                value,
                quality);
        }

        private object ExtractValue(InformationObject informationObject)
        {
            switch (informationObject)
            {
                case SinglePointInformation spi:
                    return spi.Value;

                case DoublePointInformation dpi:
                    return dpi.Value;

                case MeasuredValueNormalized mvn:
                    return mvn.NormalizedValue;

                case MeasuredValueScaled mvs:
                    return mvs.ScaledValue;

                case MeasuredValueShort mvs:
                    return mvs.Value;

                default:
                    return informationObject.ToString();
            }
        }

        private QualityDescriptor? ExtractQuality(InformationObject informationObject)
        {
            switch (informationObject)
            {
                case SinglePointInformation spi:
                    return spi.Quality;

                case DoublePointInformation dpi:
                    return dpi.Quality;

                default:
                    return null;
            }
        }

        private string GetValueType(InformationObject informationObject)
        {
            return informationObject.GetType().Name;
        }

        private string GetTypeName(TypeID typeId)
        {
            return typeId switch
            {
                TypeID.M_SP_NA_1 => "M_SP_NA_1",
                TypeID.M_SP_TA_1 => "M_SP_TA_1",
                TypeID.M_DP_NA_1 => "M_DP_NA_1",
                TypeID.M_DP_TA_1 => "M_DP_TA_1",
                TypeID.M_ST_NA_1 => "M_ST_NA_1",
                TypeID.M_ST_TA_1 => "M_ST_TA_1",
                TypeID.M_BO_NA_1 => "M_BO_NA_1",
                TypeID.M_BO_TA_1 => "M_BO_TA_1",
                TypeID.M_ME_NA_1 => "M_ME_NA_1",
                TypeID.M_ME_TA_1 => "M_ME_TA_1",
                TypeID.M_ME_NB_1 => "M_ME_NB_1",
                TypeID.M_ME_TB_1 => "M_ME_TB_1",
                TypeID.M_ME_NC_1 => "M_ME_NC_1",
                TypeID.M_ME_TC_1 => "M_ME_TC_1",
                TypeID.M_IT_NA_1 => "M_IT_NA_1",
                TypeID.M_IT_TA_1 => "M_IT_TA_1",
                TypeID.M_EP_TA_1 => "M_EP_TA_1",
                TypeID.M_EP_TB_1 => "M_EP_TB_1",
                TypeID.M_EP_TC_1 => "M_EP_TC_1",
                TypeID.M_PS_NA_1 => "M_PS_NA_1",
                TypeID.M_ME_ND_1 => "M_ME_ND_1",
                TypeID.M_SP_TB_1 => "M_SP_TB_1",
                TypeID.M_DP_TB_1 => "M_DP_TB_1",
                TypeID.M_ST_TB_1 => "M_ST_TB_1",
                TypeID.M_BO_TB_1 => "M_BO_TB_1",
                TypeID.M_ME_TD_1 => "M_ME_TD_1",
                TypeID.M_ME_TE_1 => "M_ME_TE_1",
                TypeID.M_ME_TF_1 => "M_ME_TF_1",
                TypeID.M_IT_TB_1 => "M_IT_TB_1",
                TypeID.M_EP_TD_1 => "M_EP_TD_1",
                TypeID.M_EP_TE_1 => "M_EP_TE_1",
                TypeID.M_EP_TF_1 => "M_EP_TF_1",
                TypeID.C_SC_NA_1 => "C_SC_NA_1",
                TypeID.C_DC_NA_1 => "C_DC_NA_1",
                TypeID.C_RC_NA_1 => "C_RC_NA_1",
                TypeID.C_SE_NA_1 => "C_SE_NA_1",
                TypeID.C_SE_NB_1 => "C_SE_NB_1",
                TypeID.C_SE_NC_1 => "C_SE_NC_1",
                TypeID.C_BO_NA_1 => "C_BO_NA_1",
                TypeID.C_SC_TA_1 => "C_SC_TA_1",
                TypeID.C_DC_TA_1 => "C_DC_TA_1",
                TypeID.C_RC_TA_1 => "C_RC_TA_1",
                TypeID.C_SE_TA_1 => "C_SE_TA_1",
                TypeID.C_SE_TB_1 => "C_SE_TB_1",
                TypeID.C_SE_TC_1 => "C_SE_TC_1",
                TypeID.C_BO_TA_1 => "C_BO_TA_1",
                TypeID.M_EI_NA_1 => "M_EI_NA_1",
                TypeID.C_IC_NA_1 => "C_IC_NA_1",
                TypeID.C_CI_NA_1 => "C_CI_NA_1",
                TypeID.C_RD_NA_1 => "C_RD_NA_1",
                TypeID.C_CS_NA_1 => "C_CS_NA_1",
                TypeID.C_TS_NA_1 => "C_TS_NA_1",
                TypeID.C_RP_NA_1 => "C_RP_NA_1",
                TypeID.C_CD_NA_1 => "C_CD_NA_1",
                TypeID.C_TS_TA_1 => "C_TS_TA_1",
                _ => $"Unknown Type ({typeId})"
            };
        }

        private string GetCotDescription(CauseOfTransmission cot)
        {
            return cot switch
            {
                CauseOfTransmission.PERIODIC => "Periodic",
                CauseOfTransmission.BACKGROUND_SCAN => "Background Scan",
                CauseOfTransmission.SPONTANEOUS => "Spontaneous",
                CauseOfTransmission.INITIALIZED => "Initialized",
                CauseOfTransmission.REQUEST => "Request",
                CauseOfTransmission.ACTIVATION => "Activation",
                CauseOfTransmission.ACTIVATION_CON => "Activation Confirmation",
                CauseOfTransmission.DEACTIVATION => "Deactivation",
                CauseOfTransmission.DEACTIVATION_CON => "Deactivation Confirmation",
                CauseOfTransmission.ACTIVATION_TERMINATION => "Activation Termination",
                CauseOfTransmission.RETURN_INFO_REMOTE => "Return Info Remote",
                CauseOfTransmission.RETURN_INFO_LOCAL => "Return Info Local",
                CauseOfTransmission.FILE_TRANSFER => "File Transfer",
                CauseOfTransmission.INTERROGATED_BY_STATION => "Interrogated by Station",
                CauseOfTransmission.INTERROGATED_BY_GROUP_1 => "Interrogated by Group 1",
                CauseOfTransmission.INTERROGATED_BY_GROUP_2 => "Interrogated by Group 2",
                CauseOfTransmission.INTERROGATED_BY_GROUP_3 => "Interrogated by Group 3",
                CauseOfTransmission.INTERROGATED_BY_GROUP_4 => "Interrogated by Group 4",
                CauseOfTransmission.INTERROGATED_BY_GROUP_5 => "Interrogated by Group 5",
                CauseOfTransmission.INTERROGATED_BY_GROUP_6 => "Interrogated by Group 6",
                CauseOfTransmission.INTERROGATED_BY_GROUP_7 => "Interrogated by Group 7",
                CauseOfTransmission.INTERROGATED_BY_GROUP_8 => "Interrogated by Group 8",
                CauseOfTransmission.INTERROGATED_BY_GROUP_9 => "Interrogated by Group 9",
                CauseOfTransmission.INTERROGATED_BY_GROUP_10 => "Interrogated by Group 10",
                CauseOfTransmission.INTERROGATED_BY_GROUP_11 => "Interrogated by Group 11",
                CauseOfTransmission.INTERROGATED_BY_GROUP_12 => "Interrogated by Group 12",
                CauseOfTransmission.INTERROGATED_BY_GROUP_13 => "Interrogated by Group 13",
                CauseOfTransmission.INTERROGATED_BY_GROUP_14 => "Interrogated by Group 14",
                CauseOfTransmission.INTERROGATED_BY_GROUP_15 => "Interrogated by Group 15",
                CauseOfTransmission.INTERROGATED_BY_GROUP_16 => "Interrogated by Group 16",
                _ => $"Unknown COT ({(int)cot})"
            };
        }
    }
}