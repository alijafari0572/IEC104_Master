using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.WinForms.Models
{
    public class AsduDisplayModel
    {
        public DateTimeOffset Timestamp { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string ValueType { get; set; }
        public int CommonAddress { get; set; }
        public string CotDescription { get; set; }
        public int InformationObjectAddress { get; set; }
        public object Value { get; set; }
        public string Quality { get; set; }
    }
}