using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Application.Abstractions.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPointRepository Points { get; }
        IEventRepository Events { get; }

        Task<int> CompleteAsync();
    }
}