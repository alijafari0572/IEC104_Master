using IEC104.Master.Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using IEC104.Master.Domain.Entities;

namespace IEC104.Master.Infrastructure.Data.Repositories
{
    public class PointRepository : GenericRepository<Point>, IPointRepository
    {
        public PointRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Point?> GetByIOAAsync(int informationObjectAddress)
            => await _dbSet.FirstOrDefaultAsync(p => p.InformationObjectAddress == informationObjectAddress);

        public async Task<IEnumerable<Point>> GetPointsWithEventsAsync()
            => await _dbSet.Include(p => p.Events).ToListAsync();
    }
}