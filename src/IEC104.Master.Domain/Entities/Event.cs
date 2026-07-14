using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// فضای نام موجودیت‌های دامنه (Domain Entities)
/// </summary>
/// <remarks>
/// Responsibility:
/// - شامل کلاس‌های مدل‌سازی شده‌ی داده‌های اصلی کسب‌وکار (Business Objects) که نشان‌دهنده‌ی مفاهیم دنیای واقعی در سیستم هستند.
///
/// Design Notes:
/// - این موجودیت‌ها هیچ وابستگی به لایه‌های بالاتر (مانند Application یا Infrastructure) ندارند و کاملاً مستقل هستند.
/// - از ویژگی‌های ساده (Plain Old CLR Objects) برای تعریف مدل‌ها استفاده شده است.
/// - روابط بین موجودیت‌ها با استفاده از پراپرتی‌های virtual برای پشتیبانی از Lazy Loading در Entity Framework Core تعریف شده‌اند.
///
/// Dependencies:
/// - این فضای نام به هیچ کتابخانه یا پروژه‌ی دیگری وابسته نیست و صرفاً از نوع‌های پایه‌ی دات‌نت استفاده می‌کند.
/// </remarks>
namespace IEC104.Master.Domain.Entities
{
    /// <summary>
    /// موجودیت Event نمایانگر یک رویداد (Event) در پروتکل IEC 104 است که به یک نقطه (Point) مرتبط می‌باشد.
    /// هر Event شامل یک مقدار، کیفیت، زمان وقوع و اطلاعات تکمیلی است.
    /// </summary>
    /// <remarks>
    /// Role:
    /// - این کلاس، یک نمونه‌ی خاص از داده‌های دریافتی از RTU را برای یک نقطه‌ی مشخص ذخیره می‌کند.
    /// - هر بار که یک ASDU جدید دریافت می‌شود، یک یا چند Event جدید برای نقاط مربوطه ایجاد می‌شود.
    /// - این موجودیت به عنوان تاریخچه‌ای از تغییرات مقادیر نقاط عمل می‌کند و امکان تحلیل روند و گزارش‌گیری را فراهم می‌آورد.
    ///
    /// Collaboration:
    /// - این کلاس با موجودیت Point رابطه‌ی چند-به-یک (Many-to-One) دارد؛ هر Event به یک Point تعلق دارد و هر Point می‌تواند چندین Event داشته باشد.
    /// - توسط لایه‌ی Infrastructure (Repository و UnitOfWork) برای ذخیره‌سازی و بازیابی داده‌ها استفاده می‌شود.
    /// - توسط لایه‌ی Application (از طریق سرویس‌ها) برای ثبت رویدادهای جدید و گزارش‌گیری استفاده می‌شود.
    /// - در لایه‌ی Presentation، داده‌های این موجودیت برای نمایش تاریخچه‌ی یک نقطه در UI استفاده می‌شوند.
    ///
    /// Lifecycle:
    /// - نمونه‌های این کلاس در زمان دریافت هر ASDU جدید (از طریق رویداد MessagePublished) ایجاد می‌شوند.
    /// - طول عمر آنها تا زمانی که کاربر یا سیستم آن‌ها را حذف نکند، در دیتابیس باقی می‌مانند.
    /// - معمولاً به عنوان داده‌های تاریخی نگهداری می‌شوند و حذف نمی‌شوند (مگر در عملیات‌های نگهداری).
    ///
    /// Constraints:
    /// - مقدار Value نباید null باشد و در صورت عدم وجود مقدار، باید به رشته‌ی خالی تنظیم شود.
    /// - Quality باید یکی از مقادیر معتبر (GOOD, INVALID, QUESTIONABLE, ...) داشته باشد.
    /// - Timestamp باید زمان دریافت داده از RTU را نشان دهد (نه زمان ذخیره‌سازی در دیتابیس).
    /// - PointId باید به یک Point موجود در دیتابیس اشاره کند (برای حفظ یکپارچگی ارجاعی).
    /// </remarks>
    public class Event
    {
        /// <summary>
        /// شناسه‌ی یکتای رویداد
        /// </summary>
        /// <remarks>
        /// Business Rule:
        /// - این مقدار به عنوان کلید اصلی (Primary Key) در دیتابیس استفاده می‌شود.
        /// - توسط دیتابیس به‌صورت خودکار (Auto Increment) تولید می‌شود.
        /// - برای شناسایی منحصربه‌فرد هر رویداد در سیستم استفاده می‌شود.
        ///
        /// Mutability:
        /// - این مقدار پس از ایجاد رویداد تغییر نمی‌کند (Immutable).
        /// </remarks>
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