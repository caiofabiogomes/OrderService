using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OrderService.Application.Commands.PlaceOrder
{ 
    public class PlaceOrderCommand : IRequest<Result<Guid>>, IValidatableObject
    {
        [Required]
        [JsonIgnore]
        public Guid CustomerId { get; set; } = Guid.Empty;

        [Required]
        public OrderMode Mode { get; set; }

        [Required]
        public List<PlaceOrderCommandItem> Items { get; set; } = new List<PlaceOrderCommandItem>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        { 
            if (Items.GroupBy(i => i.ProductId).Any(g => g.Count() > 1))
            {
                yield return new ValidationResult(DomainExceptionMessages.InvalidOrderItems, new[] { nameof(Items) });
            }
        }
    }

    public class PlaceOrderCommandItem
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

}
