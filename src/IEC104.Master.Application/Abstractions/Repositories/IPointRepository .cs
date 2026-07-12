using System;
using System.Collections.Generic;
using System.Text;
using IEC104.Master.Domain.Entities;

namespace IEC104.Master.Application.Abstractions.Repositories
{
    public interface IPointRepository : IRepository<Point>
    {
        Task<Point?> GetByIOAAsync(int informationObjectAddress);

        Task<IEnumerable<Point>> GetPointsWithEventsAsync();
    }
}