using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Domain.Entities
{
    public class PeriodicRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;          // نام توصیفی (مثلاً "Polling گروه 1")
        public RequestType Type { get; set; }                     // نوع درخواست (GeneralInterrogation, SinglePoint, ...)
        public string Parameter { get; set; } = string.Empty;      // پارامتر (مثلاً QOI=20 یا IOA=101)
        public int CommonAddress { get; set; } = 1; // ← اضافه کنید
        public int IntervalMinutes { get; set; } = 10;            // بازه‌ی زمانی بر حسب دقیقه
        public bool IsActive { get; set; } = true;                // فعال/غیرفعال
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastExecutedAt { get; set; }              // آخرین زمان اجرا
        public string? ErrorMessage { get; set; }                 // آخرین خطا (در صورت وجود)
    }

    public enum RequestType
    {
        GeneralInterrogation = 0,   // بازجویی عمومی (با QOI)
        GroupInterrogation = 1,     // بازجویی گروهی (با QOI گروه)
        SinglePoint = 2             // درخواست یک نقطه‌ی خاص (با IOA)
    }
}