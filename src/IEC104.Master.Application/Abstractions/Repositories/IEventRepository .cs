using IEC104.Master.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Application.Abstractions.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsByPointIdAsync(int pointId);

        Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTimeOffset from, DateTimeOffset to);
    }
}