using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promrub.Services.API.Entities;

namespace Promrub.Services.API.Configurations;

public class ReceiptEntityConfiguration : IEntityTypeConfiguration<ReceiptPaymentEntity>
{
    public void Configure(EntityTypeBuilder<ReceiptPaymentEntity> builder)
    {
        
        builder.HasOne(b => b.Receipt)
            .WithMany()
            .HasForeignKey(x => x.ReceiptId);
    }
}