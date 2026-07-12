using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Domain.Entities
{
    /// <summary>
    /// کلاس Point نمایانگر یک نقطه داده در سیستم است که شامل اطلاعاتی مانند شناسه، آدرس شیء اطلاعاتی (IOA)، نام، نوع و زمان‌های ایجاد و به‌روزرسانی می‌باشد. این کلاس همچنین رابطه‌ای یک‌به‌چند با کلاس Event دارد که نشان‌دهنده رویدادهای مرتبط با این نقطه داده است.
    /// </summary>
    public class Point
    {
        public int Id { get; set; }
        public int InformationObjectAddress { get; set; } // IOA
        public string Name { get; set; } = string.Empty;
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // رابطه‌ی یک‌به‌چند با Events
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}