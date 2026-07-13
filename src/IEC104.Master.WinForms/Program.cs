//using IEC104.Master.Application.Abstractions;
//using IEC104.Master.Application.Services;
//using IEC104.Master.WinForms.Form;

//namespace IEC104.Master.WinForms;

//internal static class Program
//{
//    /// <summary>
//    ///  The main entry point for the application.
//    /// </summary>
//    [STAThread]
//    private static void Main()
//    {
//        // To customize application configuration such as set high DPI settings or default font,
//        // see https://aka.ms/applicationconfiguration.
//        ApplicationConfiguration.Initialize();
//        System.Windows.Forms.Application.Run(new Form1());
//    }
//}

using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.Abstractions.Repositories;
using IEC104.Master.Application.Services;
using IEC104.Master.Infrastructure.Adapters;
using IEC104.Master.Infrastructure.Data;
using IEC104.Master.Infrastructure.Data.Repositories;
using IEC104.Master.Infrastructure.Options;
using IEC104.Master.Infrastructure.Services;
using IEC104.Master.WinForms.Form;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IEC104.Master.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var builder = Host.CreateApplicationBuilder();

        // ===== تنظیمات دیتابیس SQLite =====
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "IEC104_Master",
            "iec104_data.db");

        var dbDirectory = Path.GetDirectoryName(dbPath);
        if (!Directory.Exists(dbDirectory))
            Directory.CreateDirectory(dbDirectory!);

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite($"Data Source={dbPath}");
        });

        // ===== ثبت Repositoryها و UnitOfWork =====
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IPointRepository, PointRepository>();
        builder.Services.AddScoped<IEventRepository, EventRepository>();

        builder.Services.AddSingleton(new Iec104Options
        {
            Host = "172.18.20.186",
            Port = 2404,
            CommonAddress = 1,
            TimeoutMs = 5000,
            UseTls = false
        });

        builder.Services.AddSingleton<ILib60870ConnectionAdapter, Lib60870ConnectionAdapter>();
        builder.Services.AddSingleton<IIec104Transport, Iec104Transport>();
        builder.Services.AddSingleton<IIec104MasterAppService, Iec104MasterAppService>();

        // در Program.cs، سرویس‌های جدید را ثبت کنید

        builder.Services.AddScoped<IPeriodicRequestRepository, PeriodicRequestRepository>();
        builder.Services.AddSingleton<IPeriodicRequestScheduler, PeriodicRequestScheduler>();

        builder.Services.AddTransient<MainForm>();
        builder.Services.AddTransient<ConnectionSettingsForm>();
        // در Program.cs، فرم‌های جدید را به DI اضافه کنید:

        builder.Services.AddTransient<PeriodicRequestsForm>();
        builder.Services.AddTransient<PeriodicRequestEditForm>();
        builder.Services.AddTransient<SendRequestForm>();

        using var host = builder.Build();
        host.Start();

        System.Windows.Forms.Application.Run(host.Services.GetRequiredService<MainForm>());
    }
}