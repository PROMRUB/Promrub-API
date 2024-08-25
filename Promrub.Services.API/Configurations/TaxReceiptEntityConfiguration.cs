using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promrub.Services.API.Entities;

namespace Promrub.Services.API.Configurations;

public class TaxReceiptEntityConfiguration : IEntityTypeConfiguration<TaxReceiptEntity>
{
    public void Configure(EntityTypeBuilder<TaxReceiptEntity> builder)
    {
        
        // builder.HasOne(b => b.PaymentTransaction)
        //     .WithMany()
        //     .HasForeignKey(x => x.PaymentTransactionId);
        //
        builder.HasOne(b => b.Tax)
            .WithMany()
            .HasForeignKey(x => x.TaxId);
    }
}