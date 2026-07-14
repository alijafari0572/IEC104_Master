using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// فضای نام مدل‌های نمایشی برای لایه‌ی Presentation
/// </summary>
/// <remarks>
/// Responsibility:
/// - شامل مدل‌هایی است که برای نمایش داده‌ها در UI (مانند DataGridView) استفاده می‌شوند.
/// - این مدل‌ها به‌طور خاص برای لایه‌ی WinForms طراحی شده‌اند و داده‌های دریافتی از لایه‌های پایین‌تر (Application/Infrastructure) را برای نمایش آماده می‌کنند.
///
/// Design Notes:
/// - این مدل‌ها صرفاً برای نمایش هستند و نباید شامل منطق تجاری (Business Logic) باشند.
/// - از این مدل‌ها برای اتصال به کنترل‌های Data Binding (مانند BindingSource) استفاده می‌شود.
///
/// Dependencies:
/// - وابسته به DTOهای لایه‌ی Application برای دریافت داده‌های اصلی.
/// - از فضای نام `System` و `System.Collections.Generic` استفاده می‌کند.
/// </remarks>
namespace IEC104.Master.WinForms.Models
{
    /// <summary>
    /// مدل نمایشی برای داده‌های ASDU در DataGridView
    /// </summary>
    /// <remarks>
    /// Role:
    /// - این کلاس به عنوان یک ViewModel ساده عمل می‌کند و داده‌های هر نقطه (Information Point) را از یک ASDU دریافتی برای نمایش در UI آماده می‌سازد.
    /// - هر نمونه از این کلاس نشان‌دهنده‌ی یک ردیف در DataGridView است که شامل اطلاعات یک نقطه‌ی خاص (مانند یک کلید یا یک مقدار آنالوگ) می‌باشد.
    ///
    /// Collaboration:
    /// - توسط `MainForm` و متد `OnMessagePublished` برای تبدیل `ProtocolMessageDto` به ردیف‌های قابل نمایش استفاده می‌شود.
    /// - از `BindingSource` برای اتصال به `DataGridView` بهره می‌برد.
    ///
    /// Lifecycle:
    /// - نمونه‌های این کلاس در زمان دریافت هر ASDU ایجاد می‌شوند و تا زمانی که کاربر فرم را ببندد یا لیست پاک شود، در حافظه باقی می‌مانند.
    /// - طول عمر آنها به طول عمر `List&lt;AsduDisplayModel&gt;` در `MainForm` گره خورده است.
    ///
    /// Constraints:
    /// - این کلاس نباید شامل هیچ گونه منطق تجاری یا عملیات محاسباتی باشد.
    /// - تمام پراپرتی‌ها باید `public` و دارای `get; set;` باشند تا Data Binding به درستی کار کند.
    /// </remarks>
    public class AsduDisplayModel
    {
        /// <summary>
        /// زمان دریافت داده از RTU
        /// </summary>
        /// <remarks>
        /// Business Rule:
        /// - این زمان نشان‌دهنده‌ی لحظه‌ای است که ASDU در سمت Master دریافت شده است (نه زمان وقوع رویداد در RTU).
        /// - برای نمایش ترتیب زمانی رویدادها در UI استفاده می‌شود.
        ///
        /// Mutability:
        /// - مقدار این پراپرتی پس از ایجاد شیء تغییر نمی‌کند (Immutable).
        /// </remarks>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// شناسه‌ی نوع داده (TypeID) مطابق با استاندارد IEC 104
        /// </summary>
        /// <remarks>
        /// Business Rule:
        /// - مقدار آن از فیلد `TypeId` در `ProtocolMessageDto` گرفته می‌شود.
        /// - این مقدار نشان‌دهنده‌ی نوع ASDU است (مانند `M_SP_NA_1` برای وضعیت کلید یا `M_ME_NB_1` برای مقدار آنالوگ).
        /// - برای تشخیص نوع داده و نحوه‌ی تفسیر مقدار از آن استفاده می‌شود.
        ///
        /// Mutability:
        /// - مقدار این پراپرتی پس از ایجاد شیء تغییر نمی‌کند (Immutable).
        /// </remarks>
        public int TypeId { get; set; }

        /// <summary>
        /// نام توصیفی نوع داده
        /// </summary>
        /// <remarks>
        /// Business Rule:
        /// - این نام از متد `GetTypeIdTitle` در لایه‌ی `Lib60870ConnectionAdapter` تولید می‌شود.
        /// - برای نمایش نام خوانا و قابل‌فهم از نوع داده به کاربر استفاده می‌شود (مانند "Single Point Information").
        ///
        /// Mutability:
        /// - مقدار این پراپرتی پس از ایجاد شیء تغییر نمی‌کند (Immutable).
        /// </remarks>
        public string TypeName { get; set; }

