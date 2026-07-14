
# Project Name
## Purpose

هدف اصلی این پروژه، پیاده‌سازی یک نرم‌افزار مانیتورینگ و کنترل (SCADA Client) برای ارتباط با تجهیزات صنعتی از طریق پروتکل IEC 60870-5-104 (IEC 104) است. این نرم‌افزار به عنوان Master Station عمل کرده و امکان برقراری ارتباط با RTUها (Remote Terminal Units) و IEDها (Intelligent Electronic Devices) در پست‌های برق، نیروگاه‌ها و سیستم‌های اتوماسیون انرژی را فراهم می‌کند.

این پروژه مشکلات زیر را حل می‌کند:

نیاز به یک کلاینت استاندارد IEC 104 برای دریافت داده‌های تله‌متری (ولتاژ، جریان، توان، وضعیت کلیدها و ...) از تجهیزات برق.

ذخیره‌سازی داده‌های دریافتی برای تحلیل و گزارش‌گیری بعدی.

ارسال درخواست‌های دوره‌ای و دستی برای دریافت داده‌ها از RTU.

امکان مدیریت چندین درخواست زمان‌بندی شده برای Polling خودکار.

## Scope
این پروژه شامل چه کارهایی است (In Scope):

✅ اتصال به یک یا چند RTU از طریق پروتکل IEC 104 روی TCP/IP.

✅ دریافت و نمایش داده‌های تله‌متری (Single Point, Double Point, Measured Values, Step Position, Bit String, Integrated Totals).

✅ پارس کردن (Parsing) خودکار ASDUهای دریافتی و استخراج اطلاعات ساختاریافته.

✅ ذخیره‌سازی داده‌ها در دیتابیس SQLite با استفاده از Entity Framework Core.

✅ ارسال بازجویی عمومی (General Interrogation) با QOI دلخواه.

✅ تعریف و مدیریت درخواست‌های دوره‌ای (Polling) با بازه‌های زمانی قابل تنظیم.

✅ ارسال دستی درخواست برای یک نقطه‌ی خاص (Single Point Read) یا بازجویی گروهی.

✅ نمایش داده‌ها در DataGridView با قابلیت اسکرول خودکار و نمایش کیفیت داده.

✅ رابط کاربری زیبا و پاسخگو با استفاده از Windows Forms و طراحی مدرن.

✅ اتصال خودکار در زمان اجرای برنامه با استفاده از تنظیمات ذخیره‌شده.

✅ مدیریت خطا و نمایش پیام‌های خطا در لاگ.

این پروژه چه کارهایی را انجام نمی‌دهد (Out of Scope):

❌ پیاده‌سازی سمت Slave (RTU)؛ این پروژه صرفاً یک Master است.

❌ پشتیبانی از پروتکل‌های دیگر مانند Modbus یا DNP3 (در حال حاضر).

❌ ارسال فرمان‌های کنترلی (مانند قطع/وصل کلید) - این قابلیت در نسخه‌های آینده اضافه خواهد شد.

❌ تولید گزارش یا تحلیل آماری پیشرفته.

❌ اتصال به چندین RTU به‌صورت همزمان (در نسخه‌ی فعلی فقط یک اتصال همزمان پشتیبانی می‌شود).

❌ پشتیبانی از TLS/SSL برای ارتباط امن (قابلیت در کتابخانه وجود دارد اما فعلاً غیرفعال است).

## Features

۱. اتصال و ارتباط

🔗 اتصال خودکار به RTU در زمان اجرا با استفاده از تنظیمات پیش‌فرض.

🔗 اتصال دستی از طریق فرم تنظیمات (IP، پورت، Common Address، Timeout، TLS).

🔗 قطع اتصال به‌صورت دستی و غیرهمگام.

🔗 مدیریت وضعیت اتصال و نمایش آن در UI (Connected/Disconnected).

۲. دریافت و پردازش داده

📡 دریافت ASDU از RTU و پارس کردن خودکار بر اساس TypeID.

📡 پشتیبانی از انواع داده: Single Point, Double Point, Measured Value (Normalized, Scaled, Short Float), Step Position, Bit String, Integrated Totals.

📡 استخراج کیفیت داده (Quality Descriptor) و نمایش آن در UI.

📡 نمایش داده‌ها در DataGridView با ستون‌های: زمان، Type ID، نوع، نوع مقدار، آدرس مشترک، COT، IOA، مقدار و کیفیت.

۳. ذخیره‌سازی در دیتابیس

💾 ذخیره‌سازی خودکار تمام داده‌های دریافتی در دیتابیس SQLite.

💾 مدیریت نقاط (Points) با شناسایی خودکار بر اساس IOA و به‌روزرسانی اطلاعات.

💾 ذخیره‌سازی رویدادها (Events) با زمان، مقدار، کیفیت و داده‌ی خام.

💾 استفاده از Entity Framework Core با معماری Repository و UnitOfWork.

۴. درخواست‌های دوره‌ای (Polling)

