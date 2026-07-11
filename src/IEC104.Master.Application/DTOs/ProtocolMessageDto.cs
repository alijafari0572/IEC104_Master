using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;

namespace IEC104.Master.Application.DTOs
{
    public class ProtocolMessageDto
    {
        // ===== فیلدهای اصلی =====
        public DateTimeOffset Timestamp { get; set; }

        public string Kind { get; set; }          // "ASDU", "Info", "Error", "Command"
        public string Title { get; set; }
        public string Details { get; set; }

        // ===== فیلدهای IEC 104 ساختاریافته =====
        public int TypeId { get; set; }           // TypeID به صورت int

        public string TypeName { get; set; }      // عنوان توصیفی
        public int CauseOfTransmission { get; set; } // COT عددی
        public string CotDescription { get; set; }   // توضیح COT
        public int CommonAddress { get; set; }
        public List<InformationPointDto> Points { get; set; } = new List<InformationPointDto>();

        // ===== سازنده‌ها =====
        public ProtocolMessageDto()
        { }

        public ProtocolMessageDto(DateTimeOffset timestamp, string kind, string title, string details)
        {
            Timestamp = timestamp;
            Kind = kind;
            Title = title;
            Details = details;
        }
    }

    public class InformationPointDto
    {
        public int ObjectAddress { get; set; }    // IOA
        public string ValueType { get; set; }     // مثلاً "SinglePoint"
        public object Value { get; set; }         // مقدار واقعی
        public string Quality { get; set; }       // کیفیت (به صورت رشته)
    }
}