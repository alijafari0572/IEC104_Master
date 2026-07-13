using IEC104.Master.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Application.Abstractions.Repositories
{
    public interface IPeriodicRequestRepository : IRepository<PeriodicRequest>
    {
        Task<IEnumerable<PeriodicRequest>> GetActiveRequestsAsync();

        Task<PeriodicRequest?> GetByNameAsync(string name);
    }
}