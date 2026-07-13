using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Application.DTOs
{
    public class PeriodicRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "GeneralInterrogation", "GroupInterrogation", "SinglePoint"
        public string Parameter { get; set; } = string.Empty; // QOI یا IOA
        public int IntervalMinutes { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastExecutedAt { get; set; }
        public string? ErrorMessage { get; set; }
        public string Status => IsActive ? "فعال" : "غیرفعال";
        public string LastExecution => LastExecutedAt?.ToString("HH:mm:ss") ?? "—";
    }

    public class CreatePeriodicRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Parameter { get; set; } = string.Empty;
        public int IntervalMinutes { get; set; } = 10;
        public bool IsActive { get; set; } = true;
    }

    public class UpdatePeriodicRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Parameter { get; set; } = string.Empty;
        public int IntervalMinutes { get; set; }
        public bool IsActive { get; set; }
    }
}