        /// <summary>
        /// نوع مقداری که در این نقطه ذخیره شده است
        /// </summary>
        /// <remarks>
        /// Business Rule:
        /// - این مقدار از فیلد `ValueType` در `InformationPointDto` گرفته می‌شود.
        /// - مشخص می‌کند که مقدار `Value` از چه نوعی است (مانند `SinglePoint`, `MeasuredValueNormalized` و ...).
        ///
        /// Mutability:
        /// - مقدار این پراپرتی پس از ایجاد شیء تغییر نمی‌کند (Immutable).
        /// </remarks>
        public string ValueType { get; set; }

        /// <summary>
        /// آدرس مشترک (Common Address) که مشخص‌کننده‌ی ایستگاه مبدأ ASDU است
        /// </summary>
        /// <remarks>
        /// Business Rule:
        /// - این مقدار از فیلد `CommonAddress` در `ProtocolMessageDto` گرفته می‌شود.
        /// - در سیستم‌هایی که چندین RTU به Master متصل هستند، این مقدار برای تشخیص مبدأ داده استفاده می‌شود.
        ///
        /// Mutability:
        /// - مقدار این پراپرتی پس از ایجاد شیء تغییر نمی‌کند (Immutable).
        /// </remarks>
        public int CommonAddress { get; set; }

        /// <summary>
        /// توضیح علت ارسال (Cause of Transmission)
        /// </summary>
        /// <remarks>
        /// Business Rule:
        /// - این مقدار از فیلد `CotDescription` در `ProtocolMessageDto` گرفته می‌شود.
        /// - علت ارسال را مشخص می‌کند (مانند `Spontaneous` برای تغییر خودکار، `GeneralInterrogation` برای پاسخ به بازجویی عمومی).
        ///
        /// Mutability:
        /// - مقدار این پراپرتی پس از ایجاد شیء تغییر نمی‌کند (Immutable).
        /// </remarks>
        public string CotDescription { get; set; }

        /// <summary>
        /// آدرس شیء اطلاعاتی (Information Object Address)
        /// </summary>
        /// <remarks>
        /// Business Rule:
        /// - این مقدار از فیلد `ObjectAddress` در `InformationPointDto` گرفته می‌شود.
        /// - آدرس یکتای هر نقطه در RTU است که برای شناسایی نقطه‌ی مورد نظر استفاده می‌شود.
        /// - معمولاً به صورت عددی ۳ بایتی (حداکثر ۱۶,۷۷۷,۲۱۵) نمایش داده می‌شود.
        ///
        /// Mutability:
        /// - مقدار این پراپرتی پس از ایجاد شیء تغییر نمی‌کند (Immutable).
        /// </remarks>
        public int InformationObjectAddress { get; set; }

        /// <summary>
        /// مقدار واقعی داده
        /// </summary>
        /// <remarks>
        /// Business Rule:
        /// - این مقدار از فیلد `Value` در `InformationPointDto` گرفته می‌شود.
        /// - نوع آن با توجه به `TypeId` و `ValueType` تعیین می‌شود:
        ///   - برای `SinglePoint`: `bool` (true = ON, false = OFF)
        ///   - برای مقادیر آنالوگ: `float` (برای ولتاژ، جریان، دما و ...)
        ///   - برای شمارنده‌ها: `int`
        ///   - برای دستورات: `bool` یا `enum`
        /// - این مقدار باید با توجه به `ValueType` به‌درستی تفسیر و نمایش داده شود.
        ///
        /// Mutability:
        /// - مقدار این پراپرتی ممکن است با هر ASDU جدید به‌روز شود (Mutable).
        /// - آخرین مقدار دریافتی برای هر نقطه در UI نمایش داده می‌شود.
        /// </remarks>
        public object Value { get; set; }

        /// <summary>
        /// کیفیت داده (Quality)
        /// </summary>
        /// <remarks>
        /// Business Rule:
        /// - این مقدار از فیلد `Quality` در `InformationPointDto` گرفته می‌شود.
        /// - نشان‌دهنده‌ی اعتبار و وضعیت داده است و می‌تواند یکی از مقادیر زیر باشد:
        ///   - `GOOD`: داده معتبر است.
        ///   - `INVALID`: داده نامعتبر است (نباید به آن اعتماد کرد).
        ///   - `QUESTIONABLE`: داده مشکوک است (ممکن است معتبر باشد یا نباشد).
        ///   - `OVERFLOW`: مقدار از محدوده خارج شده است.
        ///   - `BLOCKED`: داده مسدود شده است.
        /// - این مقدار باید در UI با رنگ یا نشانه‌ی خاصی نمایش داده شود تا کاربر از وضعیت داده آگاه باشد.
        ///
        /// Mutability:
        /// - مقدار این پراپرتی ممکن است با هر ASDU جدید به‌روز شود (Mutable).
        /// - آخرین کیفیت دریافتی برای هر نقطه در UI نمایش داده می‌شود.
        /// </remarks>
        public string Quality { get; set; }
    }
}