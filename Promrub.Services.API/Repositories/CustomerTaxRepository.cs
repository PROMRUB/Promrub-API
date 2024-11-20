using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.PromServiceDbContext;

namespace Promrub.Services.API.Repositories;

public class CustomerTaxRepository : BaseRepository, ICustomerTaxRepository
{
    private readonly PromrubDbContext _context;

    public CustomerTaxRepository(PromrubDbContext context)
    {
        _context = context;
    }
    public void Add(CustomerTaxEntity entity)
    {
        this._context.Add(entity);
    }

    public IQueryable<CustomerTaxEntity> GetCustomerTaxQuery()
    {
        return this._context.CustomerTax.AsQueryable();
    }

    public DbContext Context()
    {
        return this._context;
    }
}