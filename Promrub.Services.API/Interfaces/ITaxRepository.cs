using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Entities;

namespace Promrub.Services.API.Interfaces;

public interface ITaxRepository
{
    public IQueryable<TaxScheduleEntity> GetTaxScheduleQuery();
    public void Add(TaxScheduleEntity entity);
    public DbContext? Context();
}