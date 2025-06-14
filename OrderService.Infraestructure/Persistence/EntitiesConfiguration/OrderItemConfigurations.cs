using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infraestructure.Persistence.EntitiesConfiguration
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(a => a.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(x => x.Price, price =>
            {
                price.Property(x => x.Amount).HasColumnName("Amount").IsRequired(true);
            });
            
            builder.OwnsOne(x => x.Quantity, quantity =>
            {
                quantity.Property(x => x.Value).HasColumnName("Quantity").IsRequired(true);
            });  
            
            builder.OwnsOne(x => x.Description, description =>
            {
                description.Property(x => x.Title).HasColumnName("Title").IsRequired(true);
                description.Property(x => x.Description).HasColumnName("Description").IsRequired(false);
            });
        }
    }
}
