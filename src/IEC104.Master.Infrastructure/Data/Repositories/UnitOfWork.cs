using IEC104.Master.Application.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IPointRepository? _pointRepository;
        private IEventRepository? _eventRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IPointRepository Points
            => _pointRepository ??= new PointRepository(_context);

        public IEventRepository Events
            => _eventRepository ??= new EventRepository(_context);

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}