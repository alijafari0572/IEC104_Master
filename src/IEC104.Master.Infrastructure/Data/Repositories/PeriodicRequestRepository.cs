using IEC104.Master.Application.Abstractions.Repositories;
using IEC104.Master.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace IEC104.Master.Infrastructure.Data.Repositories
{
    public class PeriodicRequestRepository : GenericRepository<PeriodicRequest>, IPeriodicRequestRepository
    {
        public PeriodicRequestRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PeriodicRequest>> GetActiveRequestsAsync()
            => await _dbSet.Where(r => r.IsActive).ToListAsync();

        public async Task<PeriodicRequest?> GetByNameAsync(string name)
            => await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
    }
}