⏱️ تعریف درخواست‌های دوره‌ای با نام، نوع (General Interrogation, Group Interrogation, Single Point)، پارامتر (QOI یا IOA) و بازه‌ی زمانی (دقیقه).

⏱️ فعال/غیرفعال کردن هر درخواست به‌صورت جداگانه.

⏱️ ارسال فوری هر درخواست بدون انتظار برای بازه‌ی زمانی.

⏱️ مدیریت کامل (افزودن، ویرایش، حذف، تغییر وضعیت) از طریق فرم اختصاصی.

⏱️ ثبت تاریخچه‌ی اجرا و نمایش آخرین وضعیت (موفق/ناموفق).

۵. ارسال دستی درخواست

📤 ارسال دستی بازجویی عمومی با QOI دلخواه.

📤 ارسال دستی بازجویی گروهی با QOI گروه.

📤 ارسال دستی درخواست برای یک نقطه‌ی خاص (Single Point Read) با IOA.

📤 نمایش نتیجه‌ی ارسال در فرم اختصاصی.

۶. رابط کاربری

🖥️ طراحی مدرن و پاسخگو با استفاده از FlowLayoutPanel و SplitContainer.

🖥️ دکمه‌های زیبا با رنگ‌های هماهنگ (آبی برای اتصال، قرمز برای قطع، سبز برای GI).

🖥️ پنل لودینگ با ProgressBar متحرک در هنگام اتصال.

🖥️ نمایش وضعیت اتصال با آیکون و رنگ.

🖥️ لیست لاگ با فونت Consolas برای نمایش پیام‌های سیستم.

🖥️ DataGridView با سطرهای متناوب و هدر آبی.

۷. معماری و کدنویسی

🧱 معماری لایه‌بندی شده (Clean Architecture) با تفکیک Domain, Application, Infrastructure و Presentation.

🧱 استفاده از Dependency Injection (DI) برای مدیریت وابستگی‌ها.

🧱 برنامه‌نویسی غیرهمگام (Async/Await) برای جلوگیری از قفل شدن UI.

🧱 مدیریت خطا و نمایش خطاها در لاگ.

## Architecture

نوع معماری
Clean Architecture (معماری تمیز) با لایه‌بندی زیر:

┌──────────────────────────────────────────────┐
│  IEC104.Master.WinForms (Presentation)      │  ← Windows Forms, UI, DI Container
├──────────────────────────────────────────────┤
│  IEC104.Master.Application (Application)    │  ← Use Cases, Interfaces, DTOs
├──────────────────────────────────────────────┤
│  IEC104.Master.Domain (Domain)              │  ← Entities, Business Rules
├──────────────────────────────────────────────┤
│  IEC104.Master.Infrastructure (Infrastructure)│ ← Repositories, Services, Adapters, DB
└──────────────────────────────────────────────┘

## Key Concepts
۱. پروتکل IEC 60870-5-104
پروتکل Master-Slave برای ارتباط در سیستم‌های برق و انرژی.

مبتنی بر TCP/IP و پورت استاندارد ۲۴۰۴.

داده‌ها در قالب ASDU (Application Service Data Unit) ارسال می‌شوند.

۲. ASDU (Application Service Data Unit)
واحد اصلی داده در IEC 104 شامل:

Type ID: نوع داده (Single Point, Measured Value, ...)

Cause of Transmission (COT): علت ارسال (Spontaneous, General Interrogation, ...)

Common Address: آدرس ایستگاه

Information Object: شامل آدرس (IOA)، مقدار و کیفیت.

۳. Information Object Address (IOA)
آدرس یکتای هر نقطه در RTU.

برای شناسایی نقاط (مانند کلیدها، سنسورها) استفاده می‌شود.

۴. Quality Descriptor
نشان‌دهنده‌ی اعتبار و وضعیت داده.

مقادیر: GOOD, INVALID, QUESTIONABLE, OVERFLOW, BLOCKED.

۵. General Interrogation (GI)
درخواست بازجویی عمومی برای دریافت تمام داده‌ها از RTU.

پارامتر QOI (Qualifier of Interrogation) مشخص می‌کند کدام گروه یا همه داده‌ها درخواست شوند.

۶. Repository و UnitOfWork
الگوی Repository برای جداسازی منطق دسترسی به داده از لایه‌ی бизнес.

الگوی UnitOfWork برای مدیریت تراکنش‌های دیتابیس.

۷. Dependency Injection (DI)
مدیریت وابستگی‌ها با استفاده از Microsoft.Extensions.DependencyInjection.

ثبت سرویس‌ها به‌صورت Singleton، Scoped و Transient در Program.cs.
## How to Navigate the Code

ساختار پوشه‌ها و فایل‌های کلیدی

