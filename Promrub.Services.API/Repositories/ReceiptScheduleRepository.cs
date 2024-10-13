using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.PromServiceDbContext;

namespace Promrub.Services.API.Repositories;

public class ReceiptScheduleRepository : BaseRepository, IReceiptRepository
{
    private readonly PromrubDbContext _context;

    public ReceiptScheduleRepository(PromrubDbContext context)
    {
        this._context = context;
    }

    public IQueryable<ReceiptScheduleEntity> GetReceiptScheduleQuery()
    {
        return _context.ReceiptSchedule.AsQueryable();
    }

    public void Add(ReceiptScheduleEntity entity)
    {
        _context.Add(entity);
    }

    public void AddRange(List<ReceiptScheduleEntity> entity)
    {
        _context.AddRange(entity);
    }

    public PromrubDbContext Context()
    {
        return _context;
    }
}