using Promrub.Services.API.Entities;
using Promrub.Services.API.PromServiceDbContext;

namespace Promrub.Services.API.Interfaces;

public interface IReceiptRepository
{
    public IQueryable<ReceiptScheduleEntity> GetReceiptScheduleQuery();
    public void Add(ReceiptScheduleEntity entity);
    public void AddRange(List<ReceiptScheduleEntity> entity);

    public PromrubDbContext Context();
}