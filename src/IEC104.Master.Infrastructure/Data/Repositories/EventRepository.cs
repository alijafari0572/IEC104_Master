using System;
using System.Collections.Generic;
using System.Text;
using IEC104.Master.Application.Abstractions.Repositories;
using IEC104.Master.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IEC104.Master.Infrastructure.Data.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Event>> GetEventsByPointIdAsync(int pointId)
            => await _dbSet.Where(e => e.PointId == pointId)
                .OrderByDescending(e => e.Timestamp)
                .ToListAsync();

        public async Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTimeOffset from, DateTimeOffset to)
            => await _dbSet.Where(e => e.Timestamp >= from && e.Timestamp <= to)
                .OrderByDescending(e => e.Timestamp)
                .ToListAsync();
    }
}