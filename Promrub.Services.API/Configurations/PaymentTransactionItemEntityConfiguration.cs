using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promrub.Services.API.Entities;

namespace Promrub.Services.API.Configurations;

// public class PaymentTransactionItemEntityConfiguration : IEntityTypeConfiguration<PaymentTransactionEntity>
// {
//     public void Configure(EntityTypeBuilder<PaymentTransactionEntity> builder)
//     {
//         
//         builder.HasMany(b => b.Item)
//             .WithOne(x => x.PaymentTransactionEntity)
//             .HasForeignKey(x => x.PaymentTransactionId);
//     }
// }

