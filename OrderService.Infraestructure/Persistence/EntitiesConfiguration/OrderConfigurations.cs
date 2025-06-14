using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using OrderService.Domain.Entities;

namespace OrderService.Infraestructure.Persistence.EntitiesConfiguration
{
    public class DoctorConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(p => p.Id);


            builder.OwnsOne(x => x.OrderStatus, orderStatus =>
            {
                orderStatus.Property(x => x.Status).HasColumnName("Status").IsRequired(true);
                orderStatus.Property(x => x.Justification).HasColumnName("Justification").IsRequired(false);
            });

            builder.OwnsOne(x => x.TotalAmount, totalAmount =>
            {
                totalAmount.Property(x => x.Amount).HasColumnName("Amount").IsRequired(true);
            });

        }
    }
}
