using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Entities;

namespace Promrub.Services.API.Interfaces;

public interface ICustomerTaxRepository
{
    public void Add(CustomerTaxEntity entity);
    public IQueryable<CustomerTaxEntity> GetCustomerTaxQuery();
    public DbContext Context();
}