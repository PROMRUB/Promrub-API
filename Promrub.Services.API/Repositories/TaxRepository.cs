using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.PromServiceDbContext;

namespace Promrub.Services.API.Repositories;

public class TaxRepository : BaseRepository, ITaxRepository
{
    private readonly PromrubDbContext _context;

    public TaxRepository(PromrubDbContext context)
    {
        _context = context;
    }

    public IQueryable<TaxScheduleEntity> GetTaxScheduleQuery()
    {
        return _context.Tax.AsQueryable();
    }

    public void Add(TaxScheduleEntity entity)
    {
        _context.Add(entity);
    }

    public DbContext? Context()
    {
        return this._context;
    }
}