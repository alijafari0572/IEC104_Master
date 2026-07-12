using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Domain.Entities
{
    /// <summary>
    /// کلاس Event نمایانگر یک رویداد در پروتکل IEC 104 است که به یک Point مرتبط است و دارای ویژگی‌هایی مانند Id، PointId، Timestamp، Value، Quality، RawData و CreatedAt می‌باشد.
    /// </summary>
    public class Event
    {
        public int Id { get; set; }
        public int PointId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Value { get; set; } = string.Empty;
        public string Quality { get; set; } = string.Empty;
        public string? RawData { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // رابطه‌ی ارجاع به Point
        public virtual Point Point { get; set; } = null!;
    }
}