using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promrub.Services.API.Entities;

namespace Promrub.Services.API.Configurations;

public class CustomerTaxEntityConfiguration : IEntityTypeConfiguration<CustomerTaxEntity>
{
    public void Configure(EntityTypeBuilder<CustomerTaxEntity> builder)
    {
        builder.HasMany(b => b.TransactionEntities)
            .WithOne(x => x.CustomerTaxEntity)
            .HasForeignKey(x => x.CustomerTaxId);
    }
}