IEC104_Master/
├── IEC104.Master.Domain/
│   └── Entities/
│       ├── Point.cs              // مدل نقطه (IOA، نام، نوع)
│       ├── Event.cs              // مدل رویداد (زمان، مقدار، کیفیت)
│       └── PeriodicRequest.cs    // مدل درخواست دوره‌ای
│
├── IEC104.Master.Application/
│   ├── Abstractions/
│   │   ├── Repositories/
│   │   │   ├── IRepository.cs
│   │   │   ├── IPointRepository.cs
│   │   │   ├── IEventRepository.cs
│   │   │   └── IPeriodicRequestRepository.cs
│   │   ├── IIec104MasterAppService.cs
│   │   ├── IIec104Transport.cs
│   │   ├── IUnitOfWork.cs
│   │   └── IPeriodicRequestScheduler.cs
│   └── DTOs/
│       ├── ProtocolMessageDto.cs
│       ├── InformationPointDto.cs
│       ├── ConnectRequestDto.cs
│       ├── PeriodicRequestDto.cs
│       └── ...
│
├── IEC104.Master.Infrastructure/
│   ├── Adapters/
│   │   ├── ILib60870ConnectionAdapter.cs
│   │   └── Lib60870ConnectionAdapter.cs  // پیاده‌سازی ارتباط با lib60870
│   ├── Data/
│   │   ├── AppDbContext.cs               // DbContext اصلی
│   │   └── Repositories/
│   │       ├── GenericRepository.cs
│   │       ├── PointRepository.cs
│   │       ├── EventRepository.cs
│   │       ├── PeriodicRequestRepository.cs
│   │       └── UnitOfWork.cs
│   ├── Services/
│   │   ├── Iec104Transport.cs            // لایه انتزاعی ارتباط
│   │   └── PeriodicRequestScheduler.cs   // زمان‌بند درخواست‌های دوره‌ای
│   └── Options/
│       └── Iec104Options.cs              // تنظیمات برنامه
│
└── IEC104.Master.WinForms/
    ├── Form/
    │   ├── MainForm.cs                   // فرم اصلی برنامه
    │   ├── MainForm.Designer.cs          // طراحی فرم اصلی
    │   ├── ConnectionSettingsForm.cs     // فرم تنظیمات اتصال
    │   ├── PeriodicRequestsForm.cs       // فرم مدیریت درخواست‌های دوره‌ای
    │   ├── PeriodicRequestEditForm.cs    // فرم افزودن/ویرایش درخواست دوره‌ای
    │   └── SendRequestForm.cs            // فرم ارسال دستی درخواست
    ├── Models/
    │   └── AsduDisplayModel.cs           // مدل نمایشی برای DataGridView
    └── Program.cs                        // نقطه‌ی ورود و DI Container


    جریان داده (Data Flow)
دریافت داده از RTU:

Lib60870ConnectionAdapter داده‌های خام ASDU را از کتابخانه lib60870 دریافت می‌کند.

ParseAsdu داده‌ها را به ProtocolMessageDto تبدیل می‌کند.

Iec104Transport پیام را به Iec104MasterAppService می‌فرستد.

Iec104MasterAppService رویداد MessagePublished را فعال می‌کند.

MainForm در رویداد OnMessagePublished داده‌ها را در دیتابیس ذخیره و در UI نمایش می‌دهد.

ارسال درخواست به RTU:

کاربر روی دکمه‌ی GI، ارسال دستی یا از زمان‌بند کلیک می‌کند.

درخواست از طریق MainForm → Iec104MasterAppService → Iec104Transport → Lib60870ConnectionAdapter به کتابخانه lib60870 می‌رسد.

کتابخانه درخواست را به RTU ارسال می‌کند.

زمان‌بند (Scheduler):

PeriodicRequestScheduler با استفاده از System.Threading.Timer درخواست‌های فعال را در بازه‌های زمانی مشخص ارسال می‌کند.

پس از هر اجرا، رویداد RequestExecuted فعال می‌شود و نتیجه در UI نمایش داده می‌شود.

راهنمای شروع (Getting Started)
اجرای برنامه:

فایل IEC104.Master.WinForms.exe را اجرا کنید.

برنامه به‌صورت خودکار به RTU با تنظیمات پیش‌فرض (127.0.0.1:2404) متصل می‌شود.

تغییر تنظیمات اتصال:

روی دکمه‌ی Connect کلیک کنید.

IP، پورت، Common Address و Timeout را وارد کنید.

روی تأیید کلیک کنید.

ارسال بازجویی عمومی:

پس از اتصال، روی دکمه‌ی GI کلیک کنید.

داده‌های دریافتی در DataGridView و لاگ نمایش داده می‌شوند.

مدیریت درخواست‌های دوره‌ای:

روی دکمه‌ی زمان‌بندی کلیک کنید.

یک درخواست جدید اضافه کنید (نام، نوع، پارامتر، بازه).

درخواست به‌صورت خودکار در بازه‌های زمانی مشخص اجرا می‌شود.

ارسال دستی درخواست:

روی دکمه‌ی ارسال دستی کلیک کنید.

نوع درخواست و پارامتر را انتخاب کنید.

نتیجه در فرم نمایش داده می‌شود.